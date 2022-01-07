import Vue from 'vue';
import App from './App.vue';
import axios from 'axios';
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import { Button, Select } from 'element-ui';
import VueAxios from 'vue-axios';
import router from './router/index';

Vue.use(VueAxios, axios);
Vue.use(ElementUI);
Vue.component(Button.name, Button);
Vue.component(Select.name, Select);

Vue.config.productionTip = false;

new Vue({
  router,
  render: (h) => h(App),
}).$mount('#app');
