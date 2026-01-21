// Write your JavaScript code.
const sidebar = document.getElementById('sidebar');
const dropdownElement = document.querySelector('.dropdown-menu');

sidebar.addEventListener('mouseenter', () => {
    sidebar.classList.remove('sidebar-collapsed');
});

sidebar.addEventListener('mouseleave', () => {
    sidebar.classList.add('sidebar-collapsed');
    dropdownElement.classList.remove('show');
    dropdownElement.setAttribute('aria-expanded', 'false');
});
//console.log('Sidebar hover functionality loaded', sidebar);
// Active link highlighting
const menuLinks = document.querySelectorAll('#sidebarMenu .nav-link');

menuLinks.forEach(link => {
    link.addEventListener('click', function () {
        menuLinks.forEach(l => l.classList.remove('active'));
        this.classList.add('active');
    });
});







