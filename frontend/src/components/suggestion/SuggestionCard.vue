<template>
  <v-card class="mb-4 pa-4" @mouseenter="preloadDetail">
    <!-- Header -->
    <div class="d-flex justify-space-between align-start mb-2">
      <RouterLink :to="`/suggestion/${suggestion.id}`" class="text-h6 text-decoration-none text-primary">
        <span class="text-grey-darken-1 font-weight-light">#{{ suggestion.id }}</span>
        -
        <span class="font-weight-medium hover:underline">{{ suggestion.title }}</span>
      </RouterLink>

      <div class="d-flex flex-column align-end text-body-2">
        <v-chip :color="statusColor" text-color="white" small class="mb-1">
          {{ statusLabel }}
        </v-chip>
        <span class="text-grey-darken-1">
          Criado em: {{ formatDate(suggestion.createdAt) }}
        </span>
      </div>
    </div>

    <!-- Descrição -->
    <div class="description-clamp text-body-1" v-html="safeDescription"></div>
    <!-- Ações -->
    <v-card-actions class="pa-0">
      <v-row class="w-100 d-flex" align="center" justify="space-between" no-gutters>
        <!-- Botão de subscrição -->
        <v-col cols="auto">
          <v-btn :color="suggestion.hasUserSubscribed ? 'primary' : 'secondary'"
            :variant="suggestion.hasUserSubscribed ? 'elevated' : 'outlined'" @click="dialogVisible = true">
            {{ suggestion.hasUserSubscribed ? 'Subscrito' : 'Subscrever' }}
          </v-btn>
        </v-col>

        <!-- Likes e comentários -->
        <v-col cols="auto">
          <div class="d-flex align-center">
            <v-icon small class="mr-1">mdi-comment</v-icon>
            <span class="mr-4">{{ suggestion.commentCount }}</span>

            <v-icon small class="mr-1">mdi-thumb-up</v-icon>
            <span>{{ suggestion.subscriptionCount }}</span>
          </div>
        </v-col>
      </v-row>
    </v-card-actions>

  </v-card>

  <!-- Dialog de confirmação -->
  <v-dialog v-model="dialogVisible" max-width="400">
    <v-card>
      <v-card-title class="text-h6">
        {{ suggestion.hasUserSubscribed ? 'Remover subscrição?' : 'Confirmar subscrição?' }}
      </v-card-title>
      <v-card-text>
        {{ suggestion.hasUserSubscribed
          ? 'Você deseja remover sua subscrição desta sugestão?'
          : 'Você deseja se subscrever nesta sugestão?' }}
      </v-card-text>
      <v-card-actions class="justify-end">
        <v-btn variant="text" @click="dialogVisible = false">Cancelar</v-btn>
        <v-btn color="primary" variant="flat" @click="confirmSubscribe">Confirmar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { subscribeSuggestion, removeSubscriptionSuggestion, fetchSuggestionById } from '@/services/suggestion'
import type { SuggestionDTO } from '@/types/suggestion/suggestionDTO'
import { SuggestionStatus } from '@/types/suggestion/suggestionStatus'

const props = defineProps<{ suggestion: SuggestionDTO; userId: string }>()
const suggestion = ref({ ...props.suggestion })
const dialogVisible = ref(false)

import DOMPurify from 'dompurify'

const safeDescription = computed(() =>
  DOMPurify.sanitize(props.suggestion.description, {
    FORBID_ATTR: ['style'], // remove style=""
  })
)

const statusLabelMap: Record<keyof typeof SuggestionStatus, string> = {
  Pending: 'Pendente',
  InReview: 'Em análise',
  Approved: 'Aprovada',
  InProgress: 'Em progresso',
  Implemented: 'Implementada',
  Rejected: 'Rejeitada',
}

const statusColorMap: Record<keyof typeof SuggestionStatus, string> = {
  Pending: 'grey',
  InReview: 'blue',
  Approved: 'green',
  InProgress: 'orange',
  Implemented: 'teal',
  Rejected: 'red',
}

const statusKey = computed(() => {
  const entry = Object.entries(SuggestionStatus).find(
    ([, value]) => value === suggestion.value.status
  )
  return entry?.[0] as keyof typeof SuggestionStatus
})

const statusLabel = computed(() => statusLabelMap[statusKey.value] || 'Desconhecido')
const statusColor = computed(() => statusColorMap[statusKey.value] || 'grey')

function formatDate(dateStr: string): string {
  const date = new Date(dateStr)
  return date.toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}

async function confirmSubscribe() {
  try {
    if (suggestion.value.hasUserSubscribed) {
      await removeSubscriptionSuggestion(suggestion.value.id, props.userId)
      suggestion.value.hasUserSubscribed = false
      suggestion.value.subscriptionCount = Math.max(0, suggestion.value.subscriptionCount - 1)
    } else {
      await subscribeSuggestion(suggestion.value.id, props.userId)
      suggestion.value.hasUserSubscribed = true
      suggestion.value.subscriptionCount++
    }
  } catch (err) {
    console.error('Erro ao alternar subscrição:', err)
  } finally {
    dialogVisible.value = false
  }
}

// Pré-carregamento e salvar no localStorage para evitar proxies no router
async function preloadDetail() {
  try {
    const result = await fetchSuggestionById(suggestion.value.id)
    // Salva no localStorage com chave única para evitar conflito
    localStorage.setItem(`suggestion_${result.id}`, JSON.stringify(result))
  } catch (e) {
    console.error('Erro no preload:', e)
  }
}
</script>

<style scoped>
.description-clamp {
  max-height: 120px;
  /* Ajuste conforme a altura desejada */
  overflow: hidden;
  /* Esconde conteúdo que passa da altura */
  position: relative;
}
</style>
