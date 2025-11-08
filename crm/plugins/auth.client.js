// plugins/auth.client.js
import { useAuthStore } from '@/stores/auth'

export default defineNuxtPlugin(async () => {
  const auth = useAuthStore()
  await auth.hydrateFromToken()
})
