<template lang="">
  <div class="m-combobox" v-click-outside="clickAway" :class="classList">
    <input
      type="text"
      v-model="searchText"
      :placeholder="placeholder"
      class="m-combobox-input"
      :class="classList"
      :tabindex="tabindex"
      ref="input"
      @input="isShow = true"
    />
    <div
      class="m-combobox-btn icon-angle-down"
      @click="toggleCombobox"
      ref="comboboxBtn"
    ></div>
    <div v-if="isShow" class="m-combobox-select" :class="direction">
      <div
        v-for="option in filterData"
        :value="option.id"
        :key="option.id"
        @click="handleChooseItem(option.id)"
        class="m-combobox-item"
        :class="{ 'm-item-selected': option.id === value }"
      >
        {{ option.label }}
      </div>
    </div>
  </div>
</template>
<script>
import Vue from 'vue';

// ẩn select khi click ra ngoài combobox
Vue.directive('click-outside', {
  bind(el, binding, vnode) {
    el.clickOutsideEvent = (event) => {
      if (!(el === event.target || el.contains(event.target))) {
        vnode.context[binding.expression](event);
      }
    };
    document.body.addEventListener('click', el.clickOutsideEvent);
  },
  unbind(el) {
    document.body.removeEventListener('click', el.clickOutsideEvent);
  },
});

export default {
  name: 'base-combobox',
  props: [
    'options',
    'value',
    'placeholder',
    'classList',
    'tabindex',
    'direction',
    'filter',
  ],
  data() {
    return {
      searchText: '',
      isShow: false,
    };
  },
  methods: {
    /*
    Hàm ẩn hiến select của component
    Author: NVThang (09/11/2021)
    */
    toggleCombobox() {
      this.isShow = !this.isShow;
      if (this.isShow) {
        this.$refs.input.focus();
      }
    },
    /*
    Hàm chọn id của rồi bind ra bên ngoài
    Author: NVThang (09/11/2021)
    */
    handleChooseItem(id) {
      this.$emit('input', id);
      this.isShow = false;
    },
    /*
    Hàm ẩn select khi click ra ngoài
    Author: NVThang (09/11/2021)
    */
    clickAway() {
      this.isShow = false;
    },
    /*
    Hàm ẩn hiến select của component
    Author: NVThang (09/11/2021)
    */
    showOptions() {
      this.isShow = true;
    },
  },
  computed: {
    // lọc các giá trị phù hợp trong select khi đang type
    filterData() {
      if (this.filter) {
        return this.options.filter(
          (option) =>
            !this.searchText ||
            option.label.toLowerCase().includes(this.searchText.toLowerCase()),
        );
      } else {
        return this.options;
      }
    },
  },
  created() {
    if (this.value || this.value === 0) {
      this.searchText = this.options.filter(
        (option) => option.id === this.value,
      )[0].label;
    }
  },
  watch: {
    value: function () {
      // kiểm tra giá trị value để cập nhật giá trị cho combobox
      if (this.value !== null && this.options && this.options.length > 0) {
        this.searchText = this.options.filter(
          (option) => option.id === this.value,
        )[0].label;
      }
    },
  },
};
</script>

<style>
@import url('../../style/component/combobox.css');
</style>
