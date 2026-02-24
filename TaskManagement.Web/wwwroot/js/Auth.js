
document.addEventListener("DOMContentLoaded", function () { });
document.addEventListener("click", function (e)
{

    

    // Javascript code for toggle show and hide password 
    const iconBtn = e.target.closest("#newPasswordIcon");
    if (iconBtn) {
        const passwordInput = document.getElementById('newPassword');
        const isPassword = passwordInput.type === 'password';
        passwordInput.type = isPassword ? 'text' : 'password';
        iconBtn.classList.toggle('bi-eye');
        iconBtn.classList.toggle('bi-eye-slash');
    }

    const iconBtn2 = e.target.closest("#confirmPasswordIcon");
    if (iconBtn2) {
        const ConformPasswordInput = document.getElementById('confirmPassword');
        console.log("Toggle Confirm Password Clicked :", ConformPasswordInput);

        const isPassword = ConformPasswordInput.type === 'password';
        console.log("isPassword in the conformpase: ", isPassword);
        ConformPasswordInput.type = isPassword ? 'text' : 'password';
        iconBtn2.classList.toggle('bi-eye');
        iconBtn2.classList.toggle('bi-eye-slash');
    }
    // For Login page
    const iconLoginBtn = e.target.closest("#passwordIcon");
    if (iconLoginBtn) {
        const PasswordInput = document.getElementById('passwordField');

        const isPassword = PasswordInput.type === 'password';
        console.log("isPassword in the conformpase: ", isPassword);
        PasswordInput.type = isPassword ? 'text' : 'password';
        iconLoginBtn.classList.toggle('bi-eye');
        iconLoginBtn.classList.toggle('bi-eye-slash');
    }
    
});

