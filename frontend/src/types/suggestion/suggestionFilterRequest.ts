export interface SuggestionFilterRequest {
  categoryId?: number
  status?: string | number | undefined
  createdByUserId?: string
  searchTerm?: string

  pageSize: number
  offset: number

  sortBy?: string
  descending: boolean
}
