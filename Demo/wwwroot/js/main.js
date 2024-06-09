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


document.addEventListener('DOMContentLoaded', (event) => {
    const searchBox = document.getElementById('search-box');

    const resultBox = document.querySelector('.result-box');

    const data = [
        'Apple',
        'Banana',
        'Cherry',
        'Date',
        'Fig',
        'Grape',
        'Kiwi'
    ];



    searchBox.addEventListener('keyup', (event) => {
        performSearch(searchBox.value);
    });

    function performSearch(keyword) {
        if (keyword.trim() === '') {
            resultBox.innerHTML = ''; // Clear results when input is empty
            return;
        }

        const results = data.filter(item => item.toLowerCase().includes(keyword.toLowerCase()));
        displayResults(results);
    }

    function displayResults(results) {
        if (results.length) {
            const content = results.map(item => `<li onclick="selectResult('${item}')">${item}</li>`).join('');
            resultBox.innerHTML = `<ul>${content}</ul>`;
        } else {
            resultBox.innerHTML = ''; // Clear results when no matches found
        }
    }

    window.selectResult = function (item) {
        searchBox.value = item; // Update input box with the selected result
        resultBox.innerHTML = ''; // Clear results
    }
});