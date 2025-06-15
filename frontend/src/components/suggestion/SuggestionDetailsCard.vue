<template>
  <v-card class="pa-5 mb-4" elevation="4">
    <div class="d-flex justify-space-between align-center mb-4">
      <div>
        <div class="text-h6">#{{ suggestion.id }} - {{ suggestion.title }}</div>
        <div class="text-caption text-grey">
          Criado em {{ new Date(suggestion.createdAt).toLocaleString() }}
        </div>
      </div>
      <div class="d-flex justify-center align-center">
        <v-btn v-if="isAdmin" color="primary" @click="dialog = true">
          Alterar Status / Gerar Evento
        </v-btn>

        <v-chip :color="statusColor" class="text-white ml-4">
          {{ statusLabel }}
        </v-chip>
      </div>
    </div>

    <!-- Render com imagens embutidas -->
    <div class="text-body-1" v-html="descriptionWithImages"></div>

    <SuggestionEventDialog v-if="dialog" :model-value="dialog" @update:modelValue="dialog = $event"
      :suggestionId="suggestion.id" :currentStatus="suggestion.status" @updated="$emit('status-updated')" />

    <div class="d-flex align-center justify-center mb-3">
      <v-spacer></v-spacer>
      <v-btn :color="suggestion.hasUserLiked ? 'primary' : 'secondary'" @click="dialogVisible = true"
        :variant="suggestion.hasUserLiked ? 'elevated' : 'outlined'">
        {{ suggestion.hasUserLiked ? 'Subscrito' : 'Subscrever' }}
      </v-btn>
      <v-icon small class="ml-1">mdi-thumb-up</v-icon>
      <span class="ml-1">{{ suggestion.likeCount }}</span>
    </div>
  </v-card>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import SuggestionEventDialog from './SuggestionEventDialog.vue'
import { likeSuggestion, removeLikeSuggestion } from '@/services/suggestion'
import type { SuggestionDetailsDTO } from '@/types/suggestion/suggestionDetailsDTO'

const props = defineProps<{
  suggestion: SuggestionDetailsDTO
  userId: string
  isAdmin: boolean
}>()

const emit = defineEmits(['status-updated'])
const dialog = ref(false)

// 🔁 Substitui links por imagens, convertendo também links de álbuns Imgur para links diretos
function convertImageLinksToImgTags(html: string): string {
  // Primeiro converte álbuns Imgur para links diretos (jpg)
  html = html.replace(/https:\/\/imgur\.com\/a\/([a-zA-Z0-9]+)/g, (_, id) => {
    return `https://i.imgur.com/${id}.jpg`
  })

  // Regex para detectar links diretos de imagens nos domínios suportados
  const imageRegex = /https:\/\/(?:prnt\.sc|i\.imgur\.com|cdn\.discordapp\.com|media\.tenor\.com)\/[^\s<>"']+/g

  return html.replace(imageRegex, (url) => {
    return `<img src="${url}" alt="Imagem" style="max-width: 100%; border-radius: 8px; margin: 0.5rem 0;" />`
  })
}

// 💡 Conteúdo com imagens embutidas
const descriptionWithImages = computed(() => {
  return convertImageLinksToImgTags(props.suggestion.description)
})

// Curtir / descurtir
function toggleLike() {
  if (props.suggestion.hasUserLiked) {
    removeLikeSuggestion(props.suggestion.id, props.userId)
    props.suggestion.likeCount--
  } else {
    likeSuggestion(props.suggestion.id, props.userId)
    props.suggestion.likeCount++
  }
  props.suggestion.hasUserLiked = !props.suggestion.hasUserLiked
}

// Status visual
const statusLabelMap: Record<number, string> = {
  0: 'Pendente',
  1: 'Em Análise',
  2: 'Aprovada',
  3: 'Em Progresso',
  4: 'Implementada',
  5: 'Rejeitada',
}

const statusColorMap: Record<number, string> = {
  0: 'grey',
  1: 'blue',
  2: 'green',
  3: 'orange',
  4: 'teal',
  5: 'red',
}

const statusLabel = computed(() => statusLabelMap[props.suggestion.status] ?? 'Desconhecido')
const statusColor = computed(() => statusColorMap[props.suggestion.status] ?? 'blue-grey')
</script>
