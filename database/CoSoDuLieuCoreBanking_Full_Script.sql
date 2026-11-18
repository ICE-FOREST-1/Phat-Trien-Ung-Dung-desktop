-- =================================================================================
-- HỆ THỐNG CƠ SỞ DỮ LIỆU CORE BANKING - TECHCOMBANK WINUI 3
-- Tệp tin: CoSoDuLieuCoreBanking_Full_Script.sql
-- Mô tả: Bản sao dữ liệu đầy đủ bao gồm cấu trúc bảng (Schema), ràng buộc khóa (Constraints),
--        chỉ mục truy vấn (Indexes), thủ tục lưu trữ (Stored Procedures), triggers tự động
--        và toàn bộ dữ liệu thực tế đang hoạt động của hệ thống Core Banking.
-- Tương thích: Mọi phiên bản SQL Server (2016, 2017, 2019, 2022, Express, Developer, LocalDB, Azure SQL).
-- =================================================================================

USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'CoSoDuLieuCoreBanking')
BEGIN
    CREATE DATABASE CoSoDuLieuCoreBanking COLLATE Vietnamese_CI_AS;
END
GO

USE CoSoDuLieuCoreBanking;
GO

-- =================================================================================
-- 1. XÓA CÁC RÀNG BUỘC KHÓA NGOẠI VÀ BẢNG CŨ (NẾU CẦN TẠO LẠI TỪ ĐẦU)
-- =================================================================================
IF OBJECT_ID('dbo.CanhBaoAML', 'U') IS NOT NULL DROP TABLE dbo.CanhBaoAML;
IF OBJECT_ID('dbo.GiaoDich', 'U') IS NOT NULL DROP TABLE dbo.GiaoDich;
IF OBJECT_ID('dbo.KhoanVay', 'U') IS NOT NULL DROP TABLE dbo.KhoanVay;
IF OBJECT_ID('dbo.TaiSanDamBao', 'U') IS NOT NULL DROP TABLE dbo.TaiSanDamBao;
IF OBJECT_ID('dbo.SoTietKiem', 'U') IS NOT NULL DROP TABLE dbo.SoTietKiem;
IF OBJECT_ID('dbo.TaiKhoanThanhToan', 'U') IS NOT NULL DROP TABLE dbo.TaiKhoanThanhToan;
IF OBJECT_ID('dbo.KhachHang', 'U') IS NOT NULL DROP TABLE dbo.KhachHang;
IF OBJECT_ID('dbo.NguoiDung', 'U') IS NOT NULL DROP TABLE dbo.NguoiDung;
GO

-- =================================================================================
-- 2. TẠO CẤU TRÚC BẢNG (SCHEMA)
-- =================================================================================

-- 2.1. Bảng Người dùng hệ thống (NguoiDung)
CREATE TABLE dbo.NguoiDung (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    TenDangNhap NVARCHAR(50) NOT NULL UNIQUE,
    MatKhauMaHoa NVARCHAR(256) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    VaiTro NVARCHAR(30) NOT NULL CHECK (VaiTro IN (N'QUAN_TRI_VIEN', N'GIAO_DICH_VIEN', N'KIEM_SOAT_VIEN', N'CHUYEN_VIEN_TIN_DUNG', N'GIAM_DOC_RUI_RO')),
    ChiNhanh NVARCHAR(100) DEFAULT N'Hội Sở Chính - Techcombank Tower',
    TrangThaiKichHoat BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    DangNhapLanCuoi DATETIME NULL
);
GO

-- 2.2. Bảng Hồ sơ Khách hàng định danh CIF (KhachHang)
CREATE TABLE dbo.KhachHang (
    MaKhachHang INT IDENTITY(1,1) PRIMARY KEY,
    MaCIF NVARCHAR(20) NOT NULL UNIQUE,
    HoTenKhachHang NVARCHAR(100) NOT NULL,
    SoCCCD NVARCHAR(20) NOT NULL UNIQUE,
    SoDienThoai NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NULL,
    DiaChiThuongTru NVARCHAR(255) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'NAM', N'NU', N'KHAC')),
    NgheNghiep NVARCHAR(100) NULL,
    ThuNhapHangThang DECIMAL(18,2) DEFAULT 0,
    DiemTinDung INT DEFAULT 700 CHECK (DiemTinDung BETWEEN 300 AND 850),
    HangHoiVien NVARCHAR(30) DEFAULT N'CHUAN' CHECK (HangHoiVien IN (N'CHUAN', N'BAC', N'VANG', N'BACH_KIM', N'KIM_CUONG')),
    DiemTichLuyChiTieu DECIMAL(18,2) DEFAULT 0,
    NgayGiaNhap DATETIME DEFAULT GETDATE()
);
GO

-- 2.3. Bảng Tài khoản Thanh toán (TaiKhoanThanhToan)
CREATE TABLE dbo.TaiKhoanThanhToan (
    MaTaiKhoan INT IDENTITY(1,1) PRIMARY KEY,
    SoTaiKhoan NVARCHAR(20) NOT NULL UNIQUE,
    MaKhachHang INT NOT NULL FOREIGN KEY REFERENCES dbo.KhachHang(MaKhachHang),
    LoaiTaiKhoan NVARCHAR(30) DEFAULT N'THANH_TOAN',
    SoDuHienTai DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (SoDuHienTai >= 0),
    SoDuKhaDung AS (SoDuHienTai + CASE WHEN ChoPhepThauChi = 1 THEN HanMucThauChi ELSE 0 END),
    TrangThaiTaiKhoan NVARCHAR(30) DEFAULT N'HOAT_DONG' CHECK (TrangThaiTaiKhoan IN (N'HOAT_DONG', N'TAM_KHOA', N'DONG_TAI_KHOAN')),
    HangTaiKhoan NVARCHAR(30) DEFAULT N'CHUAN',
    HanMucThauChi DECIMAL(18,2) DEFAULT 0 CHECK (HanMucThauChi >= 0),
    ChoPhepThauChi BIT DEFAULT 0,
    NgayMoTaiKhoan DATETIME DEFAULT GETDATE(),
    NgayDongTaiKhoan DATETIME NULL
);
GO

