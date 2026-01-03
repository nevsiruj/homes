const articulos = [
  {
    id: "1",
    title: "¿Cómo calcular el Retorno de Inversión?",
    content: `<h2>¿QUÉ ES EL RETORNO DE INVERSIÓN?</h2>
<p>Para entender el retorno de inversión, debemos conocer que la mayoría de los bancos en Guatemala ofrecen plazos fijos entre 2-3%, una propiedad en venta o renta en una zona de alta demanda puede rendir un retorno de 4-6% y algunas propiedades comerciales aún más.</p>
<h2>CÓMO CALCULAR EL RETORNO DE INVERSIÓN ANUAL</h2>
<p>Para calcular el retorno de inversión es necesario multiplicar la renta mensual por 12. Descontar mantenimiento y IUSI anual. Luego dividir por precio del apartamento.</p>
<blockquote>
<p><i>Ejemplo</i>:<br /><br /><em>Apartamento en Venta en $175,000 rentado en $1,000</em></p>
<p>Renta Mensual $1000 x 12 =$12,000 - Mantenimiento Anual $1400 - IUSI Anual $600. Ingreso Anual $10,000 / Precio total de $175,000 = 5.7% retorno anual.</p>
</blockquote>
<h2>PLUSVALÍA</h2>
<p>Mientras en el banco tu dinero solo genera intereses, una propiedad inmobiliaria ofrece intereses mensuales y además la posibilidad de plusvalía. Según nuestra experiencia en zonas de alta demanda como zona 10, 13, 14, 15 en los últimos 10 años el promedio del valor por metro² ha subido un aproximado del 50%.</p>`,
    slug: "calcular-retorno-de-inversion",
    permalink: "https://homesguatemala.com/informativo/calcular-retorno-de-inversion/",
    imageUrl: "https://old-web.homesguatemala.com/wp-content/uploads/2019/06/calcular-retorno-de-inversion.jpg",
    categorias: "Informativo",
    fechaCreacion: "2024-06-15",
    activo: true
  },
  {
    id: "2",
    title: "Guía para comprar tu primera casa",
    content: `<h2>INTRODUCCIÓN</h2>
<p>Comprar tu primera casa es una de las decisiones más importantes de tu vida. En esta guía te explicaremos los pasos principales.</p>
<h2>PASOS PARA COMPRAR</h2>
<ul>
<li>Evalúa tu presupuesto</li>
<li>Busca financiamiento</li>
<li>Encuentra la propiedad ideal</li>
<li>Realiza la oferta</li>
<li>Cierra la negociación</li>
</ul>`,
    slug: "guia-primera-casa",
    permalink: "https://homesguatemala.com/informativo/guia-primera-casa/",
    imageUrl: "https://images.unsplash.com/photo-1560518883-ce09059eeffa",
    categorias: "Guías",
    fechaCreacion: "2024-08-10",
    activo: true
  },
  {
    id: "3",
    title: "Tendencias del mercado inmobiliario 2025",
    content: `<h2>EL MERCADO EN 2025</h2>
<p>El mercado inmobiliario guatemalteco continúa en crecimiento con nuevas oportunidades de inversión.</p>
<h2>ZONAS DE MAYOR DEMANDA</h2>
<p>Las zonas 10, 14, 15 y 16 siguen siendo las más cotizadas para inversión residencial.</p>`,
    slug: "tendencias-mercado-2025",
    permalink: "https://homesguatemala.com/informativo/tendencias-mercado-2025/",
    imageUrl: "https://images.unsplash.com/photo-1560520653-9e0e4c89eb11",
    categorias: "Mercado",
    fechaCreacion: "2025-01-01",
    activo: true
  }
];

export const getAllArticulos = () => {
  return new Promise((resolve) => {
    setTimeout(() => {
      resolve({
        success: true,
        data: articulos,
        total: articulos.length
      });
    }, 300);
  });
};

export const getArticuloById = (id) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const articulo = articulos.find(a => a.id === id);
      if (articulo) {
        resolve({
          success: true,
          data: articulo
        });
      } else {
        reject({
          success: false,
          message: "Artículo no encontrado"
        });
      }
    }, 300);
  });
};

export const createArticulo = (articuloData) => {
  return new Promise((resolve) => {
    setTimeout(() => {
      const newArticulo = {
        id: (articulos.length + 1).toString(),
        ...articuloData,
        fechaCreacion: new Date().toISOString().split('T')[0],
        activo: true
      };
      articulos.push(newArticulo);
      resolve({
        success: true,
        data: newArticulo,
        message: "Artículo creado exitosamente"
      });
    }, 300);
  });
};

export const updateArticulo = (id, articuloData) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const index = articulos.findIndex(a => a.id === id);
      if (index !== -1) {
        articulos[index] = { ...articulos[index], ...articuloData };
        resolve({
          success: true,
          data: articulos[index],
          message: "Artículo actualizado exitosamente"
        });
      } else {
        reject({
          success: false,
          message: "Artículo no encontrado"
        });
      }
    }, 300);
  });
};

export const deleteArticulo = (id) => {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      const index = articulos.findIndex(a => a.id === id);
      if (index !== -1) {
        articulos.splice(index, 1);
        resolve({
          success: true,
          message: "Artículo eliminado exitosamente"
        });
      } else {
        reject({
          success: false,
          message: "Artículo no encontrado"
        });
      }
    }, 300);
  });
};
