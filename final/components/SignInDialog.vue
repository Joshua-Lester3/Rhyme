<template>
  <v-dialog v-model="modelValue" width="500" :persistent="!isLoggedIn" @update:model-value="closeDialog">
    <v-card v-if="!isLoggedIn">
      <v-tabs v-model="tab" color="secondary">
        <v-tab :value="0">Sign In</v-tab>
        <v-tab :value="1">Register</v-tab>
      </v-tabs>
      <v-alert v-if="errorMessage && tab === errorTab" type="error">
        {{ errorMessage }}
      </v-alert>
      <v-alert v-if="warningMessage && tab === errorTab" type="warning">{{
        'You must log in before using Rhyme.'
      }}</v-alert>
      <v-alert v-if="success" type="success">{{ successMessage }}</v-alert>
      <v-card-text>
        <div v-if="tab === 0">
          <p>Please sign in to use Rhyme.</p>
          <br />
        </div>
        <v-text-field v-model="username" label="Username" />
        <v-text-field v-if="tab === 1" v-model="email" label="Email" />
        <v-text-field v-model="password" label="Password" />
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn variant="tonal" color="secondary" @click="closeDialog">Cancel</v-btn>
        <v-btn class="ma-3" variant="flat" color="secondary" @click="submitInfo">Enter</v-btn>
      </v-card-actions>
    </v-card>
    <v-card v-else>
      <v-sheet color="secondary">
        <v-card-title>
          {{ tokenService?.getUserName() }}
        </v-card-title>
      </v-sheet>
      <v-card-text>
        <p>
          {{ `Email: ${profileInfo?.email}` }}
        </p>
        <p>
          {{ `Number of documents: ${profileInfo?.numberOfDocuments}` }}
        </p>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn @click="signOut">Sign out</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';
import { useEventBus } from '@vueuse/core';
import { signInOrOutKey } from '~/scripts/signInOrOutKey';

const bus = useEventBus(signInOrOutKey);

const modelValue = defineModel<boolean>({ default: false });
const emits = defineEmits(['signInOrOut']);
const tokenService: Ref<TokenService> | undefined = inject('TOKEN');
const tab = ref(0);
const email = ref('');
const password = ref('');
const username = ref('');
const success = ref(false);
const errorMessage = ref('');
const isLoggedIn = ref(tokenService?.value.isLoggedIn());
const successMessage =
  "You've successfully registered! Now log in to access Rhyme's features.";
const errorTab = ref(0);
const warningMessage = ref(false);

const profileInfo = ref<ProfileInfo | undefined>();

interface ProfileInfo {
  numberOfDocuments: number;
  email: string;
}

onMounted(async () => {
  await getProfileInfo();
});

watch(modelValue, async () => {
  await getProfileInfo();
})

async function getProfileInfo() {
  if (tokenService?.value.isLoggedIn()) {
    const url = `user/profileInfo?userId=${tokenService?.value.getGuid()}`;
    Axios.get(url)
      .then(response => {
        profileInfo.value = response.data;
      })
      .catch(error => {
        console.log(error);
      });
  }
}

function submitInfo() {
  if (tab.value === 0) {
    signIn();
  } else {
    register();
  }
}

function register() {
  const url = 'user/addUser';
  Axios.post(url, {
    username: username.value,
    email: email.value,
    password: password.value,
  })
    .then(response => {
      errorMessage.value = '';
      success.value = true;
    })
    .catch(error => {
      warningMessage.value = false;
      errorTab.value = tab.value;
      success.value = false;
      errorMessage.value = error.response.data;
    });
}

function signIn() {
  const url = 'token/getToken';
  Axios.post(url, {
    username: username.value,
    password: password.value,
  })
    .then(async (response) => {
      tokenService?.value.setToken(response.data.token);
      tokenService?.value.setGuid(undefined);
      errorMessage.value = '';
      modelValue.value = false;
      emits('signInOrOut');
      bus.emit({ loggedIn: true });
      await getProfileInfo();
      setTimeout(() => {
        isLoggedIn.value = true;
      }, 1000);
    })
    .catch(error => {
      warningMessage.value = false;
      errorTab.value = tab.value;
      success.value = false;
      errorMessage.value = error.response.data;
    });
}

function signOut() {
  tokenService?.value.setToken('');
  tokenService?.value.setGuid(null);
  isLoggedIn.value = false;
  emits('signInOrOut');
  bus.emit({ loggedIn: false });
}

function closeDialog() {
  if (tokenService?.value.isLoggedIn()) {
    email.value = '';
    password.value = '';
    modelValue.value = false;
  } else {
    warningMessage.value = true;
    errorTab.value = tab.value;
  }
}
</script>
