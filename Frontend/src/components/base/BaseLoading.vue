<template>
  <div>
    <div class="backdrop"></div>
    <div class="loader"></div>
    <!-- <div class="loader-model">
      <object :data="spinnerSVG" type=""></object>
    </div> -->
  </div>
</template>
<script>
export default {
  name: 'BaseSpinner',
  data() {
    return {
      spinnerSVG: require('../../assets/Icons/loading.svg'),
    };
  },
};
</script>
<style>
.backdrop {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #000;
  opacity: 0.4;
  z-index: 1000;
}

.loader {
  position: fixed;
  bottom: 50%;
  left: 50%;
  transform: translateY(-50%);
  border: 16px solid #f3f3f3; /* Light grey */
  border-top: 16px solid #0dd469; /* Blue */
  border-radius: 50%;
  width: 120px;
  height: 120px;
  animation: spin 2s linear infinite;
  z-index: 1000;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.loader-model {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  /* background-color: #fff; */
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2;
}
</style>
