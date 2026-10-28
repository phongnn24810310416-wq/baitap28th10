using System.Data.SqlClient;

namespace Bai42DangKyHocVien
{
    public static class KetNoi
    {
        public static string ChuoiKetNoi =
            @"Data Source=PHBEE;Initial Catalog=BaiTap4;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection TaoKetNoi()
        {
            return new SqlConnection(ChuoiKetNoi);
        }
    }
}
