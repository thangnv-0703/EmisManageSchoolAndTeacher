class Format {
  /* hàm format tiền
  author:  Nguyễn Việt Thắng (31/10/2021)
  */
  static formatMoney(money) {
    if (money) {
      return money
        .toString()
        .replaceAll('.', '')
        .replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    } else return '';
  }

  static changeToNumber(number) {
    if (number) {
      return number.toString().replace('.', '');
    } else return '0';
  }

  static formatDate(date) {
    if (date) {
      let _date = new Date(date);
      let day = _date.getDate(),
        month = _date.getMonth() + 1,
        year = _date.getFullYear();
      day = day < 10 ? '0' + day : day;
      month = month < 10 ? '0' + month : month;
      return day + '/' + month + '/' + year;
    } else return '';
  }
}

export default Format;
