<template lang="">
  <div class="header">
    <ul class="header-left">
      <li class="header-title">
        <span>Cán bộ, giáo viên</span>
      </li>
      <li class="header-help">
        <div class="icon-help header-icon"></div>
      </li>
    </ul>
    <ul class="header-right">
      <li class="header-item">
        <div class="header-icon-notification icon-notification"></div>
      </li>
      <li class="header-item"><div class="header-seperate"></div></li>
      <li class="header-item">
        <div class="header-user-name">Admin</div>
      </li>
      <li class="header-item">
        <div class="header-icon-angle icon-angle-down"></div>
        <div class="header-user-avatar icon-avatar"></div>
      </li>
    </ul>
  </div>
</template>
<script>
export default {
  data() {
    return {
      schoolYear: [{ name: '2020-2021', id: '1' }],
      selectedSchoolYear: 1,
    };
  },
};
</script>
<style scoped>
@import url('../../style/layout/header.css');
</style>
