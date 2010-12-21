using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections;

namespace IndoorWorx.Library.Controls
{
    public class PagerViewModel : INotifyPropertyChanged
    {
        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return this._selectedIndex;
            }
            set
            {
                if (this._selectedIndex != value)
                {
                    this._selectedIndex = value;
                    this.OnPropertyChanged("SelectedIndex");
                }
            }
        }

        public void SelectNext()
        {
            if (this.SelectedIndex == this.Count)
            {
                this.SelectedIndex = 0;
            }
            else
            {
                this.SelectedIndex++;
            }
        }

        public void SelectPrev()
        {
            if (this.SelectedIndex == 0)
            {
                this.SelectedIndex = this.Count;
            }
            else
            {
                this.SelectedIndex--;
            }
        }

        private int _count;
        public int Count
        {
            get
            {
                return this._count;
            }
            private set
            {
                this._count = value;
            }
        }

        private IEnumerable _pages;
        public IEnumerable Pages
        {
            get
            {
                return this._pages;
            }
            set
            {
                if (this._pages != value)
                {
                    this._pages = value;
                    this.Count = this.Pages.Cast<object>().Count() - 1;
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, args);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
