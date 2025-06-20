// src/types/suggestion/suggestionEventRequest.ts
export interface SuggestionEventRequest {
  userId: string
  userName: string
  isPublic: boolean
  action: string
  changeDescription?: string | null
  newStatus: string
}
