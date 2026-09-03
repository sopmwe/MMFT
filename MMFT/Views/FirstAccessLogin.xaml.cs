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

    //Passwortanforderungen??
    //Bild in DB speichern
    //PW und Nutzernamen speichern in DB, PreparedStatements verwenden
    //
    public partial class FirstAccessLogin : Window, INotifyPropertyChanged
    {
        private const string defaultProfilbild = "Ressourcen/Profilbild.png";

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

        public string fehlermeldung;

        public FirstAccessLogin()
        {
            profilbild = defaultProfilbild;
            InitializeComponent();
            DataContext = this;
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
    }

}
