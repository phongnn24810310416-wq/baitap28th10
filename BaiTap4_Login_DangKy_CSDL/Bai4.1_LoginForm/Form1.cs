using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Bai41LoginForm
{
    public class Form1 : Form
    {
        TextBox txtUsername, txtPassword;
        CheckBox chkShowPassword;
        Button btnLogin, btnExit;
        ErrorProvider errorProvider1;

        public Form1()
        {
            Text = "Bai 4.1 - Login Form"; Width = 420; Height = 280;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false;

            Label lblUser = new Label { Text = "Ten dang nhap:", Left = 30, Top = 35, Width = 120 };
            txtUsername = new TextBox { Name = "txtUsername", Left = 155, Top = 32, Width = 210 };
            Label lblPass = new Label { Text = "Mat khau:", Left = 30, Top = 80, Width = 120 };
            txtPassword = new TextBox { Name = "txtPassword", Left = 155, Top = 77, Width = 210, UseSystemPasswordChar = true };
            chkShowPassword = new CheckBox { Name = "chkShowPassword", Text = "Hien mat khau", Left = 155, Top = 110, Width = 150 };
            chkShowPassword.CheckedChanged += (s, e) => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            btnLogin = new Button { Name = "btnLogin", Text = "Dang nhap", Left = 155, Top = 150, Width = 100, Height = 32 };
            btnLogin.Click += btnLogin_Click;
            btnExit = new Button { Name = "btnExit", Text = "Thoat", Left = 265, Top = 150, Width = 100, Height = 32 };
            btnExit.Click += (s, e) => Close();
            Controls.AddRange(new Control[] { lblUser, txtUsername, lblPass, txtPassword, chkShowPassword, btnLogin, btnExit });

            errorProvider1 = new ErrorProvider { ContainerControl = this };
            AcceptButton = btnLogin;
            CancelButton = btnExit;
        }

        static string MaHoaMatKhau(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes) sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool hopLe = true;
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(txtUsername, "Vui long nhap ten dang nhap");
                hopLe = false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Vui long nhap mat khau");
                hopLe = false;
            }
            if (!hopLe) return;

            try
            {
                bool dung = false;
                using (SqlConnection conn = KetNoi.TaoKetNoi())
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM dbo.TaiKhoan WHERE TenDangNhap=@user AND MatKhauHash=@hash", conn))
                {
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@hash", MaHoaMatKhau(txtPassword.Text));
                    conn.Open();
                    dung = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }

                using (SqlConnection conn = KetNoi.TaoKetNoi())
                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO dbo.NhatKyDangNhap(TenDangNhap, ThanhCong) VALUES(@user, @ok)", conn))
                {
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@ok", dung);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                if (dung) MessageBox.Show("Dang nhap thanh cong!", "Thong bao");
                else MessageBox.Show("Ten dang nhap hoac mat khau khong dung.", "Dang nhap");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Khong ket noi duoc SQL Server. Hay chay database.sql va kiem tra KetNoi.cs.\n" + ex.Message,
                    "Loi co so du lieu");
            }
        }
    }
}
