div
<template lang="">
  <div>
    <BaseLoading v-if="isShowLoading"/>
    <div class="dialog">
      <div class="dialog-modal"></div>
      <div class="dialog-box">
        <div class="dialog-header">
          <div class="dialog-header-help dialog-header-btn"></div>
          <div
            class="dialog-header-close dialog-header-btn"
            @click="handleCloseDialog"
          ></div>
        </div>
        <div class="dialog-body">
          <div class="user-image">
            <div class="row-image">
              <div class="avatar-image"></div>
              <div class="choose-image-btn">
                <input type="file" />
                <span>Chọn ảnh</span>
              </div>
            </div>
            <div class="row-info">
              <p class="row-user-name">
                {{ employee.FullName || 'Họ và tên' }}
              </p>
              <p class="row-user-code">{{ employee.EmployeeCode }}</p>
            </div>
          </div>
          <div class="line-divide"></div>
          <div class="user-info">
            <div class="user-info-title">{{ formTitle }}</div>
            <div class="m-row">
              <div class="form-group">
                <label class="form-label"
                  >Họ và tên<span class="m-required">*</span></label
                >
                <BaseInput
                  v-model="employee.FullName"
                  required="true"
                  ref="fullName"
                  tabindex="1"
                />
              </div>
              <div class="form-group">
                <label class="form-label ml-25"
                  >Số hiệu cán bộ<span class="m-required">*</span></label
                >
                <BaseInput
                  v-model="employee.EmployeeCode"
                  required="true"
                  ref="employeeCode"
                  tabindex="2"
                />
              </div>
            </div>
            <div class="m-row">
              <div class="form-group">
                <label class="form-label">Số điện thoại</label>
                <BaseInput
                  v-model="employee.PhoneNumber"
                  valueType="phoneNumber"
                  ref="phoneNumber"
                  tabindex="3"
                />
              </div>
              <div class="form-group">
                <label class="form-label ml-25">Email</label>
                <BaseInput
                  v-model="employee.Email"
                  valueType="email"
                  ref="email"
                  tabindex="4"
                />
              </div>
            </div>
            <div class="m-row">
              <div class="form-group">
                <label class="form-label">Tổ bộ môn</label>
                <BaseCombobox
                  :filter="true"
                  direction="downward"
                  v-model="employee.DepartmentId"
                  :options="departments"
                  tabindex="5"
                />
              </div>
              <div class="form-group">
                <label class="form-label ml-25">Môn dạy</label>
                <BaseMultipleCombobox
                  :filter="true"
                  direction="downward"
                  v-model="employee.SubjectList"
                  :options="subjects"
                  tabindex="6"
                />  
              </div>
            </div>
            <div class="m-row">
              <div class="form-group">
                <label class="form-label">Quản lý kho phòng</label>
                <BaseMultipleCombobox
                  :filter="true"
                  direction="downward"
                  v-model="employee.RoomList"
                  :options="rooms"
                  tabindex="7"
                  classList="form-rooms"
                />              
              </div>
            </div>
          
            <div class="m-row row-checkbox">
              <div class="form-group" style="height: 31.6px">
                <BaseCheckbox
                  label="Trình độ nghiệp vụ QLTB"
                  v-model="employee.ManageService"
                  tabindex="8"
                />
              </div>
              <div class="form-group" style="height: 31.6px">
                <BaseCheckbox
                  label="Đang làm việc"
                  classList="ml-25"
                  v-model="employee.WorkStatus"
                  tabindex="9"
                />
              </div>
              <div
                class="form-group ml-25 form-group-date"
                v-show="!employee.WorkStatus"
              >
                <label>Ngày nghỉ việc</label>
                <date-picker
                  type="date"
                  class="ml-25 form-date"
                  v-model="employee.EndDate"
                  format="DD/MM/YYYY"
                  tabindex="10"
                  value-type="format"
                  :disabled-date="notAfterToday"
                ></date-picker>
              </div>
            </div>
          </div>
        </div>
        <div class="dialog-footer">
          <BaseButton
            class="m-second-btn"
            btnText="Đóng"
            @click="$emit('onCloseDialog')"
            tabindex="11"
          ></BaseButton>
          <BaseButton btnText="Lưu" @click="handleSaveData" tabindex="12"></BaseButton>
        </div>
      </div>
    </div>
    <BasePopup v-if="isShowPopup"  @closePopup="handleClosePopup">
      <template v-slot:content>
        <p>{{ popupContent }}</p>
      </template>
      <template v-slot:btn>
        <div class="m-popup-footer">
          <BaseButton
            v-for="(btn, index) in popupButtons"
            :key="index"
            :classList="btn.class"
            :btnText="btn.text"
            @click="handleClickPopupBtn(index)"
          />
        </div>
      </template>
    </BasePopup>
  </div>
