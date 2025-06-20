<template>
  <v-app :theme="currentTheme">
    <v-main>
      <AppHeader v-if="showHeader" @toggle-theme="toggleTheme" />
      <router-view />
    </v-main>
  </v-app>
</template>

<script lang="ts" setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { onMounted } from 'vue'

import { useTheme } from 'vuetify'

const route = useRoute()

const showHeader = computed(() => route.path !== '/login')

const theme = useTheme()
const isDark = computed(() => theme.global.name.value === 'dark')
const currentTheme = computed(() => theme.global.name.value)

onMounted(() => {
  const saved = localStorage.getItem('theme')
  if (saved === 'dark' || saved === 'light') {
    theme.global.name.value = saved
  }
})

function toggleTheme() {
  const newTheme = theme.global.name.value === 'light' ? 'dark' : 'light'
  theme.global.name.value = newTheme
  localStorage.setItem('theme', newTheme)
}
</script>
