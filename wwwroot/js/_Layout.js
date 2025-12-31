

document.getElementById("loginForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;
    const errorDiv = document.getElementById("loginError");

    fetch("/Account/LoginAjax", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            SEmail: email,
            SPassword: password
        })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                location.reload();
            } else {
                errorDiv.innerText = data.message;
                errorDiv.style.display = "block";
            }
        })
        .catch(err => {
            errorDiv.innerText = "Có lỗi xảy ra, vui lòng thử lại!";
            errorDiv.style.display = "block";
        });
});

document.getElementById("registerForm").addEventListener("submit", function (e) {
    e.preventDefault();

    const data = {
        SEmail: this.SEmail.value,
        SFullName: this.SFullName.value,
        SPhoneNumber: this.SPhoneNumber.value,
        SPassword: this.SPassword.value,
        RePassword: this.RePassword.value
    };

    const errorDiv = document.getElementById("registerError");

    fetch("/Account/RegisterAjax", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    })
        .then(res => res.json())
        .then(result => {
            if (result.success) {
                alert("Đăng ký thành công!");
                const modal = bootstrap.Modal.getInstance(document.getElementById("registerModal"));
                modal.hide();
                location.reload();
            } else {
                errorDiv.innerText = result.message;
                errorDiv.style.display = "block";
            }
        })
        .catch(err => {
            errorDiv.innerText = "Có lỗi xảy ra, vui lòng thử lại!";
            errorDiv.style.display = "block";
        });
});
document.addEventListener('DOMContentLoaded', function () {
    const notificationBell = document.getElementById('notificationBell');
    const notificationDropdown = document.getElementById('notificationDropdown');
    const markAllRead = document.getElementById('markAllRead');
    const notificationList = document.getElementById('notificationList');
    const notificationCount = document.getElementById('notificationCount');

    if (notificationBell) {
        notificationBell.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            notificationDropdown.classList.toggle('show');
            if (notificationDropdown.classList.contains('show')) {
                loadNotifications();
            }
        });
    }

    document.addEventListener('click', function (e) {
        if (!notificationBell?.contains(e.target) && !notificationDropdown?.contains(e.target)) {
            notificationDropdown?.classList.remove('show');
        }
    });
    if (markAllRead) {
        markAllRead.addEventListener('click', function () {
            fetch('/Notification/MarkAllAsRead', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        loadNotifications();
                        updateNotificationCount(0);
                    }
                })
                .catch(error => console.error('Error:', error));
        });
    }

    function loadNotifications() {
        fetch('/Notification/GetNotifications')
            .then(response => response.json())
            .then(data => {
                if (data.success && data.notifications) {
                    renderNotifications(data.notifications);
                    updateNotificationCount(data.unreadCount);
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showSampleNotifications();
            });
    }
    function renderNotifications(notifications) {
        if (!notifications || notifications.length === 0) {
            notificationList.innerHTML = `
                    <div class="notification-empty">
                        <i class="fa-solid fa-bell-slash fa-2x mb-2"></i>
                        <p>Không có thông báo nào</p>
                    </div>
                `;
            return;
        }

        notificationList.innerHTML = notifications.map(notif => `
                <div class="notification-item ${notif.isRead ? '' : 'unread'}" 
                     onclick="markAsRead(${notif.id}, '${notif.url}')">
                    <div class="notification-title">${notif.title}</div>
                    <div class="notification-message">${notif.message}</div>
                    <div class="notification-time">${notif.timeAgo}</div>
                </div>
            `).join('');
    }
    function showSampleNotifications() {
        const sampleNotifications = [
            {
                id: 1,
                title: '🎉 Bạn đã thắng đấu giá!',
                message: 'Chúc mừng! Bạn đã thắng phiên đấu giá "iPhone 15 Pro Max"',
                timeAgo: '5 phút trước',
                isRead: false,
                url: '/Auction/Details/1'
            },
            {
                id: 2,
                title: '⏰ Sắp kết thúc',
                message: 'Phiên đấu giá "Laptop Dell XPS 13" sẽ kết thúc trong 10 phút',
                timeAgo: '15 phút trước',
                isRead: false,
                url: '/Auction/Details/2'
            },
            {
                id: 3,
                title: '💰 Có người đấu giá cao hơn',
                message: 'Ai đó đã đặt giá cao hơn bạn cho "Samsung Galaxy S24"',
                timeAgo: '1 giờ trước',
                isRead: false,
                url: '/Auction/Details/3'
            },
            {
                id: 4,
                title: '✅ Thanh toán thành công',
                message: 'Đơn hàng #12345 đã được thanh toán thành công',
                timeAgo: '2 giờ trước',
                isRead: true,
                url: '/Transaction/Details/12345'
            }
        ];
        renderNotifications(sampleNotifications);
        updateNotificationCount(3);
    }
    function updateNotificationCount(count) {
        if (notificationCount) {
            if (count > 0) {
                notificationCount.textContent = count > 9 ? '9+' : count;
                notificationCount.style.display = 'block';
            } else {
                notificationCount.style.display = 'none';
            }
        }
    }
    window.markAsRead = function (notificationId, url) {
        fetch('/Notification/MarkAsRead', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ notificationId: notificationId })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success && url) {
                    window.location.href = url;
                }
            })
            .catch(error => console.error('Error:', error));
    };
    showSampleNotifications();
});