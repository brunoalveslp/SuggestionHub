<template>
  <v-container fluid>
    <!-- Filtro mês e ano -->
    <v-row class="d-flex align-center justify-end">
      <v-col cols="12" sm="4" md="3" class="align-self-start pa-0 ma-0">
        <v-select v-model="selectedMonth" :items="months" label="Mês" density="compact" outlined item-title="label"
          item-value="value" />
      </v-col>

      <v-col cols="12" sm="4" md="3" class="align-self-start pa-0 ma-0">
        <v-select v-model="selectedYear" :items="years" label="Ano" density="compact" outlined />
      </v-col>

      <v-col cols="12" sm="4" md="2" class="align-self-start pa-0 ma-0">
        <v-btn color="primary" class="mt-2" @click="loadDashboardData">Aplicar</v-btn>
      </v-col>
    </v-row>

    <!-- Cards de métricas -->
    <v-row>
      <v-col cols="12" md="4">
        <v-card class="pa-4">
          <h3>Total de Sugestões no Mês</h3>
          <p class="text-h4">{{ dashboardSummary?.totalSuggestions ?? 0 }}</p>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4">
          <h3>Sugestões Pendentes/Sem interação</h3>
          <p class="text-h4">{{ dashboardSummary?.pendingCount ?? 0 }}</p>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4">
          <h3>Total de Subscrições no Mês</h3>
          <p class="text-h4">{{ dashboardSummary?.totalSubscriptions ?? 0 }}</p>
        </v-card>
      </v-col>
    </v-row>

    <!-- Gráficos e Listas -->
    <v-row>
      <v-col cols="12" md="6">
        <v-card class="pa-4">
          <h3>Top 10 Mais Votadas</h3>
          <v-list v-if="dashboardSummary?.topSubscribed?.length">
            <v-list-item v-for="suggestion in dashboardSummary.topSubscribed.slice(0, 10)" :key="suggestion.id"
              :to="`/suggestion/${suggestion.id}`" link>
              <v-list-item-title>
                #{{ suggestion.id }} {{ suggestion.title }} ({{ suggestion.subscriptionCount }} subscrições)
              </v-list-item-title>
            </v-list-item>
          </v-list>
          <div v-else>Nenhuma sugestão encontrada.</div>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card class="pa-4">
          <h3>Sugestões por Dia</h3>
          <BarChart v-if="suggestionsByDayData" :data="suggestionsByDayData" />
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { fetchMonthlySummary } from '@/services/suggestion'
import type { DashboardSummary } from '@/types/suggestion/dashboardSummaryDTO'
import type { ChartData } from 'chart.js'
import { useTheme } from 'vuetify'
import { watch } from 'vue'

const theme = useTheme()
const isDark = computed(() => theme.global.name.value === 'dark')

const dashboardSummary = ref<DashboardSummary | null>(null)
const suggestionsByDayData = ref<ChartData<'bar'>>()

const now = new Date()
const selectedMonth = ref(now.getMonth() + 1)
const selectedYear = ref(now.getFullYear())

const months = [
  { label: 'Janeiro', value: 1 },
  { label: 'Fevereiro', value: 2 },
  { label: 'Março', value: 3 },
  { label: 'Abril', value: 4 },
  { label: 'Maio', value: 5 },
  { label: 'Junho', value: 6 },
  { label: 'Julho', value: 7 },
  { label: 'Agosto', value: 8 },
  { label: 'Setembro', value: 9 },
  { label: 'Outubro', value: 10 },
  { label: 'Novembro', value: 11 },
  { label: 'Dezembro', value: 12 },
]

const currentYear = now.getFullYear()
const years = Array.from({ length: 5 }).map((_, i) => currentYear - i)

async function loadDashboardData() {
  try {
    const data = await fetchMonthlySummary(selectedMonth.value, selectedYear.value)
    dashboardSummary.value = data

    updateChart()

    const dayLabels = data.suggestionsByDay.map(item => item.day)
    const dayValues = data.suggestionsByDay.map(item => item.count)

    suggestionsByDayData.value = {
      labels: dayLabels,
      datasets: [
        {
          label: 'Sugestões',
          data: dayValues,
          backgroundColor: isDark.value ? '#90caf9' : '#42a5f5',
        },
      ],
    }
  } catch (error) {
    console.error('Erro ao carregar dados do dashboard:', error)
  }
}

function updateChart() {
  if (!dashboardSummary.value) return

  const dayLabels = dashboardSummary.value.suggestionsByDay.map(item => item.day)
  const dayValues = dashboardSummary.value.suggestionsByDay.map(item => item.count)

  suggestionsByDayData.value = {
    labels: dayLabels,
    datasets: [
      {
        label: 'Sugestões',
        data: dayValues,
        backgroundColor: isDark.value ? '#90caf9' : '#42a5f5',
      },
    ],
  }
}


watch(isDark, () => {
  if (dashboardSummary.value) {
    updateChart()
  }
})

onMounted(() => {
  loadDashboardData()
})
</script>
