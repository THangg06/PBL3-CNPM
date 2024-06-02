//const productRows = document.querySelectorAll('.product_row');
//const buttons = document.querySelectorAll('.choose_food');

//// Ẩn tất cả các danh mục sản phẩm ban đầu (tùy chọn)
//productRows.forEach(productRow => productRow.style.display = 'none');

//// Hiển thị danh mục mặc định (tùy chọn, ví dụ: Burger)
//document.getElementById('Burger').style.display = 'grid';

//buttons.forEach(button => {
//  button.addEventListener('click', () => {
//    const productRowId = button.id.replace('btn-', '');
//    const productRow = document.getElementById(productRowId);

//    // Ẩn tất cả các danh mục sản phẩm
//    productRows.forEach(productRow => productRow.style.display = 'none');

//    // Hiển thị danh mục sản phẩm được chọn
//    productRow.style.display = 'grid';
//  });
//});

function updateContainerClass() {
    const container = document.querySelector('.body .container .data_table_container .data-table');
    const containeradd = document.querySelector('.body .container .data_table_container');
    const containerWidth = container.offsetWidth;
    const containerHeight = container.offsetHeight;

    if (containerWidth >= 1200 || containerHeight >= 650) {
        containeradd.classList.add('large');

    } else {

        containeradd.classList.remove('large');
    }
}

window.addEventListener('resize', updateContainerClass);
document.addEventListener('DOMContentLoaded', updateContainerClass);