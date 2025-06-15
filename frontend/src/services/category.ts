// src/services/category.ts
import axios from '@/plugins/axios'
import { useAuthStore } from '@/stores/auth'
import type { CategoryDTO } from '@/types/category/categoryDTO'

function getAuthHeaders() {
  const authStore = useAuthStore()
  const token = authStore.token
  return token ? { Authorization: `Bearer ${token}` } : {}
}

// Buscar todas as categorias
export async function fetchCategories(): Promise<CategoryDTO[]> {
  const response = await axios.get<CategoryDTO[]>('/category', {
    headers: getAuthHeaders(),
  })
  return response.data
}

// Buscar categoria por ID
export async function fetchCategoryById(id: number): Promise<CategoryDTO> {
  const response = await axios.get<CategoryDTO>(`/category/${id}`, {
    headers: getAuthHeaders(),
  })
  return response.data
}

// Criar nova categoria
export async function createCategory(name: string): Promise<void> {
  await axios.post('/category', { name }, {
    headers: getAuthHeaders(),
  })
}

// Atualizar categoria
export async function updateCategory(id: number, name: string): Promise<void> {
  await axios.put(`/category/${id}`, { name }, {
    headers: getAuthHeaders(),
  })
}

// Deletar categoria
export async function deleteCategory(id: number): Promise<void> {
  await axios.delete(`/category/${id}`, {
    headers: getAuthHeaders(),
  })
}
