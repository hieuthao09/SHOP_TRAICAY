using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace QL_TraiCay.Models
{
    public class Cartitem
    {
        public string iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnh { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public int itags { get; set; }

        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }

        QL_TraiCayDataContext dt = new QL_TraiCayDataContext();

        public Cartitem(string id)
        {
            iMaSP = id;
            TRAICAY sanpham = dt.TRAICAYs.Single(n => n.MATC == id);
            if (id != null)
            {
                sTenSP = sanpham.TENTC;
                sAnh = sanpham.DUONGDAN;
                dDonGia = double.Parse(sanpham.GIAMOI.ToString());
                //itags = int.Parse(sanpham..ToString());
                iSoLuong = 1;
            }
        }

    }
     public class RandomString
        {
            public static int RandomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }

            public static string GetString(int size, bool lowerCase)
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }


        }
        public class GioHang
        {
            List<Cartitem> ds;

            public List<Cartitem> Ds
            {
                get { return ds; }
                set { ds = value; }
            }
            public GioHang()
            {
                Ds = new List<Cartitem>();
            }
            public GioHang(List<Cartitem> dsGH)
            {
                if (dsGH == null)
                    Ds = new List<Cartitem>();
                else
                    Ds = dsGH;
            }
            public int SoMatHang()
            {
                if (ds == null)
                    return 0;
                return ds.Count;
            }
            public int TongSLHang()
            {
                int tong = 0;
                if (ds != null)
                {
                    tong = ds.Sum(n => n.iSoLuong);
                    return tong;
                }
                return 0;
            }
            public double TongThanhTien()
            {
                double tong = 0;
                if (ds != null)
                {
                    tong = ds.Sum(n => n.ThanhTien);
                    return tong;
                }
                return 0;
            }
            public int Them(string iMa, int sl)
            {
                Cartitem sp = ds.Find(n => n.iMaSP == iMa);
                if (sp == null)
                {
                    Cartitem sanpham = new Cartitem(iMa);
                    if (sanpham == null)
                    {
                        return -1;
                    }
                    sanpham.iSoLuong = sl;
                    ds.Add(sanpham);
                }
                else
                {
                    sp.iSoLuong += sl;
                }
                return 1;
            }


            public int Xoa(string iMa)
            {
                Cartitem sp = ds.Find(n => n.iMaSP == iMa);


                Cartitem sanpham = new Cartitem(iMa);
                ds.Remove(sanpham);
                sp.iSoLuong--;

                return 1;

            }

            public int XoaGioHang()
            {
                if (ds == null)
                    return 1;
                else
                {
                    ds.Clear();
                    return 1;
                }
            }

        }
    }