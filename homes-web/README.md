# Nuxt Minimal Starter

Look at the [Nuxt documentation](https://nuxt.com/docs/getting-started/introduction) to learn more.

## Setup

Make sure to install dependencies:

```bash
# npm
npm install

# pnpm
pnpm install

# yarn
yarn install

# bun
bun install
```

## Development Server

Start the development server on `http://localhost:3000`:

```bash
# npm
npm run dev

# pnpm
pnpm dev

# yarn
yarn dev

# bun
bun run dev
```

## Production

Build the application for production:

```bash
# npm
npm run build

# pnpm
pnpm build

# yarn
yarn build

# bun
bun run build
```

Locally preview production build:

```bash
# npm
npm run preview

# pnpm
pnpm preview

# yarn
yarn preview

# bun
bun run preview
```

Check out the [deployment documentation](https://nuxt.com/docs/getting-started/deployment) for more information.

## Deploy automático a Netlify con GitHub Actions

Para que el archivo `_redirects` se copie y el sitio se despliegue automáticamente en Netlify al hacer push en la rama `dev`, agrega este workflow en `.github/workflows/deploy-netlify.yml`:

```yaml
name: Deploy

on:
  push:
    branches:
      - dev

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Instalar dependencias
        run: npm install

      - name: Build del proyecto
        run: npm run build

      - name: Copiar _redirects
        run: cp _redirects .output/public/_redirects 
```

## Automatización de _redirects en rama dev

Para que el archivo `_redirects` se copie automáticamente al directorio de salida (`.output/public`) cada vez que hagas push a la rama `dev`, agrega el siguiente workflow en `.github/workflows/copy-redirects.yml`:

```yaml
name: Copiar _redirects a .output/public

on:
  push:
    branches:
      - dev

jobs:
  build-and-copy-redirects:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Instalar dependencias
        run: npm install

      - name: Build del proyecto
        run: npm run build

      - name: Copiar _redirects
        run: cp _redirects .output/public/_redirects
```

Este workflow solo copia el archivo `_redirects` al directorio de salida cada vez que se hace push a la rama `dev`. Puedes modificar el directorio según tu configuración de build.
