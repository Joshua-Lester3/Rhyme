import Axios from 'axios';

export default defineNuxtPlugin(() => {
  if (process.client) {
    if (
      window.location.hostname === 'localhost'
    ) {
      Axios.defaults.baseURL = 'https://localhost:7166/';
    } else {
      Axios.defaults.baseURL = 'https://rhymeapi.azurewebsites.net/';
    }
  }
});