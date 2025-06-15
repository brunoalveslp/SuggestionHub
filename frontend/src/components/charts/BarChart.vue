<!-- BarChart.vue -->
<template>
  <Bar :data="data" :options="options" />
</template>

<script setup lang="ts">
import { Bar } from 'vue-chartjs'
import {
  Chart as ChartJS,
  Title, Tooltip, Legend,
  BarElement, CategoryScale, LinearScale,
} from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

defineProps<{ data: any }>()

const options = {
  responsive: true,
  plugins: {
    legend: { display: false },
    title: { display: false }
  }
}
</script>
