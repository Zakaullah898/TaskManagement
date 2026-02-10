
document.addEventListener("DOMContentLoaded", function () { });
document.addEventListener("click", function (e)
{
    const goToVarifyEmail = e.target.closest("#goToVerifyEmail");
    if (goToVarifyEmail)
    {
        goToVerifyEmail()
        return; // Stop further processing if the button was clicked
    }
    const verifyEmailBtn = e.target.closest(".submit-btn");
    if (verifyEmailBtn)
    {
        const emailInput = document.querySelector("#email");
        console.log("email input is : ", emailInput.value)
        verifyEmail(emailInput.value)

    }

    // Javascript code for toggle show and hide password 
    const iconBtn = e.target.closest("#newPasswordIcon");
    if (iconBtn) {
        const passwordInput = document.getElementById('newPassword');
        const togglePasswordBtn = document.getElementById('newPasswordIcon');
        const isPassword = passwordInput.type === 'password';
        passwordInput.type = isPassword ? 'text' : 'password';
        togglePasswordBtn.classList.toggle('bi-eye');
        togglePasswordBtn.classList.toggle('bi-eye-slash');
    }

    const iconBtn2 = e.target.closest("#confirmPasswordIcon");
    if (iconBtn2) {
        const ConformPasswordInput = document.getElementById('confirmPassword');
        console.log("Toggle Confirm Password Clicked :", ConformPasswordInput);
        const togglePasswordBtn = document.getElementById('confirmPasswordIcon');

        const isPassword = ConformPasswordInput.type === 'password';
        console.log("isPassword in the conformpase: ", isPassword);
        ConformPasswordInput.type = isPassword ? 'text' : 'password';
        togglePasswordBtn.classList.toggle('bi-eye');
        togglePasswordBtn.classList.toggle('bi-eye-slash');
    }
    // Javascript for  reset password
    const submitPassword = e.target.closest(".reset-password-btn")
    if (submitPassword)
    {
        const userEmail = document.getElementById("user-email").value
        const newPassword = document.getElementById("newPassword").value;
        const conformPassword = document.getElementById("confirmPassword").value;
        let password = ""; // Initialize password variable
        if (newPassword === conformPassword) {
            password = newPassword;
        }
        else {
            Swal.fire({
                title: 'Error!',
                text: 'Password and Confirm Password do not match.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
        console.log("User Email : ", userEmail);
        console.log("User New Password : ", newPassword);
        console.log("User conform password : ", password);
        const data =
        {
            Password: password,
            Email: userEmail
        }
        resetPassword(data)
            return; // Stop further processing
    }
});

// function goToVerifyEmail
async function goToVerifyEmail() {
    try {
        const response = await fetch("/Auth/GettingVerifyEmail");
        if (!response.ok) throw new Error("Failed to load partial view");

        const html = await response.text();
        const cardBody = document.querySelector(".loging-form");
        if (cardBody) {
            cardBody.innerHTML = html;
        }
    } catch (error) {
        console.error(error);
        Swal.fire(
            'Error!',
            'An error occurred while verifying the email. Please try again.',
            'error'
        );
    }
}

// function verifyEmail
async function verifyEmail(email)
{
    try
    {
        const response = await fetch(`/Auth/GettingVerifyEmail?email=${email}`,
            {
                method: "POST",
            });
        if (!response.ok) throw new Error("Failed to verify email");
        Swal.fire(
            'success',
            'Email verified successfully',
            'success'
        )
        const html = await response.text();
        const cardBody = document.querySelector(".loging-form");
        if (cardBody) {
            cardBody.innerHTML = html;
        }
    }
    catch (error)
    {
        console.error(error);
        Swal.fire(
            'Error!',
            'User has been deletedAn error occurred while verifying the email. Please try again.',
            'error'
        );
    }
}
// Restting password
async function resetPassword(model) {
    try {
        const response = await fetch(`/Auth/ResetPassword`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(model)
        });

        if (!response.ok) throw new Error("Failed to reset password");

        const html = await response.text();
        const cardBody = document.querySelector(".loging-form");
        if (cardBody) {
            cardBody.innerHTML = html;
        }

        Swal.fire(
            'Success!',
            'Password reset successfully.',
            'success'
        );
    }
    catch (error) {
        console.error(error);
        Swal.fire(
            'Error!',
            'An error occurred while resetting the password. Please try again.',
            'error'
        );
    }
}
