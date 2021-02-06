using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MM2Randomizer
{
    /// <summary> Provides an implementation for the <see cref="INotifyPropertyChanged"/> interface. </summary>
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        /// <summary> Occurs when a property value cahnges. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Raises the property changed event. </summary>
        /// <param name="propertyName">The property name. </param>
        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Set the value of a property, firing NotifyPropertyChanged if the value is new. This is used
        /// for a cleaner implementation of ViewModel properties, so they have fewer lines of code.
        /// </summary>
        /// <typeparam name="T">The type of the property being changed.</typeparam>
        /// <param name="privateField">The property's backing field.</param>
        /// <param name="newValue">The property's new value to set.</param>
        /// <param name="propertyName">The name of the property (leave blank to use the caller's name)</param>
        /// <returns>True if the property has changed, false if it is still the same value.</returns>
        protected bool SetProperty<T>(
            ref T privateField,
            T newValue,
            [CallerMemberName]string propertyName = null,
            params string[] additionalProperties)
        {
            // If value hasn't changed, just return false
            if (EqualityComparer<T>.Default.Equals(privateField, newValue))
            {
                return false;
            }

            // Value has changed, invoke propertychanged and return true
            privateField = newValue;
            NotifyPropertyChanged(propertyName);

            // Notify any additional properties that might depend on this one
            foreach (string additionalProperty in additionalProperties)
            {
                NotifyPropertyChanged(additionalProperty);
            }

            return true;
        }
    }
}
