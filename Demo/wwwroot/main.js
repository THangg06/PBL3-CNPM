<<<<<<< HEAD
<<<<<<< HEAD
const productRows = document.querySelectorAll('.product_row');
const buttons = document.querySelectorAll('.choose_food');

// Ẩn tất cả các danh mục sản phẩm ban đầu (tùy chọn)
productRows.forEach(productRow => productRow.style.display = 'none');

// Hiển thị danh mục mặc định (tùy chọn, ví dụ: Burger)
document.getElementById('Burger').style.display = 'grid';

buttons.forEach(button => {
  button.addEventListener('click', () => {
    const productRowId = button.id.replace('btn-', '');
    const productRow = document.getElementById(productRowId);

    // Ẩn tất cả các danh mục sản phẩm
    productRows.forEach(productRow => productRow.style.display = 'none');

    // Hiển thị danh mục sản phẩm được chọn
    productRow.style.display = 'grid';
  });
=======
=======
>>>>>>> 06235904bb357cf68a24e145336d06811074051c
const productRows = document.querySelectorAll('.product_row');
const buttons = document.querySelectorAll('.choose_food');

// Ẩn tất cả các danh mục sản phẩm ban đầu (tùy chọn)
productRows.forEach(productRow => productRow.style.display = 'none');

// Hiển thị danh mục mặc định (tùy chọn, ví dụ: Burger)
document.getElementById('Burger').style.display = 'grid';

buttons.forEach(button => {
  button.addEventListener('click', () => {
    const productRowId = button.id.replace('btn-', '');
    const productRow = document.getElementById(productRowId);

    // Ẩn tất cả các danh mục sản phẩm
    productRows.forEach(productRow => productRow.style.display = 'none');

    // Hiển thị danh mục sản phẩm được chọn
    productRow.style.display = 'grid';
  });
<<<<<<< HEAD
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228
=======
>>>>>>> 06235904bb357cf68a24e145336d06811074051c
});