using SquidEyes.Generic;
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace SquidEyes.GUI
{
    public class ModelBase<M> : MvvmBase, INotifyPropertyChanged
        where M: ModelBase<M>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged<R>(Expression<Func<M, R>> property)
        {
            Contract.Requires(property != null);

            BindingHelper.NotifyPropertyChanged(property, this, PropertyChanged, Dispatcher);
        }
    }
}
