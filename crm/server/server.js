// server.js
import http from 'http';
import { parse } from 'url';
import propiedadService from '../services/inmuebleService'; 

/**
 * MANEJADOR DE RUTAS
 */
const handleRequest = async (req, res) => {
  const { pathname, query } = parse(req.url, true);
  
  try {
    // Configuración de CORS
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE');
    res.setHeader('Content-Type', 'application/json');
    
    // Endpoints
    if (pathname === '/api/propiedades' && req.method === 'GET') {
      const data = await propiedadService.getPropiedades(query);
      res.writeHead(200);
      res.end(JSON.stringify(data));
      return;
    }
    
    if (pathname.startsWith('/api/propiedades/') && req.method === 'GET') {
      const id = parseInt(pathname.split('/')[3]);
      const data = await propiedadService.getPropiedadById(id);
      res.writeHead(200);
      res.end(JSON.stringify(data));
      return;
    }

    if (pathname === '/api/propiedades' && req.method === 'POST') {
      let body = '';
      req.on('data', chunk => {
        body += chunk.toString(); // Convertir Buffer a string
      });
      req.on('end', async () => {
        const propiedadData = JSON.parse(body);
        const newPropiedad = await propiedadService.createPropiedad(propiedadData);
        res.writeHead(201);
        res.end(JSON.stringify(newPropiedad));
      });
      return;
    }

    if (pathname.startsWith('/api/propiedades/') && req.method === 'PUT') {
      const id = parseInt(pathname.split('/')[3]);
      let body = '';
      req.on('data', chunk => {
        body += chunk.toString(); // Convertir Buffer a string
      });
      req.on('end', async () => {
        const propiedadData = JSON.parse(body);
        const updatedPropiedad = await propiedadService.updatePropiedad(id, propiedadData);
        res.writeHead(200);
        res.end(JSON.stringify(updatedPropiedad));
      });
      return;
    }

    if (pathname.startsWith('/api/propiedades/') && req.method === 'DELETE') {
      const id = parseInt(pathname.split('/')[3]);
      await propiedadService.deletePropiedad(id);
      res.writeHead(204); // No Content
      res.end();
      return;
    }

    res.writeHead(404);
    res.end(JSON.stringify({ error: 'Endpoint no encontrado' }));
    
  } catch (error) {
    res.writeHead(500);
    res.end(JSON.stringify({ error: error.message }));
  }
};

/**
 * INICIO DEL SERVIDOR
 */
const server = http.createServer(handleRequest);

const PORT = 3005;
server.listen(PORT, () => {
  console.log(`✅ Servidor API (ESM) corriendo en http://localhost:${PORT}`);
});
