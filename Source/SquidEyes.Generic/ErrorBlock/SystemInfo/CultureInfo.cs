using System.Xml.Linq;
using SG = System.Globalization;

namespace SquidEyes.Generic
{
    public class CultureInfo
    {
        internal CultureInfo()
        {
            Current = GetName(SG.CultureInfo.CurrentCulture);
            CurrentUI = GetName(SG.CultureInfo.CurrentUICulture);
        }

        internal CultureInfo(XElement software)
        {
            var culture = software.Element("culture");

            Current = (string)culture.Element("current");
            CurrentUI = (string)culture.Element("currentUI");
        }

        private string GetName(SG.CultureInfo cultureInfo)
        {
            return cultureInfo.Name + ": " + cultureInfo.EnglishName;
        }

        public string Current { get; private set; }
        public string CurrentUI { get; private set; }

        internal XElement GetElement()
        {
            return new XElement("culture",
                new XElement("current", Current),
                new XElement("currentUI", CurrentUI));
        }
    }
}