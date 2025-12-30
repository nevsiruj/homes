// Schema.org mejorado para SEO local
export const businessSchema = {
    "@context": "https://schema.org",
    "@type": "RealEstateAgent",
    "name": "Homes Guatemala",
    "alternateName": "Homes Guatemala - Bienes Raíces de Lujo",
    "description": "Agencia inmobiliaria líder en Guatemala especializada en venta y renta de propiedades de lujo. Más de 18 años de experiencia.",
    "image": "https://app-pool.vylaris.online/dcmigserver/homes/5369ffc1-5e81-4be1-a01e-617c564b7eed.webp",
    "logo": "https://app-pool.vylaris.online/dcmigserver/homes/0ecfe259-77d7-450f-afb3-4ec21231dc6f.webp",
    "@id": "https://homesguatemala.com",
    "url": "https://homesguatemala.com",
    "telephone": "+502-5633-0961",
    "email": "info@homesguatemala.com",
    "priceRange": "$$$",
    "address": {
        "@type": "PostalAddress",
        "streetAddress": "Design Center Local 217, Zona 10",
        "addressLocality": "Guatemala City",
        "addressRegion": "Guatemala",
        "postalCode": "01010",
        "addressCountry": "GT"
    },
    "geo": {
        "@type": "GeoCoordinates",
        "latitude": "14.5995",
        "longitude": "-90.5069"
    },
    "areaServed": [
        {
            "@type": "City",
            "name": "Ciudad de Guatemala"
        },
        {
            "@type": "City",
            "name": "Antigua Guatemala"
        }
    ],
    "openingHoursSpecification": [
        {
            "@type": "OpeningHoursSpecification",
            "dayOfWeek": ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"],
            "opens": "09:00",
            "closes": "18:00"
        },
        {
            "@type": "OpeningHoursSpecification",
            "dayOfWeek": "Saturday",
            "opens": "09:00",
            "closes": "13:00"
        }
    ],
    "sameAs": [
        "https://www.facebook.com/homesguatemala",
        "https://www.instagram.com/homesguatemala",
        "https://www.youtube.com/@homesguatemala4975",
        "https://www.linkedin.com/company/homes-guatemala",
        "https://www.twitter.com/homesguatemala"
    ],
    "aggregateRating": {
        "@type": "AggregateRating",
        "ratingValue": "4.8",
        "reviewCount": "150",
        "bestRating": "5",
        "worstRating": "1"
    },
    "founder": {
        "@type": "Person",
        "name": "Ryan Sisco"
    },
    "foundingDate": "2007",
    "numberOfEmployees": {
        "@type": "QuantitativeValue",
        "value": "12"
    },
    "slogan": "Tu hogar ideal en Guatemala",
    "knowsAbout": [
        "Bienes Raíces",
        "Propiedades de Lujo",
        "Inversión Inmobiliaria",
        "Renta de Propiedades",
        "Venta de Casas",
        "Apartamentos en Guatemala"
    ],
    "hasOfferCatalog": {
        "@type": "OfferCatalog",
        "name": "Servicios Inmobiliarios",
        "itemListElement": [
            {
                "@type": "Offer",
                "itemOffered": {
                    "@type": "Service",
                    "name": "Venta de Propiedades",
                    "description": "Asesoría especializada en venta de casas y apartamentos de lujo"
                }
            },
            {
                "@type": "Offer",
                "itemOffered": {
                    "@type": "Service",
                    "name": "Renta de Propiedades",
                    "description": "Gestión y renta de propiedades residenciales y comerciales"
                }
            },
            {
                "@type": "Offer",
                "itemOffered": {
                    "@type": "Service",
                    "name": "Proyectos Inmobiliarios",
                    "description": "Desarrollo y comercialización de proyectos inmobiliarios exclusivos"
                }
            }
        ]
    }
};
