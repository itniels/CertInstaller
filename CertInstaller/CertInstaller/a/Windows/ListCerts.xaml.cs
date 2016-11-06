using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CertInstaller.a.Model;

namespace CertInstaller.a.Windows
{
    /// <summary>
    /// Interaction logic for ListCerts.xaml
    /// </summary>
    public partial class ListCerts : Window
    {
        private ObservableCollection<Certificate> certs = new ObservableCollection<Certificate>();

        public ListCerts()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listView.DataContext = certs;
            listView.ItemsSource = certs;
        }

        private void cm_Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetList();
        }

        private void cm_Location_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetList();
        }


        private void GetList()
        {
            string Name = cm_Name.Text;
            string Location = cm_Location.Text;

            for (int i = 0; i < certs.Count; i++)
            {
                certs.RemoveAt(0);
            }

            if (Name.Length > 0 && Location.Length > 0)
            {
                foreach (Certificate cert in a.Logic.Certs.GetAllCerts(Name, Location))
                {
                    certs.Add(cert);
                }
                
            }
        }


    }
}
