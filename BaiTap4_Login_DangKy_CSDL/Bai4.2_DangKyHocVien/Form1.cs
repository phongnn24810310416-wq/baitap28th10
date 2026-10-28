using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Bai42DangKyHocVien
{
    public class Form1 : Form
    {
        TextBox txtHoTen, txtEmail;
        MaskedTextBox mtxtPhone;
        DateTimePicker dtpBirthDate;
        ComboBox cboCourse;
        RadioButton radNam, radNu, radKhac;
        CheckBox chkDieuKhoan;
        DataGridView dgvHocVien;

        class KhoaHoc
        {
            public string MaKhoaHoc { get; set; }
            public string TenKhoaHoc { get; set; }
        }

        public Form1()
        {
            Text = "Bai 4.2 - Dang ky hoc vien"; Width = 760; Height = 650;
            StartPosition = FormStartPosition.CenterScreen; Font = new Font("Arial", 10);

            ThemNhan("Ho va ten:", 25, 20); txtHoTen = ThemTextBox(175, 20);
            ThemNhan("Email:", 25, 55); txtEmail = ThemTextBox(175, 55);
            ThemNhan("So dien thoai:", 25, 90);
            mtxtPhone = new MaskedTextBox { Name = "mtxtPhone", Mask = "(000) 000-0000", Left = 175, Top = 90, Width = 220 };
            Controls.Add(mtxtPhone);
            ThemNhan("Ngay sinh:", 25, 125);
            dtpBirthDate = new DateTimePicker { Name = "dtpBirthDate", Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Left = 175, Top = 125, Width = 220 };
            Controls.Add(dtpBirthDate);
            ThemNhan("Khoa hoc:", 25, 160);
            cboCourse = new ComboBox { Name = "cboCourse", DropDownStyle = ComboBoxStyle.DropDownList, DisplayMember = "TenKhoaHoc", ValueMember = "MaKhoaHoc", Left = 175, Top = 160, Width = 220 };
            cboCourse.DataSource = new List<KhoaHoc>
            {
                new KhoaHoc { MaKhoaHoc = "CSharp", TenKhoaHoc = "Lap trinh C#" },
                new KhoaHoc { MaKhoaHoc = "Java", TenKhoaHoc = "Lap trinh Java" },
                new KhoaHoc { MaKhoaHoc = "Web", TenKhoaHoc = "Thiet ke web" },
                new KhoaHoc { MaKhoaHoc = "SQL", TenKhoaHoc = "Co so du lieu SQL" }
            };
            Controls.Add(cboCourse);

            ThemNhan("Gioi tinh:", 25, 195);
            radNam = TaoRadio("Nam", 175, 195); radNu = TaoRadio("Nu", 245, 195); radKhac = TaoRadio("Khac", 305, 195);
            radNam.Checked = true;
            chkDieuKhoan = new CheckBox { Name = "chkDieuKhoan", Text = "Toi dong y voi dieu khoan", Left = 175, Top = 230, Width = 250 };
            Controls.Add(chkDieuKhoan);

            Button btnDangKy = new Button { Text = "Dang ky", Left = 175, Top = 265, Width = 100, Height = 32 };
            btnDangKy.Click += btnDangKy_Click; Controls.Add(btnDangKy);
            Button btnLamMoi = new Button { Text = "Lam moi", Left = 285, Top = 265, Width = 100, Height = 32 };
            btnLamMoi.Click += btnLamMoi_Click; Controls.Add(btnLamMoi);
            Button btnTaiLai = new Button { Text = "Tai danh sach", Left = 395, Top = 265, Width = 120, Height = 32 };
            btnTaiLai.Click += (s, e) => TaiDanhSach(); Controls.Add(btnTaiLai);

            dgvHocVien = new DataGridView { Left = 25, Top = 315, Width = 690, Height = 270, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            Controls.Add(dgvHocVien);
            Load += (s, e) => TaiDanhSach();
        }

        void ThemNhan(string text, int x, int y)
        {
            Label l = new Label { Text = text, Left = x, Top = y + 3, Width = 140 };
            Controls.Add(l);
        }

        TextBox ThemTextBox(int x, int y)
        {
            TextBox t = new TextBox { Left = x, Top = y, Width = 220 };
            Controls.Add(t);
            return t;
        }

        RadioButton TaoRadio(string text, int x, int y)
        {
            RadioButton r = new RadioButton { Text = text, Left = x, Top = y, Width = 65 };
            Controls.Add(r);
            return r;
        }

        void btnDangKy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            { MessageBox.Show("Vui long nhap ho va ten."); txtHoTen.Focus(); return; }
            if (!mtxtPhone.MaskCompleted)
            { MessageBox.Show("Vui long nhap day du so dien thoai."); mtxtPhone.Focus(); return; }
            if (!chkDieuKhoan.Checked)
            { MessageBox.Show("Ban can dong y dieu khoan dang ky."); return; }

            string gioiTinh = radNam.Checked ? "Nam" : radNu.Checked ? "Nu" : "Khac";
            try
            {
                using (SqlConnection conn = KetNoi.TaoKetNoi())
                using (SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO dbo.DangKyHocVien
                    (HoTen, Email, SoDienThoai, NgaySinh, GioiTinh, MaKhoaHoc, TenKhoaHoc, DongYDieuKhoan)
                    VALUES (@ten, @email, @phone, @birth, @gender, @courseId, @courseName, @terms)", conn))
                {
                    cmd.Parameters.Add("@ten", SqlDbType.NVarChar, 100).Value = txtHoTen.Text.Trim();
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim();
                    cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 20).Value = mtxtPhone.Text;
                    cmd.Parameters.Add("@birth", SqlDbType.Date).Value = dtpBirthDate.Value.Date;
                    cmd.Parameters.Add("@gender", SqlDbType.NVarChar, 20).Value = gioiTinh;
                    cmd.Parameters.Add("@courseId", SqlDbType.NVarChar, 30).Value = Convert.ToString(cboCourse.SelectedValue);
                    cmd.Parameters.Add("@courseName", SqlDbType.NVarChar, 100).Value = cboCourse.Text;
                    cmd.Parameters.Add("@terms", SqlDbType.Bit).Value = chkDieuKhoan.Checked;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Dang ky va luu CSDL thanh cong!", "Thong bao");
                TaiDanhSach();
                btnLamMoi_Click(null, EventArgs.Empty);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Khong ket noi duoc SQL Server. Hay chay database.sql va kiem tra KetNoi.cs.\n" + ex.Message,
                    "Loi co so du lieu");
            }
        }

        void TaiDanhSach()
        {
            try
            {
                using (SqlConnection conn = KetNoi.TaoKetNoi())
                using (SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT MaHocVien, HoTen, Email, SoDienThoai, NgaySinh, GioiTinh, TenKhoaHoc, NgayDangKy
                      FROM dbo.DangKyHocVien ORDER BY MaHocVien DESC", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvHocVien.DataSource = dt;
                }
            }
            catch (SqlException ex)
            {
                if (IsHandleCreated)
                    MessageBox.Show("Khong tai duoc danh sach. Hay chay database.sql va kiem tra KetNoi.cs.\n" + ex.Message,
                        "Loi co so du lieu");
            }
        }

        void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear(); txtEmail.Clear(); mtxtPhone.Clear(); dtpBirthDate.Value = DateTime.Today;
            cboCourse.SelectedIndex = 0; radNam.Checked = true; chkDieuKhoan.Checked = false; txtHoTen.Focus();
        }
    }
}
