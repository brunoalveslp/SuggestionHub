<template>
  <v-container>
    <!-- Cabeçalho -->
    <v-row class="d-flex align-center justify-space-between">
      <v-col cols="6" class="text-h5 mb-6 align-self-start">Sugestões</v-col>
      <v-col cols="6" class="text-right align-self-start">
        <v-hover v-slot:default="{ isHovering, props }">
          <v-btn v-bind="props" :variant="isHovering ? 'outlined' : 'elevated'" color="primary" prepend-icon="mdi-plus"
            @click="$router.push('/suggestion/create')">
            Nova Sugestão
          </v-btn>
        </v-hover>
      </v-col>
    </v-row>

    <!-- Filtros -->
    <v-card class="pa-4" elevated="4" variant="outlined" color="primary">
      <v-row>
        <v-col cols="12" md="8">
          <v-text-field v-model="searchTerm" color="primary" append-inner-icon="mdi-magnify" density="compact"
            variant="outlined" label="Buscar por título ou conteúdo" clearable />
        </v-col>

        <v-col cols="6" md="2">
          <v-select v-model="selectedCategory" :items="categories" item-title="name" item-value="id" color="primary"
            density="compact" variant="outlined" label="Categoria" clearable />
        </v-col>

        <v-col cols="6" md="2">
          <v-select v-model="selectedStatus" :items="statusOptions" item-title="label" item-value="value"
            color="primary" density="compact" variant="outlined" label="Status" clearable />
        </v-col>
      </v-row>

      <v-row class="mt-2">
        <v-col cols="6" md="4">
          <v-select v-model="sortBy" :items="sortOptions" item-title="title" item-value="value" color="primary"
            density="compact" variant="outlined" label="Ordenar por" clearable />
        </v-col>

        <v-col cols="6" md="4">
          <v-select v-model="descending" :items="orderOptions" item-title="label" item-value="value" color="primary"
            density="compact" variant="outlined" label="Ordem" />
        </v-col>

        <v-col cols="12" md="4" class="text-right">
          <v-hover v-slot:default="{ isHovering, props }">
            <v-btn v-bind="props" :variant="isHovering ? 'elevated' : 'outlined'" color="primary" @click="applyFilters"
              style="height: 40px; min-height: 40px;">
              Filtrar
            </v-btn>
          </v-hover>
        </v-col>
      </v-row>
    </v-card>

    <!-- Lista de sugestões -->
    <!-- Lista de sugestões -->
    <v-card class="pa-4 mt-3" elevated="4" variant="outlined" style="max-height: 600px; overflow-y: auto;">
      <v-data-table :headers="headers" :items="suggestions" :items-per-page="-1" item-value="id" show-expand class="elevation-1"
        :expanded.sync="expanded" :hide-default-footer="true">

        <!-- Botão expandir na primeira coluna -->
        <!-- @ts-ignore -->
        <template #data-table-expand="{ item, expand, isExpanded }">
          <td @mouseenter="preloadDetail(item.id)">
            <v-btn icon @click="expand(item)">
              <v-icon>
                {{ isExpanded(item) ? 'mdi-chevron-up' : 'mdi-chevron-down' }}
              </v-icon>
            </v-btn>
          </td>
        </template>

        <!-- Título com preload ao mouseenter -->
        <template #item.title="{ item }">
          <td @mouseenter="preloadDetail(item.id)">
            <RouterLink :to="`/suggestion/${item.id}`" class="text-primary text-decoration-none font-weight-medium"
              @click.prevent="goToSuggestionDetail(item.id)">
              <span class="text-grey-darken-1 font-weight-light">#{{ item.id }}</span> -
              <span class="hover:underline">{{ item.title }}</span>
            </RouterLink>
          </td>
        </template>

        <!-- Status com preload -->
        <template #item.status="{ item }">
          <td @mouseenter="preloadDetail(item.id)">
            <v-chip :color="getStatusColor(item.status)" text-color="white" small>
              {{ getStatusLabel(item.status) }}
            </v-chip>
          </td>
        </template>

        <!-- Categoria com preload -->
        <template #item.categoryId="{ item }">
          <td @mouseenter="preloadDetail(item.id)">
            {{ getCategoryName(item.categoryId) }}
          </td>
        </template>

        <!-- Data com preload -->
        <template #item.createdAt="{ item }">
          <td @mouseenter="preloadDetail(item.id)">
            {{ formatDate(item.createdAt) }}
          </td>
        </template>

        <!-- Linha expandida -->
        <template #expanded-row="{ item }">
          <tr>
            <td :colspan="headers.length">
              <v-card class="mt-4 pa-4" flat width="100%">
                <div>
                  <v-card-subtitle class="ml-0 pl-0">Assunto</v-card-subtitle>
                  <div class="description-clamp text-body-1 pa-1">{{ item.subject }}</div>
                </div>
                <div>
                  <v-card-subtitle class="ml-0 pl-0">Descrição</v-card-subtitle>
                  <div class="description-clamp text-body-1 pa-1" v-html="sanitizeDescription(item.description)"></div>
                </div>

                <v-card-actions class="d-flex justify-space-between px-0 mt-2" width="100%">
                  <div class="d-flex align-center">
                    <v-icon small class="mr-1">mdi-comment</v-icon>
                    <span class="mr-4">{{ item.commentCount }}</span>

                    <v-icon small class="mr-1">mdi-thumb-up</v-icon>
                    <span>{{ item.subscriptionCount }}</span>
                  </div>

                  <v-hover v-slot:default="{ isHovering, props }">
                    <v-btn v-bind="props" :color="item.hasUserSubscribed ? (isHovering ? 'error' : 'secondary') : 'primary'"
                      :variant="isHovering ? 'elevated' : 'outlined'"
                      @click="openDialogForSubscription(item, !item.hasUserSubscribed)">
                      <template v-if="item.hasUserSubscribed">
                        {{ isHovering ? 'Cancelar inscrição' : 'Subscrito' }}
                      </template>
                      <template v-else>
                        {{ isHovering ? 'Clique para subscrever' : 'Subscrever' }}
                      </template>
                    </v-btn>
                  </v-hover>
                </v-card-actions>
              </v-card>
            </td>
          </tr>
        </template>
      </v-data-table>

      <div ref="loadMoreTrigger" style="height: 1px;"></div>
    </v-card>

    <!-- Diálogo confirmação -->
    <v-dialog v-model="dialogVisible" max-width="400">
      <v-card>
        <v-card-title class="text-h6">
          {{ dialogActionSubscribe ? 'Confirmar subscrição?' : 'Remover subscrição?' }}
        </v-card-title>
        <v-card-text>
          {{ dialogActionSubscribe
            ? 'Você deseja se subscrever nesta sugestão?'
            : 'Você deseja remover sua subscrição desta sugestão?' }}
        </v-card-text>
        <v-card-actions class="justify-end">
          <v-btn variant="text" @click="dialogVisible = false">Cancelar</v-btn>
          <v-btn color="primary" variant="flat" @click="confirmSubscribe">Confirmar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useAuthStore } from '@/stores/auth'
