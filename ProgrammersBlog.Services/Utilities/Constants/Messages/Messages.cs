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
            public static string UndoDeleted(string categoryName) => $"{categoryName} adlı kategori başarıyla arşivden geri getirilmiştir.";
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
            public static string UndoDeleted(string articleName) => $"{articleName} başlıklı makale başarıyla arşivden geri getirilmiştir.";
            public static string IncreaseViewCount(string articleName) => $"{articleName} başlıklı makalenin okunma sayısı başarıyla artırılmıştır.";
        }

        public static class Comment
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural) return "Hiç bir yorum bulunamadı.";
                return "Böyle bir yorum bulunamadı.";
            }
            public static string Approved(int commentId) => $"{commentId} no'lu yorum başarıyla onaylanmıştır.";

            public static string Add(string createdByName)
            {
                return $"Sayın {createdByName}, yorumunuz başarıyla eklenmiştir.";
            }

            public static string Update(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla güncellenmiştir.";
            }
            public static string Delete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla silinmiştir.";
            }
            public static string HardDelete(string createdByName)
            {
                return $"{createdByName} tarafından eklenen yorum başarıyla veritabanından silinmiştir.";
            }
            public static string UndoDeleted(string createdByName) => $"{createdByName} tarafından eklenen yorum başarıyla arşivden geri getirilmiştir.";

        }


    }
}
