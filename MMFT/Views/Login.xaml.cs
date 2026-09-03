using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MMFT.DB;
using MMFT.DB.Models;
using MMFT.Views;
using System.IO;
using System.Linq;

//TODOs: tatsächlichen Nutzernamen anzeigen
//TODOs: Logo hinzufügen
//TODOs: Login-Button hinzufügen
//TODOs: DB-Prüfung einbauen, ob DB existiert
//TODOs: Fehleranzeige eher als Label ohne Inhalt einfügen? stattdessen Binding je nach Inhalt?

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string _nutzername;
        public string fehlermeldung;
        public string nutzername
        {
            get { return _nutzername; }
            set { _nutzername = value; }
        }
        public Login()
        {
            nutzername = File.ReadLines("Zugang.txt").First();
            if (string.IsNullOrEmpty(nutzername))
            {
                FirstAccessLogin firstAccessLogin = new FirstAccessLogin();
                firstAccessLogin.Show();
            }
            else
            {
                InitializeComponent();
                var verwalter = new NutzerVerwalten();
                verwalter.NutzerAnlegen(nutzername);

                DataContext = this;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswortPB.Password))
            {
                fehlermeldung = "Bitte geben Sie ein Passwort ein.";
                return;
            }
            else if (PasswortPB.Password != File.ReadLines("Zugang.txt").ElementAt(1))
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
