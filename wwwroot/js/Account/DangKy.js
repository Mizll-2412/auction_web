let txtEmail = document.querySelector("#txtEmail");
let txtSdt = document.querySelector("#txtSoDienThoai");
let txtMatKhau = document.querySelector("#txtMatKhau");
let txtHoten = document.querySelector("#txtFullName")
let txtNhapLaiMatKhau = document.querySelector("#txtNhapLaiMatKhau");
let btnDangKy = document.querySelector("#btnDangKy");

let loiRongHoTen = document.querySelector("#LoiRongTxtFullName");
let LoiRongSoDienThoai = document.querySelector("#LoiRongTxtSoDienThoai");
let LoiRongEmail = document.querySelector('#LoiRongTxtEmail');
let LoiRongTxtMatKhau = document.querySelector("#LoiRongTxtMatKhau");
let LoiLapLaiTxtMatKhau = document.querySelector("#LoiLapLaiTxtMatKhau");
let LoiSaiTaiKhoan = document.querySelector("#LoiSaiTaiKhoan");
let loiSaidinhDangSDT =document.querySelector("#LoiSaiDinhDangSDT")
let loiSaidinhDangEmail = document.querySelector("#LoiSaiDinhDang")

btnDangKy.addEventListener("click", function(event) {
    event.preventDefault();
    if (txtEmail.value.trim() === "") {
        LoiRongEmail.style.display = "block";
        loiSaidinhDangEmail.style.display = "none";
    } else if (!regex_email.test(txtEmail.value)) {
        loiSaidinhDangEmail.style.display = "block";
        LoiRongEmail.style.display = "none";
    } else {
        LoiRongEmail.style.display = "none";
        loiSaidinhDangEmail.style.display = "none";
    }
    if (txtSdt.value.trim() === "") {
        LoiRongSoDienThoai.style.display = "block";
        loiSaidinhDangSDT.style.display = "none";
    } else if (!regex_phone.test(txtSdt.value)) {
        loiSaidinhDangSDT.style.display = "block";
        LoiRongSoDienThoai.style.display = "none";
    } else {
        LoiRongSoDienThoai.style.display = "none";
        loiSaidinhDangSDT.style.display = "none";
    }
    if (txtMatKhau.value.trim() === "") {
        LoiRongTxtMatKhau.style.display = "block";
        LoiRongDoDaiMK.style.display = "none";
    } else if (!regex_MK.test(txtMatKhau.value)) {
        LoiRongDoDaiMK.style.display = "block";
        LoiRongTxtMatKhau.style.display = "none";
    } else {
        LoiRongTxtMatKhau.style.display = "none";
        LoiRongDoDaiMK.style.display = "none";
    }
    if (txtHoten.value.trim() === "") {
        loiRongHoTen.style.display = "block";
    } else {
        loiRongHoTen.style.display = "none";
    }
    if (txtMatKhau.value.trim() !== txtNhapLaiMatKhau.value.trim()) {
        LoiLapLaiTxtMatKhau.style.display = "block";
    } else {
        LoiLapLaiTxtMatKhau.style.display = "none";
    }
    if (
        txtEmail.value.trim() !== "" &&
        regex_email.test(txtEmail.value) &&
        txtSdt.value.trim() !== "" &&
        regex_phone.test(txtSdt.value) &&
        txtMatKhau.value.trim() !== "" &&
        regex_MK.test(txtMatKhau.value) &&
        txtHoten.value.trim() !== "" &&
        txtMatKhau.value.trim() === txtNhapLaiMatKhau.value.trim()
    ) {
        form1.submit();
    }
});
var regex_MK = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;
var regex_email = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
var regex_phone = /^(0|\+84)([235789]|[0-9]{3})[0-9]{7,8}$/;

// hiển thị mật khẩu 
document.querySelector('.password-toggle').addEventListener('click', function() {
    const passwordInput = document.getElementById('txtMatKhau');
    passwordInput.type = passwordInput.type === 'password' ? 'text' : 'password';
});
document.querySelector('.password-togglee').addEventListener('click', function() {
    const passwordInput = document.getElementById('txtNhapLaiMatKhau');
    passwordInput.type = passwordInput.type === 'password' ? 'text' : 'password';
});

