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
using MMFT.Views;

//TODOs: Passwort soll wärend Eingabe nicht angezeigt werden, sondern als Sternchen oder Punkte dargestellt werden.
//TODOs: tatsächlichen Nutzernamen anzeigen
//TODOs: Logo hinzufügen
//TODOs: Login-Button hinzufügen
//TODOs: Passwort-Button hinzufügen
//TODOs: DB-Prüfung einbauen, ob DB existiert
//TODOs: Fehleranzeige eher als Label ohne Inhalt einfügen? stattdessen Binding je nach Inhalt?

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string _Nutzername;
        public string Fehlermeldung;
        public string Nutzername
        {
            get { return _Nutzername; }
            set { _Nutzername = value; }
        }
        public Login()
        { 

            //Nutzername = Environment.UserName;
            //if (string.IsNullOrEmpty(Nutzername))
            //{
                Nutzername = "Test";
            //}

            InitializeComponent();

            DataContext = this;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Login IF-Statement
            Chat chat = new Chat();
            chat.Show();
            this.Close();
        }
    }
}
