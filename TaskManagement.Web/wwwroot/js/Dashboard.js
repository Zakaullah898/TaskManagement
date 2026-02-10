
document.addEventListener('DOMContentLoaded', function () {
    const currentView = localStorage.getItem("last-page");
    if (currentView === "all-task") loadTasksToTable();
    else  loadingAllUser();
});
document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'all-task') {
        loadTasksToTable()
    }
    else if (e.target && e.target.id === 'all-users')
    {
        loadingAllUser();
    }


});
function loadTasksToTable() {
    $.ajax({
        url: '/Dashboard/GetAllTasks', // URL to the GET method
        type: 'GET',
        success: function (data) {
            // On success, insert the returned HTML (partial view content) into the container
            const activeTab = document.querySelector('.tasks-active-tab');
            if (activeTab) {
                activeTab.classList.add('active');
            }
            $('#taskTable').html(data);
            localStorage.setItem("last-page", "all-task");
            hideLoader()
        },
        error: function () {
            //alert('.');
            Swal.fire({
                title: 'Error!',
                text: 'Error loading items',
                icon: 'AlertType',
                confirmButtonText: 'OK'
            });
            hideLoader()
        }
    });
}
//
function loadingAllUser() {
    showLoader();
    $.ajax({
        url: '/Dashboard/GetAllUsers', // URL to the GET method
        type: 'GET',
        success: function (data) {
            // On success, insert the returned HTML (partial view content) into the container
            $('#taskTable').html(data);
            const activeTab = document.querySelector('.user-active-tab');
            if (activeTab) {
                activeTab.classList.add('active');
            }
            localStorage.setItem("last-page", "all-user");
            hideLoader()
        },
        error: function () {
            //alert('.');
            Swal.fire({
                title: 'Error!',
                text: 'Error loading items',
                icon: 'AlertType',
                confirmButtonText: 'OK'
            });
        }
    });

}
// for loader functions
function showLoader() {
    //$("#preloaded").css("display", "block");
    document.getElementById("preloaded").style.display = "block";
}

function hideLoader() {
    document.getElementById("preloaded").style.display = "none";
}




