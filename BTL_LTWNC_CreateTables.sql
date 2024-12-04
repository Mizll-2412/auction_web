create database db_BTL_LTWNC
USE db_BTL_LTWNC

CREATE TABLE tblUsers (
    iUserId INT PRIMARY KEY IDENTITY(1,1),
    sUsername NVARCHAR(50) NOT NULL,
    sPassword NVARCHAR(100) NOT NULL,
    sEmail NVARCHAR(100) NOT NULL,
    sFullName NVARCHAR(100) NOT NULL,
    sPhoneNumber NVARCHAR(20),
);

alter table tblUsers add sRole NVARCHAR(20) CHECK (sRole IN (N'Người dùng', N'Quản trị viên'))
update tblUsers
set sRole = N'Quản trị viên'
where iUserId = 14

select * from tblUsers
CREATE TABLE tblCategories (
    iCategoryId INT PRIMARY KEY IDENTITY(1,1),
    sCategoryName NVARCHAR(100) NOT NULL,
    sDescription NVARCHAR(255)
);


CREATE TABLE tblProducts (
    iProductId INT PRIMARY KEY IDENTITY(1,1),
    iSellerId INT FOREIGN KEY REFERENCES tblUsers(iUserId),
    sProductName NVARCHAR(100) NOT NULL,
    sDescription NVARCHAR(255),
    dStartingPrice DECIMAL(18,2) NOT NULL,
    iCategoryId INT FOREIGN KEY REFERENCES tblCategories(iCategoryId),
    sImageUrl NVARCHAR(255),
    sStatus NVARCHAR(20) CHECK (sStatus IN (N'Còn hàng', N'Đã bán', N'Hết hàng'))
);


CREATE TABLE tblAuctions (
    iAuctionId INT PRIMARY KEY IDENTITY(1,1),
    iProductId INT FOREIGN KEY REFERENCES tblProducts(iProductId),
    dtStartTime DATETIME NOT NULL,
    dtEndTime DATETIME NOT NULL,
    dHighestBid DECIMAL(18,2),
    iWinnerId INT FOREIGN KEY REFERENCES tblUsers(iUserId),
    sStatus NVARCHAR(20) CHECK (sStatus IN (N'Đang diễn ra', N'Đã kết thúc',N'Sắp diễn ra'))
);


CREATE TABLE tblBids (
    iBidId INT PRIMARY KEY IDENTITY(1,1),
    iAuctionId INT FOREIGN KEY REFERENCES tblAuctions(iAuctionId),
    iBidderId INT FOREIGN KEY REFERENCES tblUsers(iUserId),
    dBidAmount DECIMAL(18,2) NOT NULL,
    dtBidTime DATETIME DEFAULT GETDATE()
);


CREATE TABLE tblTransactions (
    iTransactionId INT PRIMARY KEY IDENTITY(1,1),
    iAuctionId INT FOREIGN KEY REFERENCES tblAuctions(iAuctionId),
    iBuyerId INT FOREIGN KEY REFERENCES tblUsers(iUserId),
    dAmount DECIMAL(18,2) NOT NULL,
    dtTransactionTime DATETIME DEFAULT GETDATE()
);


CREATE TABLE tblReviews (
    iReviewId INT PRIMARY KEY IDENTITY(1,1),
    iProductId INT FOREIGN KEY REFERENCES tblProducts(iProductId),
    iReviewerId INT FOREIGN KEY REFERENCES tblUsers(iUserId),
    iRating INT CHECK (iRating BETWEEN 1 AND 5),
    sComment NVARCHAR(255),
    dtReviewTime DATETIME DEFAULT GETDATE()
);


CREATE TABLE tblWatchlist (
    iWatchlistId INT PRIMARY KEY IDENTITY(1,1),
    iUserId INT FOREIGN KEY REFERENCES tblUsers(iUserId),
    iProductId INT FOREIGN KEY REFERENCES tblProducts(iProductId),
    dtAddedTime DATETIME DEFAULT GETDATE()
);
ALTER TABLE tblUsers DROP COLUMN sUsername
select * from tblUsers
-- Insert records into tblUsers
INSERT INTO tblUsers (sPassword, sEmail, sFullName, sPhoneNumber)
VALUES 
('password123', 'nguyenvana@gmail.com', N'Nguyễn Văn A', '0912345678'),
( 'password456', 'tranthib@gmail.com', N'Trần Thị B', '0912345679'),
('password789', 'ledong@gmail.com', N'Lê Đỗ Ng', '0912345680'),
( 'password1234', 'phamthic@gmail.com', N'Phạm Thị C', '0912345681'),
( 'password5678', 'nguyenphucd@gmail.com', N'Nguyễn Phúc D', '0912345682');

