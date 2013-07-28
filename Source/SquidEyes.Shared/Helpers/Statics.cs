//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Linq;

//namespace SquidEyes.Shared
//{
//    public static class Statics
//    {
//        static Statics()
//        {
//            LoadCountries();

//            AzureInfo = new AzureInfo();
//        }

//        private static void LoadCountries()
//        {
//            var doc = XDocument.Parse(Properties.Resources.Countries);

//            var q = from c in doc.Element("countries").Elements("country")
//                    select new Country
//                    {
//                        Code = c.Attribute("code").Value,
//                        Name = c.Attribute("name").Value
//                    };

//            Countries =
//                q.ToDictionary(country => country.Code, country => country);
//        }

//        public static Dictionary<string, Country> Countries;
//        public static AzureInfo AzureInfo { get; private set; }
//    }
//}
