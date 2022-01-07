import Vue from 'vue';
import VueRouter from 'vue-router';
import Employee from '../view/employee/Employee.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/employee',
    name: 'Employee',
    component: Employee,
  },
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});
export default router;
