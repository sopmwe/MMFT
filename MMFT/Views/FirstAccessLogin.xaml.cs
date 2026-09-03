using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für FirstAccessLogin.xaml
    /// </summary>
    /// 

    //Bild in DB speichern, umwandeln in BLOB?
    //Registrierung in Anwendung
    public partial class FirstAccessLogin : Window, INotifyPropertyChanged
    {
        private const string defaultProfilbild = "pack://application:,,,/Ressourcen/StandardProfilbild.png";

        private string _profilbild;

        public string profilbild
        {
            get => _profilbild;
            set
            {
                if (_profilbild != value)
                {
                    _profilbild = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _fehlermeldung;
        public string fehlermeldung
        {
            get => _fehlermeldung;
            set
            {
                if (_fehlermeldung != value)
                {
                    _fehlermeldung = value;
                    OnPropertyChanged();
                }
            }
        }

        public FirstAccessLogin()
        {
            InitializeComponent();

            DataContext = this;
            profilbild = defaultProfilbild;
        }

        private void btnEinfuegen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Bilddateien (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Alle Dateien (*.*)|*.*",
                Title = "Profilbild auswählen"
            };

            if (dialog.ShowDialog() == true)
            {
                profilbild = dialog.FileName;
            }
        }

        private void btnLoeschen_Click(object sender, RoutedEventArgs e)
        {

            profilbild = defaultProfilbild;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnFirstAccessLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NutzernameTB.Text))
            {
                fehlermeldung = "Bitte geben Sie einen Benutzernamen ein.";
            }
            else if (string.IsNullOrWhiteSpace(PasswortEinsPB.Password) || string.IsNullOrWhiteSpace(PasswortZweiPB.Password))
            {
                fehlermeldung = "Bitte geben Sie das Passwort zweimal ein.";
            }
            else if (PasswortEinsPB.Password != PasswortZweiPB.Password)
            {
                fehlermeldung = "Die Passwörter stimmen nicht überein.";
            }
            else
            {
                //Nutzername und Passwort als Variable speichern
                //string nutzername = NutzernameTB.Text;
                //string passwort = PasswortEinsPB.Password;
                Chat chat = new Chat();
                chat.Show();
                this.Close();
            }
        }

    }
}