import {
  fetchSuggestions,
  subscribeSuggestion,
  removeSubscriptionSuggestion,
  fetchSuggestionById
} from '@/services/suggestion'
import { fetchCategories } from '@/services/category'
import type { SuggestionDTO } from '@/types/suggestion/suggestionDTO'
import type { SuggestionFilterRequest } from '@/types/suggestion/suggestionFilterRequest'
import type { CategoryDTO } from '@/types/category/categoryDTO'
import { SuggestionStatus } from '@/types/suggestion/suggestionStatus'
import DOMPurify from 'dompurify'
import { isTemplateExpression } from 'typescript'

const router = useRouter();

const authStore = useAuthStore()
const currentUserId = authStore.userId ?? ''

const headers = [
  { title: '', key: 'data-table-expand'},
  { title: 'Título', key: 'title' },
  { title: 'Status', key: 'status' },
  { title: 'Categoria', key: 'categoryId' },
  { title: 'Data de criação', key: 'createdAt' },
]

const searchTerm = ref('')
const selectedCategory = ref<number | null>(null)
const selectedStatus = ref<number | null>(null)
const sortBy = ref<'CreatedAt' | 'subscriptionCount' | 'Title'>('CreatedAt')
const descending = ref(true)

const sortOptions = [
  { value: 'CreatedAt', title: 'Data de criação' },
  { value: 'subscriptionCount', title: 'Mais Subscrições' },
  { value: 'Title', title: 'Título' }
]

const orderOptions = [
  { value: true, label: 'Decrescente' },
  { value: false, label: 'Crescente' }
]

const suggestions = ref<SuggestionDTO[]>([])
const categories = ref<CategoryDTO[]>([])
const offset = ref(0)
const pageSize = 5
const finished = ref(false)
const loading = ref(false)
const loadMoreTrigger = ref<HTMLElement | null>(null)
let observer: IntersectionObserver | null = null
const expanded = ref<string[]>([])

const statusOptions = [
  { value: SuggestionStatus.Pending, label: 'Pendente' },
  { value: SuggestionStatus.InReview, label: 'Em análise' },
  { value: SuggestionStatus.Approved, label: 'Aprovada' },
  { value: SuggestionStatus.InProgress, label: 'Em progresso' },
  { value: SuggestionStatus.Implemented, label: 'Implementada' },
  { value: SuggestionStatus.Rejected, label: 'Rejeitada' },
]

