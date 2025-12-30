export default defineEventHandler((event) => {
    // Eliminar header X-Powered-By por seguridad
    // Esto oculta información sobre la tecnología del servidor
    setResponseHeader(event, 'X-Powered-By', '');

    // También intentar eliminarlo si ya existe
    const headers = getResponseHeaders(event);
    if (headers['x-powered-by']) {
        delete headers['x-powered-by'];
    }
});
