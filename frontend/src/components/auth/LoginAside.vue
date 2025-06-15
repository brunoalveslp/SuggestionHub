<template>
  <div class="login-aside d-flex flex-column align-start justify-center">
    <h2 class="mb-2 welcome">Bem vindo ao</h2>
    <h1 class="mb-4 welcome">Portal do Hiperador!</h1>
    <h2 class="greeting mb-6">Mais um dia incrível para as suas vendas!</h2>
    <div class="text-h2">:)</div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,300..800;1,300..800&family=Roboto:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&display=swap');
.welcome {
  font-family: 'Open Sans';
  font-size: 36px;
  font-weight: 100;
  line-height: 1.33;
}

.greeting{
  font-family: 'Open Sans';
  font-size: 48px;
  font-weight: 700;
  line-height: 1.33;
}

.login-aside {
  min-height: 100%;
  padding-left: 95px;
  z-index: 0;

  background-image: linear-gradient(44deg, #663399, #663399, #6f2879 41%, #ffc200, #ffc200);
  background-size: 400% 400%;

  animation: Gradient 10s ease infinite;
  -webkit-animation: Gradient 10s ease infinite;
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
