/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{html,js,cs,razor,cshtml}", "!./node_modules/**/*"],
  theme: {
    extend: {},
  },
    plugins: [require('@tailwindcss/forms'),],
}

