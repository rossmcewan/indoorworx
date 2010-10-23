using System;
using System.Net;
using System.ComponentModel;

namespace VideoPlayerTelemetry.Models
{
    /// <summary>
    /// Implements the <see cref="INotifyPropertyChanged"/>. 
    /// All the classes which require binding support are derived from this class.
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                try
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
                catch
                {
                }
            }
        }

      
    }
}
