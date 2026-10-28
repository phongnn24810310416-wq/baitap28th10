# Bai tap 4.1 va 4.2 - WinForms + SQL Server

## Ket noi SQL Server
Chuoi ket noi dung ten may chu `PHBEE` va database `BaiTap4`:
`Data Source=PHBEE;Initial Catalog=BaiTap4;Integrated Security=True;TrustServerCertificate=True`

Neu SSMS cua ban dang ket noi bang ten may chu khac, sua `Data Source` trong `KetNoi.cs` cua ca hai project.

## Cai dat database
1. Mo SQL Server Management Studio (SSMS).
2. Ket noi toi server `PHBEE` bang Windows Authentication.
3. Mo file `database.sql`, bam Execute.
4. Database `BaiTap4` va cac bang can thiet se duoc tao neu chua co.

Tai khoan mau de thu dang nhap:
- Ten dang nhap: `admin`
- Mat khau: `admin123`

Mat khau duoc luu dang SHA-256 hash, khong luu dang van ban thuong.

## Chay chuong trinh
1. Mo `BaiTap4_Login_DangKy_CSDL.sln` trong Visual Studio 2019/2022.
2. Chuot phai project `Bai4.1_LoginForm` > Set as Startup Project > F5.
3. De chay bai 4.2, chon project `Bai4.2_DangKyHocVien` > Set as Startup Project > F5.
4. Can cai .NET Framework 4.8 Developer Pack.

## Chuc nang
- Bai 4.1: ErrorProvider, hien/an mat khau, Enter/Esc, xac thuc tai khoan trong SQL Server va ghi nhat ky dang nhap.
- Bai 4.2: MaskedTextBox, DateTimePicker, ComboBox binding, RadioButton, CheckBox; luu hoc vien vao SQL Server va hien danh sach da dang ky.
