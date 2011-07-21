using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace IndoorWorx.Infrastructure.Models
{
    [DataContract(IsReference = true)]
    public abstract partial class BaseModel : INotifyPropertyChanged
    {
        private Guid id;
        [DataMember]
        public virtual Guid Id
        {
            get { return id; }
            set
            {
                id = value;
                FirePropertyChanged("Id");
            }
        }
       
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void FirePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                try
                {
                    SmartDispatcher.BeginInvoke(() => handler(this, new PropertyChangedEventArgs(propertyName)));
                }
                catch
                {
                }
            }
        }

        public override bool Equals(object obj)
        {
            var model = obj as BaseModel;
            if (model != null)
            {
                if (model.Id != Guid.Empty && this.Id != Guid.Empty)
                    return model.Id == this.Id;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
