// src/services/suggestion.ts
import axios from '@/plugins/axios'
import { useAuthStore } from '@/stores/auth'

import type { SuggestionDTO } from '../types/suggestion/suggestionDTO'
import type { PaginatedResult } from '@/types/suggestion/paginatedResult'
import type { CommentRequest } from '@/types/suggestion/commentRequest'
import type { SuggestionFilterRequest } from '@/types/suggestion/suggestionFilterRequest'
import type { StatusUpdateRequest } from '@/types/suggestion/statusUpdateRequest'
import type { CreateSuggestionRequest } from '@/types/suggestion/createSuggestionRequest'
import type { SuggestionDetailsDTO } from '@/types/suggestion/suggestionDetailsDTO'
import type { DashboardSummary } from '@/types/suggestion/dashboardSummaryDTO'

function getAuthHeaders() {
  const authStore = useAuthStore()
  const token = authStore.token
  return token ? { Authorization: `Bearer ${token}` } : {}
}

export async function fetchMonthlySummary(month: number, year: number) {
  const res = await axios.get<DashboardSummary>('/suggestion/monthly-summary', {
    params: { month, year },
    headers: getAuthHeaders(),
  })
  return res.data
}

export async function fetchSuggestions(filter: SuggestionFilterRequest, currentUserId: string) {
  const res = await axios.get<PaginatedResult<SuggestionDTO>>('/suggestion', {
    params: { ...filter, currentUserId },
    headers: getAuthHeaders(),
  })
  return res.data
}

export async function fetchSuggestionById(id: number) {

  const res = await axios.get<SuggestionDetailsDTO>(`/suggestion/${id}`, {
    headers: getAuthHeaders(),
  })
  return res.data
}

export async function createSuggestion(request: CreateSuggestionRequest) {
  await axios.post('/suggestion', request, {
    headers: getAuthHeaders(),
  })
}

export async function likeSuggestion(suggestionId: number, userId: string) {
  await axios.post(`/suggestion/${suggestionId}/like`, null, {
    params: { userId },
    headers: getAuthHeaders(),
  })
}

export async function removeLikeSuggestion(suggestionId: number, userId: string) {
  await axios.delete(`/suggestion/${suggestionId}/like`, {
    params: { userId },
    headers: getAuthHeaders(),
  })
}

export async function addComment(suggestionId: number, request: CommentRequest) {
  console.log('suggestion ID add comment',suggestionId)
  await axios.post(`/suggestion/${suggestionId}/comments`, request, {
    headers: getAuthHeaders(),
  })
}

export async function updateComment(
  suggestionId: number,
  commentId: number,
  request: CommentRequest
) {
  await axios.put(`/suggestion/${suggestionId}/comments/${commentId}`, request, {
    headers: getAuthHeaders(),
  })
}

export async function removeComment(
  suggestionId: number,
  commentId: number,
  userId: string
) {
  await axios.delete(`/suggestion/${suggestionId}/comments/${commentId}`, {
    headers: {
      ...getAuthHeaders(),
      userId,
    },
  })
}

export async function updateSuggestionStatus(
  suggestionId: number,
  request: StatusUpdateRequest
) {
  await axios.patch(`/suggestion/${suggestionId}/status`, request, {
    headers: getAuthHeaders(),
  })
}
