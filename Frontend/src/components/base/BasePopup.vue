<template>
  <div class="m-popup">
    <div class="m-popup-backdrop"></div>
    <div class="m-popup-container">
      <div class="m-popup-header">
        <p class="m-popup-title">Thông báo</p>
        <div class="m-popup-btn-close" @click="onClosePopup"></div>
      </div>
      <div class="m-popup-body">
        <div class="m-popup-text">
          <slot name="content"></slot>
        </div>
      </div>
      <slot name="btn"></slot>
    </div>
  </div>
</template>
<script>
// import BaseButton from './BaseButton.vue';
export default {
  props: ['content', 'footer'],
  methods: {
    onConfirmDelete() {
      // debugger;
      this.$emit('confirmDelete');
      this.onClosePopup();
    },
    onClosePopup() {
      this.$emit('closePopup');
    },
  },
};
</script>
<style>
@import url('../../style/component/popup.css');
</style>
