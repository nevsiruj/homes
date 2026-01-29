// server/routes/ping-google.ts
export default defineEventHandler(async (event) => {
  const query = getQuery(event)
  
  const receivedToken = String(query.token || '').replace(/\+/g, ' ')
  const secretToken = 'homes2024' 

  if (receivedToken !== secretToken) {
    return { 
      status: 'error', 
      message: 'Token inválido',
      tip: 'Prueba usando ?token=homes2024'
    }
  }

  try {    
    await $fetch('https://www.google.com/ping?sitemap=https://homesguatemala.com/sitemap.xml')
    return { status: 'success', message: 'Google notificado' }
  } catch (err) {
    return { status: 'error', message: 'Error al contactar a Google' }
  }
})