using System;

namespace PublicHolidayApp2
{
    // Uygulamanın başlangıç noktası olarak klasik Program sınıfını kullanıyorum.
    // Main metodu çalıştığında, asıl işlemleri yapan PublicHolidayTracker sınıfını başlatıyorum.
    internal class Program
    {
        static void Main(string[] args)
        {
            // Uygulama açıldığında kullanıcıya kısa bir karşılama mesajı gösteriyorum.
            Console.WriteLine("PublicHolidayApp2 başlatılıyor...");
            Console.WriteLine();

            // Resmi tatil verilerini yönetecek sınıfımdan bir nesne oluşturuyorum.
            PublicHolidayTracker tracker = new PublicHolidayTracker();

            // Bütün iş akışını tek bir Run() metodu içinde topladım.
            tracker.Run();

            // Program kapanmadan önce ekrandaki çıktıları rahatça görebilmek için bekletiyorum.
            Console.WriteLine();
            Console.WriteLine("Program sona erdi. Çıkmak için bir tuşa basabilirsiniz.");
            Console.ReadKey();
        }
    }
}
