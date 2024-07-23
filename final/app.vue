<template>
  <v-app>
    <v-app-bar
      class="elevation-0"
      :color="theme.global.name.value === 'dark' ? '' : 'secondary'">
      <template v-slot:prepend>
        <v-icon
          class="ml-5 mr-2"
          @click="$router.push('/')"
          :color="theme.global.name.value === 'dark' ? 'secondary' : ''"
          >mdi-book-open-blank-variant</v-icon
        >
        <v-app-bar-title class="cursor-pointer" @click="$router.push('/')"
          >Rhyme
        </v-app-bar-title></template
      >
      <v-btn v-if="profileText.length !== 0" @click="signInDialog = true" prepend-icon="mdi-account">{{ profileText }}</v-btn>
      <v-btn v-else icon="mdi-account" @click="signInDialog = true" />
        <v-btn
          @click="toggleTheme"
          icon="mdi-theme-light-dark" />

      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
    </v-app-bar>
    <v-navigation-drawer location="right" :width="navigationDrawerWidth" v-model="drawer">
      <v-list class="text-center">
        <v-list-item @click="$router.push('/about')">
          <v-icon>mdi-information-slab-box-outline</v-icon> About
        </v-list-item>
        <v-list-item @click="$router.push('/')">
          <v-icon>mdi-home</v-icon> Home
        </v-list-item>
        <v-list-item @click="$router.push('/wordList')"
          ><v-icon> mdi-book-open-blank-variant-outline</v-icon> Rhyme
          Editor</v-list-item
        >
      </v-list>
    </v-navigation-drawer>
    <v-main>
      <NuxtPage />
    </v-main>
    <SignInDialog v-model="signInDialog" />
  </v-app>
</template>

<script setup lang="ts">
import nuxtStorage from 'nuxt-storage';
import { useTheme } from 'vuetify';
import TokenService from './scripts/tokenService';
import { useDisplay } from 'vuetify';

const tokenService = ref(new TokenService());
const theme = useTheme();
const drawer = ref(false);
const settingsDialog = ref(false);
const signInDialog = ref<boolean>(!tokenService.value.isLoggedIn());
const display = ref(useDisplay());
provide('TOKEN', tokenService);

onMounted(() => {
  var defaultTheme = nuxtStorage.localStorage.getData('theme');
  theme.global.name.value = defaultTheme ?? 'dark';
});

const profileText = computed<string>(() => {
  if (display.value.smAndUp) {
    if (tokenService?.value.isLoggedIn()) {
      return tokenService?.value.getUserName();
    } else {
      return 'Login';
    }
  } else {
    return '';
  }
});

const navigationDrawerWidth = computed(() => {
  switch (display.value.name) {
    case 'xs': return 175;
    case 'sm': return 200;
    case 'md': return 225;
    case 'lg': return 250;
    case 'xl': return 275;
    case 'xxl': return 300;
  }
});

function toggleTheme() {
  if (theme.global.name.value === 'dark') {
    theme.global.name.value = 'light';
  } else {
    theme.global.name.value = 'dark';
  }
  nuxtStorage.localStorage.setData('theme', theme.global.name.value);
}
</script>
