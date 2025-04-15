using System;

public class Kromozom
{
    public double[] Genler;  // Kromozomun genleri (değişkenler)
    public double Uygunluk;  // Uygunluk (fitness) değeri
    private static Random rastgele = new Random();

    // Kromozomun kaç gen içerdiğini belirleyen kurucu metot
    public Kromozom(int genSayisi)
    {
        Genler = new double[genSayisi];
        GenleriRastgeleAta();
        Uygunluk = 0;  // Başlangıçta uygunluk sıfır
    }

    // Genleri rastgele başlatan metot
    private void GenleriRastgeleAta()
    {
        for (int i = 0; i < Genler.Length; i++)
        {
            Genler[i] = -5 + (rastgele.NextDouble() * 10);  // -5 ile 5 arasında rastgele değer
        }
    }

    // Uygunluk değerini hesapla (Bu test problemine göre değiştirilecek)
    public void UygunlukHesapla()
    {
        // x ve y değerlerini kromozom genlerinden al
        double x = Genler[0];
        double y = Genler[1];

        // f(x, y) = 2x^2 - 1.05x^4 + (x^6)/6 + xy + y^2
        Uygunluk = 2 * Math.Pow(x, 2) - 1.05 * Math.Pow(x, 4) + (Math.Pow(x, 6) / 6) + (x * y) + Math.Pow(y, 2);
    }

    // Kendi kopyasını oluştur (Genetik işlemler için gerekli)
    public Kromozom Kopyala()
    {
        Kromozom kopya = new Kromozom(Genler.Length);
        for (int i = 0; i < Genler.Length; i++)
        {
            kopya.Genler[i] = this.Genler[i];
        }
        kopya.Uygunluk = this.Uygunluk;
        return kopya;
    }
}
