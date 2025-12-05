using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace PublicHolidayApp2
{
    // Bu sınıf, resmi tatil verilerini API üzerinden alıp hafızada saklıyor
    // ve kullanıcının konsol menüsü ile bu veriler üzerinde arama yapmasını sağlıyor.
    internal class PublicHolidayTracker
    {
        // Her yıl için tatil listesini ayrı ayrı tutuyorum.
        // Böylece kodu okurken "hangi yılın verisi nerede" sorusu daha net oluyor.
        private List<Holiday> _holidays2023 = new List<Holiday>();
        private List<Holiday> _holidays2024 = new List<Holiday>();
        private List<Holiday> _holidays2025 = new List<Holiday>();

        // Uygulamanın temel akışını başlatan metot.
        public void Run()
        {
            Console.WriteLine("Resmi tatil verileri API'den çekiliyor...");
            Console.WriteLine();

            // Uygulama açıldığında üç yılın verisini de belleğe alıyorum.
            LoadAllHolidays();

            // Veriler yüklendikten sonra kullanıcıya menü göstermeye başlıyorum.
            ShowMenuLoop();
        }

        // 2023, 2024 ve 2025 yıllarının verilerini art arda yükleyen metot.
        private void LoadAllHolidays()
        {
            _holidays2023 = LoadHolidaysForYear(2023);
            _holidays2024 = LoadHolidaysForYear(2024);
            _holidays2025 = LoadHolidaysForYear(2025);
        }

        // Verilen yıl için API'den JSON verisini çekip List<Holiday> olarak geriye döndürüyorum.
        private List<Holiday> LoadHolidaysForYear(int year)
        {
            // Hocanın verdiği API adresini yıl parametresi ile birleştiriyorum.
            string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/TR";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // HttpClient ile GET isteği atıyorum.
                    // async/await kullanmak yerine Result ile bekletiyorum; derste gösterilen tarza daha yakın.
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"{year} yılı için veriler alınırken hata oluştu. StatusCode: {response.StatusCode}");
                        return new List<Holiday>();
                    }

                    // Gelen JSON içeriğini string olarak okuyorum.
                    string json = response.Content.ReadAsStringAsync().Result;

                    // Property adlarındaki büyük/küçük harf farkını önemsememek için case-insensitive ayarı veriyorum.
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // JSON'u List<Holiday> tipine çeviriyorum.
                    List<Holiday>? holidays = JsonSerializer.Deserialize<List<Holiday>>(json, options);

                    // Her ihtimale karşı null gelirse boş listeye çeviriyorum.
                    holidays ??= new List<Holiday>();

                    Console.WriteLine($"{year} yılı için {holidays.Count} adet resmi tatil yüklendi.");

                    return holidays;
                }
                catch (Exception ex)
                {
                    // Beklenmedik bir hata durumunda kullanıcıya kısa bir bilgi verip boş liste dönüyorum.
                    Console.WriteLine($"{year} yılı verileri alınırken istisna oluştu: {ex.Message}");
                    return new List<Holiday>();
                }
            }
        }

        // Kullanıcıya menü gösterip seçimini alan döngü.
        private void ShowMenuLoop()
        {
            bool devam = true;

            while (devam)
            {
                Console.WriteLine();
                Console.WriteLine("===== PublicHolidayTracker =====");
                Console.WriteLine("1. Tatil listesini göster (yıl seçmeli)");
                Console.WriteLine("2. Tarihe göre tatil ara (gg-aa formatı)");
                Console.WriteLine("3. İsme göre tatil ara");
                Console.WriteLine("4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminiz: ");

                string? secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        ShowHolidayListByYear();
                        break;

                    case "2":
                        SearchByDate();
                        break;

                    case "3":
                        SearchByName();
                        break;

                    case "4":
                        ShowAllHolidays();
                        break;

                    case "5":
                        // Kullanıcı 5 seçerse döngüden çıkıyorum ve program sonlanıyor.
                        devam = false;
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim yaptınız. Lütfen 1–5 arasında bir değer girin.");
                        break;
                }
            }
        }

        // 1. seçenek: Kullanıcının seçtiği yılın tatil listesini ekrana yazdırıyorum.
        private void ShowHolidayListByYear()
        {
            Console.Write("Listelemek istediğiniz yılı girin (2023, 2024, 2025): ");
            string? yilStr = Console.ReadLine();

            if (!int.TryParse(yilStr, out int year))
            {
                Console.WriteLine("Geçerli bir yıl değeri girmediniz.");
                return;
            }

            List<Holiday>? list;

            if (year == 2023)
            {
                list = _holidays2023;
            }
            else if (year == 2024)
            {
                list = _holidays2024;
            }
            else if (year == 2025)
            {
                list = _holidays2025;
            }
            else
            {
                Console.WriteLine("Bu proje sadece 2023, 2024 ve 2025 yıllarını destekliyor.");
                return;
            }

            if (list.Count == 0)
            {
                Console.WriteLine("Bu yıl için yüklü tatil bulunmuyor.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"*** {year} yılı resmi tatilleri ***");

            foreach (Holiday holiday in list)
            {
                Console.WriteLine($"{holiday.date} - {holiday.localName} ({holiday.name})");
            }
        }

        // 2. seçenek: Tarihe göre (gg-aa) tatil arıyorum.
        private void SearchByDate()
        {
            Console.Write("Aramak istediğiniz tarih (gg-aa formatı, örnek: 01-01): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Tarih alanı boş olamaz.");
                return;
            }

            bool bulundu = false;

            // 2023 listesinde arıyorum.
            SearchByDateInList(_holidays2023, 2023, input, ref bulundu);
            // 2024 listesinde arıyorum.
            SearchByDateInList(_holidays2024, 2024, input, ref bulundu);
            // 2025 listesinde arıyorum.
            SearchByDateInList(_holidays2025, 2025, input, ref bulundu);

            if (!bulundu)
            {
                Console.WriteLine("Bu tarihe denk gelen resmi tatil bulunamadı.");
            }
        }

        // Tarihe göre aramada tekrarı azaltmak için küçük bir yardımcı metot kullandım.
        private void SearchByDateInList(List<Holiday> list, int year, string input, ref bool bulundu)
        {
            foreach (Holiday holiday in list)
            {
                // API'den gelen tarih "2023-01-01" formatında.
                // Kullanıcı sadece "01-01" girdiği için string'in sonuna bakıyorum.
                if (!string.IsNullOrEmpty(holiday.date) && holiday.date.EndsWith(input))
                {
                    Console.WriteLine($"{year} - {holiday.date} - {holiday.localName} ({holiday.name})");
                    bulundu = true;
                }
            }
        }

        // 3. seçenek: İsme göre tatil arama.
        private void SearchByName()
        {
            Console.Write("Aramak istediğiniz tatil adını girin (yerel veya İngilizce): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("İsim alanı boş olamaz.");
                return;
            }

            string aranacak = input.ToLower();
            bool bulundu = false;

            // Üç yıl için de aynı arama mantığını uyguluyorum.
            SearchByNameInList(_holidays2023, 2023, aranacak, ref bulundu);
            SearchByNameInList(_holidays2024, 2024, aranacak, ref bulundu);
            SearchByNameInList(_holidays2025, 2025, aranacak, ref bulundu);

            if (!bulundu)
            {
                Console.WriteLine("Bu ismi içeren bir resmi tatil bulunamadı.");
            }
        }

        // İsme göre aramada kullanılan yardımcı metot.
        private void SearchByNameInList(List<Holiday> list, int year, string aranacak, ref bool bulundu)
        {
            foreach (Holiday holiday in list)
            {
                string local = holiday.localName?.ToLower() ?? string.Empty;
                string english = holiday.name?.ToLower() ?? string.Empty;

                // Kullanıcının girdiği ifade hem localName hem de name içinde geçebilir.
                if (local.Contains(aranacak) || english.Contains(aranacak))
                {
                    Console.WriteLine($"{year} - {holiday.date} - {holiday.localName} ({holiday.name})");
                    bulundu = true;
                }
            }
        }

        // 4. seçenek: 3 yılın tüm tatillerini tek seferde listeliyorum.
        private void ShowAllHolidays()
        {
            Console.WriteLine("=== 2023 – 2025 ARASINDAKİ TÜM RESMİ TATİLLER ===");
            Console.WriteLine();

            Console.WriteLine("--- 2023 ---");
            PrintYearHolidays(_holidays2023);

            Console.WriteLine();
            Console.WriteLine("--- 2024 ---");
            PrintYearHolidays(_holidays2024);

            Console.WriteLine();
            Console.WriteLine("--- 2025 ---");
            PrintYearHolidays(_holidays2025);
        }

        // Bir yılın tüm tatillerini ekrana basan küçük yardımcı metot.
        private void PrintYearHolidays(List<Holiday> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("Bu yıl için kayıtlı tatil yok.");
                return;
            }

            foreach (Holiday holiday in list)
            {
                Console.WriteLine($"{holiday.date} - {holiday.localName} ({holiday.name})");
            }
        }
    }
}
