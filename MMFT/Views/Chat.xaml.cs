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
    /// Interaktionslogik für Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        public Chat()
        {
            InitializeComponent();

            var items = new List<ListBoxItemModel>
                {
                     new ListBoxItemModel { ImagePath = "pack://application:,,,/Ressourcen/Standardpfp.png", Title = "Maren", TemplateTyp = ItemTyp.Standard },
                     new ListBoxItemModel { ImagePath = "pack://application:,,,/Ressourcen/Standardpfp.png", Title = "Milena", TemplateTyp = ItemTyp.MitZahl, Anzahl = 5 },
                     new ListBoxItemModel { ImagePath = "pack://application:,,,/Ressourcen/Standardpfp.png", Title = "Tim", TemplateTyp = ItemTyp.StandardStatus, Subtitle = "Ich benutze WhatsApp!" },
                     new ListBoxItemModel { ImagePath = "pack://application:,,,/Ressourcen/Standardpfp.png", Title = "Franz", TemplateTyp = ItemTyp.StatusMitZahl, Subtitle = "Schaut mal, was ich gemacht habe!", Anzahl = 3 }
                };

            Kontakte.ItemsSource = items;
        }

        

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
    }

    public enum ItemTyp
    {
        Standard,
        MitZahl,
        StandardStatus,
        StatusMitZahl

    }
    public class KontakttypSelector : DataTemplateSelector
    {
        public DataTemplate StandardTemplate { get; set; }
        public DataTemplate ZahlTemplate { get; set; }
        public DataTemplate StandardStatusTemplate { get; set; }

        public DataTemplate ZahlStatusTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ListBoxItemModel model)
            {
                if (model.TemplateTyp == ItemTyp.MitZahl)
                    return ZahlTemplate;
                else if (model.TemplateTyp == ItemTyp.Standard)
                    return StandardTemplate;
                else if (model.TemplateTyp == ItemTyp.StatusMitZahl)
                    return ZahlStatusTemplate;
                else
                    return StandardStatusTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }

    public class ListBoxItemModel
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public ItemTyp TemplateTyp { get; set; }
        public int? Anzahl { get; set; }
    }
}
