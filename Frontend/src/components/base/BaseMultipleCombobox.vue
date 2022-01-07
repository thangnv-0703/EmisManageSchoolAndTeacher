<template lang="">
  <div class="m-combobox" v-click-outside="clickAway" :class="classList">
    <div class="m-combobox-tags">
      <div class="m-tag" v-if="itemList.length > 0">
        <span class="m-tag-name">{{ itemList[0].label }}</span>
        <span class="m-tag-x" @click="handleRemoveTag(itemList[0].id)"></span>
      </div>
    </div>
    <input
      type="text"
      :placeholder="placeholder"
      :style="itemList.length > 0 ? 'width:40px' : ''"
      :tabindex="tabindex"
      @input="isShow = true"
      v-model="searchText"
      ref="input"
      class="m-combobox-input"
    />
    <div class="m-combobox-bagde-container" v-if="itemList.length > 1">
      <div class="m-combobox-dot">...</div>
      <div class="m-combobox-badge">{{ itemList.length }}</div>
    </div>
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
        :class="{ 'm-item-selected': value.indexOf(option.id) > -1 }"
      >
        <BaseCheckbox
          :value="option.id"
          v-model="idList"
          :label="option.label"
        />
      </div>
    </div>
  </div>
</template>
<script>
import BaseCheckbox from './BaseCheckbox.vue';
import Vue from 'vue';

// ẩn select khi click ra ngoài combobox
Vue.directive('click-outside', {
  bind(el, binding, vnode) {
    el.clickOutsideEvent = (event) => {
      if (!(el === event.target || el.contains(event.target))) {
        // debugger;
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
  components: { BaseCheckbox },
  // props: ['options', 'value', 'tabindex', 'classList', 'placeholder'],
  props: {
    value: { type: Array },
    options: { type: Array },
    placeholder: { type: String },
    classList: { type: String },
    tabindex: { type: String },
    direction: { type: String },
    filter: { type: Boolean },
  },
  data() {
    return {
      idList: [],
      isShow: false,
      searchText: '',
    };
  },
  methods: {
    /*
    Hàm xóa tag đã chọn
    Author: NVThang (24/11/2021)
    */
    handleRemoveTag(id) {
      this.idList.splice(this.idList.indexOf(id), 1);
    },
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
      let newValue = [...this.idList];
      if (newValue.includes(id)) {
        newValue.splice(newValue.indexOf(id), 1);
      } else {
        newValue.push(id);
      }
      this.searchText = '';
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
    /*
    Lấy dánh sách những item đã chọn
    Author: NVThang (24/11/2021)
    */
    itemList() {
      return this.options.filter((option) => this.idList.includes(option.id));
    },
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

  watch: {
    /*
    Kiểm tra điều kiện trước khi emit sự kiện cập nhật value
    Author: NVThang (24/11/2021)
    */
    idList: function () {
      if (this.idList.length === this.value.length) {
        for (var i = 0; i < this.idList.length; i++) {
          if (this.idList[i] !== this.value[i]) {
            break;
          }
        }
        return;
      }
      this.$emit('input', this.idList);
    },
    /*
    Kiểm tra điều kiện trước khi cập nhật giá trị idList
    Author: NVThang (24/11/2021)
    */
    value: function () {
      if (this.idList.length === this.value.length) {
        for (var i = 0; i < this.idList.length; i++) {
          if (this.idList[i] !== this.value[i]) {
            break;
          }
        }
        return;
      }
      this.idList = [...this.value];
    },
  },
};
</script>

<style>
@import url('../../style/component/combobox.css');
</style>
