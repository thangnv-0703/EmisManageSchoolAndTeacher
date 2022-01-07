<template lang="">
  <div :class="'m-toast m-toast-' + type" ref="toast">
    <div class="m-toast-icon" :class="'m-toast-icon-' + type"></div>
    <div class="m-toast-body">
      <div class="m-toast-title">{{ title }}</div>
      <div class="m-toast-message">{{ message }}</div>
    </div>
    <div class="m-toast-close icon-24" @click="removeToast"></div>
  </div>
</template>
<script>
export default {
  props: ['type', 'message', 'title'],
  data() {
    return {
      icon: {
        success: 'fas fa-check',
        error: 'fas fa-exclamation-triangle',
        infor: 'fas fa-info-circle',
        warning: 'fas fa-exclamation',
      },
    };
  },
  methods: {
    /*
    Hàm ẩn toast khi ấn nút X
    Author: NVThang (10/11/2021)
    */
    removeToast() {
      this.$refs.toast.remove();
    },
  },
  // Hiển thị tooltip trong 3s
  mounted() {
    setTimeout(() => this.removeToast(), 5000);
  },
};
</script>
<style lang=""></style>
