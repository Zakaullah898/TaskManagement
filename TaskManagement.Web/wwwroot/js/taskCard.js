$(document).ready(function () {

    loadTasks();
});
function loadTasks() {
    $.ajax({
        url: '/Home/GetAllTasks', // URL to the GET method
        type: 'GET',
        success: function (data) {
            // On success, insert the returned HTML (partial view content) into the container
            $('#task-item').html(data);

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
            alert('Error loading items.');
        }
    });
}
