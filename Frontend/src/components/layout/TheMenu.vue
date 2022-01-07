<template lang="">
  <div class="menu">
    <div class="menu-logo">
      <div class="logo-site">
        <div class="logo-site-icon icon-logo"></div>
        <span class="logo-site-text">Thiết bị</span>
      </div>
      <div class="icon-toggle toggle-btn"></div>
    </div>
    <div class="menu-content">
      <router-link
        class="menu-item"
        v-for="(menuItem, index) in menuItems"
        :to="menuItem.link"
        :key="index"
        :class="menuItem.isActive && 'router-link-active'"
        @click="selectItem(index)"
      >
        <div class="menu-item-icon" :class="menuItem.iconClass"></div>
        <div class="menu-item-text">{{ menuItem.title }}</div>
        <!-- <div class="submenu" v-if="menuItem.submenu">
          <p class="submenu-title">{{ menuItem.title }}</p>
          <router-link
            class="submenu-item"
            v-for="(subItem, subIndex) in menuItem.submenu"
            :to="subItem.link"
            :key="subIndex"
          >
            <p class="submenu-text">{{ subItem.title }}</p>
          </router-link>
        </div> -->
      </router-link>
    </div>
  </div>
</template>
<script>
import Resource from '../../script/base/resource';
export default {
  data() {
    return {
      menuItems: Resource.menuItems,
    };
  },
  methods: {
    selectItem: function (index) {
      this.menuItems.forEach((menuItem) => {
        menuItem.isActive = false;
      });
      this.menuItems[index].isActive = true;
    },
  },
};
</script>
<style scoped>
@import url('../../style/layout/menu.css');
</style>
