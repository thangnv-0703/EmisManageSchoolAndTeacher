<template>
  <div class="app">
    <TheMenu />
    <div class="header-content">
      <TheHeader />
      <TheContent />
    </div>
  </div>
</template>

<script>
import TheMenu from './components/layout/TheMenu.vue';
import TheHeader from './components/layout/TheHeader.vue';
import TheContent from './components/layout/TheContent.vue';

export default {
  name: 'App',
  components: {
    TheMenu,
    TheHeader,
    TheContent,
  },
};
</script>

<style>
@import url('./style/main.css');
</style>
