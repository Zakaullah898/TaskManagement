document.addEventListener("DOMContentLoaded", function () {
    showLoader();

    const currentView = localStorage.getItem("currentView");

    if (currentView === "assigned") {
        loadAllAssigned();
    } else if (currentView === "deleted") {
        openDeletedTasks();
    } else {
        loadTasks();
    }
});


document.addEventListener('click', function (e) {
    if (e.target && e.target.id === 'showModal') {
        const taskId = e.target.dataset.id;
        document.getElementById("taskId").value = taskId;
        loadingAllUser();
    }
    if (e.target && e.target.classList.contains('status-btn')) {
        const taskId = e.target.dataset.taskId;
        const status = e.target.dataset.status;
        updateTaskStatus(taskId, status);
    }
})
function loadTasks() {
    $.ajax({
        url: '/Tasks/GetAllTasks', // URL to the GET method
        type: 'GET',
        success: function (data) {
            // On success, insert the returned HTML (partial view content) into the container
            const activeTab = document.querySelector('.home-active-btn');
            if (activeTab) {
                activeTab.classList.add('active');
            }
            $('#task-item').html(data);
            localStorage.setItem("currentView", "all");
            hideLoader()
            // Attach mouseleave event listeners to dropdown menus
            const cardDropdown = document.querySelectorAll('.dropdown-menu-card');
            cardDropdown.forEach(function (dropdown) {
                dropdown.addEventListener('mouseleave', function () {
                    this.classList.remove('show');
                    console.log('mouseleave event triggered for card dropdown');
                });
            });
            //const showModalButton = document.getElementById('showModal');
            //showModalButton.addEventListener('click', loadingAllUser());
            console.log("Card-class", cardDropdown)
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




function loadingAllUser() {
    $.ajax({
        url: '/Tasks/GetAllUsers', // URL to the GET method
        type: 'GET',
        success: function (data) {
            // On success, insert the returned HTML (partial view content) into the container
            $('#assignee').html(data);
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


// Method for calling delete method
function conformationDelete(taskId) {
    if (taskId > 0) {
        console.log("task Id", taskId)
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

                fetch(`/Home/DeleteTask?id=${taskId}`, {
                    method: 'PUT'

                }).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Task moved to recycle bin',
                            'success'
                        ).then(() => {
                            location.reload(); // Reload the page to reflect changes
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error!',
                            text: 'Error deleting item',
                            icon: 'AlertType',
                            confirmButtonText: 'OK'
                        });
                    }
                }).catch(error => {
                    console.error('Error:', error);
                    Swal.fire({
                        title: 'Error!',
                        text: 'Error deleting item',
                        icon: 'AlertType',
                        confirmButtonText: 'OK'
                    });
                });
            }
        })
    }
    else {
        Swal.fire({
            title: 'Error!',
            text: 'Missing Id',
            icon: 'AlertType',
            confirmButtonText: 'OK'
        })
    }
}


function showLoader() {
    //$("#preloaded").css("display", "block");
    document.getElementById("preloaded").style.display = "block";
}

function hideLoader() {
    document.getElementById("preloaded").style.display = "none";
}

// function to open deleted tasks page
function openDeletedTasks() {
    fetch('/Home/DeletedTasks')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text(); // expecting HTML content
        })
        .then(data => {
            const activeTab = document.querySelector('.deleted-active-btn');
            if (activeTab) {
                activeTab.classList.add('active');
            }
            // Insert the returned HTML into the container
            document.getElementById('task-item').innerHTML = data;
            localStorage.setItem("currentView", "deleted");
            hideLoader(); // Call your loader hiding function
        })
        .catch(error => {
            console.error('Error fetching deleted tasks:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Error loading items',
                icon: 'error', // corrected icon type
                confirmButtonText: 'OK'
            });
        });
}


// Loading all assigned tasks
function loadAllAssigned() {
    $.ajax({
        url: '/Tasks/GetAllAssignedTask', // URL to the GET method
        type: 'GET',
        success: function (data) {
            const activeTab = document.querySelector('.assign-active-btn');
            if (activeTab) {
                activeTab.classList.add('active');
            }
            // On success, insert the returned HTML (partial view content) into the container
            $('#task-item').html(data);
            localStorage.setItem("currentView", "assigned");
            
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


// updating task status partially
function updateTaskStatus(taskId, status) {
    fetch(`/Tasks/PartialUpdateTaskStatus?taskId=${taskId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify([
            {
                op: "replace",
                path: "/status",
                value: status
            }
        ])
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Failed to update task");
            }
            return response.json();
        })
        .then(data => {
            Swal.fire(
                'Updated!',
                data.message || 'Task status updated successfully',
                'success'
            ).then(() => {
                const currentView = localStorage.getItem("currentView");
                if (currentView === "assigned") {
                    loadAllAssigned();
                }
                else {
                    loadTasks();
                }
            });
        })
        .catch(error => {
            console.error('Error:', error);
            Swal.fire({
                title: 'Error!',
                text: 'Error updating task status',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        });
}

//On change event for status dropdown
