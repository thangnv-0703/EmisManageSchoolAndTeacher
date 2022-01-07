<template>
  <div>
    <BaseLoading v-if="isShowLoading" />
    <!-- Toolbar -->
    <div class="top-toolbar">
      <div class="top-toolbar-left">
        <div class="top-toolbar-search">
          <BaseInput
            iconClass="icon-search"
            classList="top-search"
            :placeholder="placeholderInputSearch"
            v-model="searchText"
          />
          <button class="btn-search" @click="filterData"></button>
        </div>
        <BaseCombobox
          v-model="departmentId"
          :options="departments"
          direction="downward"
          :filter="false"
          class="ml-25"
        />
      </div>
      <div class="top-toolbar-right">
        <BaseButton btnText="Thêm" @click="openDialog(null)" />
        <BaseButton
          btnText="Xuất khẩu"
          classList="m-second-btn"
          @click="exportFile"
        />
        <BaseButton btnText="Xóa nhiều" @click="openComfirmDeletePopup(null)" />
      </div>
    </div>

    <!-- Grid -->
    <div class="grid">
      <table
        class="m-table"
        width="100%"
        cellspacing="0"
        id="tbEmployeeData"
        keyId="EmployeeId"
      >
        <thead>
          <tr>
            <th class="align-center">
              <BaseCheckbox />
            </th>
            <th class="align-center">Số hiệu cán bộ</th>
            <th class="align-center">Họ và tên</th>
            <th class="align-center">Số điện thoại</th>
            <th class="align-center">Tổ bộ môn</th>
            <th class="align-center">Môn dạy</th>
            <th class="align-center" title="Quản lý kho phòng">
              QL kho, phòng
            </th>
            <th class="align-center" title="Đào tạo quản lý thiết bị">
              Đào tạo QLTB
            </th>
            <th class="align-center">Đang làm việc</th>
            <th class="align-center"></th>
          </tr>
        </thead>
        <tbody id="listData">
          <tr
            class="table-row"
            :class="{
              'm-row-selected':
                selectedEmployeeList.indexOf(employee.EmployeeId) !== -1,
            }"
            v-for="employee in employees"
            :key="employee.EmployeeId"
            @dblclick="openDialog(employee.EmployeeId)"
          >
            <td class="align-center">
              <BaseCheckbox
                :value="employee.EmployeeId"
                v-model="selectedEmployeeList"
              />
            </td>
            <td class="align-left">{{ employee.EmployeeCode }}</td>
            <td class="align-left">{{ employee.FullName }}</td>
            <td class="align-center">{{ employee.PhoneNumber }}</td>
            <td class="align-left" :title="employee.DepartmentName">
              {{ employee.DepartmentName }}
            </td>
            <td class="align-left" :title="employee.SubjectListString">
              {{ employee.SubjectListString }}
            </td>
            <td class="align-left" :title="employee.RoomListString">
              {{ employee.RoomListString }}
            </td>
            <td class="align-center">
              <div
                class="td-check icon-check"
                v-if="employee.ManageService"
              ></div>
            </td>
            <td class="align-center">
              <div class="td-check icon-check" v-if="employee.WorkStatus"></div>
            </td>
            <td class="align-center">
              <div class="flex-center">
                <div
                  class="btn-edit icon-edit"
                  @click="openDialog(employee.EmployeeId)"
                ></div>
                <div
                  class="btn-delete icon-delete"
                  @click="openComfirmDeletePopup(employee.EmployeeId)"
                ></div>
              </div>
            </td>
          </tr>
        </tbody>
        <p v-if="employees.length === 0">Không có dữ liệu</p>
      </table>
    </div>

    <!-- Footer -->
    <div class="footer">
      <div class="footer-left">
        <div class="paginate-full">
          <a
            class="paginate-btn icon-first"
            :class="{ 'paginate-btn-disabled': pageIndex === 1 }"
            @click="pageIndex = 1"
          ></a>
          <a
            class="paginate-btn icon-back"
            :class="{ 'paginate-btn-disabled': pageIndex === 1 }"
            @click="decreasePageIndex"
          ></a>
          <div class="paginate-input align-center">{{ pageIndex }}</div>
          <a
            class="paginate-btn icon-next"
            :class="{ 'paginate-btn-disabled': pageIndex === totalPage }"
            @click="increasePageIndex"
          ></a>
          <a
            class="paginate-btn icon-last"
            :class="{ 'paginate-btn-disabled': pageIndex === totalPage }"
            @click="pageIndex = totalPage"
          ></a>
        </div>
        <div class="paginate-infor">
          <span class="paginate-total-page-number"
            >{{ pageIndex }}/{{ totalPage }}</span
          >
          trang (<span class="paginate-total-record-number">{{
            totalRecord
          }}</span>
          bản ghi)
        </div>
      </div>
      <div class="footer-right">
        <BaseCombobox
          :filter="false"
          direction="upward"
          v-model="pageSize"
          :options="pageSizeOptions"
        />
      </div>
    </div>

    <!-- Employee Dialog -->
    <EmployeeDetail
      v-if="isShowDialog"
      @onCloseDialog="closeDialog"
      :employeeId="selectedEmployeeId"
      :departments="departments.slice(1)"
      :subjects="subjects"
      :rooms="rooms"
      :showToast="makeToast"
      :loadData="reloadData"
    />

    <!-- Popup -->
    <BasePopup v-if="isShowConfirmDelete" @closePopup="closeConfirmDeletePopup">
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

    <!-- Toast Message -->
    <div id="toast" ref="toast"></div>
  </div>
