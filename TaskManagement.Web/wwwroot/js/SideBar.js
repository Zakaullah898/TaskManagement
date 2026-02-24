document.addEventListener('DOMContentLoaded', function ()
{

    const activeLink = localStorage.getItem('activeLink');
    if (activeLink) {
        const link = document.getElementById(activeLink);
        if (link) link.classList.add('active');
    }
    else
    {
        // If no active link is stored, you can set a default active link here if needed
        const defaultLink = document.getElementById('home'); // Replace with your default link ID
        if (defaultLink) defaultLink.classList.add('active');
    }
    console.log('Clicked link:', localStorage.getItem("activeLink"));
    console.log('Sidebar script loaded');
});
// Write your JavaScript code.
const sidebar = document.getElementById('sidebar');
const main = document.querySelector('.render-body');
const dropdownElement = document.querySelector('.dropdown-menu');

sidebar.addEventListener('mouseenter', () => {
    sidebar.classList.remove('sidebar-collapsed');
    sidebar.classList.add('expanded');
    main.classList.add('main-expanded');
});

sidebar.addEventListener('mouseleave', () => {
    sidebar.classList.add('sidebar-collapsed');
    sidebar.classList.remove('expanded');
    main.classList.remove('main-expanded');
    dropdownElement.classList.remove('show');
    dropdownElement.setAttribute('aria-expanded', 'false');
    
    
});

//console.log('Sidebar hover functionality loaded', sidebar);
// Active link highlighting
const sidebarMenu = document.getElementById('sidebarMenu');

sidebarMenu.addEventListener('click', function (e) {
    const clickedLink = e.target.closest('.nav-link');
    if (!clickedLink) return; // Ignore clicks outside links

    const linkId = clickedLink.id;
    console.log('Clicked link ID:', linkId);

    // Save active link
    localStorage.setItem('activeLink', linkId);

    // Remove active class from all links
    const menuLinks = sidebarMenu.querySelectorAll('.nav-link');
    menuLinks.forEach(link => link.classList.remove('active'));

    // Add active class to clicked link
    clickedLink.classList.add('active');
});









