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
        private Action<IDropTarget,object> onDropped;
        private Func<IDropTarget,object, bool> canDrop;
        private Func<IDropTarget,int> getItemCount;

        public DropTarget(Action<IDropTarget, object> onDropped, Func<IDropTarget, object, bool> canDrop, Func<IDropTarget, int> getItemCount)
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
                onDropped(this, payload);
        }

        public bool CanDrop(object payload)
        {
            if (canDrop != null)
                return canDrop(this, payload);
            return true;
        }

        public int ItemCount
        {
            get
            {
                return getItemCount(this);
            }
            set
            {
                FirePropertyChanged("ItemCount");
            }
        }

        private bool busy;
        public bool IsBusy
        {
            get { return this.busy; }
            set
            {
                this.busy = value;
                FirePropertyChanged("IsBusy");
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
