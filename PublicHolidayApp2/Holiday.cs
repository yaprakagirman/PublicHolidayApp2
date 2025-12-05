namespace PublicHolidayApp2
{
    // API'den gelen JSON verisini bu sınıf üzerinden temsil ediyorum.
    // Hocanın verdiği class tanımındaki alan isimlerini özellikle değiştirmedim.
    internal class Holiday
    {
        // Tatilin tarih bilgisi (örnek: 2023-01-01). JSON'da da string geldiği için string olarak tutuyorum.
        public string date { get; set; }

        // Tatilin yerel (Türkçe) adı. Örnek: "Yılbaşı".
        public string localName { get; set; }

        // Tatilin İngilizce adı. Örnek: "New Year's Day".
        public string name { get; set; }

        // Ülke kodu. Bu projede hep "TR" gelecek ama gene de modele ekliyorum.
        public string countryCode { get; set; }

        // C# dilinde fixed anahtar kelime olduğu için başına @ koyarak property adı olarak kullanıyorum.
        // Böylece JSON'daki "fixed" alanı ile bire bir aynı ismi koruyabiliyorum.
        public bool @fixed { get; set; }

        // Tatilin tüm dünyada mı yoksa sadece belirli ülkelerde mi geçerli olduğunu gösteren alan.
        public bool global { get; set; }
    }
}
