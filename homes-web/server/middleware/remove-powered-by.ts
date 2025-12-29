// Middleware de servidor para eliminar cabeceras que revelan información del servidor
// Esto mejora la seguridad al ocultar la tecnología utilizada

export default defineEventHandler((event) => {
    // Eliminar cabecera X-Powered-By que revela información del servidor
    event.node.res.removeHeader('X-Powered-By');

    // También podemos establecerla como vacía para mayor seguridad
    event.node.res.setHeader('X-Powered-By', '');
});
