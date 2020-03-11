# PaymentServicesLibrary
Bankaların ve ödeme operatörlerinin sanal pos entegrasyonlarını barındıran bir sınıf kütüphanesi. 
   
<br>
<h1>Kullanım</h1>

<hr />

<img src="https://112paykasa.com/paykasa_upload/2018/09/Akbank-Logo-PNG.png" height="50" />

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
    
<hr />

<hr />

<img src="https://github.com/tunahanertekin/PaymentServicesLibrary/blob/master/PaymentServicesLibrary/Images/pngs/qnbfinansbank.png?raw=true" height="50" />

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
<hr />

<hr />

<img src="https://github.com/tunahanertekin/PaymentServicesLibrary/blob/master/PaymentServicesLibrary/Images/pngs/garantibankasi.png?raw=true" height="50" />

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

<hr />

<hr />

<img src="https://github.com/tunahanertekin/PaymentServicesLibrary/blob/master/PaymentServicesLibrary/Images/pngs/isbankasi.png?raw=true" height="50" />

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

<hr />

<hr />

<img src="https://github.com/tunahanertekin/PaymentServicesLibrary/blob/master/PaymentServicesLibrary/Images/pngs/moka.png?raw=true" height="50" />

  - <h3>Gidiş</h3>

    ````csharp
            Pos pos = new Pos(0, "DealerCode|Username|Password", "DonusSayfasi.aspx");
            KKart kkart = new KKart("4444333322221111", "12", "20", "123");
            Kullanici kullanici = new Kullanici("ornekinsan@gmail.com", "Ornek Insan");
            PosIslem posIslem = new PosIslem("1.25", "0", "aciklama", 0);

            MokaSanalPos msp = new MokaSanalPos();
            String response = msp.PosMoka3D(pos, kkart, kullanici, posIslem);
    ````

  - <h3>Dönüş</h3>

    ````csharp
            MokaSanalPos msp = new MokaSanalPos();
            Pos pos = new Pos(0, "DealerCode|Username|Password", "DonusSayfasi.aspx");
            PosYanit posYanit = msp.PosMokaDonus(pos);
    ````

<hr />

<hr />

<img src="https://github.com/tunahanertekin/PaymentServicesLibrary/blob/master/PaymentServicesLibrary/Images/pngs/payu.png?raw=true" height="50" />


  - <h3>Gidiş</h3>

    ````csharp
            Pos pos = new Pos(0, "Merchant$SecretKey", "DonusSayfasi.aspx");
            KKart kkart = new KKart("4444333322221111", "12", "20", "123");
            Kullanici kullanici = new Kullanici("ornekinsan@gmail.com", "Ornek Insan");
            PosIslem posIslem = new PosIslem("1.25", "0", "aciklama", 0);

            PayUSanalPos msp = new PayUSanalPos();
            String response = msp.PosPayU(pos, kkart, kullanici, posIslem);
    ````

  - <h3>Dönüş</h3>

    ````csharp
            PayUSanalPos msp = new PayUSanalPos();
            Pos pos = new Pos(0, "Merchant$SecretKey", "DonusSayfasi.aspx");
            PosYanit posYanit = msp.PosPayUDonus(pos);
    ````

<hr />

