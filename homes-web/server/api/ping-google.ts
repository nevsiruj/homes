export default defineEventHandler(async (event) => {

  const sitemapUrl = 'https://homesguatemala.com/sitemap.xml';
  const googlePingUrl = `https://www.google.com/ping?sitemap=${encodeURIComponent(sitemapUrl)}`;

  
  const query = getQuery(event);
  const secretToken = 'dbl@%Mt7H]c3n8r7a{\oPi3aS=ds\.!@#}'; 

  if (query.token !== secretToken) {
    throw createError({
      statusCode: 401,
      statusMessage: 'No autorizado. Se requiere un token válido.',
    });
  }

  try {
    
    console.log(`Enviando ping a Google para: ${sitemapUrl}`);
    const response = await fetch(googlePingUrl);

    if (response.ok) {
      return {
        status: 'success',
        message: 'Google ha sido notificado. El rastreo comenzará pronto.',
        timestamp: new Date().toISOString()
      };
    } else {
      throw new Error(`Google respondió con status: ${response.status}`);
    }
  } catch (error) {
    let details = 'Error desconocido';
    if (error && typeof error === 'object' && 'message' in error) {
      details = (error as any).message;
    } else if (typeof error === 'string') {
      details = error;
    }
    return {
      status: 'error',
      message: 'No se pudo notificar a Google',
      details
    };
  }
});