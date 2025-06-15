<template>
  <v-container fluid class="fill-height pa-0">
    <v-row no-gutters class="ma-0 fill-height">
      <v-col
        cols="12"
        md="6"
        class="d-flex align-center justify-end position-relative"
      >
        <LoginForm />
      </v-col>
      <v-col
        cols="12"
        md="6"
        class="d-flex align-center justify-center pa-10 aside"
      >
        <LoginAside />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">

</script>

<style scoped>
.aside {
  background-image: linear-gradient(
    44deg,
    #663399,
    #663399,
    #6f2879 41%,
    #ffc200,
    #ffc200
  );
  background-size: 400% 400%;
  animation: Gradient 10s ease infinite;
  color: white;
}

@keyframes Gradient {
  0% {
    background-position: 0% 50%;
  }
  50% {
    background-position: 100% 50%;
  }
  100% {
    background-position: 0% 50%;
  }
}
</style>
