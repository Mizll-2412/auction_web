function filterPosts() {
    // Similar to filterUsers
}

function openAddPostForm() {
    // TODO
}

function editPost(id) {
    window.location.href = `/Admin/EditPost?id=${id}`;
}

function deletePost(id) {
    if (confirm('Xóa bài đăng này?')) {
        fetch(`/Admin/DeletePost?id=${id}`, { method: 'DELETE' })
            .then(response => response.json())
            .then(result => {
                if (result.success) {
                    document.getElementById(`post-row-${id}`).remove();
                }
            });
    }
}

function viewPost(id) {
    window.open(`/Post/Detail?id=${id}`, '_blank');
}

function exportPosts() {
    window.location.href = '/Admin/ExportPosts';
}