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
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Enums;

namespace IndoorWorx.Settings.Views
{
    public class GeneralSettingsPresentationModel : BaseModel, IGeneralSettingsPresentationModel
    {
        public GeneralSettingsPresentationModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.FirstName = ApplicationUser.CurrentUser.Firstname;
            this.Surname = ApplicationUser.CurrentUser.Lastname;
            this.About = ApplicationUser.CurrentUser.About;
            this.Gender = ApplicationUser.CurrentUser.Gender;
            this.FTP = ApplicationUser.CurrentUser.FTP.GetValueOrDefault();
            this.FTHR = ApplicationUser.CurrentUser.FTHR.GetValueOrDefault();
        }

        public IGeneralSettingsView View { get;set; }

        private string firstName;
        public virtual string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                FirePropertyChanged("FirstName");
            }
        }

        private string surname;
        public virtual string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                FirePropertyChanged("Surname");
            }
        }

        private Genders gender;
        public virtual Genders Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                FirePropertyChanged("Gender");
            }
        }

        private string about;
        public virtual string About
        {
            get { return about; }
            set
            {
                about = value;
                FirePropertyChanged("About");
            }
        }

        private int ftp;
        public virtual int FTP
        {
            get { return ftp; }
            set
            {
                ftp = value;
                FirePropertyChanged("FTP");
            }
        }

        private int fthr;
        public virtual int FTHR
        {
            get { return fthr; }
            set
            {
                fthr = value;
                FirePropertyChanged("FTHR");
            }
        }
    }
}
