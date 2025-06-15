<template>
  <v-card class="pa-4" elevation="2">
    <div>
      <v-textarea v-model="newComment" label="Adicionar comentário" auto-grow clearable />
      <div class="d-flex">
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="submitComment">Comentar</v-btn>
      </div>
    </div>
    <div class="text-subtitle-1 mb-2">Comentários</div>
    <CommentItem class="mt-4" v-for="c in comments" :key="c.id" :comment="c" :suggestionId="suggestionId"
      :userId="userId" :isAdmin="isAdmin" @refresh="$emit('refresh')" />
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import CommentItem from './CommentItem.vue'
import { addComment } from '@/services/suggestion'

// ✅ Pegando as props corretamente
const props = defineProps<{
  comments: any[]
  suggestionId: number
  userId: string
  isAdmin: boolean
}>()

const emit = defineEmits(['refresh'])

const newComment = ref('')

async function submitComment() {
  if (!newComment.value.trim()) return
  await addComment(props.suggestionId, {
    userId: props.userId,
    content: newComment.value.trim(),
  })
  newComment.value = ''
  emit('refresh')
}
</script>
