export default {
  content: [
    "./components/**/*.{js,vue,ts}",
    "./layouts/**/*.vue",
    "./pages/**/*.vue",
    "./plugins/**/*.{js,ts}",
    "./app.vue",
    "./error.vue",
    "./node_modules/flowbite/**/*.js"
  ],
  plugins: [
    require('flowbite'),
    require('@tailwindcss/typography')
  ],
  theme: {
    extend: {
      fontFamily: {
        alta: ['Alta', 'sans-serif'],
        optima: ['Optima', 'serif'],  
        relawey:['Ralewey'],  
        roboto: ['Roboto Condensed', 'sans-serif'] 
      },
      fontWeight: {
        extralight: 200,
        light: 300,
        regular: 400,
        medium: 500
      }
    },
    safelist: [
      'prose',
      'prose-sm',
      'prose-lg',
      'prose-xl',
      'prose-2xl',
      'prose-stone',
     
      'list-disc',
      'list-decimal',
      
    ],
  }
}