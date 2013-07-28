using System.Xml.Linq;

namespace SquidEyes.Shared
{
    public interface IModelData<T>
    {
        T Clone();
        XElement ToElement();

        bool IsValid { get; }
    }
}
