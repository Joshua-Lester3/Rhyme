<template>
  <!-- <v-progress-linear v-if="isLoading" color="secondary" indeterminate />
  <v-row>
    <v-col cols="5">
      <v-select class="ma-5" width="50px" :items="[10, 25, 50, 100]" v-model="countPerPage" label="Words per page"
      @update:modelValue="setWords" />
    </v-col>
    <v-col class="ma-7" cols="3">
    </v-col>
    <v-btn color="secondary" @click="showAddRhyme = true">Add Rhyme</v-btn>
    </v-row>
    <v-card-text>
      <v-text-field label="Search for rhyme" append-inner-icon="mdi-magnify" @click:append-inner="setWords"
      v-model="searchTerm" />
    </v-card-text>
    <v-table>
      <thead>
        <tr>
          <th class="text-center text-h6">Word</th>
          <th class="text-center text-h6">Pronunciation</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(word, index) in words" :key="index">
          <td class="text-center">{{ word?.word }}</td>
          <td class="text-center">{{ word?.syllablesPronunciation }}</td>
        </tr>
      </tbody>
    </v-table>   -->
  <div v-if="isAdmin" class="d-flex">
    <v-btn class="mt-3 mx-auto" color="secondary" @click="showAddRhyme = true">Add Rhyme</v-btn>
  </div>
  <v-card class="ma-5">
    <v-row>
      <v-col cols="6">
        <v-text-field v-model="wordSearchTerm" class="ml-2 mt-2" density="compact" placeholder="Search word..."
          hide-details min-width="150"></v-text-field>
      </v-col>
      <v-col cols="6">
        <v-select class="mt-2 mr-2" density="compact" min-width="150" :items="[10, 25, 50, 100]" v-model="countPerPage"
          label="Words per page" @update:modelValue="setWords" />
      </v-col>
    </v-row>
    <v-data-table-server v-model:items-per-page="countPerPage" :headers="headers" :items="words"
      :items-length="totalItems" :loading="isLoading" :search="search" item-value="name" @update:options="setWords">
      <template v-slot:thead>

      </template>
      <template v-slot:bottom></template>
    </v-data-table-server>
    <v-pagination :size="size" v-model="page" :length rounded="circle" @update:modelValue="setWords" />
  </v-card>
  <AddRhyme v-model="showAddRhyme" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import { useDisplay } from 'vuetify';
import TokenService from '~/scripts/tokenService';

const isLoading = ref(true);
const countPerPage = ref(25);
const previousCountPerPage = ref(25);
const page = ref(1);
const words = ref<Array<WordDto>>([]);
const length = ref(0);
const showAddRhyme = ref(false);
const wordSearchTerm = ref('');
const previousSearchTerm = ref('');
const display = ref(useDisplay());
const headers = [
  { title: 'Word', align: 'start', sortable: false, key: 'word', },
  { title: 'Pronunciation', align: 'end', sortable: false, key: 'syllablesPronunciation', },
];
const totalItems = ref(0);
const search = ref('');
const tokenService: Ref<TokenService> | undefined = inject('TOKEN');
const isAdmin = computed(() => tokenService?.value.isAdmin());

watch([wordSearchTerm], () => {
  search.value = String(Date.now());
});

const size = computed(() => {
  switch (display.value.name) {
    case 'xs': return 'x-small';
    case 'sm': return 'small';
    case 'md': return 'default';
    case 'lg': return 'large';
    case 'xl': return 'x-large';
    case 'xxl': return 'x-large';
  }
})

interface WordDto {
  word: string;
  phonemes: string[];
  syllablesPronunciation: string[];
  plainTextSyllables: string[];
}

onMounted(() => {
  setWords();
});

async function setWords() {
  try {
    if (previousCountPerPage.value !== countPerPage.value) {
      page.value = 1;
      previousCountPerPage.value = countPerPage.value;
    }
    if (
      wordSearchTerm.value.trim().localeCompare(previousSearchTerm.value.trim()) !==
      0
    ) {
      page.value = 1;
      previousSearchTerm.value = wordSearchTerm.value.trim();
    }
    let url = `word/wordListPaginated?countPerPage=${countPerPage.value
      }&pageNumber=${page.value - 1}`;
    if (wordSearchTerm.value.trim() !== '') {
      url += `&word=${wordSearchTerm.value}`;
    }
    const response = await Axios.get(url);
    words.value = response.data.words;
    length.value = response.data.pages;
    totalItems.value = response.data.totalItems;
    isLoading.value = false;
  } catch (error) {
    console.error('Error getting word information', error);
  }
}
</script>
