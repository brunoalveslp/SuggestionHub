export interface SuggestionEventDTO {
  id: number
  suggestionId: number
  userId: number
  userName: string
  action: string
  changeDescription?: string | null
  changeDate: string // DateTime vindo como string ISO
}