// Listening all click events for dynamically created elements
document.addEventListener('click', function (e) {
    // Check for Edit User button
    
        const editBtn = e.target.closest('.edit-user-btn');
    if (editBtn) {
        const row = editBtn.closest('tr');
            const userId = editBtn.dataset.userId;
        const userName = row.querySelector('.user-name').dataset.userName;

        const rolesData = row.querySelector('.role').dataset.roles;
        const isActive = row.querySelector('.is-active').dataset.isActive;
        //const isActive = status.toLowerCase() === "true";
        console.log("Is Active:", isActive);
        const roles = JSON.parse(rolesData);  // Now this works
        console.log("Roles:", rolesData);

            document.getElementById('userId').value = userId;
        document.getElementById('userName').value = userName;
const  selectedStatus = document.getElementById('userStatus'); // Convert boolean to string for select input
        selectedStatus.value = isActive.toLowerCase() === "true"? 'active' : 'inactive';        

        console.log("Selected", document.getElementById('userStatus').value) 

        document.querySelectorAll('.role-checkbox').forEach(cb => cb.checked = false);
        roles.forEach(role => {
            const checkbox = document.querySelector(`.role-checkbox[value="${role}"]`);
            if (checkbox) {
                checkbox.checked = true;
            }   
        });

            console.log("User Name:", userName);
        return; // Stop further processing
        }

    // Check for Delete User button
    const deleteBtn = e.target.closest('.delete-user-btn');
    if (deleteBtn) {
        const userId = deleteBtn.dataset.deleteUserId;
        console.log("Delete User ID:", userId);
        deletingUser(userId); // your function
        return; // Stop further processing
    }

    // Javascript code for toggle show and hide password 
    const iconBtn = e.target.closest(".newPassword");
    if (iconBtn) {
        const passwordInput = document.getElementById('newPasswordInput');
        const togglePasswordBtn = document.getElementById('togglePassword');
        const icon = togglePasswordBtn.querySelector('i');
        console.log("isPassword : ", icon);
        const isPassword = passwordInput.type === 'password';
        passwordInput.type = isPassword ? 'text' : 'password';
        icon.classList.toggle('bi-eye');
        icon.classList.toggle('bi-eye-slash');
    }

    const iconBtn2 = e.target.closest(".ConformPassword");
    if (iconBtn2) {
        const ConformPasswordInput = document.getElementById('confirmPasswordInput');
        console.log("Toggle Confirm Password Clicked :", ConformPasswordInput);
        const togglePasswordBtn = document.getElementById('toggleConfromPassword');
        const icon = togglePasswordBtn.querySelector('i');

        const isPassword = ConformPasswordInput.type === 'password';
        console.log("isPassword in the conformpase: ", isPassword);
        ConformPasswordInput.type = isPassword ? 'text' : 'password';
        icon.classList.toggle('bi-eye');
        icon.classList.toggle('bi-eye-slash');
    }
    // Check for Update User button
    const updateBtn = e.target.closest('.updating-btn');
    if (updateBtn)
    {
        const passwordInput = document.getElementById('newPasswordInput');
        const ConformPasswordInput = document.getElementById('confirmPasswordInput');
        let password = ""; // Initialize password variable
        if (passwordInput.value === ConformPasswordInput.value) {
            password = passwordInput.value;
        }
        else
        {
            Swal.fire({
                title: 'Error!',
                text: 'Password and Confirm Password do not match.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
            return; // Stop further processing
        }
        console.log("Password : ", password);
        const userId = document.getElementById('userId').value;
        const userName = document.getElementById('userName').value;
        const statusValue = document.getElementById('userStatus').value;
        const isActive = statusValue === 'active'; // Convert string to boolean
        const selectedRoles = Array.from(document.querySelectorAll('.role-checkbox:checked')).map(cb => cb.value);

        const userData = {
            Id: userId,
            UserName: userName,
            IsActive: isActive,
            Role: selectedRoles,
            Password: password // Include password if needed
        };
        console.log("User Data to Update:", userData);
        const showModal = document.getElementById('userModal');
        const modal = bootstrap.Modal.getInstance(showModal) || new bootstrap.Modal(showModal);
        modal.hide();
        updatingUser(userData); // your function
        return; // Stop further processing
    }

    // Check for Delete Task button
    const deleteTaskBtn = e.target.closest('.delete-task-btn');
    if (deleteTaskBtn) {
        const taskId = deleteTaskBtn.dataset.taskId;
        console.log("Delete Task ID:", taskId);
        deletingTask(taskId); // your function
        return; // Stop further processing
    }
});

// Deleting user
async function deletingUser(userId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
             fetch(`/Dashboard/DeleteUser?id=${userId}`, {
                method: 'DELETE'
             }).then(data => {
                 console.log(data);
                 if (data.ok) {
                        Swal.fire(
                            'Deleted!',
                            'User has been deleted.',
                            'success'
                        );
                        // Reload the user list
                        loadingAllUser();
                    } else {
                        Swal.fire(
                            'Error!',
                            'There was an error deleting the user.',
                            'error'
                     );
                     loadingAllUser();
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    Swal.fire(
                        'Error!',
                        'There was an error deleting the user.',
                        'error'
                    );
                    loadingAllUser();
                });
        }
    })
}


// Updating user and assigning role to user
function updatingUser(userData) {
    console.log("Updating data is : ", userData);
    fetch('/Dashboard/UpdateUser', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    }).then(response => {
        if (response.ok) {
            Swal.fire(
                'Updated!',
                'User has been updated.',
                'success'
            );
            loadingAllUser(); // Refresh the user list
        } else {
            Swal.fire(
                'Error!',
                'There was an error updating the user.',
                'error'
            );
        }
    }).catch(error => {
        console.error('Error:', error);
        Swal.fire(
            'Error!',
            'There was an error updating the user.',
            'error'
        );
    });
}

// Deleting task
function deletingTask(taskId)
{
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Dashboard/DeleteTask?id=${taskId}`, {
                method: 'DELETE'
             }).then(data => {
                 console.log(data);
                 if (data.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Task has been deleted.',
                            'success'
                        );
                        // Reload the task list
                        loadTasksToTable();
                    } else {
                        Swal.fire(
                            'Error!',
                            'There was an error deleting the task.',
                            'error'
                     );
                     loadTasksToTable();
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    Swal.fire(
                        'Error!',
                        'There was an error deleting the task.',
                        'error'
                    );
                    loadTasksToTable();
                });
        }
    })
}