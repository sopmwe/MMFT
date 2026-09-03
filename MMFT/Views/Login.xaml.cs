using MMFT.DB;
using MMFT.DB.Models;
using MMFT.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

//TODOs: tatsächlichen Nutzernamen anzeigen
//TODOs: Login-Button hinzufügen

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window, INotifyPropertyChanged
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ressourcen", "Zugang.txt");
        private string _nutzername;
        public string nutzername
        {
            get { return _nutzername; }
            set
            {
                _nutzername = value;
                OnPropertyChanged();
            }
        }
        private string _fehlermeldung;

        public string fehlermeldung
        {
            get { return _fehlermeldung; }
            set
            {
                _fehlermeldung = value;
                OnPropertyChanged();
            }
        }

        public Login()
        {
            InitializeComponent();
            DataContext = this;

            if (!File.Exists(path) || new FileInfo(path).Length == 0)
            {
                FirstAccessLogin firstAccessLogin = new FirstAccessLogin();
                firstAccessLogin.Show();
                this.Close();
            }
            else
            {
                var lines = File.ReadAllLines(path);
                if (lines.Length == 0)
                {
                    FirstAccessLogin firstAccessLogin = new FirstAccessLogin();
                    firstAccessLogin.Show();
                    this.Close();
                    return;
                }
                var nutzername = lines.First();
                var verwalter = new NutzerVerwalten();
                verwalter.NutzerAnlegen(nutzername);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswortPB.Password))
            {
                fehlermeldung = "Bitte geben Sie ein Passwort ein.";
                return;
            }
            else if (PasswortPB.Password != File.ReadLines(path).ElementAt(1))
            {
                fehlermeldung = "Falsches Passwort.";
                return;
            }
            else
            {
                Chat chat = new Chat();
                chat.Show();
                this.Close();
            }
        }
    }
}
