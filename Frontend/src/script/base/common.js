$(document).ready(function () {
  CommonJS.LanguageCode = 'VN';
});

class CommonJS {
  LanguageCode = null;
  static showMsg() {}

  /** -----------------------------
   * Hiện thị toast message
   * author: Nguyen Viet Thang - 19/10/2021
   */
  static showToastMsg(msg, status) {
    let icon = '';
    switch (status) {
      case 'success':
        icon = '<i class="fas fa-check-circle"></i>';
        break;
      case 'error':
        icon = '<i class="fas fa-exclamation-triangle"></i>';
        break;
    }
    // Định nghĩ HTML cho toastMsg:
    let tsHTML = $(`<div class="m-toast m-toast-${status}">
                <div class="m-toast-icon-symbol">
                  ${icon}
                </div>
                <div class="m-toast-message">${msg}</div>
                <div class="m-toast-icon-x icon-24">
                  <i class="fas fa-times"></i>
                </div>
              </div>`);
    $('body').append(tsHTML);
    setTimeout(() => {
      tsHTML.remove();
    }, 3000);
  }

  /** -----------------------------
   * Build table từ dữ liệu
   * author: Nguyen Viet Thang
   */
  static buildTable(tableElement, data) {
    $(tableElement).find('tbody').empty();
    // Lấy ra các cột trong bảng
    let keyId = $(tableElement).attr('keyId');
    let cols = $(tableElement).find('thead th');
    // Duyệt từng đối tượng trong mảng để lấy thông tin tương ứng
    for (const item of data) {
      // tạo tr
      let trHtml = $(`<tr></tr>`);
      $.each(cols, function (index, col) {
        // lấy fieldname, dữ liệu tương ứng
        let fieldName = col.getAttribute('fieldName');
        let value = item[fieldName];
        let formatType = col.getAttribute('formatType');
        // format lại dữ liệu
        value = CommonJS.formatData(value, formatType);
        let td = $(`<td>${value}</td>`);
        trHtml.append(td);
      });

      trHtml.attr('keyId', item[keyId]);
      // thêm tr vào table
      $(tableElement).append(trHtml);
    }
  }

  /** -----------------------------
   * format dữ liệu trong table
   * author: Nguyen Viet Thang
   */
  static formatData(value, formatType) {
    switch (formatType) {
      case 'ddmmyyyy':
        if (value) {
          // dateOfBirth != 0,dateOfBirth != null, dateOfBirth != undefined,dateOfBirth!= "";
          value = new Date(value);
          let date = value.getDate();
          let month = value.getMonth() + 1;
          let year = value.getFullYear();
          month = month < 10 ? `0${month}` : month;
          date = date < 10 ? `0${date}` : date;
          value = `<div class="align-center">${date}/${month}/${year}</div>`;
        } else {
          value = '';
        }
        break;
      case 'Gender':
        let language = Resource[CommonJS.LanguageCode];
        if (!language) {
          language = Resource['EN'];
          console.log(language);
        }
        switch (value) {
          case 1:
            value = language.Gender.Male;
            break;
          case 0:
            value = language.Gender.Female;
            break;
          default:
            value = '';
            break;
        }
        break;
      case 'Money':
        if (value) {
          value = new Intl.NumberFormat('vi-VI', {
            style: 'currency',
            currency: 'VND',
          }).format(value);
          return `<div class="align-right">${value}</div>`;
        } else {
          value = '';
        }
        break;
    }
    if (value === null) {
      value = '';
    }
    return value;
  }

  static bindingDialogData(data) {
    console.log(data);
    let inputs = $('#dialogEmployee input[fieldName],div[fieldName]');
    $.each(inputs, function (index, input) {
      console.log(input);
      let fieldName = $(input).attr('fieldName');
      let value = data[fieldName];
      console.log(value);
      $(input).val(value);
    });
  }

  static clearInputValueInDialog() {
    let inputs = $('#dialogEmployee input[fieldName],div[fieldName]');
    $.each(inputs, function (index, input) {
      $(input).val('');
    });
    let inputRequires = $('input[required]');
    if (inputRequires.length > 0) {
      for (const input of inputRequires) {
        $(input).removeClass('input-invalid');
        $(input).removeAttr('title');
      }
    }
  }
}
