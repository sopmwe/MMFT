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

namespace MMFT.Views
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string _Nutzername;
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
    }
}