ALTER TABLE tblCategories ADD ImageUrl text;
select * from tblCategories
-- Insert records into tblCategories
INSERT INTO tblCategories (sCategoryName, sDescription,ImageUrl)
VALUES 
(N'Đồ điện tử', N'Sản phẩm điện tử như điện thoại, máy tính, máy ảnh...','https://owa.bestprice.vn/images/articles/uploads/huong-dan-cach-mua-do-dien-tu-o-thai-lan-chat-luong-nhat-gia-tot-nhat-5e821e1c413fb.jpg'),
(N'Nội thất', N'Nội thất gia đình như bàn, ghế, tủ, giường...','https://anviethouse.vn/wp-content/uploads/2020/05/n%E1%BB%99i-th%E1%BA%A5t-g%E1%BB%97-g%C3%B5-%C4%91%E1%BB%8F-ph%C3%B2ng-kh%C3%A1ch-e1620267359280.jpg'),
(N'Thời trang', N'Sản phẩm thời trang như quần áo, giày dép, phụ kiện...','https://viracresearch.com/wp-content/uploads/2021/05/image.jpg'),
(N'Đồ gia dụng', N'Sản phẩm sử dụng trong gia đình như nồi cơm, lò vi sóng...','https://locknlockvietnam.com/wp-content/uploads/2023/05/10-mon-do-gia-dung-khong-the-thieu-trong-gia-dinh-1.jpg'),
(N'Tranh', N'Tranh cổ điển, tranh hiện đại,...','https://images2.thanhnien.vn/528068263637045248/2024/1/18/famous-landscape-paintings-03-1705584669451290925083.jpg');

