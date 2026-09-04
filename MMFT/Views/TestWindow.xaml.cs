using MMFT.DB.Models;
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
using System.Windows.Shapes;

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für TestWindow.xaml
    /// </summary>
    /// 
    public partial class TestWindow : Window
    {
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
        public TestWindow()
        {
            InitializeComponent();
            DataContext = this;

            using (var db = new MessengerDbContext())
            {
                var nutzer = db.Nutzers.First();
                nutzername = nutzer.Name;
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
    }
}
