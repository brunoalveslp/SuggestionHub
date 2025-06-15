// src/services/auth.ts
import { useAuthStore } from '@/stores/auth'

export async function login(email: string, password: string) {
  const authStore = useAuthStore()
  await authStore.login(email, password)
}

export function logout() {
  const authStore = useAuthStore()
  authStore.logout()
}
