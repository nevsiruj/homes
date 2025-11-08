import { useAuthStore } from '@/stores/auth'

export default defineNuxtRouteMiddleware(async (to) => {
  const auth = useAuthStore()
  const tokenCookie = useCookie('jwt-token')

  if (tokenCookie.value && !auth.isAuthenticated) {
    auth.token = tokenCookie.value
    auth.status = 'loading'

    if (process.server) {
      try {
        const payload = decodeJwt(tokenCookie.value)

        const rawRole =
          payload.role ??
          payload.rol ??
          payload.roles ??
          payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ??
          []

        const rolesArr = Array.isArray(rawRole) ? rawRole : [rawRole].filter(Boolean)
        const roles = rolesArr.map(r => String(r).toLowerCase())

        auth.user = {
          id: payload.sub || payload.id || payload.nameid || null,
          email: payload.email || payload.unique_name || null,
          name: payload.name || payload.unique_name || payload.email || payload['given_name'] || null,
          roles,
          raw: payload,
        }
        auth.status = 'authenticated'
      } catch {
        auth.user = null
        auth.roles = []
        auth.status = 'idle'
      }
    } else {
      if (typeof auth.hydrateFromToken === 'function') {
        await auth.hydrateFromToken()
      }
    }
  }

  const needsAuth = to.meta?.requiresAuth === true
  if (needsAuth && !auth.isAuthenticated) {
    return navigateTo(`/?redirect=${encodeURIComponent(to.fullPath)}`)
  }

  const required = Array.isArray(to.meta?.roles) ? to.meta.roles.map(r => String(r).toLowerCase()) : []
  const hasAny = auth.hasAnyRole?.(required) ?? required.some(r => auth.user?.roles?.includes(r))
  if (required.length && !hasAny) {
    return navigateTo('/admin') // o una 403
  }
})

function decodeJwt(token) {
  const parts = token.split('.')
  if (parts.length !== 3) return {}
  const b64 = parts[1].replace(/-/g, '+').replace(/_/g, '/')
  const json = process.server
    ? Buffer.from(b64, 'base64').toString('utf-8')
    : atob(b64)
  return JSON.parse(json)
}
