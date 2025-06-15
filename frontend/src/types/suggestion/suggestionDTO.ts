import { SuggestionStatus } from './suggestionStatus'

export interface SuggestionDTO {
  id: number
  title: string
  description: string
  status: SuggestionStatus
  categoryId: number
  createdAt: string // ou `Date`, mas geralmente string ao vir da API
  commentCount: number
  likeCount: number
  hasUserLiked: boolean
}
