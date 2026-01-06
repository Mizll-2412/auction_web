
// Navigation between sections
function showSection(section) {
    document.querySelectorAll('.section').forEach(s => s.classList.remove('active'));
    document.querySelectorAll('.menu-item').forEach(item => {
        item.classList.remove('active');
    });
    
    document.getElementById(section + '-section').classList.add('active');
    event.target.classList.add('active');
}

// Modal functions
function openAddForm() {
    document.getElementById('addForm').style.display = 'block';
}

function closeAddForm() {
    document.getElementById('addForm').style.display = 'none';
    document.getElementById('addUserForm').reset();
}

function openEditModal(userId) {
    fetch(`/Admin/GetUserDetails?userId=${userId}`)
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById("editUserId").value = data.user.iUserId;
                document.getElementById("editFullName").value = data.user.sFullName;
                document.getElementById("editEmail").value = data.user.sEmail;
                document.getElementById("editRole").value = data.user.sRole;
                document.getElementById('editForm').style.display = 'block';
            } else {
                alert('Không thể lấy thông tin người dùng.');
            }
        })
        .catch(() => alert('Đã xảy ra lỗi khi lấy dữ liệu.'));
}

function closeEditForm() {
    document.getElementById('editForm').style.display = 'none';
}

function confirmDelete(userId) {
    if (confirm('Bạn có chắc chắn muốn xóa người dùng này?')) {
        fetch(`/Admin/DeleteUserAJAX?userId=${userId}`, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' }
        })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                const row = document.getElementById(`user-row-${userId}`);
                if (row) {
                    row.remove();
                }
                showToast('Xóa người dùng thành công!', 'success');
            } else {
                showToast('Không thể xóa: ' + result.message, 'danger');
            }
        })
        .catch(() => showToast('Đã xảy ra lỗi khi xóa người dùng.', 'danger'));
    }
}

function addUser() {
    const fullName = document.getElementById('addFullName').value;
    const email = document.getElementById('addEmail').value;
    const role = document.getElementById('addRole').value;
    const password = document.getElementById('addPass').value;
    const phone = document.getElementById('addPhone').value;
    const key = document.getElementById('Key').value;

    const user = {
        SFullName: fullName,
        SEmail: email,
        SRole: role,
        SPassword: password,
        SPhoneNumber: phone,
        VerifyKey: key
    };

    fetch('/Admin/AddUser', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            showToast('Thêm người dùng thành công!', 'success');
            closeAddForm();
            location.reload();
        } else {
            showToast('Thêm người dùng thất bại: ' + result.message, 'danger');
        }
    })
    .catch(() => showToast('Đã xảy ra lỗi khi thêm người dùng.', 'danger'));
}

function updateUser() {
    const id = document.getElementById('editUserId').value;
    const fullName = document.getElementById('editFullName').value;
    const email = document.getElementById('editEmail').value;
    const role = document.getElementById('editRole').value;

    const user = {
        iUserId: id,
        SFullName: fullName,
        SEmail: email,
        SRole: role
    };

    fetch('/Admin/UpdateUser', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    })
    .then(response => response.json())
    .then(result => {
        if (result.success) {
            showToast('Cập nhật thành công!', 'success');
            closeEditForm();
            location.reload();
        } else {
            showToast('Cập nhật thất bại: ' + result.message, 'danger');
        }
    })
    .catch(() => showToast('Đã xảy ra lỗi khi cập nhật.', 'danger'));
}

