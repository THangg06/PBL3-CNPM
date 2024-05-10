<<<<<<< HEAD
﻿const tableBody = document.querySelector('tbody');
const paginationUl = document.querySelector('.pagination ul');

// Sample data (replace with your actual data source)
const data = [
  { id: 1, name: 'Nguyen Thi Thanh Thao', role: 'Admin', phone: '0942.231.231', email: 'nguyenthithanhthao12345@gmail.com', address: '142 Le Random Street, Le Random Kiet' },
  { id: 2, name: 'Le Van Dat', role: 'Nhan Vien', phone: '0912.211.831', email: 'lvd@gmail.com', address: '222 Le Random Street' },
  // Add more data here
];

// Function to display data on a specific page
function displayData(data, page) {
  tableBody.innerHTML = ''; // Clear existing data

  const dataPerPage = 10; // Number of items per page
  const startIndex = (page - 1) * dataPerPage;
  const endIndex = startIndex + dataPerPage;

  for (let i = startIndex; i < endIndex && i < data.length; i++) {
    const row = data[i];
    const tableRow = `<tr>
      <td><span class="math-inline">\{row\.id\}</td\>
<td\></span>{row.name}</td>
      <td><span class="math-inline">\{row\.role\}</td\>
<td\></span>{row.phone}</td>
      <td><span class="math-inline">\{row\.email\}</td\>
<td\></span>{row.address}</td>
    </tr>`;
    tableBody.innerHTML += tableRow;
  }
}

// Function to generate pagination buttons
function generatePaginationButtons(totalPages) {
  paginationUl.innerHTML = ''; // Clear existing buttons

  for (let i = 1; i <= totalPages; i++) {
    const button = document.createElement('li');
    button.classList.add('page-item');
    button.innerHTML = `<a class="page-link" href="#">${i}</a>`;
    button.addEventListener('click', () => handlePageChange(i));
    paginationUl.appendChild(button);
  }
}

// Function to handle page change
function handlePageChange(page) {
  displayData(data, page);
  setActivePageButton(page);
}

// Set active page button
function setActivePageButton(page) {
  const pageButtons = paginationUl.querySelectorAll('.page-link');
  pageButtons.forEach(button => {
    button.classList.remove('active');
    if (button.textContent === page.toString()) {
      button.classList.add('active');
    }
  });
}


// them image cho product
const imageContainer = document.querySelector('.image-container');
const productImage = document.getElementById('productImage');
const fileInput = document.getElementById('fileInput');

imageContainer.addEventListener('click', () => {
  fileInput.click(); // Trigger the file input click event
});

fileInput.addEventListener('change', (event) => {
  const file = event.target.files[0]; // Get the selected file

  if (file) {
    const reader = new FileReader();

    reader.onload = (event) => {
      productImage.src = event.target.result; // Set the image source to the selected file
    };

    reader.readAsDataURL(file); // Read the file as a data URL
  }
});

// script.js

// const btnViewProduct = document.getElementById('btnViewProduct');
// const btnEditProduct = document.getElementById('btnEditProduct');
// const btnViewOrders = document.getElementById('btnViewOrders');


// const pageProduct = document.getElementById('page-1');
// const pageEdit = document.getElementById('page-2');
// const pageOrders = document.getElementById('page-3');

// // Initially show the "Sản phẩm" page
// pageProduct.classList.add('active');

// // Function to handle "Xem sản phẩm" button click
// btnViewProduct.addEventListener('click', () => {
//   showPage(pageProduct, pageEdit, pageOrders);
// });

// // Function to handle "Edit sản phẩm" button click
// btnEditProduct.addEventListener('click', () => {
//   showPage(pageEdit, pageProduct, pageOrders);
// });

// // Function to handle "Xem đơn hàng" button click
// btnViewOrders.addEventListener('click', () => {
//   showPage(pageOrders, pageProduct, pageEdit);
// });

