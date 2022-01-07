<template>
  <div
    class="m-input-container"
    :class="classList"
    @mouseover="fshowTooltip"
    @mouseleave="hideTooltip"
  >
    <input
      :tabindex="tabindex"
      :required="required"
      :type="type"
      :placeholder="placeholder"
      class="m-input"
      :class="[classList, isError ? 'error' : '']"
      :value="formatData(value)"
      :style="iconClass ? 'padding-right: 32px' : 'padding-left: 12px'"
      @input="onInput"
      @change="onChange"
      @blur="validateValue"
      @focus="$emit('focus', $event.target.value)"
      ref="input"
    />
    <div v-if="iconClass" class="m-input-icon" :class="iconClass"></div>
    <div v-if="showTooltip" class="m-tooltip" ref="tooltip">
      {{ errorContent }}
    </div>
  </div>
</template>
<script>
import Resource from '../../script/base/resource';
import Format from '../../script/base/format';
import Validate from '../../script/base/validate';

export default {
  name: 'base-input',
  props: [
    'type',
    'valueType',
    'classList',
    'value',
    'placeholder',
    'iconClass',
    'required',
    'classContainer',
    'tabindex',
  ],
  data() {
    return {
      isError: false,
      errorContent: '',
      active: false,
      isChange: false,
      showTooltip: false,
    };
  },
  methods: {
    onInput(event) {
      // Can add validation here

      this.$emit('input', event.target.value);
    },
    onChange(event) {
      // Can add validation here
      this.$emit('change', event.target.value);
    },
    focusInput() {
      // focus vào ô input
      this.$refs.input.focus();
    },
    /*
    Hàm format lại dữ liệu
    Author: NVThang (10/11/2021)
    */
    formatData(value) {
      switch (this.valueType) {
        case 'money':
          return Format.formatMoney(value);
        default:
          return value;
      }
    },
    /*
    Hàm validate dữ liệu và xét error
    Author: NVThang (10/11/2021)
    */
    validateValue() {
      if (this.required) {
        if (this.value && this.value.length > 0) {
          this.errorContent = '';
          this.isError = false;
        } else {
          this.isError = true;
          this.errorContent = Resource.ErrorContent.Required;
          return;
        }
      }
      switch (this.valueType) {
        case 'email':
          if (Validate.validateEmail(this.value) || !this.value) {
            this.isError = false;
          } else {
            this.isError = true;
            this.errorContent = Resource.ErrorContent.InvalidEmail;
          }
          break;
        case 'phoneNumber':
          if (Validate.validatePhoneNumber(this.value) || !this.value) {
            this.isError = false;
          } else {
            this.isError = true;
            this.errorContent = Resource.ErrorContent.InvalidPhoneNumber;
          }
          break;
      }
    },
    /*
    Hàm ản tooltip
    Author: NVThang (10/11/2021)
    */
    hideTooltip() {
      this.showTooltip = false;
    },
    /*
    Hàm hiển tooltip
    Author: NVThang (10/11/2021)
    */
    fshowTooltip() {
      if (this.isError) {
        this.showTooltip = true;
      }
    },
    showTooltip3s() {
      this.showTooltip = true;
      setTimeout(() => this.hideTooltip(), 3000);
    },
  },
  watch: {
    // hiển thị tooltip trong 3s khi bắt dc lỗi
    isError: function () {
      if (this.isError) {
        this.showTooltip3s();
      }
    },
  },
};
</script>
<style scoped>
.error {
  border-color: #ff4747 !important;
}
</style>
