import type { SuggestionStatus } from './suggestionStatus'
import type { SuggestionEventDTO } from './suggestionEventDTO'
import type { CommentDTO } from './commentDTO'

export interface SuggestionDetailsDTO {
  id: number
  title: string
  description: string
  status: SuggestionStatus
  categoryId: number
  userId: string
  createdAt: string // Date como string ISO
  lastUpdatedAt?: string | null // Date opcional (nullable)

  events: SuggestionEventDTO[]
  likeCount: number
  hasUserLiked: boolean
  comments: CommentDTO[]
}
