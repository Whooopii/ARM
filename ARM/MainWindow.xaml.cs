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

namespace ARM
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HeaderLabel.Content = "     АРМ для сотрудника\nинфомрационного отдела";
            HeaderLabel.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(Login.Text) || String.IsNullOrEmpty(Password.Password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            ARMEntities database = new ARMEntities();
            List<Admin> admins = database.Admin.ToList();
            foreach(var admin in admins)
            {
                if(admin.Login == Login.Text && admin.Password == Password.Password)
                {
                    MessageBox.Show("Успешная авторизация!");
                    Windows.ViewEmployees window = new Windows.ViewEmployees();
                    window.Show();
                    this.Close();
                    return;
                }
            }
            MessageBox.Show("Пользователь не найден");
        }
    }
}
