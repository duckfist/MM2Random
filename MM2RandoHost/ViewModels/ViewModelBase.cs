using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MM2RandoHost.ViewModels
{
    /// <summary> Provides an implementation for the <see cref="INotifyPropertyChanged"/> interface. </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary> Occurs when a property value cahnges. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyName">The property name. </param>
        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
