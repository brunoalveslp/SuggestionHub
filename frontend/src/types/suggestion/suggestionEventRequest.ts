// src/types/suggestion/suggestionEventRequest.ts
export interface SuggestionEventRequest {
  userId: string
  userName: string
  action: string
  changeDescription?: string | null
  newStatus: number
}
