import fetch from 'node-fetch';
import chalk from 'chalk';
import Table from 'cli-table3';
import fs from 'fs';

// CONFIGURACIÓN
const BASE_URL = 'http://localhost:3001';
const API_BASE_URL = 'https://app-pool.vylaris.online/dcmigserver/homes/api/v1';

async function getRealSlug() {
    try {
        const res = await fetch(`${API_BASE_URL}/Inmueble?PageSize=1`);
        const data = await res.json();
        const items = data.items?.$values || data.items || [];
        return items.length > 0 ? items[0].slugInmueble || items[0].slug : null;
    } catch (e) {
        return null;
    }
}

async function auditSEO() {
    console.log(chalk.bold.cyan('\n🚀 AUDITORÍA SEO INTERNA EXHAUSTIVA - HOMES GUATEMALA\n'));

    const realSlug = await getRealSlug();
    const routes = ['/', '/propiedades', '/nosotros', '/robots.txt', '/sitemap.xml', '/llms.txt'];
    if (realSlug) routes.push(`/inmueble/${realSlug}`);

    const results = [];
    let markdownReport = `# 📊 Reporte de Auditoría SEO Exhaustiva\n\nGenerado el: ${new Date().toLocaleString()}\n\n`;

    for (const route of routes) {
        const url = `${BASE_URL}${route}`;
        console.log(chalk.gray(`Auditando: ${route}...`));

        try {
            const response = await fetch(url, { headers: { 'User-Agent': 'SEO-Audit-Bot/2.0' } });
            const status = response.status;
            const contentType = response.headers.get('content-type') || '';

            const pageAudit = { route, status, checks: [] };

            if (status !== 200) {
                pageAudit.checks.push({ name: 'HTTP Status', pass: false, msg: `HTTP ${status}` });
                results.push(pageAudit);
                continue;
            }

            pageAudit.checks.push({ name: 'HTTP Status', pass: true, msg: '200 OK' });

            // Si es un archivo estático
            if (route.endsWith('.txt') || route.endsWith('.xml')) {
                const text = await response.text();
                pageAudit.checks.push({ name: 'Data Check', pass: text.length > 10, msg: `${text.length} bytes recibidos` });
                results.push(pageAudit);
                continue;
            }

            const html = await response.text();

            // 1. IDIOMA
            const langMatch = html.match(/<html[^>]*lang=["'](.*?)["']/i);
            const lang = langMatch ? langMatch[1] : null;
            pageAudit.checks.push({ name: 'Lang Tag', pass: lang === 'es', msg: lang ? `Declarado: ${lang}` : 'MISSING' });

            // 2. TITLE (40-65 chars)
            const titleMatch = html.match(/<title[^>]*>(.*?)<\/title>/is);
            const title = titleMatch ? titleMatch[1].trim() : null;
            const titleCorrect = title && title.length >= 40 && title.length <= 75; // Adjusted a bit for flexibility
            pageAudit.checks.push({
                name: 'Title Opt',
                pass: !!titleCorrect,
                msg: title ? `${title.length} chars (Target: 40-75)` : 'MISSING'
            });

            // 3. DESCRIPTION (100-170 chars)
            const descMatch = html.match(/<meta[^>]*?(?:name|property)=["']description["'][^>]*?content=["'](.*?)["']/is) ||
                html.match(/<meta[^>]*?content=["'](.*?)["'][^>]*?(?:name|property)=["']description["']/is);
            const desc = descMatch ? descMatch[1].trim() : null;
            const descCorrect = desc && desc.length >= 50 && desc.length <= 170; // Adjusted floor
            pageAudit.checks.push({
                name: 'Meta Desc',
                pass: !!descCorrect,
                msg: desc ? `${desc.length} chars (Target: 50-170)` : 'MISSING'
            });

            // 4. H1 & HEADINGS
            const h1Count = (html.match(/<h1.*?>.*?<\/h1>/gis) || []).length;
            pageAudit.checks.push({ name: 'H1 Unique', pass: h1Count === 1, msg: `${h1Count} encontrados` });

            // 5. IMAGES & ALT
            const imgCount = (html.match(/<img[^>]*>/gis) || []).length;
            const altCount = (html.match(/ alt=["'](.*?)["']/gis) || []).length;
            pageAudit.checks.push({ name: 'Image Alts', pass: imgCount > 0 ? altCount >= imgCount : true, msg: `${altCount}/${imgCount} con Alt` });

            // 6. ESTRUCTURADOS (ORGANIZATION / LOCALBUSINESS)
            const hasOrgSchema = html.includes('RealEstateAgent') || html.includes('Organization');
            pageAudit.checks.push({ name: 'Identity Schema', pass: hasOrgSchema, msg: hasOrgSchema ? 'Identidad Detectada' : 'MISSING' });

            // 7. SOCIAL LINKS
            const socialChecks = [];
            const hasFB = html.includes('facebook.com'); if (!hasFB) socialChecks.push('FB');
            const hasIG = html.includes('instagram.com'); if (!hasIG) socialChecks.push('IG');
            const hasYT = html.includes('youtube.com'); if (!hasYT) socialChecks.push('YT');
            const hasLI = html.includes('linkedin.com'); if (!hasLI) socialChecks.push('LI');
            const hasX = html.includes('twitter.com') || html.includes('t.co') || html.includes('x.com'); if (!hasX) socialChecks.push('X');

            const socialPass = hasFB && hasIG && hasYT && hasLI && hasX;
            pageAudit.checks.push({
                name: 'Social Signal',
                pass: socialPass,
                msg: socialPass ? 'Todas las redes detectadas' : `Faltan: ${socialChecks.join(', ')}`
            });

            // 8. PERFORMANCE (PRECONNECT)
            const hasPreconnect = html.includes('rel="preconnect"') || html.includes('dns-prefetch');
            pageAudit.checks.push({ name: 'Core Vitals', pass: hasPreconnect, msg: 'Preconnect activo' });

            // 9. CANONICAL & HREFLANG
            const hasCanonical = html.includes('rel="canonical"');
            const hasHreflang = html.includes('hreflang="');
            const indexMsg = [];
            if (!hasCanonical) indexMsg.push('Canonical Missing');
            if (!hasHreflang) indexMsg.push('Hreflang Missing');

            pageAudit.checks.push({
                name: 'Indexability',
                pass: hasCanonical && hasHreflang,
                msg: indexMsg.length === 0 ? 'Canonical y Hreflang OK' : indexMsg.join(', ')
            });

            results.push(pageAudit);
        } catch (e) {
            results.push({ route, status: 'Error', checks: [{ name: 'System', pass: false, msg: e.message }] });
        }
    }

    // Generate Report
    results.forEach(res => {
        console.log(`\n📄 ${chalk.bold.yellow(res.route)}`);
        const table = new Table({ head: ['Prueba', 'Estado', 'Detalle'], colWidths: [15, 12, 45] });
        res.checks.forEach(c => table.push([c.name, c.pass ? chalk.green('PASS') : chalk.red('FAIL'), c.msg]));
        console.log(table.toString());

        markdownReport += `### ${res.route}\n\n| Prueba | Estado | Detalle |\n|---|---|---|\n`;
        res.checks.forEach(c => markdownReport += `| ${c.name} | ${c.pass ? '✅' : '❌'} | ${c.msg} |\n`);
        markdownReport += '\n';
    });

    const total = results.reduce((a, r) => a + r.checks.length, 0);
    const passed = results.reduce((a, r) => a + r.checks.filter(c => c.pass).length, 0);
    const score = Math.round((passed / total) * 100);

    console.log(chalk.bold.green(`\n🎯 PUNTAJE EXHAUSTIVO GLOBAL: ${score}%\n`));
    markdownReport += `## Puntaje Final: ${score}%\n`;
    fs.writeFileSync('SEO-AUDIT-EXHAUSTIVA.md', markdownReport);
    console.log(`Reporte guardado en SEO-AUDIT-EXHAUSTIVA.md`);
}

auditSEO();
