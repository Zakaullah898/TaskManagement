$(document).ready(function () {
    showLoader()
    loadTasks();
});
function loadTasks() {
    $.ajax({
        url: '/Home/GetAllTasks', // URL to the GET method
        type: 'GET',
        success: function (data) {
            // On success, insert the returned HTML (partial view content) into the container
            $('#task-item').html(data);
            hideLoader()
            // Attach mouseleave event listeners to dropdown menus
            const cardDropdown = document.querySelectorAll('.dropdown-menu-card');
            cardDropdown.forEach(function (dropdown) {
                dropdown.addEventListener('mouseleave', function () {
                    this.classList.remove('show');
                    console.log('mouseleave event triggered for card dropdown');
                });
            });

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
    $.ajax({
        url: '/Home/DeletedTasks', // URL to the GET method
        type: 'GET',
        success: function (data) {
            // On success, insert the returned HTML (partial view content) into the container
            $('#task-item').html(data);
            hideLoader()
            // Attach mouseleave event listeners to dropdown menus
            const cardDropdown = document.querySelectorAll('.dropdown-menu-card');
            cardDropdown.forEach(function (dropdown) {
                dropdown.addEventListener('mouseleave', function () {
                    this.classList.remove('show');
                    console.log('mouseleave event triggered for card dropdown');
                });
            });

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
        }
    });

}