using SquidEyes.Shared;
using System.Xml.Linq;

namespace SquidEyes.GUI
{
    public class ErrorAlert : IModelData<ErrorAlert>
    {
        public ErrorAlert Clone()
        {
            return new ErrorAlert();
        }

        public XElement ToElement()
        {
            return new XElement("errorAlert");
        }

        public bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}