// Dialog controls
const dialogVisible = ref(false)
const dialogSuggestion = ref<SuggestionDTO | null>(null)
const dialogActionSubscribe = ref(false)

async function loadSuggestions(reset = false) {
  if (reset) {
    offset.value = 0
    suggestions.value = []
    finished.value = false
  }
  if (finished.value || loading.value) return
  loading.value = true

  const statusString =
    selectedStatus.value !== null && selectedStatus.value !== undefined
      ? SuggestionStatus[selectedStatus.value]
      : undefined

  const filter: SuggestionFilterRequest = {
    offset: offset.value,
    pageSize,
    searchTerm: searchTerm.value,
    categoryId: selectedCategory.value ?? undefined,
    status: statusString,
    createdByUserId: undefined,
    sortBy: sortBy.value,
    descending: descending.value,
  }

  try {
    const res = await fetchSuggestions(filter, currentUserId)

    suggestions.value.push(...res.items)
    offset.value += res.items.length
    finished.value = !res.hasMore
    console.log("carregando mais!")
  } catch (error) {
    console.error('Erro ao carregar sugestões:', error)
  } finally {
    loading.value = false
  }
}

function applyFilters() {
  loadSuggestions(true)
}

function sanitizeDescription(html: string) {
  return DOMPurify.sanitize(html, { FORBID_ATTR: ['style'] })
}

function formatDate(dateStr: string): string {
  const date = new Date(dateStr)
  return date.toLocaleDateString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  })
}

function getStatusColor(status: number) {
  switch (status) {
    case SuggestionStatus.Pending: return 'grey'
    case SuggestionStatus.InReview: return 'blue'
    case SuggestionStatus.Approved: return 'green'
    case SuggestionStatus.InProgress: return 'orange'
    case SuggestionStatus.Implemented: return 'teal'
    case SuggestionStatus.Rejected: return 'red'
    default: return 'grey'
  }
}

function getStatusLabel(statusValue: number) {
  const status = statusOptions.find(s => s.value === statusValue)
  return status ? status.label : 'Desconhecido'
}

function getCategoryName(categoryId: number) {
  const cat = categories.value.find(c => c.id === categoryId)
  return cat ? cat.name : 'Desconhecido'
}

function openDialogForSubscription(suggestion: SuggestionDTO, subscribe: boolean) {
  dialogSuggestion.value = suggestion
  dialogActionSubscribe.value = subscribe
  dialogVisible.value = true
}

async function confirmSubscribe() {
  if (!dialogSuggestion.value) return

  try {
    if (dialogActionSubscribe.value) {
      await subscribeSuggestion(dialogSuggestion.value.id, currentUserId)
      dialogSuggestion.value.hasUserSubscribed = true
      dialogSuggestion.value.subscriptionCount++
    } else {
      await removeSubscriptionSuggestion(dialogSuggestion.value.id, currentUserId)
      dialogSuggestion.value.hasUserSubscribed = false
      dialogSuggestion.value.subscriptionCount = Math.max(0, dialogSuggestion.value.subscriptionCount - 1)
    }
  } catch (err) {
    console.error('Erro ao alternar subscrição:', err)
  } finally {
    dialogVisible.value = false
  }
}

function goToSuggestionDetail(id: number) {
  // Navega para detalhes da sugestão
  // Pode fechar a expansão da linha para UX melhor
  expanded.value = expanded.value.filter(eId => eId !== String(id))
  // Navega para rota detalhada
  window.setTimeout(() => {
    // usar router.push normalmente
    // em script setup, use router:
    router.push(`/suggestion/${id}`)
  }, 100)
}

async function preloadDetail(id: number) {
  try {
    const result = await fetchSuggestionById(id)
    // Salva no localStorage com chave única para evitar conflito
    localStorage.setItem(`suggestion_${result.id}`, JSON.stringify(result))
  } catch (e) {
    console.error('Erro no preload:', e)
  }
}

onMounted(async () => {
  await loadCategories()
  await loadSuggestions()

  observer = new IntersectionObserver(entries => {
    if (entries[0].isIntersecting) loadSuggestions()
  })

  if (loadMoreTrigger.value) observer.observe(loadMoreTrigger.value)
})

watch(loadMoreTrigger, el => {
  if (observer && el) observer.observe(el)
})

async function loadCategories() {
  categories.value = await fetchCategories()
}
</script>

<style scoped>
.description-clamp {
  display: -webkit-box;
  -webkit-box-orient: vertical;
  overflow: hidden;
  white-space: normal;
}
</style>
