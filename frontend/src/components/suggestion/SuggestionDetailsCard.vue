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
    <div class="text-subtitle-2">Assunto:</div>
    <div class="text-subtitle-1 border-b mb-2">{{ suggestion.subject }}</div>
    <div class="text-body-1 mb-2">Descrição:</div>
    <div class="text-body-1" v-html="descriptionWithImages"></div>

    <SuggestionEventDialog v-if="dialog" :model-value="dialog" @update:modelValue="dialog = $event"
      :suggestionId="suggestion.id" :currentStatus="suggestion.status" @updated="$emit('status-updated')" />

    <div class="d-flex align-center justify-center mb-3">
      <v-spacer></v-spacer>
      <v-hover v-slot:default="{ isHovering, props }">
        <v-btn v-bind="props" :color="suggestion.hasUserSubscribed ? (isHovering ? 'error' : 'secondary') : 'primary'"
          :variant="isHovering ? 'elevated' : 'outlined'"
          @click="openDialogForSubscription(suggestion, !suggestion.hasUserSubscribed)">
          <template v-if="suggestion.hasUserSubscribed">
            {{ isHovering ? 'Cancelar inscrição' : 'Subscrito' }}
          </template>
          <template v-else>
            {{ isHovering ? 'Clique para subscrever' : 'Subscrever' }}
          </template>
        </v-btn>
      </v-hover>
    </div>

    <!-- Diálogo confirmação -->
    <v-dialog v-model="dialogVisible" max-width="400">
      <v-card>
        <v-card-title class="text-h6">
          {{ dialogActionSubscribe ? 'Confirmar subscrição?' : 'Remover subscrição?' }}
        </v-card-title>
        <v-card-text>
          {{ dialogActionSubscribe
            ? 'Você deseja se subscrever nesta sugestão?'
            : 'Você deseja remover sua subscrição desta sugestão?' }}
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="dialogVisible = false">Cancelar</v-btn>
          <v-btn color="primary" variant="flat" @click="confirmSubscribe">Confirmar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-card>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import SuggestionEventDialog from './SuggestionEventDialog.vue'
import { subscribeSuggestion, removeSubscriptionSuggestion } from '@/services/suggestion'
import type { SuggestionDetailsDTO } from '@/types/suggestion/suggestionDetailsDTO'
import type { SuggestionDTO } from '@/types/suggestion/suggestionDTO';

const props = defineProps<{
  suggestion: SuggestionDetailsDTO
  userId: string
  isAdmin: boolean
}>()

const emit = defineEmits(['status-updated'])
const dialog = ref(false)
// Dialog controls
const dialogVisible = ref(false)
const dialogSuggestion = ref<SuggestionDetailsDTO | null>(null)
const dialogActionSubscribe = ref(false)

// 🔁 Substitui links por imagens, convertendo também links de álbuns Imgur para links diretos
function convertImageLinksToImgTags(html: string): string {
  // Converte álbuns do Imgur para links diretos
  html = html.replace(/https:\/\/imgur\.com\/a\/([a-zA-Z0-9]+)/g, (_, id) => {
    return `https://i.imgur.com/${id}.jpg`
  })

  // Regex para detectar links diretos de imagem que NÃO estão dentro de <img ...>
  const imageRegex = /(?<!["'=])\bhttps:\/\/(?:prnt\.sc|i\.imgur\.com|cdn\.discordapp\.com|media\.tenor\.com)\/[^\s<>"']+/g

  return html.replace(imageRegex, (url) => {
    return `<img src="${url}" alt="Imagem" style="max-width: 100%; border-radius: 8px; margin: 0.5rem 0;" />`
  })
}


// 💡 Conteúdo com imagens embutidas
const descriptionWithImages = computed(() => {
  return convertImageLinksToImgTags(props.suggestion.description)
})

function openDialogForSubscription(suggestion: SuggestionDetailsDTO, subscribe: boolean) {
  dialogSuggestion.value = suggestion
  dialogActionSubscribe.value = subscribe
  dialogVisible.value = true
}

async function confirmSubscribe() {
  if (!dialogSuggestion.value) return

  try {
    if (dialogActionSubscribe.value) {
      await subscribeSuggestion(dialogSuggestion.value.id, props.userId)
      dialogSuggestion.value.hasUserSubscribed = true
      dialogSuggestion.value.subscriptionCount++
    } else {
      await removeSubscriptionSuggestion(dialogSuggestion.value.id, props.userId)
      dialogSuggestion.value.hasUserSubscribed = false
      dialogSuggestion.value.subscriptionCount = Math.max(0, dialogSuggestion.value.subscriptionCount - 1)
    }
  } catch (err) {
    console.error('Erro ao alternar subscrição:', err)
  } finally {
    dialogVisible.value = false
  }
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