-- Insert records into tblProducts
select * from tblProducts
INSERT INTO tblProducts (iSellerId, sProductName, sDescription, dStartingPrice, iCategoryId, sImageUrl, sStatus)
VALUES 
(14, N'Điện thoại iPhone 13', N'Điện thoại iPhone 13 128GB', 15000000, 1, 'https://cdn2.cellphones.com.vn/insecure/rs:fill:358:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/i/p/iphone-13-02_4.jpg', N'Còn hàng'),
(13, N'Máy tính MacBook Air', N'Máy tính MacBook Air M1 256GB', 25000000, 1, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT95SAb6py9dydN3tEKKrjtQK03JUwBvzX2UQ&s', N'Còn hàng'),
(14, N'Tủ quần áo', N'Tủ quần áo gỗ tự nhiên 3 cánh', 8000000, 2, 'https://bizweb.dktcdn.net/100/391/155/files/tu-ao-canh-kinh-tada-tdta18-2.jpg?v=1700627228445', N'Còn hàng'),
(13, N'Ghế sofa', N'Ghế sofa nỉ màu xám', 6000000, 2, 'https://noithatthienvuong.com/wp-content/uploads/2019/12/sofa-vang-ni-3.jpg', N'Còn hàng'),
(14, N'Áo sơ mi nam', N'Áo sơ mi trắng nam size M', 300000, 3, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTx-KRstIBuM2K0Irz_gb-vDsCIRBbQmfXNWg&s', N'Còn hàng'),
(13, N'Giày thể thao', N'Giày thể thao Adidas', 1500000, 3, 'https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/fed489fa808240528459af3401410468_9366/Giay_Ultrabounce_trang_HP5772_01_standard.jpg', N'Còn hàng'),
(15, N'Nồi chiên không dầu', N'Nồi chiên dung tích 5L', 2000000, 4, 'https://elmich.vn/wp-content/uploads/2023/10/z3086489137864_27956e1bbd59ea8f5866c9b6d011223c-1.jpg', N'Còn hàng'),
(14, N'Lò vi sóng', N'Lò vi sóng Samsung 23L', 1800000, 4, 'https://mediamart.vn/images/uploads/2023/9dd64826-0c5e-4e75-8f54-91a2f7ed78ea.jpg', N'Còn hàng'),
(15, N'Sách lập trình Python', N'Sách học lập trình Python cho người mới bắt đầu', 150000, 5, 'https://www.magazinesdirect.com/images/covers/vlarge-BKZ-B6622.jpg', N'Còn hàng'),
(13, N'Tiểu thuyết Harry Potter', N'Tiểu thuyết Harry Potter tập 1', 250000, 5, 'https://m.media-amazon.com/images/I/81DI+BAN2SL._AC_UF1000,1000_QL80_.jpg', N'Còn hàng'),
(14, N'Điện thoại Samsung Galaxy', N'Điện thoại Samsung Galaxy A32', 12000000, 1, 'https://cdn.tgdd.vn/Products/Images/42/234315/samsung-galaxy-a32-4g-thumb-tim-600x600-600x600.jpg', N'Còn hàng'),
(15, N'Tủ lạnh Toshiba', N'Tủ lạnh Toshiba 200L', 10000000, 4, 'https://cdn.tgdd.vn/Products/Images/1943/228369/toshiba-gr-rf610we-pgv-22-xk-ava-1-600x600.jpg', N'Còn hàng'),
(13, N'Bàn làm việc', N'Bàn làm việc chân sắt mặt gỗ', 3500000, 2, 'https://product.hstatic.net/1000078439/product/0_a5459a08e3f8414ca529dbf3b311c5d4.png', N'Còn hàng'),
(14, N'Quần jeans nam', N'Quần jeans xanh nam size L', 500000, 3, 'https://360.com.vn/wp-content/uploads/2024/06/QJDTK502-2.jpg', N'Còn hàng'),
(15, N'Máy giặt LG', N'Máy giặt LG 9kg', 7000000, 4, 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSeGkjBm7E0jZW2m-MWDs9rp1ODYwPl19425Q&s', N'Còn hàng'),
(14, N'Điện thoại Xiaomi Redmi', N'Điện thoại Xiaomi Redmi Note 10', 8000000, 1, 'https://cdn.tgdd.vn/Products/Images/42/309831/xiaomi-redmi-note-13-gold-thumb-600x600.jpg', N'Còn hàng'),
(13, N'Sách toán học', N'Sách toán lớp 12', 80000, 5, 'https://bookcoverssl.com/wp-content/uploads/2023/10/Simple-Mathematics.jpg', N'Còn hàng'),
(15, N'Tủ gỗ', N'Tủ gỗ đựng sách', 5000000, 2, 'https://dongianladep.vn/wp-content/uploads/2020/09/8.jpg', N'Còn hàng'),
(14, N'Áo khoác nam', N'Áo khoác nam mùa đông', 700000, 3, 'https://product.hstatic.net/200000574651/product/gjk0064-1_bb-46_eab2105458844fa6ad29f7061c81315c_master.jpg', N'Còn hàng'),
(13, N'Máy ảnh Canon', N'Máy ảnh Canon EOS 200D', 15000000, 1, 'https://product.hstatic.net/200000354621/product/canon-200d-mark-ii-1_583ee9956747431881948031f0eb7fe0.png', N'Còn hàng');

select * from tblAuctions
-- Insert records into tblAuctions
INSERT INTO tblAuctions (iProductId, dtStartTime, dtEndTime, dHighestBid, iWinnerId, sStatus)
VALUES 
(3, '12-12-2023','12-13-2023', 0, 8, N'Đã kết thúc'),
(4, '11-11-2024','11-12-2024', 0, 12, N'Sắp diễn ra'),
(7, '11-05-2024', '11-20-2024', 0, 13, N'Đang diễn ra'),
(5, '11-05-2024', '11-21-2024', 0, 16, N'Đang diễn ra'),
(6, '11-05-2024', '11-22-2024', 0, 18, N'Đang diễn ra')
select * from tblAuctions

delete from tblAuctions
--insert đến đây 
-- Insert records into tblBids
INSERT INTO tblBids (iAuctionId, iBidderId, dBidAmount)
VALUES 
(19, 8, 15500000),
(20, 12, 25500000),
(21, 13, 8100000),
(22, 16, 6200000),
(23, 18, 320000);

-- Insert records into tblTransactions
INSERT INTO tblTransactions (iAuctionId, iBuyerId, dAmount)
VALUES 
(19, 8, 15500000),
(20, 12, 25500000);

-- Insert records into tblReviews
INSERT INTO tblReviews (iProductId, iReviewerId, iRating, sComment)
VALUES 
(1, 2, 5, N'Sản phẩm rất tốt, giá cả hợp lý!'),
(2, 4, 4, N'Máy chạy ổn, pin lâu nhưng hơi nặng.');
select * from tblProducts
-- Insert records into tblWatchlist
INSERT INTO tblWatchlist (iUserId, iProductId)
VALUES 
(14, 17),
(14, 16),
(8, 7),
(12, 8),
(13, 9),
(14, 10),
(15, 11);
SELECT * FROM TblWatchlist WHERE IWatchlistId = 13;
