import { defineStore } from 'pinia'
import axios from '@/plugins/axios'

interface User {
  token: string
  displayName: string
  email: string
  roles: string[]
  userId: string  // se você armazenar o id do usuário também
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null as User | null,
  }),
  getters: {
    isLoggedIn: (state) => !!state.user,
    userId: (state) => state.user?.userId ?? null,
    token: (state) => state.user?.token ?? null,
    roles: (state) => state.user?.roles ? [...state.user.roles] : [],
  },
  actions: {
    async login(email: string, password: string) {
      const response = await axios.post<User>(`/auth/login`, { email, password })
      this.user = response.data
      console.log(this.user.roles)
      console.log(response.data)
      // Salvar no localStorage para persistência, se desejar
      localStorage.setItem('user', JSON.stringify(this.user))
    },
    logout() {
      this.user = null
      localStorage.removeItem('user')
    },
    loadFromStorage() {
      const user = localStorage.getItem('user')
      if (user) this.user = JSON.parse(user)
    },
  },
})
