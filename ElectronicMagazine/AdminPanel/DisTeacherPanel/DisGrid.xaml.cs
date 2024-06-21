using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicMagazine.AdminPanel.DisTeacherPanel
{
    /// <summary>
    /// Логика взаимодействия для DisGrid.xaml
    /// </summary>
    
    public partial class DisGrid : Page
    {
        Discipline discipline = new Discipline();
        public DisGrid()
        {
            InitializeComponent();
            DataContext = discipline;
        }

        private void dGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClickAdd(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)

                dGrid.ItemsSource = JournalEntities.GetContext().Discipline.ToList();
        }
    }
}
