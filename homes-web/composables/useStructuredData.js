/**
 * Composable para generar datos estructurados (Schema.org JSON-LD)
 * para mejorar el SEO de la aplicación
 */

/**
 * Genera el schema de Organization para Homes Guatemala
 */
export const useOrganizationSchema = () => {
  return {
    '@context': 'https://schema.org',
    '@type': 'RealEstateAgent',
    name: 'Homes Guatemala',
    alternateName: 'Homes GT',
    url: 'https://homesguatemala.com',
    logo: 'https://app-pool.vylaris.online/dcmigserver/homes/5369ffc1-5e81-4be1-a01e-617c564b7eed.webp',
    description: 'Líderes en bienes raíces en Guatemala. Especialistas en venta y alquiler de propiedades de lujo.',
    telephone: '+502-5633-0961',
    email: 'info@homesguatemala.com',
    address: {
      '@type': 'PostalAddress',
      streetAddress: 'Design Center Local 217, Zona 10',
      addressLocality: 'Guatemala City',
      addressRegion: 'Guatemala',
      postalCode: '01010',
      addressCountry: 'GT'
    },
    geo: {
      '@type': 'GeoCoordinates',
      latitude: '14.6038',
      longitude: '-90.5064'
    },
    sameAs: [
      'https://www.facebook.com/homesguatemala',
      'https://www.instagram.com/homesguatemala',
      'https://www.youtube.com/@homesguatemala4975',
      'https://www.linkedin.com/company/homes-guatemala', // Placeholder
      'https://twitter.com/homesguatemala' // Placeholder
    ],
    contactPoint: {
      '@type': 'ContactPoint',
      telephone: '+502-5633-0961',
      contactType: 'customer service',
      areaServed: 'GT',
      availableLanguage: ['es', 'en']
    }
  }
}

/**
 * Genera el schema de RealEstateListing para una propiedad
 * @param {Object} inmueble - Datos del inmueble
 */
export const useRealEstateListingSchema = (inmueble) => {
  if (!inmueble) return null

  const schema = {
    '@context': 'https://schema.org',
    '@type': 'RealEstateListing',
    name: inmueble.titulo || 'Propiedad en Guatemala',
    description: inmueble.contenido ?
      inmueble.contenido.replace(/<[^>]*>/g, ' ').replace(/\s+/g, ' ').trim().substring(0, 200) :
      'Propiedad disponible en Guatemala',
    url: `https://homesguatemala.com/inmueble/${inmueble.slugInmueble}`,
    image: inmueble.imagenPrincipal || 'https://app-pool.vylaris.online/dcmigserver/homes/fa005e24-05c6-4ff0-a81b-3db107ce477e.webp'
  }

  // Agregar precio si está disponible
  if (inmueble.precio && inmueble.precioActivo !== false) {
    schema.offers = {
      '@type': 'Offer',
      price: inmueble.precio,
      priceCurrency: 'USD',
      availability: 'https://schema.org/InStock'
    }
  }

  // Agregar ubicación si está disponible
  if (inmueble.ubicaciones || inmueble.zona) {
    schema.address = {
      '@type': 'PostalAddress',
      addressLocality: inmueble.ubicaciones || inmueble.zona,
      addressCountry: 'GT'
    }
  }

  // Agregar características de la propiedad
  const features = []
  if (inmueble.habitaciones) features.push(`${inmueble.habitaciones} habitaciones`)
  if (inmueble.banos) features.push(`${inmueble.banos} baños`)
  if (inmueble.parqueos) features.push(`${inmueble.parqueos} parqueos`)

  if (features.length > 0) {
    schema.numberOfRooms = inmueble.habitaciones || 0
    schema.additionalProperty = features.map(f => ({
      '@type': 'PropertyValue',
      name: 'Característica',
      value: f
    }))
  }

  return schema
}

/**
 * Genera el schema de BreadcrumbList
 * @param {Array} breadcrumbs - Array de objetos {name, url}
 */
export const useBreadcrumbSchema = (breadcrumbs) => {
  if (!breadcrumbs || breadcrumbs.length === 0) return null

  return {
    '@context': 'https://schema.org',
    '@type': 'BreadcrumbList',
    itemListElement: breadcrumbs.map((crumb, index) => ({
      '@type': 'ListItem',
      position: index + 1,
      name: crumb.name,
      item: crumb.url
    }))
  }
}

/**
 * Genera el schema de Product para proyectos inmobiliarios
 * @param {Object} proyecto - Datos del proyecto
 */
export const useProjectSchema = (proyecto) => {
  if (!proyecto) return null

  const schema = {
    '@context': 'https://schema.org',
    '@type': 'Product',
    name: proyecto.titulo || 'Proyecto Inmobiliario en Guatemala',
    description: proyecto.contenido ?
      proyecto.contenido.replace(/<[^>]*>/g, ' ').replace(/\s+/g, ' ').trim().substring(0, 200) :
      'Proyecto inmobiliario disponible en Guatemala',
    url: `https://homesguatemala.com/proyecto/${proyecto.slugProyecto}`,
    image: proyecto.imagenPrincipal || 'https://app-pool.vylaris.online/dcmigserver/homes/fa005e24-05c6-4ff0-a81b-3db107ce477e.webp',
    brand: {
      '@type': 'Organization',
      name: 'Homes Guatemala'
    }
  }

  // Agregar precio si está disponible
  if (proyecto.precio && proyecto.precioActivo !== false) {
    schema.offers = {
      '@type': 'Offer',
      price: proyecto.precio,
      priceCurrency: 'USD',
      availability: 'https://schema.org/InStock'
    }
  }

  return schema
}

/**
 * Inserta el schema en el head de la página
 * @param {Object} schema - Objeto de schema
 */
export const useJsonldSchema = (schema) => {
  if (!schema) return

  useHead({
    script: [
      {
        type: 'application/ld+json',
        innerHTML: JSON.stringify(schema)
      }
    ]
  })
}
