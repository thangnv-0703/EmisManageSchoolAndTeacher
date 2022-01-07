<template lang="">
  <label class="checkbox-container" :class="classList">
    <input
      type="checkbox"
      class="checkbox"
      :checked="isChecked"
      :value="value"
      @change="updateInput"
      :tabindex="tabindex"
    />
    <span class="checkmark"></span>
    <span class="checkbox-label" v-if="label">{{ label }}</span>
  </label>
</template>
<script>
export default {
  model: {
    prop: 'modelValue',
    event: 'change',
    tabindex: 'tabindex',
  },
  props: {
    value: { type: [String, Number] },
    modelValue: { default: '' },
    label: { type: String },
    trueValue: { default: true },
    falseValue: { default: false },
    classList: { type: String },
    tabindex: { type: String },
  },
  computed: {
    // tính giá trị checked của checkbox
    isChecked() {
      if (this.modelValue instanceof Array) {
        return this.modelValue.includes(this.value);
      } else if (typeof this.modelValue === 'number') {
        return this.modelValue === 1;
      }
      return this.modelValue === this.trueValue;
    },
  },

  methods: {
    /*
    hàm tính toán và cập nhật giá trị của checkbox
    Author: NVThang (27/11/2021)
    */
    updateInput(event) {
      let isChecked = event.target.checked;
      if (this.modelValue instanceof Array) {
        let newValue = [...this.modelValue];
        if (isChecked) {
          newValue.push(this.value);
        } else {
          newValue.splice(newValue.indexOf(this.value), 1);
        }
        this.$emit('change', newValue);
      } else if (typeof this.modelValue === 'number') {
        this.$emit('change', isChecked ? 1 : 0);
      } else {
        this.$emit('change', isChecked ? this.trueValue : this.falseValue);
      }
    },
  },
};
</script>
<style lang=""></style>
