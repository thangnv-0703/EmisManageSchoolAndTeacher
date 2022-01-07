class Validate {
  /**
   * Thực hiện validate dữ liệu bắt buộc nhập trong form
   * Author: NVTHANG (16/10/2021)
   */
  // static validateInputRequire() {
  //   let isValid = true;
  //   let inputRequires = $('input[required]');
  //   if (inputRequires.length > 0) {
  //     for (const input of inputRequires) {
  //       const value = $(input).val();
  //       if (value == '') {
  //         isValid = false;
  //         $(input).addClass('input-invalid');
  //         $(input).attr('title', 'Thông tin này không được phép để trống');
  //       } else {
  //         $(input).removeClass('input-invalid');
  //         $(input).removeAttr('title');
  //       }
  //     }
  //     const inputNotValid = inputRequires.filter('.input-invalid');
  //     if (inputNotValid.length > 0) {
  //       inputNotValid[0].focus();
  //     }
  //   }
  //   return isValid;
  // }

  static validateEmail(email) {
    const re = /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/i;
    return re.test(email);
  }

  static validatePhoneNumber(phone) {
    const re = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    return re.test(String(phone));
  }
}

export default Validate;
