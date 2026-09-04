using MMFT.DB;
using MMFT.DB.Models;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
        /// Prüft, ob die DB-Datei existiert, redirected ggf. zur FirstAccessLogin-GUI.
        /// Nutzername wird aus der DB gelesen und in die Binding-Komponente geladen.
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
            { 
                using (var db = new MessengerDbContext())
                {
                    var nutzer = db.Nutzers.First();
                    nutzername = nutzer.Name;
                }
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
            else
            {
                // Private Key wird entschluesselt und in der Globalen Variable gespeichert
                string privateKey;
                using (var db = new MessengerDbContext())
                {
                    var pNutzer = db.PNutzers.FirstOrDefault();
                    byte[] verschluesselterPrivateKey = pNutzer.PrivateKey;
                    try
                    {
                         privateKey = AesHelfer.EntschluesselPrivateKey(verschluesselterPrivateKey, PasswortPB.Password);
                    }
                    // Abbruch falls aus dem eingebenen PW ein Key mit falschem Padding entsteht
                    catch (CryptographicException)
                    {
                        fehlermeldung = "Falsches Passwort";
                        return;
                    }
                    // überprüfung ob aus dem Key auch ein RSA schlüssel werden könnte (für den unwahrscheinlichen fall das das Padding zufällig gleich ist)
                    try
                    {
                        using RSA rsa = RSA.Create();
                        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                    }
                    catch
                    {
                        fehlermeldung = "Ungültiger Private Key";
                        return;
                    }
                    NutzerVerwalten.PrivateKeyEntschluesselt = privateKey;
                }

                Chat chat = new Chat();
                chat.Show();
                this.Close();
            }
        }
    }
}
