<template>
  <v-dialog :model-value="modelValue" @update:modelValue="emit('update:modelValue', $event)" max-width="600">
    <v-card class="pa-6">
      <v-card-title class="text-h6">Atualizar Status / Criar Evento</v-card-title>
      <v-card-text>
        <v-checkbox
          v-model="statusOnly"
          label="Atualizar somente status"
        />
        <v-select
          v-model="selectedStatus"
          label="Novo Status"
          :items="statusOptions"
          item-value="value"
          item-title="label"
        />

        <v-select
          v-if="!statusOnly"
          v-model="action"
          label="Ação"
          :items="actionOptions"
          class="mt-4"
        />

        <v-textarea
          v-if="!statusOnly"
          v-model="description"
          label="Descrição da Mudança"
          rows="2"
          auto-grow
          class="mt-2"
        />
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn @click="emit('update:modelValue', false)">Cancelar</v-btn>
        <v-btn color="primary" @click="submitEvent">Salvar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { addSuggestionEvent } from '@/services/suggestion'

const props = defineProps<{
  modelValue: boolean
  suggestionId: number
  currentStatus: number
}>()



const emit = defineEmits(['update:modelValue', 'updated'])

const authStore = useAuthStore()

const selectedStatus = ref(props.currentStatus)
const statusOnly = ref(false)
const action = ref('')
const description = ref('')

const statusOptions = [
  { value: 0, label: 'Pendente' },
  { value: 1, label: 'Em Análise' },
  { value: 2, label: 'Aprovada' },
  { value: 3, label: 'Em Progresso' },
  { value: 4, label: 'Implementada' },
  { value: 5, label: 'Rejeitada' }
]

const statusEnum = {
  0: 'Pending',
  1: 'InReview',
  2: 'Approved',
  3: 'InProgress',
  4: 'Implemented',
  5: 'Rejected'
} as const

const actionOptions = ['Status Alterado', 'Retorno', 'Comentário Interno']

async function submitEvent() {
  await addSuggestionEvent(props.suggestionId, {
    userId: authStore.userId!,
    userName: authStore.displayName,
    action: action.value || 'Status Alterado',
    changeDescription: statusOnly.value ? null : description.value,
    newStatus: statusEnum[selectedStatus.value]
  })

  emit('update:modelValue', false)
  emit('updated')
}
</script>
