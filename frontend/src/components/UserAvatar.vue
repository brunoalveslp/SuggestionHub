<template>
  <v-menu>
    <template #activator="{ props }">
      <v-btn v-bind="props" text class="d-flex align-center gap-2">
        <span class="d-flex align-center justify-center" v-if="user">{{ user.displayName }}
          <v-avatar size="32" color="primary" class="ml-3">
            <v-icon dark>mdi-account</v-icon>
          </v-avatar>
        </span>
        <v-progress-circular v-else indeterminate size="20" width="2"></v-progress-circular>
      </v-btn>
    </template>

    <v-card min-width="200px">
      <v-list>
        <v-list-item append-icon="mdi-exit-to-app" @click="$emit('logout')">
          Logout
        </v-list-item>
      </v-list>
    </v-card>
  </v-menu>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import type { User } from '../types/user'

const user = ref<User | null>(null)

onMounted(() => {
  const storedUser = localStorage.getItem('user')
  if (storedUser) {
    user.value = JSON.parse(storedUser)
  }
})
</script>
