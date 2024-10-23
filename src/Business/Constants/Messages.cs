using Entities.DTos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string NotEmpty = "Bu alan boş olamaz.";
        public static string AirportsCannotBeTheSame = "Gidiş ve Dönüş havalimanları aynı olamaz.";
        public static string CanNotInThePast = "Geçmiş tarihle işlem yapılamaz.";
        public static string SearchCharacterLimit = "Arama yapmak için minumum uzunluk 4 karakter olmalı.";
        public static string DepartureDateCannotBeEmpty = "Gidiş tarihi boş olamaz.";
        public static string ReturnDateCannotBeEmpty = "Gidiş-Dönüş seçiminde Dönüş tarihi boş olamaz.";
        public static string ReturnDateMustBeGreaterThanDepartureDate = "Dönüş tarihi, Gidiş tarihinden küçük olamaz.";
    }
}
