<template>
  <v-card class="pa-4" elevation="2">
    <div class="text-subtitle-1 mb-2">Eventos da Sugestão</div>
    <v-timeline density="compact" align="start">
      <v-timeline-item
        v-for="event in visibleEvents"
        :key="event.id"
        :title="event.action"
        :subtitle="formatDate(event.changeDate)"
        size="x-small"
      >
        <v-card variant="outlined">
          <div class="d-flex align-center">
            <v-card-title class="py-0 text-subtitle-1">
              <strong>{{ event.action }}</strong>
            </v-card-title>
            <v-card-subtitle>
              Data: {{ formatDate(event.changeDate) }}
            </v-card-subtitle>
          </div>
          <v-card-text v-if="event.changeDescription">
            <strong>Detalhes:</strong> {{ event.changeDescription }}
          </v-card-text>
        </v-card>
      </v-timeline-item>
    </v-timeline>
  </v-card>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import type { SuggestionEventDTO } from '@/types/suggestion/suggestionEventDTO'

const props = defineProps<{ events: SuggestionEventDTO[] }>()

const authStore = useAuthStore()

const isAdmin = computed(() => authStore.user?.roles.includes('Admin'))

const visibleEvents = computed(() => {
  return props.events.filter(event => event.isPublic || isAdmin.value)
})

function formatDate(dateStr: string) {
  const date = new Date(dateStr)
  return date.toLocaleString()
}
</script>
