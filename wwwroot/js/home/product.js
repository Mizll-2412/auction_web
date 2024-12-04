document.addEventListener('DOMContentLoaded', function() {
    const categoryItems = document.querySelectorAll('.category-item');
    const productContainer = document.querySelector('.product-container');

    categoryItems.forEach(item => {
        item.addEventListener('click', async function(e) {
            e.preventDefault();
            const categoryId = this.getAttribute('data-category-id');
            await fetchAndDisplayProducts(categoryId);
        });
    });

    async function fetchAndDisplayProducts(categoryId) {
        try {
            const response = await fetch(`/Product/GetProductsByCategory?categoryId=${categoryId}`);
            const products = await response.json();
            productContainer.innerHTML = '';
            products.forEach(product => {
                const productItem = createProductItem(product);
                productContainer.appendChild(productItem);
            });
        } catch (error) {
            console.error('Error:', error);
        }
    }

    function createProductItem(product) {
        const productItem = document.createElement('div');
        productItem.className = 'product-item';
        return productItem;
    }
});