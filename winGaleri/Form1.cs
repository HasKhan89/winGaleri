using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winGaleri
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mnuEkle_Click(object sender, EventArgs e)
        {
            // Ekle seçeneği ile belirlediğimiz dosya türündeki dosyaları seçmek için open dialog açılır. Çoklu seçim yaptırılır(bkz. ofd.Multiselect = true)
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPEG Resimler(*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG Resimler(*.png)|*.png|Tüm Dosyalar(*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.FilterIndex = 3;
            ofd.Title = "Resimleri Seç";
            ofd.Multiselect = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                // Seçilen dosya konumları elde edilir.
                string[] dosyaKonumlari = ofd.FileNames;

                // Her dosya konumu için picturebox FlowPanel'e eklenir.
                foreach (string dosyaKonum in dosyaKonumlari)
                {
                    PictureBoxEkle(dosyaKonum);
                }
            }


        }

        private void PictureBoxEkle(string dosyaKonum)
        {
            // Önce bir panel oluşturulur. Picturebox etrafında border çıkartmak için..
            Panel pnl = new Panel();
            pnl.BorderStyle = BorderStyle.FixedSingle;
            pnl.Width = 75;
            pnl.Height = 75;

            // Picturebox oluşturulur ve metoda gelen parametre ile göstereceği dosyaKOnum ayarlanır.
            PictureBox pb = new PictureBox();
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.ImageLocation = dosyaKonum;
            pb.Dock = DockStyle.Fill;

            // Picturebox'a tıklanınca çalışacak olan metot işaretlenir(Pb_Click).
            pb.Click += Pb_Click;

            // Picturebox panel'e eklenir.
            pnl.Controls.Add(pb);

            // Panel, FlowPanel'e eklenir.
            flowLayoutPanel1.Controls.Add(pnl);
        }


        private void Pb_Click(object sender, EventArgs e)
        {
            // Picturebox'a tıklandığında..

            // Ana formumuzdaki picturebox elde edilir.
            PictureBox pbox = (PictureBox)sender;

            // Yeni form oluşturulur.(Dilerseniz projeye form ekleyerek yapabilirsiniz)
            Form frm = new Form();
            frm.Text = "Resim Önizleme";
            frm.Width = 250;
            frm.Height = 250;

            // Yeni picturebox oluşturulur. Eğer Ana form'daki picturebox'ı kullanırsak, yeni form'a, ana form'daki picturebox'ı taşır. Amaç yeni bir picturebox'ı yeni form'a eklemek ve ayarlarını ana form'daki picturebox ile aynı yapmak.
            PictureBox pbNew = new PictureBox();
            pbNew.ImageLocation = pbox.ImageLocation;
            pbNew.SizeMode = pbox.SizeMode;
            pbNew.Dock = DockStyle.Fill;

            // Yeni picturebox yeni form'a eklenir.
            frm.Controls.Add(pbNew);

            // Form show edilerek gösterilir. Ana form'a geçebilir şekilde açılır.
            frm.Show();
        }

        private void mnuKaydet_Click(object sender, EventArgs e)
        {
            // Dosya konumlarını toplayacağımız bir liste tanımlanır.
            List<string> kaydedilecekDosyaKonumalari =
                new List<string>();

            // flowLayoutPanel1.Controls özelliği ile ana form'daki FlowPanel'e eklemiş olduğumuz panellere ulaşırız(Panel içinde picturebox). 
            foreach (Panel pnl in flowLayoutPanel1.Controls)
            {
                // Biliyoruz ki her bir panel içinde sadece picturebox var.
                // panel içindeki picturebox elde edilerek, bir değişkene bağlanır.
                PictureBox pbox = (PictureBox)pnl.Controls[0];
                // picturebox'ın gösterdiği dosya konumu listeye eklenir.
                kaydedilecekDosyaKonumalari.Add(pbox.ImageLocation);
            }

            // Kaydetme dialoğu açılır..
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Galeri Kaydet";
            sfd.Filter = "Galeri Dosyaları(*.glr)|*.glr";   // uzantıyı biz uydurduk.
            sfd.FilterIndex = 1;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                // Dosya konumları seçilen konuma kaydedilir.
                System.IO.File.WriteAllLines(sfd.FileName, kaydedilecekDosyaKonumalari);
                MessageBox.Show("Galeri kaydedilmiştir.");
            }
        }

        private void mnuAc_Click(object sender, EventArgs e)
        {
            // Bizim belirlediğimiz uzantıdaki dosyalar açılması için opendialog kullanılır.
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Galeri Aç";
            ofd.Filter = "Galeri Dosyaları(*.glr)|*.glr";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures); // resimlerim klasör'ü başlangıçta açılsın şekilde ayarlanır.

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Seçilen dosyadan resimlerin konumları okunur.(satır satır)
                string[] dosyalar = System.IO.File.ReadAllLines(ofd.FileName);

                // FlowPanel temizlenir.
                flowLayoutPanel1.Controls.Clear();

                // Her bir resim konumu kullanılarak resim flowPanel'e "Ekle" tuşundaki gibi eklenir.
                foreach (string dosyaKonum in dosyalar)
                {
                    PictureBoxEkle(dosyaKonum);
                }
            }
        }

        private void mnuCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
