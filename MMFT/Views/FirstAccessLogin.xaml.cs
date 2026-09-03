using Microsoft.Win32;
using MMFT.DB;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für FirstAccessLogin.xaml
    /// </summary>
    /// Diese Klasse repräsentiert das FirstAccessLogin-Fenster der Anwendung.
    /// Wird nur geöffnet, wenn die Zugangsdaten-Datei nicht existiert oder leer ist.

    //Bild in DB speichern, umwandeln in BLOB?
    //Daten generell in DB speichern
    public partial class FirstAccessLogin : Window, INotifyPropertyChanged
    {
        private readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ressourcen", "Zugang.txt");

        private const string defaultProfilbild = "pack://application:,,,/Ressourcen/Standardpfp.png";

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

        /// <summary>
        /// Initialisiert eine neue Instanz der FirstAccessLogin-Klasse.
        /// </summary>
        /// Setzt Default-Profilbild und DataContext für die Bindung.
        public FirstAccessLogin()
        {
            InitializeComponent();

            DataContext = this;
            profilbild = defaultProfilbild;
        }

        /// <summary>
        /// Öffnet einen Datei-Dialog, um ein Profilbild auszuwählen und setzt das ausgewählte Bild als Profilbild.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Setzt das Profilbild auf das Standard-Profilbild zurück.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoeschen_Click(object sender, RoutedEventArgs e)
        {

            profilbild = defaultProfilbild;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Buttonlogik zum Registrieren.
        /// </summary>
        /// Eingaben werden überprüft und bei Erfolg in eine Datei geschrieben. 
        /// Bei Fehler wird eine Fehlermeldung angezeigt.
        /// Anschließend wird das Chat-Fenster geöffnet.
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                string[] eingabe = new string[]
                {
                    NutzernameTB.Text,
                    PasswortEinsPB.Password  //später nur Nutzername speichern wegen Sicherheit 
                };
                File.WriteAllLines(path, eingabe);
                var verwalter = new NutzerVerwalten();
                verwalter.NutzerAnlegen(eingabe.First());
                // Nutzername, Passwort und PFB in DB laden??
                Chat chat = new Chat();
                chat.Show();
                this.Close();
            }
        }
    }
}