// Filter functions
function filterUsers() {
    const searchInput = document.getElementById('searchInput').value.toLowerCase();
    const roleFilter = document.getElementById('roleFilter').value;
    const dateFilter = document.getElementById('dateFilter').value;
    const statusFilter = document.getElementById('statusFilter').value;
    
    const rows = document.querySelectorAll('#usersTable tbody tr');
    let visibleCount = 0;
    
    rows.forEach(row => {
        const name = row.querySelector('td:nth-child(3)').textContent.toLowerCase();
        const email = row.querySelector('td:nth-child(4)').textContent.toLowerCase();
        const role = row.getAttribute('data-role');
        const date = row.getAttribute('data-date');
        
        let showRow = true;
        
        // Search filter
        if (searchInput && !name.includes(searchInput) && !email.includes(searchInput)) {
            showRow = false;
        }
        
        // Role filter
        if (roleFilter && role !== roleFilter) {
            showRow = false;
        }
        
        // Date filter
        if (dateFilter && date) {
            const rowDate = new Date(date);
            const today = new Date();
            
            switch(dateFilter) {
                case 'today':
                    if (rowDate.toDateString() !== today.toDateString()) showRow = false;
                    break;
                case 'week':
                    const weekAgo = new Date(today.setDate(today.getDate() - 7));
                    if (rowDate < weekAgo) showRow = false;
                    break;
                case 'month':
                    const monthAgo = new Date(today.setMonth(today.getMonth() - 1));
                    if (rowDate < monthAgo) showRow = false;
                    break;
                case 'year':
                    const yearAgo = new Date(today.setFullYear(today.getFullYear() - 1));
                    if (rowDate < yearAgo) showRow = false;
                    break;
            }
        }
        
        row.style.display = showRow ? '' : 'none';
        if (showRow) visibleCount++;
    });
    
    document.getElementById('showingCount').textContent = visibleCount;
}

function clearFilters() {
    document.getElementById('searchInput').value = '';
    document.getElementById('roleFilter').value = '';
    document.getElementById('dateFilter').value = '';
    document.getElementById('statusFilter').value = '';
    filterUsers();
}

// Sort table
let sortDirection = {};

function sortTable(columnIndex) {
    const table = document.getElementById('usersTable');
    const tbody = table.querySelector('tbody');
    const rows = Array.from(tbody.querySelectorAll('tr'));
    
    const direction = sortDirection[columnIndex] === 'asc' ? 'desc' : 'asc';
    sortDirection[columnIndex] = direction;
    
    rows.sort((a, b) => {
        const aValue = a.querySelector(`td:nth-child(${columnIndex + 1})`).textContent.trim();
        const bValue = b.querySelector(`td:nth-child(${columnIndex + 1})`).textContent.trim();
        
        if (direction === 'asc') {
            return aValue.localeCompare(bValue, 'vi');
        } else {
            return bValue.localeCompare(aValue, 'vi');
        }
    });
    
    rows.forEach(row => tbody.appendChild(row));
}

// Select all checkboxes
function toggleSelectAll() {
    const selectAll = document.getElementById('selectAll');
    const checkboxes = document.querySelectorAll('.user-checkbox');
    checkboxes.forEach(checkbox => {
        checkbox.checked = selectAll.checked;
    });
}

// Export to Excel
function exportUsers() {
    window.location.href = '/Admin/ExportUsers';
}

// View user details
function viewUser(userId) {
    window.open(`/Admin/ViewUser?userId=${userId}`, '_blank');
}

// Toast notification
function showToast(message, type = 'success') {
    const toast = document.createElement('div');
    toast.className = `alert alert-${type}`;
    toast.style.position = 'fixed';
    toast.style.top = '20px';
    toast.style.right = '20px';
    toast.style.zIndex = '9999';
    toast.style.minWidth = '300px';
    toast.style.animation = 'slideIn 0.3s ease';
    toast.textContent = message;
    
    document.body.appendChild(toast);
    
    setTimeout(() => {
        toast.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => {
            document.body.removeChild(toast);
        }, 300);
    }, 3000);
}

// Close modal with Escape key
document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        const modals = document.querySelectorAll('.modal');
        modals.forEach(modal => {
            if (modal.style.display === 'block') {
                modal.style.display = 'none';
            }
        });
    }
});

// Close modal when clicking outside
window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
        event.target.style.display = 'none';
    }
}
