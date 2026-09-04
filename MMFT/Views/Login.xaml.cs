using MMFT.DB;
using MMFT.DB.Models;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    /// Diese Klasse repräsentiert das Login-Fenster der Anwendung. 
    /// Sie implementiert das INotifyPropertyChanged-Interface, um Änderungen an den Eigenschaften zu überwachen und die Benutzeroberfläche entsprechend zu aktualisieren.
    /// Sie beinhaltet die Binding-Komponenteneigenschaften für den Benutzernamen und die Fehlermeldung, sowie die Logik für den Login-Vorgang.
    public partial class Login : Window, INotifyPropertyChanged
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MessengerDB.db");
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

        /// <summary>
        /// Initialisiert eine neue Instanz der Login-Klasse
        /// </summary>
        /// Prüft, ob die Zugangsdaten-Datei existiert und ob sie leer ist, redirected ggf. zur FirstAccessLogin-GUI.
        /// Nutzername wird aus der Datei gelesen und in die Binding-Komponente geladen.
        public Login()
        {
            InitializeComponent();
            DataContext = this;

            if (!File.Exists(path))
            {
                FirstAccessLogin firstAccessLogin = new FirstAccessLogin();
                firstAccessLogin.Show();
                this.Close();
            }
            else
            { //hier die Verbindung zur Datenbank herstellen und den Nutzernamen auslesen
                nutzername = "Platzhalter";
            }
        }

        /// <summary>
        /// Ereignis, das ausgelöst wird, wenn sich eine Eigenschaft ändert.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Methode, die aufgerufen wird, wenn sich eine Eigenschaft ändert, um das PropertyChanged-Ereignis auszulösen.        
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Ereignishandler für den Klick auf die Login-Schaltfläche.
        /// </summary>
        /// Überprüft das eingegebene Passwort und öffnet bei erfolgreicher Authentifizierung das Chat-Fenster.
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswortPB.Password))
            {
                fehlermeldung = "Bitte geben Sie ein Passwort ein.";
                return;
            }
            else if (PasswortPB.Password != File.ReadAllLines(path).ElementAt(1))
            {
                fehlermeldung = "Falsches Passwort.";
                return;
            }
            else
            {
                // Private Key wird entschluesselt und in der Globalen Variable gespeichert
                using var db = new MessengerDbContext();
                var pNutzer = db.PNutzers.FirstOrDefault();
                byte[] verschluesselterPrivateKey = pNutzer.PrivateKey;
                string privateKey = AesHelfer.EntschluesselPrivateKey(verschluesselterPrivateKey, PasswortPB.Password);
                NutzerVerwalten.PrivateKeyEntschluesselt = privateKey;

                Chat chat = new Chat();
                chat.Show();
                this.Close();
            }
        }
    }
}
