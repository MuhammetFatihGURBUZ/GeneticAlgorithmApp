using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace GA_Optimizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Grafik serilerini tanımla
            chart1.Series.Clear();  // Önce temizle

            // En iyi çözüm noktası (mavi daire)
            chart1.Series.Add("EnIyiCozum");
            chart1.Series["EnIyiCozum"].ChartType = SeriesChartType.Point;
            chart1.Series["EnIyiCozum"].Color = Color.Blue;
            chart1.Series["EnIyiCozum"].MarkerStyle = MarkerStyle.Circle;
            chart1.Series["EnIyiCozum"].MarkerSize = 10;
            // Yakınsama grafiği (turuncu çizgi)
            chart1.Series.Add("Yakinsama");
            chart1.Series["Yakinsama"].ChartType = SeriesChartType.Line;
            chart1.Series["Yakinsama"].Color = Color.Orange;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan parametreleri al
            int populasyonBoyutu = int.Parse(txtPopulasyon.Text);
            double caprazlamaOrani = double.Parse(txtCaprazlama.Text);
            double mutasyonOrani = double.Parse(txtMutasyon.Text);
            int jenerasyonSayisi = int.Parse(txtJenerasyon.Text);
            int genSayisi = 2; // Örnek olarak 2 değişkenli bir problem için
            // Genetik Algoritmayı başlat
            GenetikAlgoritma ga = new GenetikAlgoritma(populasyonBoyutu, caprazlamaOrani, mutasyonOrani, jenerasyonSayisi, genSayisi);
            Kromozom enIyiCozum = ga.Calistir();
            // En iyi çözümü ekrana yazdır
            lblSonuc.Text = $"En İyi Çözüm: {string.Join(", ", enIyiCozum.Genler)} - Uygunluk: {enIyiCozum.Uygunluk}";
            // Yakınsama grafiğini güncelle
            chart1.Series["Yakinsama"].Points.Clear();
            for (int i = 0; i < ga.YakinsamaVerisi.Count; i++)
            {
                chart1.Series["Yakinsama"].Points.AddXY(i + 1, ga.YakinsamaVerisi[i]);
            }
            // En İyi Çözüm Noktasını Ekle
            chart1.Series["EnIyiCozum"].Points.Clear();
            chart1.Series["EnIyiCozum"].Points.AddXY(ga.YakinsamaVerisi.Count, enIyiCozum.Uygunluk);

            // En iyi çözüm grafiğini güncelle
            chart1.Series["EnIyiCozum"].Points.Clear();
            for (int i = 0; i < enIyiCozum.Genler.Length; i++)
            {
                chart1.Series["EnIyiCozum"].Points.AddXY(i + 1, enIyiCozum.Genler[i]);
            }
        }
        private void lblSonuc_Click(object sender, EventArgs e)
        {

        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
