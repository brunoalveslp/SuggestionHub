<template>
  <v-dialog v-model="dialogVisible" max-width="500">
    <v-card>
      <v-card-title>Atualizar Status</v-card-title>
      <v-card-text>
        <v-select
          v-model="selectedStatus"
          :items="translatedStatuses"
          item-title="label"
          item-value="value"
          label="Novo Status"
        />
      </v-card-text>
      <v-card-actions>
        <v-btn @click="closeDialog">Cancelar</v-btn>
        <v-spacer />
        <v-btn color="primary" @click="updateStatus">Atualizar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import { updateSuggestionStatus } from '@/services/suggestion'

// Props e emits
const props = defineProps<{
  modelValue: boolean
  suggestionId: number
}>()

const emit = defineEmits(['update:modelValue', 'updated'])

// Estado local
const dialogVisible = ref(props.modelValue)
const selectedStatus = ref('')

// Mapeamento traduzido
const statusLabelMap: Record<string, string> = {
  Pending: 'Pendente',
  InReview: 'Em análise',
  Approved: 'Aprovada',
  InProgress: 'Em progresso',
  Implemented: 'Implementada',
  Rejected: 'Rejeitada',
}

// Status disponíveis para o select
const statuses = Object.keys(statusLabelMap)

const translatedStatuses = computed(() =>
  statuses.map(status => ({
    value: status,
    label: statusLabelMap[status],
  }))
)

// Watch para sincronizar abertura do diálogo
watch(() => props.modelValue, val => {
  dialogVisible.value = val
  if (val) selectedStatus.value = ''
})

watch(dialogVisible, val => emit('update:modelValue', val))

function closeDialog() {
  dialogVisible.value = false
}

async function updateStatus() {
  const auth = JSON.parse(localStorage.getItem('user') || '{}')
  await updateSuggestionStatus(props.suggestionId, {
    newStatus: selectedStatus.value,
    userId: auth.userId,
    userName: auth.displayName,
  })
  emit('updated')
  dialogVisible.value = false
}
</script>
