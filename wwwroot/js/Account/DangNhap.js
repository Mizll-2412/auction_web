
document.querySelector('.password-toggle').addEventListener('click', function() {
    const passwordInput = document.getElementById('password');
    passwordInput.type = passwordInput.type === 'password' ? 'text' : 'password';
});

/*var regextxtMatKhau = /^(?=.* [a - z])(?=.* [A - Z])(?=.*\d)[!@#$%^&* (),.? ":{}|<>].{6,}$/
btnDangKy.addEventListener("click", function () {
    if (!regextxtMatKhau.test(txtMatKhau.value)) {

    }

});*/
//mật khẩu phải có ký tự đặc biệt
//  / ^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[!@#$%^&*(),.?":{}|<>].{6,}$/
//check số điện thoại
//  /^(\+84|0)\d{9}$/