-- 2.4. Bảng Sổ Tiết kiệm (SoTietKiem) - MaTaiKhoanNguon cho phép NULL cho khách nộp tiền mặt
CREATE TABLE dbo.SoTietKiem (
    MaSoTietKiem INT IDENTITY(1,1) PRIMARY KEY,
    MaSoSo NVARCHAR(30) NOT NULL UNIQUE,
    MaTaiKhoanNguon INT NULL FOREIGN KEY REFERENCES dbo.TaiKhoanThanhToan(MaTaiKhoan),
    MaKhachHang INT NOT NULL FOREIGN KEY REFERENCES dbo.KhachHang(MaKhachHang),
    SoTienGocBanDau DECIMAL(18,2) NOT NULL CHECK (SoTienGocBanDau > 0),
    SoTienHienTai DECIMAL(18,2) NOT NULL CHECK (SoTienHienTai >= 0),
    KyHanThang INT NOT NULL CHECK (KyHanThang IN (1, 3, 6, 12, 24, 36)),
    LaiSuatNam DECIMAL(5,2) NOT NULL,
    HinhThucNhanLai NVARCHAR(30) DEFAULT N'CUOI_KY' CHECK (HinhThucNhanLai IN (N'CUOI_KY', N'HANG_THANG', N'DAU_KY')),
    NgayGui DATE NOT NULL,
    NgayDaoHan DATE NOT NULL,
    TuDongTaiTuc BIT DEFAULT 1,
    DangTheChap BIT DEFAULT 0,
    MaKhoanVayLienKet NVARCHAR(30) NULL,
    TrangThaiSo NVARCHAR(30) DEFAULT N'DANG_HOAT_DONG' CHECK (TrangThaiSo IN (N'DANG_HOAT_DONG', N'DA_TAT_TOAN', N'DA_DAO_HAN')),
    LaiTichLuyTamTinh DECIMAL(18,2) DEFAULT 0,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- 2.5. Bảng Tài sản Đảm bảo (TaiSanDamBao)
CREATE TABLE dbo.TaiSanDamBao (
    MaTaiSan INT IDENTITY(1,1) PRIMARY KEY,
    LoaiTaiSan NVARCHAR(50) NOT NULL CHECK (LoaiTaiSan IN (N'SO_TIET_KIEM', N'BAT_DONG_SAN', N'PHUONG_TIEN_XE', N'VANG_KIM_LOAI_QUY', N'TIN_CHAP_LUONG')),
    TenTaiSan NVARCHAR(200) NOT NULL,
    GiaTriDinhGia DECIMAL(18,2) NOT NULL CHECK (GiaTriDinhGia >= 0),
    SoGiayToPhapLy NVARCHAR(100) NOT NULL,
    MaSoTietKiemThamChieu INT NULL FOREIGN KEY REFERENCES dbo.SoTietKiem(MaSoTietKiem),
    TrangThaiTheChap NVARCHAR(30) DEFAULT N'DANG_CAM_CO' CHECK (TrangThaiTheChap IN (N'DANG_CAM_CO', N'DA_GIAI_CHAP'))
);
GO

-- 2.6. Bảng Khoản vay & Tín dụng (KhoanVay)
CREATE TABLE dbo.KhoanVay (
    MaKhoanVay INT IDENTITY(1,1) PRIMARY KEY,
    MaSoHopDongVay NVARCHAR(30) NOT NULL UNIQUE,
    MaKhachHang INT NOT NULL FOREIGN KEY REFERENCES dbo.KhachHang(MaKhachHang),
    MaTaiKhoanGiaiNgan INT NOT NULL FOREIGN KEY REFERENCES dbo.TaiKhoanThanhToan(MaTaiKhoan),
    MaTaiSanDamBao INT NULL FOREIGN KEY REFERENCES dbo.TaiSanDamBao(MaTaiSan),
    GoiVay NVARCHAR(50) NOT NULL,
    SoTienVayBanDau DECIMAL(18,2) NOT NULL CHECK (SoTienVayBanDau > 0),
    DuNoGocHienTai DECIMAL(18,2) NOT NULL,
    ThoiHanVayThang INT NOT NULL CHECK (ThoiHanVayThang > 0),
    LaiSuatVayNam DECIMAL(5,2) NOT NULL,
    NgayGiaiNgan DATE NOT NULL,
    NgayDaoHanHopDong DATE NOT NULL,
    NgayTraNoTiepTheo DATE NOT NULL,
    SoTienPhaiTraHangThang DECIMAL(18,2) NOT NULL,
    TrangThaiKhoanVay NVARCHAR(30) DEFAULT N'CHO_PHE_DUYET',
    SoNgayQuaHan INT DEFAULT 0,
    MucDichVay NVARCHAR(255) NOT NULL,
    NhanVienLapHoSo INT NOT NULL FOREIGN KEY REFERENCES dbo.NguoiDung(MaNguoiDung),
    LanhDaoPheDuyet INT NULL FOREIGN KEY REFERENCES dbo.NguoiDung(MaNguoiDung),
    NgayPheDuyet DATETIME NULL,
    LyDoTuChoi NVARCHAR(255) NULL,
    NgayTao DATETIME DEFAULT GETDATE()
);
GO

-- 2.7. Bảng Nhật ký Giao dịch (GiaoDich)
CREATE TABLE dbo.GiaoDich (
    MaGiaoDich BIGINT IDENTITY(1,1) PRIMARY KEY,
    MaChungTu NVARCHAR(30) NOT NULL UNIQUE,
    SoTaiKhoanNguon NVARCHAR(30) NOT NULL,
    TenNguoiGui NVARCHAR(100) NOT NULL,
    SoTaiKhoanDich NVARCHAR(30) NOT NULL,
    TenNguoiNhan NVARCHAR(100) NOT NULL,
    NganHangDich NVARCHAR(100) NULL,
    LoaiGiaoDich NVARCHAR(50) NOT NULL,
    SoTienGiaoDich DECIMAL(18,2) NOT NULL CHECK (SoTienGiaoDich > 0),
    PhiGiaoDich DECIMAL(18,2) DEFAULT 0,
    SoDuTruocGD DECIMAL(18,2) NOT NULL,
    SoDuSauGD DECIMAL(18,2) NOT NULL,
    NoiDungChuyenKhoan NVARCHAR(255) NOT NULL,
    ThoiGianGiaoDich DATETIME DEFAULT GETDATE(),
    TrangThaiGiaoDich NVARCHAR(30) DEFAULT N'THANH_CONG',
    NhanVienThucHien NVARCHAR(100) NOT NULL,
    CanhBaoRuaTienAML BIT DEFAULT 0,
    LyDoCanhBaoAML NVARCHAR(255) NULL
);
GO

-- 2.8. Bảng Cảnh báo Rửa tiền AML (CanhBaoAML)
CREATE TABLE dbo.CanhBaoAML (
    MaCanhBao INT IDENTITY(1,1) PRIMARY KEY,
    MaGiaoDichThamChieu BIGINT NOT NULL FOREIGN KEY REFERENCES dbo.GiaoDich(MaGiaoDich),
    SoTaiKhoanPhatSinh NVARCHAR(20) NOT NULL,
    TenKhachHang NVARCHAR(100) NOT NULL,
    SoTienPhatSinh DECIMAL(18,2) NOT NULL,
    MucDoRuiRo NVARCHAR(30) NOT NULL,
    LyDoTriggerPhatHien NVARCHAR(255) NOT NULL,
    TrangThaiXuLy NVARCHAR(30) DEFAULT N'CHO_KIEM_DUYET',
    NguoiXuLy INT NULL FOREIGN KEY REFERENCES dbo.NguoiDung(MaNguoiDung),
    KetLuanXuLy NVARCHAR(255) NULL,
    ThoiGianPhatHien DATETIME DEFAULT GETDATE(),
    ThoiGianXuLy DATETIME NULL
);
GO

-- =================================================================================
-- 3. CHỈ MỤC (INDEXES)
-- =================================================================================
CREATE NONCLUSTERED INDEX IX_TaiKhoan_SoTaiKhoan ON dbo.TaiKhoanThanhToan(SoTaiKhoan) INCLUDE (SoDuHienTai, TrangThaiTaiKhoan, MaKhachHang);
CREATE NONCLUSTERED INDEX IX_GiaoDich_ThoiGian ON dbo.GiaoDich(ThoiGianGiaoDich DESC) INCLUDE (SoTienGiaoDich, LoaiGiaoDich, TrangThaiGiaoDich);
CREATE NONCLUSTERED INDEX IX_KhachHang_CIF ON dbo.KhachHang(MaCIF);
CREATE NONCLUSTERED INDEX IX_KhachHang_CCCD ON dbo.KhachHang(SoCCCD);
CREATE NONCLUSTERED INDEX IX_SoTietKiem_NgayDaoHan ON dbo.SoTietKiem(NgayDaoHan, TrangThaiSo);
CREATE NONCLUSTERED INDEX IX_KhoanVay_TrangThai ON dbo.KhoanVay(TrangThaiKhoanVay, NgayTraNoTiepTheo);
GO

-- =================================================================================
-- 4. TRIGGERS TỰ ĐỘNG
-- =================================================================================

-- 4.1. Trigger Tự động thăng hạng hội viên và cập nhật thấu chi
CREATE OR ALTER TRIGGER trg_TuDongThangHangHoiVien
ON dbo.GiaoDich
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH DoanhSoMoi AS (
        SELECT 
            kh.MaKhachHang,
            SUM(i.SoTienGiaoDich) AS TongTienPhatSinh
        FROM inserted i
        JOIN dbo.TaiKhoanThanhToan tk ON i.SoTaiKhoanNguon = tk.SoTaiKhoan
        JOIN dbo.KhachHang kh ON tk.MaKhachHang = kh.MaKhachHang
        WHERE i.TrangThaiGiaoDich = N'THANH_CONG'
        GROUP BY kh.MaKhachHang
    )
    UPDATE kh
    SET 
        kh.DiemTichLuyChiTieu = kh.DiemTichLuyChiTieu + ds.TongTienPhatSinh,
        kh.HangHoiVien = CASE 
            WHEN (kh.DiemTichLuyChiTieu + ds.TongTienPhatSinh) >= 2000000000 THEN N'KIM_CUONG'
            WHEN (kh.DiemTichLuyChiTieu + ds.TongTienPhatSinh) >= 500000000  THEN N'BACH_KIM'
            WHEN (kh.DiemTichLuyChiTieu + ds.TongTienPhatSinh) >= 100000000  THEN N'VANG'
            WHEN (kh.DiemTichLuyChiTieu + ds.TongTienPhatSinh) >= 20000000   THEN N'BAC'
            ELSE N'CHUAN'
        END
    FROM dbo.KhachHang kh
    JOIN DoanhSoMoi ds ON kh.MaKhachHang = ds.MaKhachHang;

    UPDATE tk
    SET 
        tk.HangTaiKhoan = kh.HangHoiVien,
        tk.HanMucThauChi = CASE 
            WHEN kh.HangHoiVien = N'KIM_CUONG' THEN 1000000000
            WHEN kh.HangHoiVien = N'BACH_KIM' THEN 200000000
            WHEN kh.HangHoiVien = N'VANG' THEN 50000000
            WHEN kh.HangHoiVien = N'BAC' THEN 10000000
            ELSE 0
        END,
        tk.ChoPhepThauChi = CASE WHEN kh.HangHoiVien <> N'CHUAN' THEN 1 ELSE 0 END
    FROM dbo.TaiKhoanThanhToan tk
    JOIN dbo.KhachHang kh ON tk.MaKhachHang = kh.MaKhachHang
    JOIN inserted i ON tk.SoTaiKhoan = i.SoTaiKhoanNguon;
END;
GO

-- 4.2. Trigger Tự động phát hiện giao dịch bất thường / AML
CREATE OR ALTER TRIGGER trg_PhatHienGiaoDichDangNgoAML
ON dbo.GiaoDich
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NguongCanhBaoAML DECIMAL(18,2) = 400000000;

    INSERT INTO dbo.CanhBaoAML (
        MaGiaoDichThamChieu,
        SoTaiKhoanPhatSinh,
        TenKhachHang,
        SoTienPhatSinh,
        MucDoRuiRo,
        LyDoTriggerPhatHien,
        TrangThaiXuLy
    )
    SELECT 
        i.MaGiaoDich,
        i.SoTaiKhoanNguon,
        i.TenNguoiGui,
        i.SoTienGiaoDich,
        CASE 
            WHEN i.SoTienGiaoDich >= 2000000000 THEN N'DAC_BIET_NGHIEM_TRONG'
            WHEN i.SoTienGiaoDich >= 1000000000 THEN N'CAO'
            ELSE N'TRUNG_BINH'
        END,
        CONCAT(N'Phát hiện giao dịch số tiền lớn đột biến: ', FORMAT(i.SoTienGiaoDich, 'N0'), N' VNĐ theo Luật Phòng chống rửa tiền'),
        N'CHO_KIEM_DUYET'
    FROM inserted i
    WHERE i.SoTienGiaoDich >= @NguongCanhBaoAML AND i.TrangThaiGiaoDich = N'THANH_CONG';
END;
GO

-- =================================================================================
-- 5. STORED PROCEDURES
-- =================================================================================

-- 5.1. Mở tài khoản thanh toán (hỗ trợ nhập số tài khoản tùy chọn hoặc tự sinh ngẫu nhiên)
CREATE OR ALTER PROCEDURE dbo.sp_MoTaiKhoanThanhToan
    @MaKhachHang INT,
    @SoTienNapBanDau DECIMAL(18,2) = 0,
    @MaNguoiDungThucHien INT,
    @SoTaiKhoanMoi NVARCHAR(20) OUTPUT,
    @SoTaiKhoanTuChon NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        IF @SoTaiKhoanTuChon IS NOT NULL AND LEN(LTRIM(RTRIM(@SoTaiKhoanTuChon))) > 0
        BEGIN
            SET @SoTaiKhoanMoi = LTRIM(RTRIM(@SoTaiKhoanTuChon));
            IF EXISTS (SELECT 1 FROM dbo.TaiKhoanThanhToan WHERE SoTaiKhoan = @SoTaiKhoanMoi)
            BEGIN
                THROW 50003, N'Số tài khoản đã tồn tại trong hệ thống. Vui lòng chọn số khác.', 1;
            END
        END
        ELSE
        BEGIN
            DECLARE @Lap INT = 0;
            WHILE 1 = 1
            BEGIN
                SET @SoTaiKhoanMoi = CONCAT('1903', CAST(ABS(CHECKSUM(NEWID())) % 9000000000 + 1000000000 AS NVARCHAR(10)));
                IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoanThanhToan WHERE SoTaiKhoan = @SoTaiKhoanMoi) BREAK;
                SET @Lap = @Lap + 1;
                IF @Lap > 100 THROW 50004, N'Không thể tạo số tài khoản ngẫu nhiên sau nhiều lần thử.', 1;
            END
        END

        DECLARE @HangKhachHang NVARCHAR(30);
        SELECT @HangKhachHang = HangHoiVien FROM dbo.KhachHang WHERE MaKhachHang = @MaKhachHang;

        INSERT INTO dbo.TaiKhoanThanhToan (SoTaiKhoan, MaKhachHang, LoaiTaiKhoan, SoDuHienTai, TrangThaiTaiKhoan, HangTaiKhoan)
        VALUES (@SoTaiKhoanMoi, @MaKhachHang, N'THANH_TOAN', @SoTienNapBanDau, N'HOAT_DONG', ISNULL(@HangKhachHang, N'CHUAN'));

        IF @SoTienNapBanDau > 0
        BEGIN
            INSERT INTO dbo.GiaoDich (
                MaChungTu, SoTaiKhoanNguon, TenNguoiGui, SoTaiKhoanDich,
                TenNguoiNhan, LoaiGiaoDich, SoTienGiaoDich, PhiGiaoDich, SoDuTruocGD, SoDuSauGD,
                NoiDungChuyenKhoan, NhanVienThucHien, TrangThaiGiaoDich
            )
            VALUES (
                CONCAT('DP', FORMAT(GETDATE(), 'yyyyMMddHHmmssfff')),
                'QUAY_TIEN_MAT', N'Nộp tiền mặt tại quầy',
                @SoTaiKhoanMoi, (SELECT HoTenKhachHang FROM dbo.KhachHang WHERE MaKhachHang = @MaKhachHang),
                N'NAP_TIEN_MAT', @SoTienNapBanDau, 0, 0, @SoTienNapBanDau,
                N'Nộp tiền ban đầu kích hoạt tài khoản thanh toán',
                ISNULL((SELECT HoTen FROM dbo.NguoiDung WHERE MaNguoiDung = @MaNguoiDungThucHien), N'Giao dịch viên'),
                N'THANH_CONG'
            );
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- 5.2. Chuyển khoản nội bộ (tính phí theo hạng tài khoản, trả về mã chứng từ và phí giao dịch)
CREATE OR ALTER PROCEDURE dbo.sp_ChuyenKhoanNoiBo
    @SoTaiKhoanNguon   NVARCHAR(20),
    @SoTaiKhoanDich    NVARCHAR(20),
    @SoTien            DECIMAL(18,2),
    @NoiDung           NVARCHAR(255),
    @NhanVienThucHien  NVARCHAR(100),
    @MaChungTu         NVARCHAR(30)  = NULL OUTPUT,
    @PhiGiaoDich       DECIMAL(18,2) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @SoTien IS NULL OR @SoTien <= 0
        THROW 50001, N'Số tiền giao dịch phải lớn hơn 0.', 1;
    IF @SoTaiKhoanNguon = @SoTaiKhoanDich
        THROW 50002, N'Tài khoản nguồn và tài khoản thụ hưởng không được trùng nhau.', 1;

    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @MaTkNguon INT, @SoDuNguon DECIMAL(18,2), @HangNguon NVARCHAR(30),
                @ChoPhepThauChi BIT, @HanMucThauChi DECIMAL(18,2), @TrangThaiNguon NVARCHAR(30),
                @TenNguoiGui NVARCHAR(100);

        SELECT @MaTkNguon      = tk.MaTaiKhoan,
               @SoDuNguon      = tk.SoDuHienTai,
               @HangNguon      = ISNULL(tk.HangTaiKhoan, N'CHUAN'),
               @ChoPhepThauChi = ISNULL(tk.ChoPhepThauChi, 0),
               @HanMucThauChi  = ISNULL(tk.HanMucThauChi, 0),
               @TrangThaiNguon = tk.TrangThaiTaiKhoan,
               @TenNguoiGui    = kh.HoTenKhachHang
        FROM dbo.TaiKhoanThanhToan tk WITH (UPDLOCK, ROWLOCK)
        JOIN dbo.KhachHang kh ON kh.MaKhachHang = tk.MaKhachHang
        WHERE tk.SoTaiKhoan = @SoTaiKhoanNguon;

        IF @MaTkNguon IS NULL
            THROW 50003, N'Tài khoản nguồn không tồn tại.', 1;
        IF @TrangThaiNguon <> N'HOAT_DONG'
            THROW 50004, N'Tài khoản nguồn đang bị khóa hoặc không hoạt động.', 1;

        DECLARE @MaTkDich INT, @SoDuDich DECIMAL(18,2), @TrangThaiDich NVARCHAR(30), @TenNguoiNhan NVARCHAR(100);

        SELECT @MaTkDich      = tk.MaTaiKhoan,
               @SoDuDich      = tk.SoDuHienTai,
               @TrangThaiDich = tk.TrangThaiTaiKhoan,
               @TenNguoiNhan  = kh.HoTenKhachHang
        FROM dbo.TaiKhoanThanhToan tk WITH (UPDLOCK, ROWLOCK)
        JOIN dbo.KhachHang kh ON kh.MaKhachHang = tk.MaKhachHang
        WHERE tk.SoTaiKhoan = @SoTaiKhoanDich;

        IF @MaTkDich IS NULL
            THROW 50005, N'Tài khoản thụ hưởng không tồn tại.', 1;
        IF @TrangThaiDich <> N'HOAT_DONG'
            THROW 50006, N'Tài khoản thụ hưởng đang bị khóa hoặc không hoạt động.', 1;

        SET @PhiGiaoDich = CASE 
            WHEN @HangNguon IN (N'CHUAN', N'BAC') THEN 3300.00
            ELSE 0.00
        END;

        DECLARE @TongTru DECIMAL(18,2) = @SoTien + @PhiGiaoDich;
        DECLARE @KhaDung DECIMAL(18,2) = @SoDuNguon + CASE WHEN @ChoPhepThauChi = 1 THEN @HanMucThauChi ELSE 0 END;

        IF @KhaDung < @TongTru
            THROW 50007, N'Số dư khả dụng (bao gồm thấu chi) không đủ để thanh toán số tiền và phí.', 1;

        UPDATE dbo.TaiKhoanThanhToan
        SET SoDuHienTai = SoDuHienTai - @TongTru
        WHERE MaTaiKhoan = @MaTkNguon;

        UPDATE dbo.TaiKhoanThanhToan
        SET SoDuHienTai = SoDuHienTai + @SoTien
        WHERE MaTaiKhoan = @MaTkDich;

        DECLARE @NgayGio NVARCHAR(20) = FORMAT(GETDATE(), 'yyMMddHHmmssfff');
        DECLARE @RandomHaiChuSo INT = ABS(CHECKSUM(NEWID())) % 90 + 10;
        SET @MaChungTu = CONCAT('FT', @NgayGio, @RandomHaiChuSo);

        INSERT INTO dbo.GiaoDich (
            MaChungTu, SoTaiKhoanNguon, TenNguoiGui, SoTaiKhoanDich, TenNguoiNhan,
            NganHangDich, LoaiGiaoDich, SoTienGiaoDich, PhiGiaoDich,
            SoDuTruocGD, SoDuSauGD, NoiDungChuyenKhoan, ThoiGianGiaoDich,
            TrangThaiGiaoDich, NhanVienThucHien
        )
        VALUES (
            @MaChungTu, @SoTaiKhoanNguon, @TenNguoiGui, @SoTaiKhoanDich, @TenNguoiNhan,
            N'Techcombank', N'CHUYEN_KHOAN_NOI_BO', @SoTien, @PhiGiaoDich,
            @SoDuNguon, @SoDuNguon - @TongTru, @NoiDung, GETDATE(),
            N'THANH_CONG', @NhanVienThucHien
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- =================================================================================
-- 6. NẠP DỮ LIỆU ĐẦY ĐỦ VÀO HỆ THỐNG (DATA DUMP)
-- =================================================================================

-- Tạm thời tắt triggers trong quá trình nạp dữ liệu ban đầu
DISABLE TRIGGER trg_TuDongThangHangHoiVien ON dbo.GiaoDich;
DISABLE TRIGGER trg_PhatHienGiaoDichDangNgoAML ON dbo.GiaoDich;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.NguoiDung (5 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[NguoiDung] ON;

INSERT INTO dbo.[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhauMaHoa], [HoTen], [VaiTro], [ChiNhanh], [TrangThaiKichHoat], [NgayTao], [DangNhapLanCuoi]) VALUES (1, N'admin', N'$2a$11$ON7miVGvgsWdMT4kWQco.OlH7KD8ZFBwmfxeh/j1kpV/s/NKhYMKi', N'Nguyễn Đình Trọng (Giám Đốc Chi Nhánh)', N'QUAN_TRI_VIEN', N'Hội Sở Chính - Techcombank Tower', 1, '2026-08-28 00:36:55', '2026-11-18 16:26:21');
INSERT INTO dbo.[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhauMaHoa], [HoTen], [VaiTro], [ChiNhanh], [TrangThaiKichHoat], [NgayTao], [DangNhapLanCuoi]) VALUES (2, N'nhanvien_01', N'$2a$11$xOOgC7Vv31AXDKajFHw/hOJu8.f94xFnopixht9eS8elqsP3hQpNa', N'Trần Mai Linh (Giao Dịch Viên)', N'GIAO_DICH_VIEN', N'Chi Nhánh Hoàn Kiếm Hà Nội', 1, '2026-08-28 00:36:55', NULL);
INSERT INTO dbo.[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhauMaHoa], [HoTen], [VaiTro], [ChiNhanh], [TrangThaiKichHoat], [NgayTao], [DangNhapLanCuoi]) VALUES (3, N'teller', N'$2a$11$XnspbB5UP13QOy9Dp/i6puPMKenGHCQK.NpwHrN6LIEelkDYQFZuq', N'Trần Mai Linh', N'GIAO_DICH_VIEN', N'Hội Sở Techcombank Tower', 1, '2026-11-10 14:14:19', '2026-11-18 16:32:46');
INSERT INTO dbo.[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhauMaHoa], [HoTen], [VaiTro], [ChiNhanh], [TrangThaiKichHoat], [NgayTao], [DangNhapLanCuoi]) VALUES (4, N'tindung', N'$2a$11$quZ/tiDBumEyqqq08UoGN.xEFzOlDrp.3lpWlPSb5DKJ2nAWyLQA2', N'Lê Hoàng Quân', N'GIAO_DICH_VIEN', N'Chi Nhánh Ba Đình - Hà Nội', 1, '2026-11-10 14:14:19', '2026-11-18 16:32:24');
INSERT INTO dbo.[NguoiDung] ([MaNguoiDung], [TenDangNhap], [MatKhauMaHoa], [HoTen], [VaiTro], [ChiNhanh], [TrangThaiKichHoat], [NgayTao], [DangNhapLanCuoi]) VALUES (5, N'aml', N'$2a$11$kSxZjDXfMRNkeDbHFKqtQOqR4m0ne4a9U47FTKT1bEZVGPBnzXTCq', N'Phạm Quỳnh Chi', N'GIAO_DICH_VIEN', N'Khối Giám Sát Tuân Thủ', 1, '2026-11-10 14:14:19', '2026-11-18 16:32:30');

