/**
 * main.ts
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from '@/App.vue'

import AppHeader from '@/components/AppHeader.vue'
import LoginForm from '@/components/auth/LoginForm.vue'
import LoginAside from '@/components/auth/LoginAside.vue'

import BarChart from '@/components/charts/BarChart.vue'
import LineChart from '@/components/charts/LineChart.vue'

import { useAuthStore } from '@/stores/auth'

// Composables
import { createApp } from 'vue'

// Styles
import 'unfonts.css'

const app = createApp(App)

app.component('AppHeader',AppHeader)
app.component('LoginForm',LoginForm)
app.component('LoginAside',LoginAside)
app.component('BarChart',BarChart)
app.component('LineChart',LineChart)

registerPlugins(app)


const authStore = useAuthStore()
authStore.loadFromStorage()

app.mount('#app')