</template>

<script>
import axios from 'axios';
import moment from 'moment';
import api from '../../api/api';
import Enum from '../../script/base/enum';
import Resource from '../../script/base/resource';
import BaseInput from '../../components/base/BaseInput.vue';
import BaseCombobox from '../../components/base/BaseCombobox.vue';
import BaseCheckbox from '../../components/base/BaseCheckbox.vue';
import BaseMultipleCombobox from '../../components/base/BaseMultipleCombobox.vue';
import BaseButton from '../../components/base/BaseButton.vue';
import BasePopup from '../../components/base/BasePopup.vue';
import BaseLoading from '../../components/base/BaseLoading.vue';
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';

export default {
  components: {
    BaseInput,
    BaseCombobox,
    BaseCheckbox,
    BaseButton,
    BasePopup,
    BaseMultipleCombobox,
    BaseLoading,
    DatePicker,
  },
  props: [
    'employeeId',
    'departments',
    'subjects',
    'rooms',
    'showToast',
    'loadData',
  ],
  data() {
    return {
      employee: {
        EmployeeCode: '',
        FullName: '',
        Email: '',
        PhoneNumber: '',
        DepartmentId: '',
        WorkStatus: 1,
        SubjectList: [],
        RoomList: [],
        ManageService: 0,
        EndDate: new Date(),
      },
      endDate: '',
      formMode: Enum.FormMode.Add,
      formTitle: Resource.FormTitle.Add,
      initialEmployee: {},
      isShowPopup: false,
      isShowLoading: false,
      popupContent: '',
      popupButtons: [],
    };
  },
  methods: {

    /*
    Hàm kiểm tra giữ liệu có bị thay đổi không
    Author: NVThang (18/11/2021)
    */
    checkDataChange(){
      for (const field in this.employee){
        if (this.initialEmployee[field] !== this.employee[field])
          return true;
      }
      return false;
    },

    /*
    Hàm lựa chọn hành vi của button popup
    Author: NVThang (18/11/2021)
    */
    handleClickPopupBtn(index){
      switch (index) {
        case 0:
          this.handleClosePopup();
          break;
        case 1:
          this.$emit('onCloseDialog');
          break;
        case 2:
          this.isShowPopup = false;
          this.handleSaveData();
          break;
      }
    },

    /*
    Hàm đóng dialog
    Author: NVThang (19/11/2021)
    */
    handleCloseDialog(){
      const isChanged = this.checkDataChange()
      if (!isChanged){
        this.$emit('onCloseDialog');
        return;
      }
      this.isShowPopup = true;
      this.popupContent = Resource.Popup.ChangeData.content
      this.popupButtons = Resource.Popup.ChangeData.btn;
    },

    /*
    Hàm hiển popup
    Author: NVThang (18/11/2021)
    */
    handleClosePopup() {
      this.isShowPopup = false;    
    },

    /*
    Hàm chặn việc nhập ngày lớn hơn hôm nay
    Author: NVThang (18/11/2021)
    */
    notAfterToday(date) {
      return date > new Date(new Date().setHours(0, 0, 0, 0));
    },

    /*
    Hàm xư lý việc thêm sửa dữ liệu
    Author: NVThang (18/11/2021)
    */
    handleSaveData() {
      const me = this;
      const valid = me.validateData();
      if (valid) {
        const isChanged = this.checkDataChange()
        if (!isChanged){
          this.$emit('onCloseDialog');
          return;
        }
        // cập nhật lại giá trị cho WorkStatus và ManageService
        this.formatDataToSend()
        switch (this.formMode) {
          case Enum.FormMode.Add:
            me.isShowLoading = true;
            axios
              .post(api.Employee, me.employee)
              .then(function (res) {
                if (res.status === 201){
                  me.$emit('onCloseDialog')          
                  me.loadData();
                  me.showToast(Resource.Toast.AddSuccess);    
                  me.isShowLoading = false;
                }
              })
              .catch(function (res) {
                // me.showToast(Resource.Toast.AddFail);
                me.isShowPopup = true;
                me.popupContent = res.response.data.userMsg ? res.response.data.userMsg : Resource.Popup.Error.content
                me.popupButtons = Resource.Popup.Error.btn;
                me.formatDataToShow()
              });
              me.isShowLoading = false;
            break;
          case Enum.FormMode.Edit:
            me.isShowLoading = true;
            axios
              .put(api.Employee + `/${me.employeeId}`, me.employee)
              .then(function (res) {
                if (res.status === 200){
                  me.showToast(Resource.Toast.EditSuccess);
                  me.loadData();
                  me.$emit('onCloseDialog')
                  
                }
              })
              .catch(function (res) {
                me.formatDataToShow()
                me.isShowPopup = true;
                me.popupContent = res.response.data.userMsg ? res.response.data.userMsg : Resource.Popup.Error.content
                me.popupButtons = Resource.Popup.Error.btn;
              });
              me.isShowLoading = false;
            break;
        }
      }
    },

    /*
    Hàm validate dữ liệu khi bấm nút lưu
    Author: NVThang (17/11/2021)
    */
    validateData() {
      let valid = true;
      let firstChildFocus = false;
      const inputs = this.$refs;
      for (let [key] of Object.entries(inputs)) {
        if (inputs[key].isError) {
          valid = false;
          if (firstChildFocus === false) {
            inputs[key].focusInput();
            inputs[key].showTooltip3s();
            firstChildFocus = true;
          }
        }
      }
      return valid;
    },

    formatDataToShow(){
      // this.employee.WorkStatus = !!this.employee.WorkStatus;
      // this.employee.ManageService = !!this.employee.ManageService;
      this.employee.EndDate = this.employee.EndDate
            ? moment(this.employee.EndDate).format('DD/MM/YYYY')
            : '';
    },
    formatDataToSend(){
        this.employee.EndDate = this.employee.EndDate && !this.employee.WorkStatus
          ? moment(this.employee.EndDate).format('YYYY-MM-DD')
          : null;
        // this.employee.WorkStatus = this.employee.WorkStatus ? 1 : 0;
        // this.employee.ManageService = this.employee.ManageService ? 1 : 0;
    }
  },

  created() {
    const me = this;
    // Nếu có employeeId thì là sửa, không có thì là xóa
    if (this.employeeId) {
      me.formMode = Enum.FormMode.Edit;
      me.formTitle = Resource.FormTitle.Edit;
      me.isShowLoading = true;
      axios
        .get(api.Employee + `/${me.employeeId}`)
        .then(function (res) {
          me.employee = res.data;
          me.formatDataToShow()
          for (const field in me.employee){
            me.initialEmployee[field] = me.employee[field]
          }
          me.isShowLoading = false;
        })
        .catch(function () {
          me.isShowPopup = true;
          me.popupContent = Resource.Popup.Error.content;
          me.popupButtons = Resource.Popup.Error.btn
        });

    } else {
      // Lấy mã giáo viên mới
      me.isShowLoading = true;
      axios
        .get(api.Employee + '/newEmployeeCode')
        .then(function (res) {
          me.employee.EmployeeCode = res.data;
          me.isShowLoading = false;
        })
        .catch(function () {

        });
    }
  },

  mounted() {
    // focus vào ô input đầu tiên
    this.$refs.fullName.focusInput();
  },
};
</script>
<style scoped>
@import url('../../style/page/employeeDetail.css');
</style>
