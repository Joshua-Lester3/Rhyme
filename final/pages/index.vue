<template>
  <v-alert v-if="error" type="error"> Could not find other note's URL </v-alert>
  <v-app>
    <v-container class="align-center d-flex justify-center">
      <v-card :color="theme.global.name.value === 'dark' ? '' : 'secondary'" height="200" width="150" class="align-center d-flex justify-center mt-2 mb-6"
        @click="$router.push('/documentView?id=-1')">
        <v-icon class="mb-5">mdi-plus</v-icon>
      </v-card>
    </v-container>
    <v-container>
      <v-row>
        <v-col class="my-4" cols="12" sm="6" md="6" lg="4" v-for="document in documents" :key="document.documentId">
          <v-card elevation="2" height="300" :color="theme.global.name.value === 'dark' ? '' : 'secondary'"
            @click="$router.push(`/documentView?id=${document.documentId}`)">
            <template v-slot:title>
              <v-card-title>{{ document.title }}</v-card-title>
            </template>
            <template v-slot:subtitle>
              <v-card-subtitle>
                {{ `Last saved ${getDateString(document)}` }}
              </v-card-subtitle>
            </template>
            <template v-slot:append>
              <v-card-actions>

                <v-btn id="child" class="mx-4 " color="error" icon="mdi-delete-outline"
                  @click.stop.prevent="documentIdToDelete = document.documentId; deleteDocumentDialog = true;"></v-btn>
              </v-card-actions>
            </template>

              <p class="mx-7 text-body-1 text-medium-emphasis">{{ document.content + '...' }}</p>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
    <DeleteDocumentDialog v-model="deleteDocumentDialog" @accept="deleteDocument(documentIdToDelete)"></DeleteDocumentDialog>
  </v-app>
</template>

<script setup lang="ts">
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';
import { _getAppConfig } from '#app';
import { useEventBus } from '@vueuse/core';
import { signInOrOutKey } from '~/scripts/signInOrOutKey';
import { useTheme } from 'vuetify';

const bus = useEventBus(signInOrOutKey);

bus.on(async (e) => {
  await getDocumentList();
})

const theme = useTheme();
const documents = ref<Document[]>();
const tokenService: Ref<TokenService> | undefined = inject('TOKEN');
const error = ref(false);
const deleteDocumentDialog = ref(false);
const documentIdToDelete = ref<number>(-1);

interface Document {
  documentId: number;
  title: string;
  content: string;
  lastSaved: string;
}

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
  return new Date(Date.parse(document.lastSaved)).toLocaleDateString();
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
      documents.value?.sort((a, b) => {
        const aDate = new Date(Date.parse(a.lastSaved));
        const bDate = new Date(Date.parse(b.lastSaved));
        if (aDate > bDate) {
          return -1;
        }
        else if (aDate < bDate) {
          return 1;
        }
        else {
          return 0;
        }
      });
    } catch (error) {
      console.error('Error fetching selected word:', error);
    }
  } else {
    documents.value = [];
  }
}
</script>

<style scoped>
.deleteIcon:hover {
  background-color: red;
}
</style>