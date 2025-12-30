// Middleware para redirigir URLs con query params a URLs SEO-friendly
export default defineNuxtRouteMiddleware((to) => {
    // Solo aplicar en la página de propiedades principal
    if (to.path === '/propiedades' && to.query.Operaciones) {
        const operacion = String(to.query.Operaciones).toLowerCase();

        // Si es venta o renta, redirigir a la URL limpia
        if (operacion === 'venta' || operacion === 'renta') {
            // Crear los query params restantes (excluyendo Operaciones)
            const { Operaciones, ...restQuery } = to.query;

            // Construir la nueva URL
            const newPath = `/propiedades/${operacion}`;

            // Si hay otros query params, mantenerlos
            if (Object.keys(restQuery).length > 0) {
                return navigateTo({
                    path: newPath,
                    query: restQuery
                }, { redirectCode: 301 });
            }

            // Redirigir a la URL limpia con código 301 (permanente)
            return navigateTo(newPath, { redirectCode: 301 });
        }
    }
});
