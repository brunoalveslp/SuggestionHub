<template>
  <v-card class="mt-3 pa-3" variant="outlined">

    <div>
      <!-- Linha principal: nome + botões -->
      <div class="d-flex justify-space-between align-center">
        <div class="font-weight-bold">{{ comment.userName }}</div>

        <div v-if="canEditOrDelete">
          <v-btn class="mr-2" density="compact" icon @click="editMode = true">
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
          <v-btn density="compact" icon @click="deleteComment">
            <v-icon color="red">mdi-delete</v-icon>
          </v-btn>
        </div>
      </div>

      <!-- Linha de baixo: data -->
      <div class="text-caption">{{ new Date(comment.createdAt).toLocaleString() }}</div>
    </div>



    <div v-if="!editMode">{{ comment.content }}</div>

    <div v-else>
      <v-textarea v-model="edited" auto-grow />
      <v-btn color="primary" size="small" @click="saveEdit">Salvar</v-btn>
      <v-btn size="small" @click="editMode = false">Cancelar</v-btn>
    </div>
  </v-card>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { updateComment, removeComment } from '@/services/suggestion'

const props = defineProps<{
  comment: any
  suggestionId: number
  userId: string
  isAdmin: boolean
}>()

const emit = defineEmits(['refresh'])

const editMode = ref(false)
const edited = ref(props.comment.content)

const canEditOrDelete = computed(() => {
  return props.isAdmin || props.comment.userId === props.userId
})

async function saveEdit() {
  await updateComment(props.suggestionId, props.comment.id, {
    userId: props.userId,
    content: edited.value,
  })
  editMode.value = false
  emit('refresh')
}

async function deleteComment() {
  await removeComment(props.suggestionId, props.comment.id, props.userId)
  emit('refresh')
}
</script>