SET IDENTITY_INSERT dbo.[NguoiDung] OFF;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.KhachHang (18 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[KhachHang] ON;

INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (1, N'CIF100821', N'Phạm Minh Quân', N'079201004821', N'0908123456', N'quan.pham@example.com', N'128 Nguyễn Trãi, Quận 1, TP.HCM', '1990-05-14', N'NAM', N'Giám đốc Doanh nghiệp', 85000000.00, 780, N'BACH_KIM', 895000000.00, '2026-08-28 00:36:55');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (2, N'CIF100822', N'Lê Thị Thu Hương', N'001198003412', N'0912987654', N'huong.le@example.com', N'45 Lê Duẩn, Hoàn Kiếm, Hà Nội', '1988-11-20', N'NU', N'Trưởng phòng Marketing', 45000000.00, 740, N'VANG', 275000000.00, '2026-08-28 00:36:55');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (3, N'CIF100823', N'Nguyễn Văn An', N'034200001289', N'0983112233', N'an.nguyen@example.com', N'88 Cầu Giấy, Hà Nội', '1995-03-10', N'NAM', N'Kỹ sư Phần mềm', 35000000.00, 710, N'BAC', 93000000.00, '2026-08-28 00:36:55');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (4, N'CIF100824', N'Vũ Hoàng Nam', N'079203009988', N'0938776655', N'nam.vu@example.com', N'12 Bến Vân Đồn, Quận 4, TP.HCM', '1982-08-25', N'NAM', N'Chủ chuỗi Nhà hàng', 150000000.00, 820, N'KIM_CUONG', 4350000000.00, '2026-08-28 00:36:55');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (5, N'CIF100825', N'Hoàng Thu Thảo', N'048199012890', N'0933112233', N'thao.hoang@example.com', N'15 Lê Duẩn, Hải Châu, Đà Nẵng', '1999-07-02', N'NU', N'Nhân viên Kế toán', 18000000.00, 690, N'CHUAN', 2500000.00, '2026-11-10 14:14:19');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (6, N'CIF100826', N'Trần Đức Minh', N'031197004455', N'0977445566', N'minh.tran@example.com', N'22 Lạch Tray, Ngô Quyền, Hải Phòng', '1997-12-12', N'NAM', N'Kỹ sư Xây dựng', 26000000.00, 720, N'BAC', 37000000.00, '2026-11-10 14:14:19');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (7, N'CIF100827', N'Đặng Quốc Bảo', N'001201015678', N'0982345678', N'bao.dang@vinfast.vn', N'Số 88 Phố Huế, Q. Hai Bà Trưng, Hà Nội', '1983-04-12', N'NAM', N'Phó Tổng Giám Đốc', 120000000.00, 820, N'KIM_CUONG', 2850000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (8, N'CIF100828', N'Vũ Bích Ngọc', N'079201019876', N'0918765432', N'ngoc.vu@shopee.vn', N'25 Thảo Điền, TP. Thủ Đức, TP.HCM', '1991-08-25', N'NU', N'Giám Đốc Vận Hành', 48000000.00, 760, N'BACH_KIM', 620000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (9, N'CIF100829', N'Bùi Anh Dũng', N'034201012345', N'0972112233', N'dung.bui@viettel.com.vn', N'160 Hoàng Hoa Thám, Ba Đình, Hà Nội', '1993-11-15', N'NAM', N'Kỹ sư Trưởng Viễn Thông', 35000000.00, 730, N'VANG', 155000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (10, N'CIF100830', N'Ngô Thu Trang', N'001198007788', N'0909887766', N'trang.ngo@vietjetair.com', N'45 Nguyễn Đình Chiểu, Q.3, TP.HCM', '1989-02-18', N'NU', N'Tiếp Viên Trưởng', 65000000.00, 755, N'BACH_KIM', 580000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (11, N'CIF100831', N'Phan Đình Tùng', N'079202014455', N'0931223344', N'tung.phan@vng.com.vn', N'Tòa Landmark 81, Bình Thạnh, TP.HCM', '1995-09-30', N'NAM', N'Lập trình viên Game', 22000000.00, 680, N'BAC', 38000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (12, N'CIF100832', N'Trịnh Mai Phương', N'048195013322', N'0985556677', N'phuong.trinh@sunworld.vn', N'36 Bạch Đằng, Q. Hải Châu, Đà Nẵng', '1992-06-10', N'NU', N'Quản Lý Khách Sạn', 38000000.00, 715, N'VANG', 180000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (13, N'CIF100833', N'Lâm Thế Vinh', N'001200018899', N'0903119988', N'vinh.lam@masan.vn', N'Khu Biệt Thự Ciputra, Tây Hồ, Hà Nội', '1978-12-04', N'NAM', N'Chủ Doanh Nghiệp Bán Lẻ', 150000000.00, 840, N'KIM_CUONG', 4500000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (14, N'CIF100834', N'Đỗ Hoài Nam', N'031199015566', N'0944332211', N'nam.do@gmail.com', N'72 Cầu Đất, Ngô Quyền, Hải Phòng', '1998-03-22', N'NAM', N'Chuyên Viên Tuyển Dụng', 16000000.00, 660, N'CHUAN', 8500000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (15, N'CIF100835', N'Dương Mỹ Linh', N'079196016677', N'0919228833', N'linh.duong@novaland.vn', N'120 Mai Chí Thọ, An Phú, TP. Thủ Đức', '1994-07-19', N'NU', N'Trưởng Nhóm Bất Động Sản', 42000000.00, 725, N'VANG', 210000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (16, N'CIF100836', N'Hồ Gia Bảo', N'048197017788', N'0966448899', N'bao.ho@fpt.com.vn', N'50 Phan Chu Trinh, TP. Buôn Ma Thuột', '1996-10-08', N'NAM', N'Kỹ sư Nông nghiệp', 25000000.00, 695, N'BAC', 45000000.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (17, N'CIF100837', N'Nguyen van b', N'010101010101', N'0101010101', NULL, N'123 phù đổng thiên vương', '1995-01-01', N'NAM', N'Kinh doanh tự do', 20000000.00, 720, N'CHUAN', 0.00, '2026-11-18 15:55:04');
INSERT INTO dbo.[KhachHang] ([MaKhachHang], [MaCIF], [HoTenKhachHang], [SoCCCD], [SoDienThoai], [Email], [DiaChiThuongTru], [NgaySinh], [GioiTinh], [NgheNghiep], [ThuNhapHangThang], [DiemTinDung], [HangHoiVien], [DiemTichLuyChiTieu], [NgayGiaNhap]) VALUES (18, N'CIF100838', N'Nguyen Van A', N'020202020202', N'0202020202', NULL, N'25 Quang trung', '1990-01-01', N'NAM', NULL, NULL, 700, N'CHUAN', 0.00, '2026-11-18 15:56:52');

SET IDENTITY_INSERT dbo.[KhachHang] OFF;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.TaiKhoanThanhToan (17 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[TaiKhoanThanhToan] ON;

INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (1, N'19034589201928', 1, N'THANH_TOAN', 1800000000.00, N'VND', N'HOAT_DONG', N'BACH_KIM', 0.20, 1, 200000000.00, '2026-11-10 14:14:19', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (2, N'19034589201999', 2, N'THANH_TOAN', 395000000.00, N'VND', N'HOAT_DONG', N'VANG', 0.20, 1, 50000000.00, '2026-11-10 14:14:19', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (3, N'19034589201888', 3, N'THANH_TOAN', 88996700.00, N'VND', N'HOAT_DONG', N'BAC', 0.20, 1, 10000000.00, '2026-11-10 14:14:19', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (4, N'19034589201777', 4, N'THANH_TOAN', 2080000000.00, N'VND', N'TAM_KHOA', N'KIM_CUONG', 0.20, 1, 1000000000.00, '2026-11-10 14:14:19', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (5, N'19034589201666', 5, N'THANH_TOAN', 54496700.00, N'VND', N'HOAT_DONG', N'CHUAN', 0.20, 0, 0.00, '2026-11-10 14:14:19', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (6, N'19034589201555', 6, N'THANH_TOAN', 108496700.00, N'VND', N'HOAT_DONG', N'BAC', 0.20, 1, 10000000.00, '2026-11-10 14:14:19', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (7, N'19034589202001', 7, N'THANH_TOAN', 3500000000.00, N'VND', N'HOAT_DONG', N'KIM_CUONG', 0.20, 1, 1000000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (8, N'19034589202002', 8, N'THANH_TOAN', 450000000.00, N'VND', N'HOAT_DONG', N'BACH_KIM', 0.20, 1, 200000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (9, N'19034589202003', 9, N'THANH_TOAN', 110000000.00, N'VND', N'HOAT_DONG', N'VANG', 0.20, 1, 50000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (10, N'19034589202004', 10, N'THANH_TOAN', 690000000.00, N'VND', N'HOAT_DONG', N'BACH_KIM', 0.20, 1, 200000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (11, N'19034589202005', 11, N'THANH_TOAN', 35000000.00, N'VND', N'HOAT_DONG', N'BAC', 0.20, 1, 10000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (12, N'19034589202006', 12, N'THANH_TOAN', 15000000.00, N'VND', N'HOAT_DONG', N'VANG', 0.20, 1, 50000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (13, N'19034589202007', 13, N'THANH_TOAN', 4200000000.00, N'VND', N'HOAT_DONG', N'KIM_CUONG', 0.20, 1, 1000000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (14, N'19034589202008', 14, N'THANH_TOAN', 8500000.00, N'VND', N'HOAT_DONG', N'CHUAN', 0.20, 0, 0.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (15, N'19034589202009', 15, N'THANH_TOAN', 185000000.00, N'VND', N'HOAT_DONG', N'VANG', 0.20, 1, 50000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (16, N'19034589202010', 16, N'THANH_TOAN', 132000000.00, N'VND', N'HOAT_DONG', N'BAC', 0.20, 1, 10000000.00, '2026-11-18 15:35:37', NULL);
INSERT INTO dbo.[TaiKhoanThanhToan] ([MaTaiKhoan], [SoTaiKhoan], [MaKhachHang], [LoaiTaiKhoan], [SoDuHienTai], [DonViTienTe], [TrangThaiTaiKhoan], [HangTaiKhoan], [LaiSuatKhongKyHan], [ChoPhepThauChi], [HanMucThauChi], [NgayMoTaiKhoan], [NgayDongTaiKhoan]) VALUES (17, N'19032335811471', 17, N'THANH_TOAN', 1000000.00, N'VND', N'HOAT_DONG', N'CHUAN', 0.20, 0, 0.00, '2026-11-18 15:55:04', NULL);

SET IDENTITY_INSERT dbo.[TaiKhoanThanhToan] OFF;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.SoTietKiem (13 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[SoTietKiem] ON;

INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (1, N'STK-2026-0001', 1, 1, 1200000000.00, 1200000000.00, 12, 6.20, N'CUOI_KY', '2026-06-10', '2027-06-10', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-10 14:14:19');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (2, N'STK-2026-0002', 2, 2, 500000000.00, 500000000.00, 6, 5.60, N'CUOI_KY', '2026-08-10', '2027-02-10', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-10 14:14:19');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (3, N'STK-2026-0003', 4, 4, 3000000000.00, 3000000000.00, 24, 6.80, N'CUOI_KY', '2026-01-10', '2028-01-10', 1, 1, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-10 14:14:19');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (4, N'STK-2026-0004', 6, 6, 80000000.00, 80000000.00, 3, 4.50, N'CUOI_KY', '2026-10-10', '2027-01-10', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-10 14:14:19');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (5, N'STK-2026-0005', 7, 7, 5000000000.00, 5000000000.00, 24, 7.00, N'CUOI_KY', '2026-03-18', '2028-03-18', 1, 1, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (6, N'STK-2026-0006', 7, 7, 2000000000.00, 2000000000.00, 12, 6.50, N'CUOI_KY', '2026-07-18', '2027-07-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (7, N'STK-2026-0007', 8, 8, 800000000.00, 800000000.00, 6, 5.80, N'CUOI_KY', '2026-09-18', '2027-03-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (8, N'STK-2026-0008', 10, 10, 1500000000.00, 1500000000.00, 12, 6.20, N'CUOI_KY', '2026-05-18', '2027-05-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (9, N'STK-2026-0009', 10, 10, 500000000.00, 500000000.00, 3, 4.80, N'CUOI_KY', '2026-10-18', '2027-01-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (10, N'STK-2026-0010', 12, 12, 600000000.00, 600000000.00, 12, 6.30, N'CUOI_KY', '2026-06-18', '2027-06-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (11, N'STK-2026-0011', 13, 13, 10000000000.00, 10000000000.00, 36, 7.20, N'CUOI_KY', '2025-11-18', '2028-11-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (12, N'STK-2026-0012', 15, 15, 350000000.00, 350000000.00, 6, 5.50, N'CUOI_KY', '2026-08-18', '2027-02-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, '2026-11-18 15:35:37');
INSERT INTO dbo.[SoTietKiem] ([MaSoTietKiem], [MaSoSo], [MaTaiKhoanNguon], [MaKhachHang], [SoTienGocBanDau], [SoTienHienTai], [KyHanThang], [LaiSuatNam], [HinhThucNhanLai], [NgayGui], [NgayDaoHan], [TuDongTaiTuc], [DangTheChap], [MaKhoanVayLienKet], [TrangThaiSo], [LaiTichLuyTamTinh], [NgayTao]) VALUES (13, N'STK-2026-7526', NULL, 18, 10000000.00, 10000000.00, 12, 6.20, N'CUOI_KY', '2026-11-18', '2027-11-18', 1, 0, NULL, N'DANG_HOAT_DONG', 0.00, NULL);

SET IDENTITY_INSERT dbo.[SoTietKiem] OFF;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.TaiSanDamBao (3 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[TaiSanDamBao] ON;

INSERT INTO dbo.[TaiSanDamBao] ([MaTaiSan], [LoaiTaiSan], [TenTaiSan], [GiaTriDinhGia], [SoGiayToPhapLy], [MaSoTietKiemThamChieu], [TrangThaiTheChap]) VALUES (1, N'SO_TIET_KIEM', N'Sổ tiết kiệm STK-2026-0003 (24 tháng)', 3000000000.00, N'STK-2026-0003', 3, N'DANG_CAM_CO');
INSERT INTO dbo.[TaiSanDamBao] ([MaTaiSan], [LoaiTaiSan], [TenTaiSan], [GiaTriDinhGia], [SoGiayToPhapLy], [MaSoTietKiemThamChieu], [TrangThaiTheChap]) VALUES (2, N'BAT_DONG_SAN', N'Căn hộ 2PN Vinhomes Smart City, Nam Từ Liêm', 3200000000.00, N'GCN-BDS-HN-778899', NULL, N'DANG_CAM_CO');
INSERT INTO dbo.[TaiSanDamBao] ([MaTaiSan], [LoaiTaiSan], [TenTaiSan], [GiaTriDinhGia], [SoGiayToPhapLy], [MaSoTietKiemThamChieu], [TrangThaiTheChap]) VALUES (3, N'SO_TIET_KIEM', N'Sổ tiết kiệm STK-2026-0005 (5 Tỷ)', 5000000000.00, N'STK-2026-0005', 5, N'DANG_CAM_CO');

SET IDENTITY_INSERT dbo.[TaiSanDamBao] OFF;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.KhoanVay (7 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[KhoanVay] ON;

INSERT INTO dbo.[KhoanVay] ([MaKhoanVay], [MaSoHopDongVay], [MaKhachHang], [MaTaiKhoanGiaiNgan], [MaTaiSanDamBao], [GoiVay], [SoTienVayBanDau], [DuNoGocHienTai], [ThoiHanVayThang], [LaiSuatVayNam], [NgayGiaiNgan], [NgayDaoHanHopDong], [NgayTraNoTiepTheo], [SoTienPhaiTraHangThang], [TrangThaiKhoanVay], [SoNgayQuaHan], [MucDichVay], [NhanVienLapHoSo], [LanhDaoPheDuyet], [NgayPheDuyet], [LyDoTuChoi], [NgayTao]) VALUES (1, N'HDV-2026-0001', 4, 4, 1, N'THE_CHAP_SO_TIET_KIEM', 2000000000.00, 1850000000.00, 36, 7.50, '2026-07-10', '2029-07-10', '2026-11-20', 62211000.00, N'DANG_VAY', 0, N'Bổ sung vốn lưu động chuỗi nhà hàng', 4, 1, '2026-07-10 14:14:19', NULL, '2026-11-10 14:14:19');
INSERT INTO dbo.[KhoanVay] ([MaKhoanVay], [MaSoHopDongVay], [MaKhachHang], [MaTaiKhoanGiaiNgan], [MaTaiSanDamBao], [GoiVay], [SoTienVayBanDau], [DuNoGocHienTai], [ThoiHanVayThang], [LaiSuatVayNam], [NgayGiaiNgan], [NgayDaoHanHopDong], [NgayTraNoTiepTheo], [SoTienPhaiTraHangThang], [TrangThaiKhoanVay], [SoNgayQuaHan], [MucDichVay], [NhanVienLapHoSo], [LanhDaoPheDuyet], [NgayPheDuyet], [LyDoTuChoi], [NgayTao]) VALUES (2, N'HDV-2026-0002', 3, 3, NULL, N'TIEU_DUNG_TIN_CHAP', 150000000.00, 150000000.00, 12, 11.50, '2026-11-10', '2027-11-10', '2026-12-10', 13293000.00, N'CHO_PHE_DUYET', 0, N'Mua sắm thiết bị gia đình', 4, NULL, NULL, NULL, '2026-11-10 14:14:19');
INSERT INTO dbo.[KhoanVay] ([MaKhoanVay], [MaSoHopDongVay], [MaKhachHang], [MaTaiKhoanGiaiNgan], [MaTaiSanDamBao], [GoiVay], [SoTienVayBanDau], [DuNoGocHienTai], [ThoiHanVayThang], [LaiSuatVayNam], [NgayGiaiNgan], [NgayDaoHanHopDong], [NgayTraNoTiepTheo], [SoTienPhaiTraHangThang], [TrangThaiKhoanVay], [SoNgayQuaHan], [MucDichVay], [NhanVienLapHoSo], [LanhDaoPheDuyet], [NgayPheDuyet], [LyDoTuChoi], [NgayTao]) VALUES (3, N'HDV-2026-0003', 2, 2, 2, N'VAY_MUA_NHA_BDS', 2200000000.00, 2200000000.00, 36, 8.90, '2026-11-10', '2029-11-10', '2026-12-10', 69860000.00, N'CHO_PHE_DUYET', 0, N'Mua căn hộ để ở', 4, NULL, NULL, NULL, '2026-11-10 14:14:19');
INSERT INTO dbo.[KhoanVay] ([MaKhoanVay], [MaSoHopDongVay], [MaKhachHang], [MaTaiKhoanGiaiNgan], [MaTaiSanDamBao], [GoiVay], [SoTienVayBanDau], [DuNoGocHienTai], [ThoiHanVayThang], [LaiSuatVayNam], [NgayGiaiNgan], [NgayDaoHanHopDong], [NgayTraNoTiepTheo], [SoTienPhaiTraHangThang], [TrangThaiKhoanVay], [SoNgayQuaHan], [MucDichVay], [NhanVienLapHoSo], [LanhDaoPheDuyet], [NgayPheDuyet], [LyDoTuChoi], [NgayTao]) VALUES (4, N'HDV-2026-0004', 7, 7, 3, N'THE_CHAP_SO_TIET_KIEM', 3000000000.00, 2750000000.00, 36, 7.50, '2026-06-18', '2029-06-18', '2026-12-03', 93300000.00, N'DANG_VAY', 0, N'Vay kinh doanh mở rộng xưởng sản xuất', 4, 1, '2026-06-18 15:35:37', NULL, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhoanVay] ([MaKhoanVay], [MaSoHopDongVay], [MaKhachHang], [MaTaiKhoanGiaiNgan], [MaTaiSanDamBao], [GoiVay], [SoTienVayBanDau], [DuNoGocHienTai], [ThoiHanVayThang], [LaiSuatVayNam], [NgayGiaiNgan], [NgayDaoHanHopDong], [NgayTraNoTiepTheo], [SoTienPhaiTraHangThang], [TrangThaiKhoanVay], [SoNgayQuaHan], [MucDichVay], [NhanVienLapHoSo], [LanhDaoPheDuyet], [NgayPheDuyet], [LyDoTuChoi], [NgayTao]) VALUES (5, N'HDV-2026-0005', 9, 9, NULL, N'TIEU_DUNG_TIN_CHAP', 400000000.00, 400000000.00, 24, 10.50, '2026-11-18', '2028-11-18', '2026-12-18', 18550000.00, N'CHO_PHE_DUYET', 0, N'Vay mua ô tô phục vụ công tác', 4, NULL, NULL, NULL, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhoanVay] ([MaKhoanVay], [MaSoHopDongVay], [MaKhachHang], [MaTaiKhoanGiaiNgan], [MaTaiSanDamBao], [GoiVay], [SoTienVayBanDau], [DuNoGocHienTai], [ThoiHanVayThang], [LaiSuatVayNam], [NgayGiaiNgan], [NgayDaoHanHopDong], [NgayTraNoTiepTheo], [SoTienPhaiTraHangThang], [TrangThaiKhoanVay], [SoNgayQuaHan], [MucDichVay], [NhanVienLapHoSo], [LanhDaoPheDuyet], [NgayPheDuyet], [LyDoTuChoi], [NgayTao]) VALUES (6, N'HDV-2026-0006', 13, 13, NULL, N'SAN_XUAT_KINH_DOANH', 5000000000.00, 4600000000.00, 60, 8.20, '2026-05-18', '2031-05-18', '2026-11-23', 101890000.00, N'DANG_VAY', 0, N'Bổ sung vốn lưu động hệ thống siêu thị Masan', 4, 1, '2026-05-18 15:35:37', NULL, '2026-11-18 15:35:37');
INSERT INTO dbo.[KhoanVay] ([MaKhoanVay], [MaSoHopDongVay], [MaKhachHang], [MaTaiKhoanGiaiNgan], [MaTaiSanDamBao], [GoiVay], [SoTienVayBanDau], [DuNoGocHienTai], [ThoiHanVayThang], [LaiSuatVayNam], [NgayGiaiNgan], [NgayDaoHanHopDong], [NgayTraNoTiepTheo], [SoTienPhaiTraHangThang], [TrangThaiKhoanVay], [SoNgayQuaHan], [MucDichVay], [NhanVienLapHoSo], [LanhDaoPheDuyet], [NgayPheDuyet], [LyDoTuChoi], [NgayTao]) VALUES (7, N'HDV-2026-0007', 16, 16, NULL, N'TIEU_DUNG_TIN_CHAP', 80000000.00, 80000000.00, 12, 11.00, '2026-11-18', '2027-11-18', '2026-12-18', 7070000.00, N'DANG_VAY', 0, N'Mua trang thiết bị nhà kính trồng trọt', 4, 1, '2026-11-18 15:54:10', NULL, '2026-11-18 15:35:37');

SET IDENTITY_INSERT dbo.[KhoanVay] OFF;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.GiaoDich (27 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[GiaoDich] ON;

INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (1, N'FT26111014141947642', N'19034589201928', N'Phạm Minh Quân', N'19034589201999', N'Lê Thị Thu Hương', N'Techcombank', N'CHUYEN_KHOAN_NOI_BO', 50000000.00, 0.00, 1250000000.00, 1200000000.00, N'Thanh toán hợp đồng tư vấn', '2026-11-10 14:14:19', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (2, N'FT26111014142001814', N'19034589201777', N'Vũ Hoàng Nam', N'19034589201928', N'Phạm Minh Quân', N'Techcombank', N'CHUYEN_KHOAN_NOI_BO', 600000000.00, 0.00, 2680000000.00, 2080000000.00, N'Chuyển tiền đầu tư căn hộ Sala', '2026-11-10 14:14:20', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (3, N'FT26111014142005201', N'19034589201888', N'Nguyễn Văn An', N'19034589201666', N'Hoàng Thu Thảo', N'Techcombank', N'CHUYEN_KHOAN_NOI_BO', 15000000.00, 3300.00, 92000000.00, 76996700.00, N'Thanh toán tiền thuê văn phòng', '2026-11-10 14:14:20', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (4, N'FT26111014142005325', N'19034589201666', N'Hoàng Thu Thảo', N'19034589201555', N'Trần Đức Minh', N'Techcombank', N'CHUYEN_KHOAN_NOI_BO', 2500000.00, 3300.00, 57000000.00, 54496700.00, N'Trả tiền ăn trưa nhóm', '2026-11-10 14:14:20', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (5, N'FT26111014142005433', N'19034589201555', N'Trần Đức Minh', N'19034589201888', N'Nguyễn Văn An', N'Techcombank', N'CHUYEN_KHOAN_NOI_BO', 12000000.00, 3300.00, 120500000.00, 108496700.00, N'Hoàn tiền vật tư công trình', '2026-11-10 14:14:20', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (6, N'FT261112001', N'19034589201928', N'Phạm Minh Quân', N'19034589201999', N'Lê Thị Thu Hương', NULL, N'CHUYEN_KHOAN_NOI_BO', 45000000.00, 0.00, 1845000000.00, 1800000000.00, N'Chuyển tiền quyết toán dự án Q3', '2026-11-12 09:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (7, N'FT261112002', N'19034589202001', N'Đặng Quốc Bảo', N'19034589202004', N'Ngô Thu Trang', NULL, N'CHUYEN_KHOAN_NOI_BO', 120000000.00, 0.00, 3620000000.00, 3500000000.00, N'Chuyển tiền mua vật liệu xây dựng biệt thự', '2026-11-12 14:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (8, N'FT261113001', N'19034589202007', N'Lâm Thế Vinh', N'19034589202001', N'Đặng Quốc Bảo', NULL, N'CHUYEN_KHOAN_NOI_BO', 350000000.00, 0.00, 4550000000.00, 4200000000.00, N'Thanh toán cổ tức năm 2026', '2026-11-13 10:00:00', N'THANH_CONG', N'Hệ thống tự động', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (9, N'FT261113002', N'19034589201888', N'Nguyễn Văn An', N'19034589202003', N'Bùi Anh Dũng', NULL, N'CHUYEN_KHOAN_NOI_BO', 28000000.00, 0.00, 116996700.00, 88996700.00, N'Chuyển tiền thanh toán hợp đồng du lịch', '2026-11-13 15:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (10, N'FT261114001', N'19034589202002', N'Vũ Bích Ngọc', N'19034589202009', N'Dương Mỹ Linh', NULL, N'CHUYEN_KHOAN_NOI_BO', 85000000.00, 0.00, 535000000.00, 450000000.00, N'Thanh toán tiền thiết kế nội thất căn hộ', '2026-11-14 08:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (11, N'FT261114002', N'19034589201777', N'Vũ Hoàng Nam', N'19034589202007', N'Lâm Thế Vinh', NULL, N'CHUYEN_KHOAN_NOI_BO', 650000000.00, 0.00, 2730000000.00, 2080000000.00, N'Đầu tư tài chính giai đoạn 2 chuỗi Masan', '2026-11-14 11:00:00', N'THANH_CONG', N'Trần Mai Linh', 1, N'Giao dịch lớn đột biến vượt 500 triệu đồng');
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (12, N'FT261114003', N'19034589202010', N'Hồ Gia Bảo', N'19034589202008', N'Đỗ Hoài Nam', NULL, N'CHUYEN_KHOAN_NOI_BO', 15000000.00, 3300.00, 67003300.00, 52000000.00, N'Chuyển tiền hỗ trợ gia đình', '2026-11-14 16:00:00', N'THANH_CONG', N'Hệ thống tự động', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (13, N'FT261115001', N'19034589202004', N'Ngô Thu Trang', N'19034589201928', N'Phạm Minh Quân', NULL, N'CHUYEN_KHOAN_NOI_BO', 210000000.00, 0.00, 890000000.00, 680000000.00, N'Hợp đồng cung cấp dịch vụ phần mềm quản lý', '2026-11-15 10:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (14, N'FT261115002', N'19034589202003', N'Bùi Anh Dũng', N'19034589201888', N'Nguyễn Văn An', NULL, N'CHUYEN_KHOAN_NOI_BO', 42000000.00, 0.00, 162000000.00, 120000000.00, N'Thanh toán tiền máy móc thiết bị viễn thông', '2026-11-15 14:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (15, N'FT261116001', N'19034589202001', N'Đặng Quốc Bảo', N'19034589202002', N'Vũ Bích Ngọc', NULL, N'CHUYEN_KHOAN_NOI_BO', 180000000.00, 0.00, 3680000000.00, 3500000000.00, N'Chi trả chi phí quảng cáo truyền thông Q4', '2026-11-16 09:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (16, N'FT261116002', N'19034589202007', N'Lâm Thế Vinh', N'19034589201999', N'Lê Thị Thu Hương', NULL, N'CHUYEN_KHOAN_NOI_BO', 420000000.00, 0.00, 4620000000.00, 4200000000.00, N'Hợp đồng nhập khẩu nông sản sạch', '2026-11-16 13:00:00', N'THANH_CONG', N'Trần Mai Linh', 1, N'Giao dịch vượt ngưỡng 400 triệu đồng');
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (17, N'FT261116003', N'19034589202009', N'Dương Mỹ Linh', N'19034589202005', N'Phan Đình Tùng', NULL, N'CHUYEN_KHOAN_NOI_BO', 35000000.00, 0.00, 220000000.00, 185000000.00, N'Thanh toán hoa hồng môi giới dự án BĐS', '2026-11-16 17:00:00', N'THANH_CONG', N'Hệ thống tự động', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (18, N'FT261117001', N'19034589201999', N'Lê Thị Thu Hương', N'19034589202001', N'Đặng Quốc Bảo', NULL, N'CHUYEN_KHOAN_NOI_BO', 95000000.00, 0.00, 490000000.00, 395000000.00, N'Tất toán đợt 1 hợp đồng bảo dưỡng thiết bị', '2026-11-17 10:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (19, N'FT261117002', N'19034589202004', N'Ngô Thu Trang', N'19034589202010', N'Hồ Gia Bảo', NULL, N'CHUYEN_KHOAN_NOI_BO', 55000000.00, 0.00, 735000000.00, 680000000.00, N'Chuyển tiền cọc thuê mặt bằng bán lẻ', '2026-11-17 14:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (20, N'FT261117003', N'19034589201888', N'Nguyễn Văn An', N'19034589201666', N'Hoàng Thu Thảo', NULL, N'CHUYEN_KHOAN_NOI_BO', 15000000.00, 3300.00, 103996700.00, 88996700.00, N'Thanh toán tiền thuê văn phòng làm việc', '2026-11-17 16:00:00', N'THANH_CONG', N'Hệ thống tự động', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (21, N'FT261118001', N'19034589201777', N'Vũ Hoàng Nam', N'19034589201928', N'Phạm Minh Quân', NULL, N'CHUYEN_KHOAN_NOI_BO', 600000000.00, 0.00, 2680000000.00, 2080000000.00, N'Chuyển tiền đầu tư căn hộ cao cấp Sala', '2026-11-18 08:00:00', N'THANH_CONG', N'Trần Mai Linh', 1, N'Giao dịch lớn đột biến vượt 500 triệu đồng');
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (22, N'FT261118002', N'19034589201928', N'Phạm Minh Quân', N'19034589201999', N'Lê Thị Thu Hương', NULL, N'CHUYEN_KHOAN_NOI_BO', 50000000.00, 0.00, 1850000000.00, 1800000000.00, N'Chuyển tiền thanh toán hợp đồng tư vấn tài chính', '2026-11-18 11:00:00', N'THANH_CONG', N'Trần Mai Linh', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (23, N'FT261118003', N'19034589202007', N'Lâm Thế Vinh', N'19034589202004', N'Ngô Thu Trang', NULL, N'CHUYEN_KHOAN_NOI_BO', 280000000.00, 0.00, 4480000000.00, 4200000000.00, N'Thanh toán đơn hàng linh kiện công nghệ', '2026-11-18 14:00:00', N'THANH_CONG', N'Hệ thống tự động', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (24, N'GN26111815541071846', N'HOI_SO_GIAI_NGAN', N'Khối Tín dụng – Hội sở', N'19034589202010', N'Hồ Gia Bảo', NULL, N'GIAI_NGAN_KHOAN_VAY', 80000000.00, 0.00, 52000000.00, 132000000.00, N'Giải ngân hợp đồng HDV-2026-0007', NULL, N'THANH_CONG', N'Nguyễn Đình Trọng (Giám Đốc Chi Nhánh)', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (25, N'DP20261118155504815', N'QUAY_TIEN_MAT', N'Nộp Tiền Mặt Tại Quầy', N'19032335811471', N'Nguyen van b', NULL, N'NAP_TIEN_MAT', 1000000.00, 0.00, 0.00, 1000000.00, N'Nộp tiền ban đầu kích hoạt tài khoản thanh toán', '2026-11-18 15:55:04', N'THANH_CONG', N'Nguyễn Đình Trọng (Giám Đốc Chi Nhánh)', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (26, N'FT26111815560002062', N'19034589202003', N'Bùi Anh Dũng', N'19034589202004', N'Ngô Thu Trang', N'Techcombank', N'CHUYEN_KHOAN_NOI_BO', 10000000.00, 0.00, 120000000.00, 110000000.00, N'Chuyển tiền cho trang', '2026-11-18 15:56:00', N'THANH_CONG', N'Nguyễn Đình Trọng (Giám Đốc Chi Nhánh)', 0, NULL);
INSERT INTO dbo.[GiaoDich] ([MaGiaoDich], [MaChungTu], [SoTaiKhoanNguon], [TenNguoiGui], [SoTaiKhoanDich], [TenNguoiNhan], [NganHangDich], [LoaiGiaoDich], [SoTienGiaoDich], [PhiGiaoDich], [SoDuTruocGD], [SoDuSauGD], [NoiDungChuyenKhoan], [ThoiGianGiaoDich], [TrangThaiGiaoDich], [NhanVienThucHien], [CanhBaoRuaTienAML], [LyDoCanhBaoAML]) VALUES (27, N'TK26111815565270469', N'QUAY_TIEN_MAT', N'Nguyen Van A (Nộp tiền mặt tại quầy)', N'STK-2026-7526', N'Quỹ tiết kiệm Techcombank', NULL, N'MO_SO_TIET_KIEM', 10000000.00, 0.00, 0.00, 10000000.00, N'Mở sổ STK-2026-7526 nộp tiền mặt tại quầy, kỳ hạn 12 tháng, lãi suất 6,20%/năm', NULL, N'THANH_CONG', N'Nguyễn Đình Trọng (Giám Đốc Chi Nhánh)', 0, NULL);

SET IDENTITY_INSERT dbo.[GiaoDich] OFF;
GO

-- ---------------------------------------------------------------------------------
-- Dữ liệu bảng dbo.CanhBaoAML (4 bản ghi)
-- ---------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.[CanhBaoAML] ON;

INSERT INTO dbo.[CanhBaoAML] ([MaCanhBao], [MaGiaoDichThamChieu], [SoTaiKhoanPhatSinh], [TenKhachHang], [SoTienPhatSinh], [MucDoRuiRo], [LyDoTriggerPhatHien], [ThoiGianPhatHien], [TrangThaiXuLy], [NguoiKiemDuyet], [ThoiGianKiemDuyet]) VALUES (1, 2, N'19034589201777', N'Vũ Hoàng Nam', 600000000.00, N'TRUNG_BINH', N'Phát hiện giao dịch chuyển khoản giá trị lớn 600,000,000 đ theo quy định AML', '2026-11-10 14:14:20', N'KHOA_DONG_BANG_TAI_KHOAN', N'Nguyễn Đình Trọng (Giám Đốc Chi Nhánh)', '2026-11-18 15:57:21');
INSERT INTO dbo.[CanhBaoAML] ([MaCanhBao], [MaGiaoDichThamChieu], [SoTaiKhoanPhatSinh], [TenKhachHang], [SoTienPhatSinh], [MucDoRuiRo], [LyDoTriggerPhatHien], [ThoiGianPhatHien], [TrangThaiXuLy], [NguoiKiemDuyet], [ThoiGianKiemDuyet]) VALUES (2, 21, N'19034589201777', N'Vũ Hoàng Nam', 600000000.00, N'TRUNG_BINH', N'Phát hiện giao dịch chuyển khoản giá trị lớn 600,000,000 đ theo quy định AML', '2026-11-18 15:34:36', N'DA_DUYET_AN_TOAN', N'Nguyễn Đình Trọng (Giám Đốc Chi Nhánh)', '2026-11-18 15:57:18');
INSERT INTO dbo.[CanhBaoAML] ([MaCanhBao], [MaGiaoDichThamChieu], [SoTaiKhoanPhatSinh], [TenKhachHang], [SoTienPhatSinh], [MucDoRuiRo], [LyDoTriggerPhatHien], [ThoiGianPhatHien], [TrangThaiXuLy], [NguoiKiemDuyet], [ThoiGianKiemDuyet]) VALUES (3, 16, N'19034589202007', N'Lâm Thế Vinh', 420000000.00, N'TRUNG_BINH', N'Phát hiện giao dịch chuyển khoản giá trị lớn 420,000,000 đ theo quy định AML', '2026-11-18 15:34:36', N'CHO_KIEM_DUYET', NULL, NULL);
INSERT INTO dbo.[CanhBaoAML] ([MaCanhBao], [MaGiaoDichThamChieu], [SoTaiKhoanPhatSinh], [TenKhachHang], [SoTienPhatSinh], [MucDoRuiRo], [LyDoTriggerPhatHien], [ThoiGianPhatHien], [TrangThaiXuLy], [NguoiKiemDuyet], [ThoiGianKiemDuyet]) VALUES (4, 11, N'19034589201777', N'Vũ Hoàng Nam', 650000000.00, N'TRUNG_BINH', N'Phát hiện giao dịch chuyển khoản giá trị lớn 650,000,000 đ theo quy định AML', '2026-11-18 15:34:36', N'CHO_KIEM_DUYET', NULL, NULL);

SET IDENTITY_INSERT dbo.[CanhBaoAML] OFF;
GO

-- =================================================================================
-- 7. KÍCH HOẠT LẠI TRIGGERS SAU KHI NẠP DỮ LIỆU
-- =================================================================================
ENABLE TRIGGER trg_TuDongThangHangHoiVien ON dbo.GiaoDich;
ENABLE TRIGGER trg_PhatHienGiaoDichDangNgoAML ON dbo.GiaoDich;
GO

PRINT N'>>> ĐÃ TẠO TOÀN BỘ CƠ SỞ DỮ LIỆU CoSoDuLieuCoreBanking VÀ NẠP DỮ LIỆU THÀNH CÔNG! <<<';
GO
