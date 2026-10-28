IF DB_ID(N'BaiTap4') IS NULL
    CREATE DATABASE BaiTap4;
GO

USE BaiTap4;
GO

IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TaiKhoan
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TenDangNhap NVARCHAR(100) NOT NULL UNIQUE,
        MatKhauHash CHAR(64) NOT NULL,
        NgayTao DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.NhatKyDangNhap', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.NhatKyDangNhap
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TenDangNhap NVARCHAR(100) NOT NULL,
        ThanhCong BIT NOT NULL,
        ThoiGian DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.DangKyHocVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DangKyHocVien
    (
        MaHocVien INT IDENTITY(1,1) PRIMARY KEY,
        HoTen NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NULL,
        SoDienThoai NVARCHAR(20) NOT NULL,
        NgaySinh DATE NOT NULL,
        GioiTinh NVARCHAR(20) NOT NULL,
        MaKhoaHoc NVARCHAR(30) NOT NULL,
        TenKhoaHoc NVARCHAR(100) NOT NULL,
        DongYDieuKhoan BIT NOT NULL,
        NgayDangKy DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE TenDangNhap = N'admin')
BEGIN
    INSERT INTO dbo.TaiKhoan(TenDangNhap, MatKhauHash)
    VALUES (N'admin', '240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9');
END
GO

SELECT * FROM dbo.TaiKhoan;
SELECT * FROM dbo.DangKyHocVien;
SELECT * FROM dbo.NhatKyDangNhap;
GO
