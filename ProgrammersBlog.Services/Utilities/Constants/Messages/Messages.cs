using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Utilities.Constants.Messages
{
    public static class Messages
    {
        public static class Category
        {
            public static string NotFoundAny = "Hiç bir kategori bulunamadı.";
            public static string NotFound = "Böyle bir kategori bulunamadı.";
            public static string Added(string categoryName) => $"{categoryName} adlı kategori başarıyla eklenmiştir.";
            public static string Updated(string categoryName) => $"{categoryName} adlı kategori başarıyla güncellenmiştir.";
            public static string Deleted(string categoryName) => $"{categoryName} adlı kategori başarıyla silinmiştir.";
            public static string HardDeleted(string categoryName) => $"{categoryName} adlı kategori başarıyla veritabanında silinmiştir.";
        }

        public static class Article
        {
            public static string NotFoundAny = "Makaleler bulunamadı";
            public static string NotFound = "Böyle bir makale bulunamadı";
            public static string NotFoundCategory = "Böyle bir kategori bulunamadı";
            public static string Added(string articleName) => $"{articleName} başlıklı makale başarıyla eklenmiştir.";
            public static string Updated(string articleName) => $"{articleName} başlıklı makale başarıyla güncellenmiştir.";
            public static string Deleted(string articleName) => $"{articleName} başlıklı makale başarıyla silinmiştir.";
            public static string HardDeleted(string articleName) => $"{articleName} başlıklı makale başarıyla veritabanında silinmiştir.";
        }

         
    }
}
