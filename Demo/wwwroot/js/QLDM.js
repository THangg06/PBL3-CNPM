
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function updateContainerClass() {
    const container = document.querySelector('.body .container .data_table_container .data-table');
    const containeradd = document.querySelector('.body .container .data_table_container');
    const containerWidth = container.offsetWidth;
    const containerHeight = container.offsetHeight;


    if (containerWidth >= 1200) {
        if (containerHeight >= 700)
            containeradd.classList.add('large');
        else containeradd.classList.add('largeW');
    }
    else if (containerHeight >= 700) {
        containeradd.classList.add('largeH');
    }
    else {
        containeradd.classList.remove('large');
    }
}

window.addEventListener('resize', updateContainerClass);
document.addEventListener('DOMContentLoaded', updateContainerClass);