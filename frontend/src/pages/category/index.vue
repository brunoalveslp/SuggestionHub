<template>
  <v-container>
    <v-row>
      <v-spacer />
      <v-hover v-slot:default="{isHovering, props}">
        <v-btn v-bind="props" @click="openCreateDialog" color="primary" :variant="isHovering ? 'outlined': 'elevated'">Nova Categoria</v-btn>
      </v-hover>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-data-table v-if="categories.length" :headers="headers" :items="categories" item-key="id" class="elevation-1">
          <template #item.id="{ item }">
            #{{ item.id }}
          </template>
          <template #item.actions="{ item }">
            <v-hover v-slot:default="{isHovering, props}">
              <v-btn v-bind="props" @click="openEditDialog(item)" icon density="compact" class="mr-2" variant="outlined" :color="isHovering? 'primary': 'grey'">
                <v-icon :color="isHovering? 'primary': 'grey'" size="small">mdi-pencil</v-icon>
              </v-btn>
            </v-hover>
            <v-hover v-slot:default="{isHovering, props}">
              <v-btn v-bind="props" @click="handleDeleteCategory(item.id)" icon density="compact" variant="outlined" :color="isHovering? 'error': 'grey'">
                <v-icon :color="isHovering? 'error': 'grey'" size="small">mdi-delete</v-icon>
              </v-btn>
            </v-hover>
          </template>
        </v-data-table>
      </v-col>
    </v-row>
    <v-dialog v-model="dialog" max-width="500px">
      <v-card>
        <v-card-title>{{ isEdit ? 'Editar Categoria' : 'Nova Categoria' }}</v-card-title>
        <v-card-text>
          <v-text-field v-model="form.name" label="Nome da Categoria" outlined dense />
        </v-card-text>
        <v-card-actions>
          <v-hover v-slot:default="{ isHovering, props}">
            <v-btn v-bind="props" @click="dialog = false" color="warning" text
              :variant="isHovering ? 'elevated' : 'outlined'">
              Cancelar
            </v-btn>
          </v-hover>
          <v-hover v-slot:default="{ isHovering, props}">
            <v-btn v-bind="props" @click="saveCategory" color="primary" :variant="isHovering ? 'elevated' : 'outlined'">{{ isEdit ? 'Salvar' : 'Criar' }}</v-btn>
          </v-hover>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { fetchCategories, createCategory, updateCategory, deleteCategory } from '@/services/category'
import type { CategoryDTO } from '@/types/category/categoryDTO'

const categories = ref<CategoryDTO[]>([])
const dialog = ref(false)
const isEdit = ref(false)
const form = ref<{ name: string }>({ name: '' })
const headers = [
  { title: 'ID', key: 'id', align: 'start' as const },
  { title: 'Nome', key: 'name', align: 'start' as const },
  { title: 'Ações', key: 'actions', sortable: false, align: 'end' as const }
] as const


onMounted(async () => {
  categories.value = await fetchCategories()
})

const openCreateDialog = () => {
  form.value.name = ''
  isEdit.value = false
  dialog.value = true
}

const openEditDialog = (category: CategoryDTO) => {
  form.value.name = category.name
  isEdit.value = true
  dialog.value = true
}

const saveCategory = async () => {
  if (isEdit.value) {
    await updateCategory(categories.value[0].id, form.value.name)
  } else {
    await createCategory(form.value.name)
  }
  categories.value = await fetchCategories()
  dialog.value = false
}

const handleDeleteCategory = async (id: number) => {
  await deleteCategory(id)
  categories.value = await fetchCategories()
}
</script>

<style scoped>
.v-data-table {
  margin-top: 20px;
}
</style>
