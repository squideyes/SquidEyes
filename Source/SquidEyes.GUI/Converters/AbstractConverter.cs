using System;
using System.Windows.Markup;

namespace SquidEyes.GUI
{
    public abstract class AbstractConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
