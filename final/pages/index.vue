<template>
  <v-alert v-if="error" type="error"> Could not find other note's URL </v-alert>
  <v-app>
    <v-container class="align-center d-flex justify-center">
      <v-card height="200" width="150" class="align-center d-flex justify-center mt-2 mb-6"
        @click="$router.push('/documentView?id=-1')">
        <v-icon class="mb-5">mdi-plus</v-icon>
      </v-card>
    </v-container>
    <v-container>
      <v-row>
        <v-col class="my-4" cols="12" sm="6" md="6" lg="4" v-for="document in documents" :key="document.documentId">
          <v-card @click="$router.push(`/documentView?id=${document.documentId}`)" elevation="2" height="300"
          :title="document.title" :subtitle="`Opened ${getDateString(document)}`">
            <template v-slot:append>
              <v-icon class="mx-4" color="error" @click.stop.prevent="deleteDocument(document.documentId)">mdi-delete-outline</v-icon>
            </template>
            <v-sheet tile class="ma-5 text-subtitle-1 text-medium-emphasis">
              
              <p>{{ document.content.substring(0, 200) + '...' }}</p>
            </v-sheet>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-app>
</template>

<script setup lang="ts">
// get documents from database? from last open time
// loop through them in the v-col element v-for
//
// have each v-card link to documentView, connecting the id of
// each document to the documentView (somehow??), so the view knows
// document to open
import { useDisplay } from 'vuetify';
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';
import { _getAppConfig } from '#app';
import { useEventBus } from '@vueuse/core';
import { signInOrOutKey } from '~/scripts/signInOrOutKey';

const bus = useEventBus(signInOrOutKey);

bus.on(async (e) => {
  await getDocumentList();
})

const display = useDisplay();
const router = useRouter();
const documents = ref<Document[]>();
const tokenService: Ref<TokenService> | undefined = inject('TOKEN');
const error = ref(false);

interface Document {
  documentId: number;
  title: string;
  content: string;
  lastOpened: string;
}

const getHeight = computed(() => {
  if (display.sm) {
    return 300;
  }
  if (display.md) {
    return 200;
  }
  if (display.lg) {
    return 300;
  }
});

async function deleteDocument(documentId: number) {
  try {
    const url = `document/deleteDocument?documentId=${documentId}`;
    const response = await Axios.post(url, {});
    await getDocumentList();
  } catch (error) {
    console.error('Error deleting document information', error);
  }
}

function getDateString(document: Document) {
  return new Date(Date.parse(document.lastOpened)).toLocaleDateString();
}

onMounted(async () => {
  await getDocumentList();
});

async function getDocumentList() {
  const guid = tokenService?.value.getGuid();
  if (guid !== '') {
    try {
      const url = `document/getDocumentList?userId=${guid}`;
      const response = await Axios.get(url);
      documents.value = response.data;
    } catch (error) {
      console.error('Error fetching selected word:', error);
    }
  } else {
    documents.value = [];
  }
}
</script>
