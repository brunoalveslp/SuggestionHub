<!-- src/components/auth/LoginForm.vue -->
<template>
  <v-card width="500" class="pa-12 text-center form-wrapper elevated-form">
    <v-img src="../../assets/logo-hiperador.svg" max-height="30" class="mb-6" contain />
    <div class="text-subtitle-1 font-weight-bold mb-2">Entre com seu e-mail e senha</div>

    <v-text-field
      density="compact"
      v-model="email"
      label="E-mail"
      prepend-inner-icon="mdi-account"
      type="email"
      variant="outlined"
    />

    <v-text-field
      density="compact"
      v-model="password"
      :type="showPassword ? 'text' : 'password'"
      label="Senha"
      prepend-inner-icon="mdi-lock"
      :append-inner-icon="showPassword ? 'mdi-eye-off' : 'mdi-eye'"
      @click:append-inner="showPassword = !showPassword"
      variant="outlined"
    />

    <div class="text-caption">
      <a href="#" class="text-primary">Esqueci minha senha</a>
    </div>

    <v-checkbox
      v-model="remember"
      label="Lembrar-me nesse computador"
      density="compact"
    />

    <v-btn
      :loading="loading"
      color="primary"
      class="text-white"
      block
      append-icon="mdi-chevron-right"
      @click="handleLogin"
    >
      Entrar
    </v-btn>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { login } from '@/services/auth'
import { useAuthStore } from '@/stores/auth'

const email = ref('')
const password = ref('')
const remember = ref(false)
const showPassword = ref(false)
const loading = ref(false)

const router = useRouter()
const auth = useAuthStore()

const handleLogin = async () => {
  loading.value = true
  try {
    await login(email.value, password.value)

    // Aqui os dados já estão salvos na store e localStorage (se configurado)
    console.log('Usuário autenticado:', auth.user)

    router.push('/')
  } catch (error: any) {
    console.error('Erro ao fazer login:', error)
    alert(
      error?.response?.data || 'Não foi possível fazer login. Verifique suas credenciais.'
    )
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.elevated-form {
  position: absolute;
  top: 50%;
  transform: translateY(-60%);
  z-index: 10;
  width: 100%;
  max-width: 480px;
  background: white;
  padding: 48px;
  box-shadow: 20px 35px 60px rgba(0, 0, 0, 0.7);
}

.form-wrapper {
  letter-spacing: 0.7px;
  margin-right: -2.2rem;
}
</style>
