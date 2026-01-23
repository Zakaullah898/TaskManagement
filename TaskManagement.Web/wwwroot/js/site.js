// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// JavaScript for sweetalert2
// for home page menues
const tabsbtn = document.querySelectorAll('#menue-tab .btn');

tabsbtn.forEach(link => {
    link.addEventListener('click', function () {
        tabsbtn.forEach(l => l.classList.remove('active'));
        this.classList.add('active');
    });
});