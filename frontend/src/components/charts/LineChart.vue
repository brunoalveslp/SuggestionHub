<!-- LineChart.vue -->
<template>
  <Line :data="data" :options="options" />
</template>

<script setup lang="ts">
import { Line } from 'vue-chartjs'
import {
  Chart as ChartJS,
  Title, Tooltip, Legend,
  LineElement, PointElement,
  CategoryScale, LinearScale,
} from 'chart.js'

ChartJS.register(Title, Tooltip, Legend, LineElement, PointElement, CategoryScale, LinearScale)

defineProps<{ data: any }>()

const options = {
  responsive: true,
  plugins: {
    legend: { display: false },
    title: { display: false }
  }
}
</script>
