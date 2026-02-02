#!/usr/bin/env node

/**
 * Script de prueba para verificar que los metadatos OG están presentes en el HTML renderizado
 * Simula lo que haría Facebook cuando hace scraping de la página
 */

const testUrl = process.argv[2] || 'http://localhost:3000/inmueble/casa-zona-10-en-venta';

console.log(`🧪 Testing SSR metadata for: ${testUrl}\n`);

fetch(testUrl, {
  headers: {
    // Simular que somos el bot de Facebook
    'User-Agent': 'facebookexternalhit/1.1 (+http://www.facebook.com/externalhit_uatext.php)'
  }
})
  .then(response => {
    console.log(`📊 Status: ${response.status} ${response.statusText}`);
    
    if (response.status === 206) {
      console.error('❌ ERROR: Respuesta parcial (206) - Facebook verá contenido incompleto');
    } else if (response.status === 404) {
      console.error('❌ ERROR: Página no encontrada (404)');
    } else if (response.status === 200) {
      console.log('✅ Status OK (200)');
    }
    
    return response.text();
  })
  .then(html => {
    console.log('\n🔍 Verificando metadatos Open Graph:\n');
    
    // Extraer metadatos importantes
    const checks = [
      { pattern: /<meta property="og:title" content="([^"]+)"/, label: 'og:title' },
      { pattern: /<meta property="og:description" content="([^"]+)"/, label: 'og:description' },
      { pattern: /<meta property="og:image" content="([^"]+)"/, label: 'og:image' },
      { pattern: /<meta property="og:url" content="([^"]+)"/, label: 'og:url' },
      { pattern: /<meta property="og:type" content="([^"]+)"/, label: 'og:type' },
      { pattern: /<meta name="twitter:card" content="([^"]+)"/, label: 'twitter:card' },
      { pattern: /<meta name="twitter:image" content="([^"]+)"/, label: 'twitter:image' },
    ];
    
    let allPassed = true;
    
    checks.forEach(({ pattern, label }) => {
      const match = html.match(pattern);
      if (match) {
        const value = match[1];
        const preview = value.length > 60 ? value.substring(0, 60) + '...' : value;
        console.log(`  ✅ ${label}: ${preview}`);
      } else {
        console.log(`  ❌ ${label}: NO ENCONTRADO`);
        allPassed = false;
      }
    });
    
    // Verificar Schema.org
    if (html.includes('application/ld+json')) {
      console.log('\n  ✅ Schema.org: PRESENTE');
      
      // Extraer y validar JSON-LD
      const jsonLdMatch = html.match(/<script type="application\/ld\+json"[^>]*>(.*?)<\/script>/s);
      if (jsonLdMatch) {
        try {
          const jsonLd = JSON.parse(jsonLdMatch[1]);
          if (jsonLd['@type'] === 'RealEstateListing') {
            console.log('  ✅ Schema type: RealEstateListing');
          }
        } catch (e) {
          console.log('  ⚠️  Schema.org presente pero JSON inválido');
        }
      }
    } else {
      console.log('\n  ❌ Schema.org: NO ENCONTRADO');
      allPassed = false;
    }
    
    // Verificar que no sea una respuesta vacía o de error
    if (html.length < 1000) {
      console.log('\n⚠️  ADVERTENCIA: HTML muy pequeño, posible error en SSR');
      allPassed = false;
    }
    
    // Resultado final
    console.log('\n' + '='.repeat(60));
    if (allPassed) {
      console.log('✅ TODOS LOS METADATOS PRESENTES - Facebook debería poder scrapear correctamente');
    } else {
      console.log('❌ FALTAN METADATOS - Facebook puede tener problemas al scrapear');
    }
    console.log('='.repeat(60));
    
    // Guardar HTML para inspección manual si hay problemas
    if (!allPassed) {
      const fs = require('fs');
      const filename = 'debug-ssr-output.html';
      fs.writeFileSync(filename, html);
      console.log(`\n💾 HTML guardado en: ${filename}`);
    }
  })
  .catch(error => {
    console.error('❌ ERROR:', error.message);
    process.exit(1);
  });
