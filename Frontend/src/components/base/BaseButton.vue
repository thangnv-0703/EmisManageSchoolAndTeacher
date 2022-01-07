<template>
  <button
    :class="[classList, isIcon ? 'm-btn-icon' : 'm-btn']"
    @click="$emit('click', $event.target.value)"
    :style="iconClass ? 'padding-left:16px' : ''"
    :tabindex="tabindex"
  >
    <i v-if="iconClass" :class="iconClass" class="m-btn-icon-16"></i>
    {{ btnText }}
  </button>
</template>
<script>
export default {
  props: ['iconClass', 'btnText', 'classList', 'isIcon', 'tabindex'],
};
</script>
<style></style>