</template>

<script>
import axios from 'axios';
import Vue from 'vue';
import api from '../../api/api';
import Enum from '../../script/base/enum';
import Resource from '../../script/base/resource';

import BaseInput from '../../components/base/BaseInput.vue';
import BaseButton from '../../components/base/BaseButton.vue';
import BaseCheckbox from '../../components/base/BaseCheckbox.vue';
import BasePopup from '../../components/base/BasePopup.vue';
import BaseToast from '../../components/base/BaseToast.vue';
import BaseCombobox from '../../components/base/BaseCombobox.vue';
import BaseLoading from '../../components/base/BaseLoading.vue';
import EmployeeDetail from './EmployeeDetail.vue';

export default {
  components: {
    BaseInput,
    BaseButton,
    BaseCheckbox,
    BasePopup,
    BaseLoading,
    BaseCombobox,
    EmployeeDetail,
  },
  data() {
    return {
      isShowLoading: false,
      isShowDialog: false,
      isShowConfirmDelete: false,
      employees: [],
      departments: Resource.Departments,
      subjects: [],
      rooms: [],
      pageSizeOptions: Resource.PageSize,
      selectedEmployeeId: null,
      departmentId: 0,
      selectedEmployeeList: [],
      searchText: '',
      pageIndex: 1,
      pageSize: 50,
      totalPage: 0,
      totalRecord: 0,
      placeholderInputSearch: Resource.InputSearch,
      deleteMode: Enum.DeleteMode.Zero,
      popupContent: '',
      popupButtons: [],
    };
  },
  methods: {
    /**
     * Hàm xuất khẩu ra file excel
     * Author: NVThang (28/11/2021)
     */
    exportFile() {
      const me = this;
      me.isShowLoading = true;
      axios
        .get(api.Employee + '/export', {
          responseType: 'blob', // important
        })
        .then(function (response) {
          me.isShowLoading = false;
          const url = window.URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', 'Danh_sach_can_bo_giao_vien.xlsx');
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        })
        .catch(function (err) {
          console.log(err);
        });
    },

    /*
    Hàm lựa chọn hành động với popup button
    Author: NVThang (22/11/2021)
    */
    handleClickPopupBtn(index) {
      switch (index) {
        case 0:
          this.closeConfirmDeletePopup();
          break;
        case 1:
          this.closeConfirmDeletePopup();
          this.handleDeleteEmployee();
          break;
      }
    },

    /*
    Hàm giảm giá trị pageIndex
    Author: NVThang (22/11/2021)
    */
    decreasePageIndex() {
      if (this.pageIndex > 1) {
        this.pageIndex--;
      }
    },

    /*
    Hàm tăng giá trị pageIndex
    Author: NVThang (22/11/2021)
    */
    increasePageIndex() {
      if (this.pageIndex < this.totalPage) {
        this.pageIndex++;
      }
    },

    /*
    Hàm hiển thị dialog chi tiết nhân viên
    Author: NVThang (15/11/2021)
    */
    openDialog(id) {
      this.selectedEmployeeList = [id];
      this.selectedEmployeeId = id;
      this.isShowDialog = true;
    },

    /*
    Hàm hiện thị popup xác nhận xóa
    Author: NVThang (15/11/2021)
    */
    openComfirmDeletePopup(id) {
      // console.log(typeof id);
      if (id !== null || this.selectedEmployeeList.length === 1) {
        // console.log(id, this.selectedEmployeeList);
        if (id) {
          this.selectedEmployeeList = [id];
          this.selectedEmployeeId = id;
        }
        this.deleteMode = Enum.DeleteMode.One;
      } else if (this.selectedEmployeeList.length > 1) {
        this.selectedEmployeeId = null;
        this.deleteMode = Enum.DeleteMode.Multi;
      } else {
        this.deleteMode = Enum.DeleteMode.Zero;
      }
      this.popupContent = Resource.Popup.Delete[this.deleteMode].content;
      this.popupButtons = Resource.Popup.Delete[this.deleteMode].btn;
      this.isShowConfirmDelete = true;
    },

    /*
    Hàm đóng dialog chi tiết nhân viên
    Author: NVThang (15/11/2021)
    */
    closeDialog() {
      this.isShowDialog = false;
    },

    /*
    Hàm đóng popup xác nhận xóa
    Author: NVThang (15/11/2021)
    */
    closeConfirmDeletePopup() {
      this.isShowConfirmDelete = false;
    },

    /*
    Hàm xử lí xóa nhân viên
    Author: NVThang (16/11/2021)
    */
    handleDeleteEmployee() {
      const me = this;
      me.isShowLoading = true;
      switch (me.deleteMode) {
        // Xóa 1 nhân viên
        case Enum.DeleteMode.One:
          axios
            .delete(api.Employee + `/${me.selectedEmployeeId}`)
            .then((res) => {
              if (res.status === 200) {
                me.reloadData();
                me.makeToast(Resource.Toast.DeleteSuccess);
                me.selectedEmployeeId = null;
                me.selectedEmployeeList = [];
              }
            })
            .catch(function () {
              me.makeToast(Resource.Toast.DeleteFail);
            });
          me.isShowLoading = false;
          break;
        // Xóa nhiều nhân viên
        case Enum.DeleteMode.Multi:
          me.isShowLoading = true;
          axios
            .delete(
              api.Employee + '/multi/' + me.selectedEmployeeList.toString(),
            )
            .then(function (res) {
              if (res.status === 200) {
                me.reloadData();
                me.makeToast(Resource.Toast.DeleteSuccess);
                me.selectedEmployeeId = null;
                me.selectedEmployeeList = [];
              }
            })
            .catch(function () {
              me.makeToast(Resource.Toast.DeleteFail);
            });
          me.isShowLoading = false;
          break;
      }
    },

    /*
    Hàm reload lại dữ liệu
    Author: NVThang (17/11/2021)
    */
    reloadData() {
      this.filterData();
    },

    /*
    Hàm lọc dữ liệu
    Author: NVThang (17/11/2021)
    */
    filterData() {
      const me = this;
      me.pageIndex = me.pageIndex === 0 ? 1 : me.pageIndex;
      const link =
        api.Employee +
        `/search?searchText=${this.searchText}${
          this.departmentId ? `&departmentId=${this.departmentId}` : ''
        }&pageIndex=${this.pageIndex}&pageSize=${this.pageSize}`;
      me.isShowLoading = true;
      axios
        .get(link)
        .then(function (res) {
          me.employees = res.data.Data;
          me.totalRecord = res.data.TotalRecord;
          me.totalPage = res.data.TotalPage;
          if (me.totalPage === 0) {
            me.pageIndex = 0;
          }
          if (me.pageIndex > me.totalPage) {
            me.pageIndex = me.totalPage;
          }
          me.isShowLoading = false;
        })
        .catch(function (err) {
          console.log(err.response);
        });
    },
    /*
    Hàm tạo và hiển thị toast message
    Author: NVThang (17/11/2021)
    */
    makeToast(toastObj) {
      var ComponentClass = Vue.extend(BaseToast);
      var instance = new ComponentClass({
        propsData: {
          type: toastObj.type,
          message: toastObj.message,
          title: toastObj.title,
        },
      });
      instance.$mount(); // pass nothing
      this.$refs.toast.appendChild(instance.$el);
    },
  },
  created() {
    const me = this;
    // load dữ liệu đầu vào, không có searchText thì sẽ lấy toàn bộ

    me.filterData();

    // gọi api tổ bộ môn
    axios
      .get(api.Department)
      .then(function (res) {
        me.departments = me.departments.concat(
          (res.data || []).map((item) => {
            return {
              id: item.DepartmentId,
              label: item.DepartmentName,
            };
          }),
        );
      })
      .catch(function (err) {
        console.log(err.response);
      });
    // gọi api kho phòng
    axios
      .get(api.Room)
      .then(function (res) {
        me.rooms = me.rooms.concat(
          (res.data || []).map((item) => {
            return {
              id: item.RoomId,
              label: item.RoomName,
            };
          }),
        );
      })
      .catch(function (err) {
        console.log(err.response);
      });
    // gọi api môn học
    axios
      .get(api.Subject)
      .then(function (res) {
        me.subjects = me.subjects.concat(
          (res.data || []).map((item) => {
            return {
              id: item.SubjectId,
              label: item.SubjectName,
            };
          }),
        );
      })
      .catch(function (err) {
        console.log(err.response);
      });
  },
  watch: {
    // theo dõi sự thây đổi của page Index để lọc dữ liệu
    pageIndex: function () {
      if (this.pageIndex !== 0) {
        this.filterData();
      }
    },
    departmentId: function () {
      this.filterData();
    },
    pageSize: function () {
      this.filterData();
    },
  },
};
</script>
<style scoped>
@import url('../../style/page/employee.css');
</style>
