<template>
  <v-card class="pa-4" elevation="2">
    <div class="text-subtitle-1 mb-2">Eventos da Sugestão</div>
    <v-timeline density="compact">
      <v-timeline-item
        v-for="event in events"
        :key="event.id"
        :title="event.status"
        :subtitle="new Date(event.createdAt).toLocaleString()"
      >
        <div>{{ event.userName }}</div>
      </v-timeline-item>
    </v-timeline>
  </v-card>
</template>

<script setup lang="ts">
defineProps<{ events: any[] }>()
</script>
