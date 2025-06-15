<template>
  <v-app theme="light">
    <v-main>
      <AppHeader v-if="showHeader" />
      <router-view />
    </v-main>
  </v-app>
</template>

<script lang="ts" setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()

const showHeader = computed(() => route.path !== '/login')
</script>
