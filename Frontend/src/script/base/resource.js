const Resource = {
  menuItems: [
    {
      title: 'Tổng quan ',
      link: '/startpage',
      iconClass: 'icon-startpage',
    },
    {
      title: 'Thiết bị ',
      link: '/equipment',
      iconClass: 'icon-equipment',
    },
    {
      title: 'Mượn trả ',
      link: '/equipmentorderschedule',
      iconClass: 'icon-equipmentorderschedule',
    },
    {
      title: 'Đề nghị mua sắm ',
      link: '/shoppingregistration',
      iconClass: 'icon-shoppingregistration',
    },
    {
      title: 'Tra cứu ',
      link: '/equipmentsearch',
      iconClass: 'icon-equipmentsearch',
    },
    {
      title: 'Hệ thống ',
      link: '/employee',
      iconClass: 'icon-dictionary',
    },
    {
      title: 'Báo cáo ',
      link: '/report',
      iconClass: 'icon-report',
    },
  ],
  Departments: [
    {
      id: 0,
      label: 'Tất cả phòng ban',
    },
  ],
  PageSize: [
    {
      id: 10,
      label: '10 bản ghi/trang',
    },
    {
      id: 20,
      label: '20 bản ghi/trang',
    },
    {
      id: 30,
      label: '30 bản ghi/trang',
    },
    {
      id: 40,
      label: '40 bản ghi/trang',
    },
    {
      id: 50,
      label: '50 bản ghi/trang',
    },
  ],
  Popup: {
    ChangeData: {
      content: 'Dữ liệu đã bị thay đổi, bạn có muốn lưu lại không?',
      btn: [
        {
          text: 'Đóng',
          class: 'm-second-btn',
          action: 0,
        },
        {
          text: 'Không',
          class: 'm-second-btn',
          action: 1,
        },
        {
          text: 'Lưu',
          action: 2,
        },
      ],
    },
    Delete: [
      {
        content: 'Bạn chưa chọn bản ghi nào để xóa',
        btn: [
          {
            text: 'Đóng',
            action: 0,
          },
        ],
      },
      {
        content: 'Bạn có chắc chắn muốn xóa Cán bộ giáo viên đang chọn không?',
        btn: [
          {
            text: 'Không',
            class: 'm-second-btn',
            action: 0,
          },
          {
            text: 'Xóa',
            action: 1,
          },
        ],
      },
      {
        content:
          'Bạn có chắc chắn muốn xóa những Cán bộ giáo viên đang chọn không?',
        btn: [
          {
            text: 'Không',
            class: 'm-second-btn',
            action: 0,
          },
          {
            text: 'Xóa',
            action: 1,
          },
        ],
      },
    ],
    Error: {
      content: 'Có lỗi xảy ra, vui lòng liên hệ MISA',
      btn: [
        {
          text: 'Đóng',
          action: 0,
        },
      ],
    },
  },
  Toast: {
    AddSuccess: {
      type: 'success',
      message: 'Thêm hồ sơ giáo viên thành công',
      title: 'Thành công',
    },
    AddFail: {
      type: 'error',
      message: 'Thêm hồ sơ giáo viên thất bại',
      title: 'Thất bại',
    },
    EditSuccess: {
      type: 'success',
      message: 'Sửa hồ sơ giáo viên thành công',
      title: 'Thành công',
    },
    EditFail: {
      type: 'error',
      message: 'Sửa hồ sơ giáo viên thất bại',
      title: 'Thất bại',
    },
    DeleteSuccess: {
      type: 'success',
      message: 'Xóa hồ sơ giáo viên thành công',
      title: 'Thành công',
    },
    DeleteFail: {
      type: 'error',
      message: 'Xóa hồ sơ giáo viên thất bại',
      title: 'Thất bại',
    },
  },
  FormTitle: {
    Edit: 'Sửa hồ sơ Cán bộ, giảng viên',
    Add: 'Thêm hồ sơ Cán bộ, giảng viên',
  },
  ErrorContent: {
    Required: 'Nội dung này không được để trống',
    InvalidEmail: 'Email không đúng định dạng',
    InvalidPhoneNumber: 'Số điện thoại không đúng định dạng',
  },
  InputSearch: 'Nhập Số hiệu cán bộ hoặc Tên cán bộ để tìm kiếm',
};

export default Resource;
