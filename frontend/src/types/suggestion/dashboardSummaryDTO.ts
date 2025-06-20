import type { SuggestionDTO } from "./suggestionDTO"

export interface DashboardSummary {
  totalSuggestions: number
  pendingCount: number
  totalSubscriptions: number
  topSubscribed: SuggestionDTO[]
  suggestionsByDay: Record<string, number>
}
