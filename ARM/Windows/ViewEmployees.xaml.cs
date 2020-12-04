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
using System.Windows.Shapes;

namespace ARM.Windows
{
    /// <summary>
    /// Логика взаимодействия для ViewEmployees.xaml
    /// </summary>
    public partial class ViewEmployees : Window
    {
        public List<Worker> Workers { get; set; }
        public List<string> DivisionList { get; set; } = new List<string>();

        public ARMEntities database = new ARMEntities();

        public ViewEmployees()
        {
            InitializeComponent();
            Workers = database.Worker.ToList();
            DivisionList.Add("Все отделы");
            DivisionList.AddRange(database.Division.Select(d => d.Title));
            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddOrEditEmployee window = new AddOrEditEmployee(new Worker());
            window.Show();
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDatagrid.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать нужного пользователя");
                return;
            }

            Windows.AddOrEditEmployee window = new AddOrEditEmployee(MainDatagrid.SelectedItem as Worker);
            window.Show();
            this.Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDatagrid.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать нужного пользователя");
                return;
            }

            MessageBoxResult response = MessageBox.Show("Вы точно хотите удалить данного пользователя?", "Удаление", MessageBoxButton.YesNo);
            if (response == MessageBoxResult.Yes)
            {
                try
                {
                    database.Worker.Remove(MainDatagrid.SelectedItem as Worker);
                    database.SaveChanges();

                    ViewEmployees window = new ViewEmployees();
                    window.Show();
                    this.Close();

                    MessageBox.Show("Удаление прошло успешно");
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка");
                }
            }
        }

        private void DivisionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void SelectTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }

        private void AZRadiobutton_Checked(object sender, RoutedEventArgs e)
        {
            Sort();
        }

        private void ZARadiobutton_Checked(object sender, RoutedEventArgs e)
        {
            Sort();
        }

        private void Sort()
        {
            try
            {
                Workers = database.Worker.ToList();

                if (DivisionComboBox.SelectedIndex == 0)
                {
                    Workers = Workers.ToList();
                }
                else
                {
                    Workers = Workers.Where(w => w.Division.Title == DivisionComboBox.SelectedItem.ToString()).ToList();
                }

                Workers = Workers.Where(w => w.FirstName.Contains(SelectTextBox.Text) || w.LastName.Contains(SelectTextBox.Text)).ToList();

                if (AZRadiobutton.IsChecked == true)
                {

                    Workers = Workers.OrderBy(w => w.FirstName).ToList();
                }
                else
                {
                    Workers = Workers.OrderByDescending(w => w.LastName).ToList();
                }

                MainDatagrid.ItemsSource = null;
                MainDatagrid.ItemsSource = Workers;
            }
            catch (Exception ex)
            {
                if (ex.HResult != -2147467261)
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }
    }
}
