# PublicHolidayTracker

Bu proje, Türkiye’deki resmi tatil bilgilerini API üzerinden çekip kullanıcıya konsol arayüzü ile sunan bir C# Console uygulamasıdır. Uygulama, seçilen yıllara ait tatilleri JSON formatında alır, modele dönüştürür ve bellekte saklayarak kullanıcıya yıl, tarih veya isim bazlı arama yapma imkânı sağlar.

Amaç; API kullanımı, JSON işleme, listeleme/arama mantığı ve konsol uygulaması yapısını pratik bir şekilde uygulamaktır. Kod yapısı derslerde gösterilen yöntemlere uygun şekilde sade tutulmuştur.


## Kullanılan API’ler

Uygulama aşağıdaki uç noktalardan veri çeker:

* 2023 Resmi Tatilleri
* 2024 Resmi Tatilleri
* 2025 Resmi Tatilleri

Her yıl için API çağrısı ayrı yapılır ve alınan veriler bellekte üç farklı listede tutulur.


## Projenin Yapısı

### *Program.cs*

Uygulamanın başlangıç noktasıdır. PublicHolidayTracker sınıfını çalıştırır.

### *PublicHolidayTracker.cs*

Projenin ana akışını yöneten sınıftır.
Bu sınıf:

* API’den yıllık tatil verilerini çeker,
* JSON verisini modele dönüştürür,
* Tatilleri bellekte saklar,
* Konsol menüsünü oluşturur,
* Tarih veya isim bazlı arama işlemlerini yönetir,
* Tüm yılların tatillerini sıralı şekilde ekrana yazdırır.

Tüm iş mantığı bu sınıf içerisinde organize edilmiştir.

### *Holiday.cs*

Tatil nesnesinin tanımlandığı model sınıfıdır.
API’den gelen JSON verileri bu sınıfa karşılık düşer.


## Uygulamanın Çalışma Mantığı

Uygulama başlatıldığında:

1. 2023, 2024 ve 2025 yıllarına ait tatil listeleri API’den alınır.
2. Alınan veriler belleğe kaydedilir.
3. Kullanıcıya konsol üzerinden bir menü sunulur.

Menüde yer alan işlemler:

* Seçilen yıla ait tatilleri listeleme
* Belirli bir tarihte tatil olup olmadığını kontrol etme
* İsim ile tatil arama
* Üç yılın tüm tatillerini toplu gösterme
* Çıkış

Menü yapısı sadedir ve kullanıcıdan gelecek girişlere uygun şekilde yanıt verir.


## Örnek Uygulama Akışı

* Program açılır, veri çekme işlemi başlar.
* “2023 yılı tatilleri yükleniyor…” gibi mesajlar görünür.
* Ardından kullanıcı karşısına menü çıkar.
* Kullanıcı seçimini yaparak arama veya listeleme işlemlerine geçebilir.

Uygulama, konsol üzerinden sade ve anlaşılır şekilde ilerler.


## Kullanılan Teknolojiler

* *C#*
* *.NET Console Application*
* *HttpClient* (API’den veri almak için)
* *System.Text.Json* (JSON verisini modele dönüştürmek için)


## Notlar

* Kodun genel yapısı, derste işlenen örneklere uygun olarak basit, anlaşılır ve düzenli şekilde oluşturulmuştur.
* Her metodun görevini açıklayan yorum satırları eklenmiştir.
* Veri işlemleri senkron şekilde yapılmıştır.
* Proje, ders kapsamında temel API kullanımı ve modelleme pratiği kazanmak amacıyla hazırlanmıştır.


##  Geliştirici

*Yaprak Ağırman*
Bilgisayar Programcılığı Öğrencisi