// // Function to show/hide pages
// function showPage(activePage, page1, page2) {
//   activePage.classList.add('active');
//   page1.classList.remove('active');
//   page2.classList.remove('active');
=======
﻿const tableBody = document.querySelector('tbody');
const paginationUl = document.querySelector('.pagination ul');

// Sample data (replace with your actual data source)
const data = [
  { id: 1, name: 'Nguyen Thi Thanh Thao', role: 'Admin', phone: '0942.231.231', email: 'nguyenthithanhthao12345@gmail.com', address: '142 Le Random Street, Le Random Kiet' },
  { id: 2, name: 'Le Van Dat', role: 'Nhan Vien', phone: '0912.211.831', email: 'lvd@gmail.com', address: '222 Le Random Street' },
  // Add more data here
];

// Function to display data on a specific page
function displayData(data, page) {
  tableBody.innerHTML = ''; // Clear existing data

  const dataPerPage = 10; // Number of items per page
  const startIndex = (page - 1) * dataPerPage;
  const endIndex = startIndex + dataPerPage;

  for (let i = startIndex; i < endIndex && i < data.length; i++) {
    const row = data[i];
    const tableRow = `<tr>
      <td><span class="math-inline">\{row\.id\}</td\>
<td\></span>{row.name}</td>
      <td><span class="math-inline">\{row\.role\}</td\>
<td\></span>{row.phone}</td>
      <td><span class="math-inline">\{row\.email\}</td\>
<td\></span>{row.address}</td>
    </tr>`;
    tableBody.innerHTML += tableRow;
  }
}

// Function to generate pagination buttons
function generatePaginationButtons(totalPages) {
  paginationUl.innerHTML = ''; // Clear existing buttons

  for (let i = 1; i <= totalPages; i++) {
    const button = document.createElement('li');
    button.classList.add('page-item');
    button.innerHTML = `<a class="page-link" href="#">${i}</a>`;
    button.addEventListener('click', () => handlePageChange(i));
    paginationUl.appendChild(button);
  }
}

// Function to handle page change
function handlePageChange(page) {
  displayData(data, page);
  setActivePageButton(page);
}

// Set active page button
function setActivePageButton(page) {
  const pageButtons = paginationUl.querySelectorAll('.page-link');
  pageButtons.forEach(button => {
    button.classList.remove('active');
    if (button.textContent === page.toString()) {
      button.classList.add('active');
    }
  });
}


// them image cho product
const imageContainer = document.querySelector('.image-container');
const productImage = document.getElementById('productImage');
const fileInput = document.getElementById('fileInput');

imageContainer.addEventListener('click', () => {
  fileInput.click(); // Trigger the file input click event
});

fileInput.addEventListener('change', (event) => {
  const file = event.target.files[0]; // Get the selected file

  if (file) {
    const reader = new FileReader();

    reader.onload = (event) => {
      productImage.src = event.target.result; // Set the image source to the selected file
    };

    reader.readAsDataURL(file); // Read the file as a data URL
  }
});

// script.js

// const btnViewProduct = document.getElementById('btnViewProduct');
// const btnEditProduct = document.getElementById('btnEditProduct');
// const btnViewOrders = document.getElementById('btnViewOrders');


// const pageProduct = document.getElementById('page-1');
// const pageEdit = document.getElementById('page-2');
// const pageOrders = document.getElementById('page-3');

// // Initially show the "Sản phẩm" page
// pageProduct.classList.add('active');

// // Function to handle "Xem sản phẩm" button click
// btnViewProduct.addEventListener('click', () => {
//   showPage(pageProduct, pageEdit, pageOrders);
// });

// // Function to handle "Edit sản phẩm" button click
// btnEditProduct.addEventListener('click', () => {
//   showPage(pageEdit, pageProduct, pageOrders);
// });

// // Function to handle "Xem đơn hàng" button click
// btnViewOrders.addEventListener('click', () => {
//   showPage(pageOrders, pageProduct, pageEdit);
// });

// // Function to show/hide pages
// function showPage(activePage, page1, page2) {
//   activePage.classList.add('active');
//   page1.classList.remove('active');
//   page2.classList.remove('active');
>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228
// }