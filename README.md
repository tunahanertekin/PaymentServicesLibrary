# PaymentServicesLibrary
Akbank, Finansbank İş Bankası ve Garanti Bankası'nın sanal pos entegrasyonlarını barındıran bir sınıf kütüphanesi. 
   
<br>
<h1>Kullanım</h1>

- <h2>Akbank</h2>

  - <h3>Gidiş</h3>
    
    ````csharp
            Pos pos = new Pos(0, "MerchantId|MerchantPassword", "DonusSayfasi.aspx");
            KKart kkart = new KKart("4444333322221111", "12", "2020", "123");
            Kullanici kullanici = new Kullanici("ornekinsan@gmail.com", "Ornek Insan");
            PosIslem posIslem = new PosIslem("1.25", "0", "aciklama", 0);

            AkbankSanalPos asp = new AkbankSanalPos();
            String response = asp.PosESTAkbank(pos, kkart, kullanici, posIslem);
    ````

  - <h3>Dönüş</h3>

    ````csharp
            AkbankSanalPos asp = new AkbankSanalPos();
            Pos pos = new Pos(0, "MerchantId|MerchantPassword", "DonusSayfasi.aspx");
            PosYanit posYanit = asp.PosESTAkbankDonus(pos);
    ````

- <h2>Finansbank</h2>

  - <h3>Gidiş</h3>

    ````csharp
            Pos pos = new Pos(0, "MerchantId|MerchantPassword|UserCode", "DonusSayfasi.aspx");
            KKart kkart = new KKart("4444333322221111", "12", "20", "123");
            Kullanici kullanici = new Kullanici("ornekinsan@gmail.com", "Ornek Insan");
            PosIslem posIslem = new PosIslem("1.25", "0", "aciklama", 0);

            FinansbankSanalPos fsp = new FinansbankSanalPos();
            String response = fsp.PosFinansbank(pos, kkart, kullanici, posIslem);
    ````

  - <h3>Dönüş</h3>

    ````csharp
            FinansbankSanalPos fsp = new FinansbankSanalPos();
            Pos pos = new Pos(0, "MerchantId|MerchantPassword", "DonusSayfasi.aspx");
            PosYanit posYanit = fsp.PosFinansbankDonus(pos);
    ````

- <h2>Garanti Bankası</h2>

  - <h3>Gidiş</h3>

    ````csharp
            Pos pos = new Pos(0, "TerminalId|TerminalMerchantId|StoreKey", "DonusSayfasi.aspx");
            KKart kkart = new KKart("4444333322221111", "12", "2020", "123");
            Kullanici kullanici = new Kullanici("ornekinsan@gmail.com", "Ornek Insan");
            PosIslem posIslem = new PosIslem("1.25", "0", "aciklama", 0);

            GarantiSanalPos gsp = new GarantiSanalPos();
            String response = gsp.PosGaranti3D(pos, kkart, kullanici, posIslem);
    ````

  - <h3>Dönüş</h3>

    ````csharp
            GarantiSanalPos gsp = new GarantiSanalPos();
            Pos pos = new Pos(0, "TerminalId|TerminalMerchantId|StoreKey", "DonusSayfasi.aspx");
            PosYanit posYanit = gsp.PosGaranti3DDonus(pos);
    ````

- <h2>İş Bankası</h2>

  - <h3>Gidiş</h3>

    ````csharp
            Pos pos = new Pos(0, "MerchantId|MerchantPassword", "DonusSayfasi.aspx");
            KKart kkart = new KKart("4444333322221111", "12", "2020", "123");
            Kullanici kullanici = new Kullanici("ornekinsan@gmail.com", "Ornek Insan");
            PosIslem posIslem = new PosIslem("1.25", "0", "aciklama", 0);

            IsBankasiSanalPos ibsp = new IsBankasiSanalPos();
            String response = ibsp.PosIsbank3D(pos, kkart, kullanici, posIslem);
    ````

  - <h3>Dönüş</h3>

    ````csharp
            IsBankasiSanalPos ibsp = new IsBankasiSanalPos();
            Pos pos = new Pos(0, "MerchantId|MerchantPassword", "DonusSayfasi.aspx");
            PosYanit posYanit = ibsp.Isbank3DDonus(pos);
    ````
