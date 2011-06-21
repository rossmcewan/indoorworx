using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IndoorWorx.Infrastructure.DragDrop;
using System.ComponentModel;

namespace IndoorWorx.Library.DragDrop
{
    public class DropTarget : IDropTarget, INotifyPropertyChanged
    {
        private Action<object> onDropped;
        private Func<object, bool> canDrop;
        private Func<int> getItemCount;

        public DropTarget(Action<object> onDropped, Func<object, bool> canDrop, Func<int> getItemCount)
        {
            this.onDropped = onDropped;
            this.canDrop = canDrop;
            this.getItemCount = getItemCount;
        }

        public Guid Id { get; set; }

        private Uri image;
        public Uri Image
        {
            get { return image; }
            set 
            { 
                image = value;
                FirePropertyChanged("Image");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set 
            { 
                title = value;
                FirePropertyChanged("Title");
            }        
        }


        public void OnDropped(object payload)
        {
            if (onDropped != null)
                onDropped(payload);
        }

        public bool CanDrop(object payload)
        {
            if (canDrop != null)
                return canDrop(payload);
            return true;
        }

        public int ItemCount
        {
            get
            {
                return getItemCount();
            }
            set
            {
                FirePropertyChanged("ItemCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
