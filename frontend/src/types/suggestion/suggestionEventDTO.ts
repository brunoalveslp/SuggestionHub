export interface SuggestionEventDTO {
  id: number
  suggestionId: number
  userId: string
  userName: string
  isPublic: boolean
  action: string
  changeDescription?: string | null
  changeDate: string
}
