using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Collections.Generic;
using PropertyDictionary = System.Collections.Generic.Dictionary<
    string, System.ComponentModel.PropertyChangedEventHandler>;

namespace SquidEyes.GUI
{
    public abstract class ViewModelBase<VM, M> : MvvmBase, INotifyPropertyChanged
        where VM : ViewModelBase<VM, M>
        where M : ModelBase<M>
    {
        private Dictionary<string, PropertyDictionary> propertyHandlers
            = new Dictionary<string, PropertyDictionary>(); 
        
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase(M model)
        {
            Contract.Requires(model != null);

            Model = model;
        }

        public M Model { get; private set; }

        protected virtual void AssociateProperties<MR, VMR>(
            Expression<Func<M, MR>> modelProperty, Expression<Func<VM, VMR>> viewModelProperty)
        {
            var modelPropertyName = ((MemberExpression)modelProperty.Body).Member.Name;
            var viewModelPropertyName = ((MemberExpression)viewModelProperty.Body).Member.Name;

            if (!propertyHandlers.ContainsKey(modelPropertyName))
                propertyHandlers.Add(modelPropertyName, new PropertyDictionary());

            var handlers = propertyHandlers[modelPropertyName];

            PropertyChangedEventHandler handler = (s, ea) =>
            {
                if (ea.PropertyName == modelPropertyName)
                    NotifyPropertyChanged(viewModelPropertyName, this, PropertyChanged);
            };

            Model.PropertyChanged += handler;

            handlers.Add(viewModelPropertyName, handler);
        }

        private void NotifyPropertyChanged(string propertyName,
            object sender, PropertyChangedEventHandler propertyChanged)
        {
            if (propertyChanged != null)
            {
                if (Dispatcher.CheckAccess())
                {
                    propertyChanged(sender, new PropertyChangedEventArgs(propertyName));

                }
                else
                {
                    Action action = () => propertyChanged
                        (sender, new PropertyChangedEventArgs(propertyName));

                    Dispatcher.BeginInvoke(action);
                }
            }
        }

        protected virtual void NotifyPropertyChanged<R>(Expression<Func<VM, R>> property)
        {
            Contract.Requires(property != null);

            BindingHelper.NotifyPropertyChanged(property, this, PropertyChanged, Dispatcher);
        }
    }
}
