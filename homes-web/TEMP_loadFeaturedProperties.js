const loadFeaturedProperties = async () => {
    featuredPropertiesLoading.value = true;
    featuredPropertiesError.value = null;

    try {
        const featuredSlugs = [
            'casa-completamente-remodelada-en-venta-con-vistas-de-la-ciudad-zona-10-csv5506',
            'casa-de-un-nivel-en-venta-santa-catarina-pinula-cs5230',
            'casa-con-amplio-jardin-en-venta-hacienda-nueva-country-club-csv5342',
            'casa-en-venta-de-un-nivel-km-25-5-carretera-a-el-salvador-csv5591',
            'apartamento-en-venta-zona-10-asv5758',
            'apartamento-amueblado-en-venta-zona-10-aav5776'
        ];

        // Cargar propiedades destacadas individualmente (método confiable)
        const fetchPromises = featuredSlugs.map((slug) =>
            inmuebleService.getInmuebleBySlug(slug).catch(() => null)
        );

        const resolved = await Promise.all(fetchPromises);

        const featuredPropertiesData = resolved
            .filter(property => property !== null)
            .map((property) => {
                const plainProperty = JSON.parse(JSON.stringify(property));
                return {
                    ...plainProperty,
                    imagenesReferencia:
                        plainProperty.imagenesReferencia?.$values ||
                        plainProperty.imagenesReferencia ||
                        [],
                    amenidades:
                        plainProperty.amenidades?.$values || plainProperty.amenidades || [],
                    precioActivo: plainProperty.precio != null && plainProperty.precio > 0,
                };
            });

        featuredProperties.value = featuredPropertiesData;

        if (featuredPropertiesData.length === 0) {
            featuredPropertiesError.value = "No se pudieron cargar las propiedades destacadas";
        }

    } catch (err) {
        console.error("Error al cargar propiedades destacadas:", err);
        featuredPropertiesError.value = "Error al cargar propiedades destacadas";
        featuredProperties.value = [];
    } finally {
        featuredPropertiesLoading.value = false;
    }
};
