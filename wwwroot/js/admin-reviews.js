function filterReviews() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const starFilter = document.getElementById('starFilter').value;
    const statusFilter = document.getElementById('statusFilter').value;
    
    const rows = document.querySelectorAll('#reviewsTable tbody tr');
    
    rows.forEach(row => {
        const rating = row.getAttribute('data-rating');
        const status = row.getAttribute('data-status');
        const product = row.querySelector('td:nth-child(3)').textContent.toLowerCase();
        const user = row.querySelector('td:nth-child(2)').textContent.toLowerCase();
        
        let show = true;
        
        if (searchInput && !product.includes(searchInput) && !user.includes(searchInput)) {
            show = false;
        }
        
        if (starFilter && rating !== starFilter) {
            show = false;
        }
        
        if (statusFilter && status !== statusFilter) {
            show = false;
        }
        
        row.style.display = show ? '' : 'none';
    });
}

function clearFilters() {
    document.getElementById('searchInput').value = '';
    document.getElementById('starFilter').value = '';
    document.getElementById('statusFilter').value = '';
    document.getElementById('sortFilter').value = 'newest';
    filterReviews();
}