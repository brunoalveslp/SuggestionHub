
// Composables
import { createRouter, createWebHistory } from 'vue-router/auto'
import { setupLayouts } from 'virtual:generated-layouts'
import { routes } from 'vue-router/auto-routes'


import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes
})

// Corrige falha de importação dinâmica do Vite
router.onError((err: any, to: any) => {
  if (err?.message?.includes?.('Failed to fetch dynamically imported module')) {
    if (localStorage.getItem('vuetify:dynamic-reload')) {
      console.error('Dynamic import error, reloading page did not fix it', err)
    } else {
      console.log('Reloading page to fix dynamic import error')
      localStorage.setItem('vuetify:dynamic-reload', 'true')
      location.assign(to.fullPath)
    }
  } else {
    console.error(err)
  }
})

router.isReady().then(() => {
  localStorage.removeItem('vuetify:dynamic-reload')
})

// 🔐 Navigation Guard Global
router.beforeEach((to, from) => {
  const auth = useAuthStore()
  auth.loadFromStorage()

  const isPublic = to.path === '/login'
  const isAuth = auth.isLoggedIn

  if (!isPublic && !isAuth) {
    console.log(isPublic+ '- /login')
    console.log(isAuth+ '- /login')
    return '/login'
  }
  if (isPublic && isAuth) {
    console.log(isPublic+ '- /')
    console.log(isAuth+ '- /')
    return '/'
  }

  return true
})

export default router
