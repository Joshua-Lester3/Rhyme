<template>
  <v-toolbar height="50">
    <v-row class="mt-2">
      <v-col xs="5" sm="4" md="3" class="mt-2 d-flex justify-center justify-space-evenly">
        <v-btn density="compact" class="px-0 text-body-2">File
          <v-menu activator="parent">
            <v-list>
              <v-list-item density="compact" @click="$emit('saveChanges')">
                <v-list-item-title class="cursor-pointer">Save</v-list-item-title>
              </v-list-item>
              <v-list-item density="compact" @click="$emit('deleteDocument')">
                <v-list-item-title>Delete</v-list-item-title>
              </v-list-item>
              <v-list-item density="compact" @click="$emit('showLinkDialog')">
                <v-list-item-title class="text-body-2">Get link</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>
        </v-btn>
        <v-btn density="compact" class="text-body-2">Rhyme
          <v-menu activator="parent">
            <v-list>
              <v-list-item density="compact" @click="$emit('showRhymDialog')">
                <v-list-item-title>Rhyme,
                  kid</v-list-item-title>
              </v-list-item>
              <v-list-item density="compact" @click="$emit('showRhymeSchemeWindow')">
                <v-list-item-title>Show/hide rhyme
                  scheme,
                  kid</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>
        </v-btn>

      </v-col>
      <v-col xs="7" sm="8" md="9">
        <v-text-field v-model="modelValue" density="compact" variant="solo" bg-color="secondary" flat />
      </v-col>
    </v-row>
    <!-- <v-row class="mt-2">
        <v-row class="justify-start mt-2 ml-3">
          <v-btn density="compact" class="px-0">File
              <v-menu activator="parent">
                <v-list>
                  <v-list-item density="compact">
                    <v-list-item-title @click="$emit('saveChanges')">Save</v-list-item-title>
                  </v-list-item>
                  <v-list-item density="compact">
                    <v-list-item-title @click="$emit('deleteDocument')">Delete</v-list-item-title>
                  </v-list-item>
                  <v-list-item density="compact">
                    <v-list-item-title @click="$emit('showLinkDialog')">Get link</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </v-btn>
            <v-btn density="compact" class="">Rhyme
              <v-menu activator="parent">
                <v-list>
                  <v-list-item density="compact">
                    <v-list-item-title @click="$emit('showRhymDialog')">Rhyme, kid</v-list-item-title>
                  </v-list-item>
                  <v-list-item density="compact">
                    <v-list-item-title @click="$emit('showRhymeSchemeWindow')">Show/hide rhyme scheme,
                      kid</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-menu>
            </v-btn>

        </v-row>
          <v-text-field class="justify-end" v-model="modelValue" density="compact" variant="solo" bg-color="secondary" flat />
      </v-row> -->
  </v-toolbar>
</template>

<script setup lang="ts">
import { useDisplay, useTheme } from 'vuetify';

const display = ref(useDisplay());
const modelValue = defineModel<string>();
const theme = useTheme();

defineEmits([
  'deleteDocument',
  'saveChanges',
  'showRhymeSchemeWindow',
  'showRhymDialog',
  'showLinkDialog',
]);

watch(() => display.value.name, () => {
  switch (display.value.name) {
    case 'xs': console.log('xs'); break;
    case 'sm': console.log('sm'); break;
    case 'md': console.log('md'); break;
    case 'lg': console.log('lg'); break;
    case 'xl': console.log('xl'); break;
    case 'xxl': console.log('xxl'); break;
    default: console.log('hello');
  }
});
</script>

<style scoped>
v-list-tile-title.onHover:hover {
  background-color: v-bind('theme.global.current.value.colors.secondary');
}
</style>