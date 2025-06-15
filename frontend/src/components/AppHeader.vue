<!-- src/components/layouts/MainLayout.vue -->
<template>
  <v-navigation-drawer v-model="isDrawerOpen" class="pt-8 mt-10">
    <v-list>
      <v-list-item v-if="isAdmin" link :to="'/'" prepend-icon="mdi-monitor-dashboard">Dashboard</v-list-item>
      <v-list-item link :to="'/suggestion'" prepend-icon="mdi-head-lightbulb-outline">Sugestões</v-list-item>

      <!-- Visível apenas para Admin -->
      <v-list-item
        v-if="isAdmin"
        link
        :to="'/category'"
        prepend-icon="mdi-certificate-outline"
      >
        Categorias
      </v-list-item>
    </v-list>
  </v-navigation-drawer>

  <v-app-bar flat class="border-b mb-5">
    <v-app-bar-nav-icon @click="isDrawerOpen = !isDrawerOpen" />
    <v-app-bar-title>Suggestion Hub</v-app-bar-title>

    <template #append>
      <v-menu>
        <template #activator="{ props }">
          <v-btn icon class="mr-2" v-bind="props">
            <v-badge color="info" dot>
              <v-icon>mdi-bell-outline</v-icon>
            </v-badge>
          </v-btn>
        </template>
        <!-- Notificações aqui -->
      </v-menu>

      <UserAvatar @logout="handleLogout" />
    </template>
  </v-app-bar>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { logout } from '@/services/auth'
import { useAuthStore } from '@/stores/auth'

const isDrawerOpen = ref(false)
const router = useRouter()
const authStore = useAuthStore()

function handleLogout() {
  logout()
  router.push('/login')
}

// Computed para checar se usuário é admin
const isAdmin = computed(() => authStore.user?.roles.includes('Admin'))
</script>
