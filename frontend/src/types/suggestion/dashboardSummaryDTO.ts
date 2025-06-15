import type { SuggestionDTO } from "./suggestionDTO"

export interface DashboardSummary {
  totalSuggestions: number
  pendingSuggestions: number
  totalLikes: number
  topLikedSuggestions: SuggestionDTO[]
  suggestionsByDay: Record<string, number>
}
