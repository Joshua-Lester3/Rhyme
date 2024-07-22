// import this after install `@mdi/font` package
import '@mdi/font/css/materialdesignicons.css';
import colors from 'vuetify/lib/util/colors';
import 'vuetify/styles';
import { createVuetify } from 'vuetify';

const light = {
  dark: false,
  colors: {
    primary: colors.grey.lighten4,
    secondary: colors.amber.accent3,
    background: colors.amber.accent1,
    surface: colors.amber.accent2,
    accent: '#ffc107',
    warning: '#ff5722',
    error: '#e91e63',
    info: '#03a9f4',
    success: '#4caf50',
  },
};

const dark = {
  dark: true,
  colors: {
    primary: colors.grey.darken4,
    primaryLighten: colors.grey.darken3,
    secondary: colors.yellow.darken1,
    accent: '#ffc107',
    error: '#ff5722',
    warning: '#e91e63',
    info: '#03a9f4',
    success: '#4caf50',
  },
};

export default defineNuxtPlugin(app => {
  const vuetify = createVuetify({
    // ... your configuration
    theme: {
      themes: {
        defaultTheme: dark,
        light,
        dark,
      },
    },
  });
  app.vueApp.use(vuetify);
});
