
$(document).ready(function () {
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