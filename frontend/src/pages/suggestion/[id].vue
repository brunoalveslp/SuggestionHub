<template>
  <v-container fluid>
    <v-row>
      <v-col cols="12" md="8">
        <template v-if="suggestion">
          <SuggestionDetailsCard
            :suggestion="suggestion"
            :userId="userId"
            :isAdmin="isAdmin"
            @status-updated="loadSuggestion(true)"
          />

          <SuggestionCommentsCard
            :comments="suggestion.comments || []"
            :suggestionId="suggestion.id"
            :userId="userId"
            :isAdmin="isAdmin"
            @refresh="loadSuggestion(true)"
          />
        </template>

        <template v-else>
          <v-skeleton-loader type="card" class="mb-4" />
          <v-skeleton-loader type="list-item-two-line" v-for="n in 3" :key="n" />
        </template>
      </v-col>

      <v-col cols="12" md="4">
        <template v-if="suggestion && isAdmin">
          <SuggestionEventsCard :events="suggestion.events || []" />
        </template>
        <template v-else>
          <v-skeleton-loader type="card" />
        </template>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { fetchSuggestionById } from '@/services/suggestion'
import SuggestionDetailsCard from '@/components/suggestion/SuggestionDetailsCard.vue'
import SuggestionEventsCard from '@/components/suggestion/SuggestionEventsCard.vue'
import SuggestionCommentsCard from '@/components/suggestion/SuggestionCommentsCard.vue'
import type { SuggestionDetailsDTO } from '@/types/suggestion/suggestionDetailsDTO'

const route = useRoute()
const authStore = useAuthStore()
const suggestion = ref<SuggestionDetailsDTO | null>(null)

const isAdmin = authStore.roles.includes('Admin')

const userId = computed(() => authStore.userId ?? '')


async function loadSuggestion(forceRefresh = false) {
  const rawId = route.params.id
  const id = typeof rawId === 'string' ? Number(rawId) : NaN
  if (isNaN(id)) return

  const cacheKey = `suggestion_${id}`

  if (!forceRefresh) {
    const cached = localStorage.getItem(cacheKey)
    if (cached) {
      try {
        const parsed = JSON.parse(cached)
        // Garantir arrays para evitar undefined
        parsed.comments = parsed.comments ?? []
        parsed.events = parsed.events ?? []
        suggestion.value = parsed
        return
      } catch {
        localStorage.removeItem(cacheKey)
      }
    }
  }

  try {
    const fresh = await fetchSuggestionById(id)
    fresh.comments = fresh.comments ?? []
    fresh.events = fresh.events ?? []
    suggestion.value = fresh
    localStorage.setItem(cacheKey, JSON.stringify(fresh))
  } catch (error) {
    console.error('Erro ao carregar sugestão:', error)
    suggestion.value = null
  }
}

onMounted(() => loadSuggestion())
</script>
