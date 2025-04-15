using System;
using System.Collections.Generic;
using System.Linq;

public class GenetikAlgoritma
{
    public List<double> YakinsamaVerisi = new List<double>();
    private List<Kromozom> Populasyon;
    private int PopulasyonBoyutu;
    private double CaprazlamaOrani;
    private double MutasyonOrani;
    private int JenerasyonSayisi;
    private int GenSayisi;
    private static Random rastgele = new Random();

    public GenetikAlgoritma(int populasyonBoyutu, double caprazlamaOrani, double mutasyonOrani, int jenerasyonSayisi, int genSayisi)
    {
        this.PopulasyonBoyutu = populasyonBoyutu;
        this.CaprazlamaOrani = caprazlamaOrani;
        this.MutasyonOrani = mutasyonOrani;
        this.JenerasyonSayisi = jenerasyonSayisi;
        this.GenSayisi = genSayisi;
        Populasyon = new List<Kromozom>();

        PopulasyonuBaslat();
    }

    // Rastgele popülasyon oluştur
    private void PopulasyonuBaslat()
    {
        for (int i = 0; i < PopulasyonBoyutu; i++)
        {
            Populasyon.Add(new Kromozom(GenSayisi));
        }
    }

    // Uygunluk hesapla (Bütün bireyler için)
    private void UygunlukHesapla()
    {
        foreach (var kromozom in Populasyon)
        {
            kromozom.UygunlukHesapla();
        }
    }

    private double PenaltyFonksiyonu(Kromozom kromozom)
    {
        double x = kromozom.Genler[0];
        double y = kromozom.Genler[1];

        // Eğer x veya y sınırların dışındaysa, ceza uygula
        if (x < -5 || x > 5 || y < -5 || y > 5)
            return Math.Abs(x) + Math.Abs(y); // Sınır dışına ne kadar uzaksa o kadar ceza

        return 0; // Eğer sınırlar içindeyse ceza uygulanmaz
    }

    // Seçim işlemi (Turnuva seçimi)
    private Kromozom Secim()
    {
        // Rastgele iki birey seç
        int idx1 = rastgele.Next(Populasyon.Count);
        int idx2 = rastgele.Next(Populasyon.Count);

        Kromozom birey1 = Populasyon[idx1];
        Kromozom birey2 = Populasyon[idx2];

        // Eğer biri uygunsa onu seç
        if (PenaltyFonksiyonu(birey1) == 0) return birey1;
        if (PenaltyFonksiyonu(birey2) == 0) return birey2;

        // Eğer ikisi de uygunsa, uygunluk değeri küçük olanı seç (minimizasyon)
        if (PenaltyFonksiyonu(birey1) < PenaltyFonksiyonu(birey2))
            return birey1;
        else
            return birey2;
    }

    // Çaprazlama işlemi
    private (Kromozom, Kromozom) Caprazlama(Kromozom ebeveyn1, Kromozom ebeveyn2)
    {
        Kromozom cocuk1 = new Kromozom(GenSayisi);
        Kromozom cocuk2 = new Kromozom(GenSayisi);

        for (int i = 0; i < GenSayisi; i++)
        {
            double rho = rastgele.NextDouble(); // ρ katsayısı (0 ile 1 arasında rastgele)

            // Çaprazlama oranına göre genleri değiştir
            if (rastgele.NextDouble() < CaprazlamaOrani)
            {
                cocuk1.Genler[i] = rho * ebeveyn1.Genler[i] + (1 - rho) * ebeveyn2.Genler[i];
                cocuk2.Genler[i] = (1 - rho) * ebeveyn1.Genler[i] + rho * ebeveyn2.Genler[i];
            }
            else
            {
                // Eğer çaprazlama olmazsa, genler aynı kalır
                cocuk1.Genler[i] = ebeveyn1.Genler[i];
                cocuk2.Genler[i] = ebeveyn2.Genler[i];
            }
        }

        return (cocuk1, cocuk2);
    }


    // Mutasyon işlemi
    private void Mutasyon(Kromozom kromozom)
    {
        for (int i = 0; i < kromozom.Genler.Length; i++)
        {
            double rho = rastgele.NextDouble(); // ρ değerini üret

            if (rho < MutasyonOrani)
            {
                // Yeni gen değerini -5 ile 5 arasında rastgele seç
                kromozom.Genler[i] = rastgele.NextDouble() * 10 - 5;
            }
        }
    }

    // Algoritmayı çalıştır
    public Kromozom Calistir()
    {
        double hedefUygunluk = 0.0001;  // 🔸 Burayı ayarlayabilirsin

        for (int jenerasyon = 0; jenerasyon < JenerasyonSayisi; jenerasyon++)
        {
            List<Kromozom> yeniNesil = new List<Kromozom>();

            for (int i = 0; i < PopulasyonBoyutu / 2; i++)  // Her turda 2 çocuk üretilecek
            {
                Kromozom ebeveyn1 = Secim();
                Kromozom ebeveyn2 = Secim();

                (Kromozom cocuk1, Kromozom cocuk2) = Caprazlama(ebeveyn1, ebeveyn2);

                Mutasyon(cocuk1);
                Mutasyon(cocuk2);

                yeniNesil.Add(cocuk1);
                yeniNesil.Add(cocuk2);
            }
            Populasyon = yeniNesil;
            // Uygunlukları güncelle
            UygunlukHesapla();

            // En iyi bireyi bul
            Kromozom enIyi = Populasyon.OrderBy(k => k.Uygunluk).First();

            // 🔹 Yakınsama verisini kaydet
            YakinsamaVerisi.Add(enIyi.Uygunluk);

            // 🔸 Eğer hedef uygunluk sağlanırsa erken durdur
            if (enIyi.Uygunluk < hedefUygunluk)
                break;
         }

        // 🔚 En iyi çözümü döndür
        return Populasyon.OrderBy(k => k.Uygunluk).First();
    }
}
