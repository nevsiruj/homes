// Middleware del servidor para manejar mejor las solicitudes de bots/crawlers
export default defineEventHandler((event) => {
  const userAgent = getHeader(event, 'user-agent') || '';
  
  // Detectar si es un bot de redes sociales
  const isSocialBot = /facebookexternalhit|Facebot|Twitterbot|WhatsApp|LinkedInBot|Slackbot/i.test(userAgent);
  
  if (isSocialBot) {
    console.log(`🤖 [BOT DETECTED] ${userAgent}`);
    console.log(`🔗 [BOT REQUEST] ${event.node.req.url}`);
    
    // Asegurar que se use SSR completo para bots
    event.context.ssr = true;
    
    // Dar más tiempo para que los datos se carguen
    event.context.fetchTimeout = 10000;
    
    // Asegurar que las respuestas sean completas, no parciales
    setHeader(event, 'Cache-Control', 'public, max-age=3600, must-revalidate');
  }
});
