//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Xml.Linq;
//using SquidEyes.Generic;
//using System.Diagnostics.Contracts;

//namespace SquidEyes.Shared
//{
//    public static class MimeExtenders
//    {
//        private static Dictionary<string, string> mediaTypes = new Dictionary<string, string>();

//        static MimeExtenders()
//        {
//            var doc = XDocument.Parse(Properties.Resources.MediaTypes);

//            var q = from ct in doc.Element("mediaTypes").Elements("mediaType")
//                    select new
//                    {
//                        Extension = ct.Attribute("extension").Value,
//                        Type = ct.Attribute("type").Value,
//                    };

//            foreach (var contentType in q)
//                mediaTypes.Add(contentType.Extension, contentType.Type);
//        }

//        public static string ToMediaType(this string fileName)
//        {
//            Contract.Requires(fileName.IsTrimmed());

//            var extension = Path.GetExtension(fileName).Remove(0, 1);

//            string contentType;

//            if (mediaTypes.TryGetValue(extension, out contentType))
//                return contentType;

//            return "application/octet-stream";
//        }

//        public static bool IsMediaType(this string mediaType)
//        {
//            Contract.Requires(mediaType.IsTrimmed());

//            return mediaTypes.ContainsKey(mediaType);
//        }
//    }
//}
