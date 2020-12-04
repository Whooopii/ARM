using Microsoft.Win32;
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
    /// Логика взаимодействия для AddOrEditEmployee.xaml
    /// </summary>
    public partial class AddOrEditEmployee : Window
    {
        public Worker CurrentWorker { get; set; }
        public ARMEntities database { get; set; } = new ARMEntities();
        private string FullFileName;

        public AddOrEditEmployee(Worker worker)
        {
            InitializeComponent();
            CurrentWorker = worker;
            FullFileName = worker.Photo;
            List<Division> DivisionList = database.Division.ToList();
            foreach (var d in DivisionList)
            {
                DivisionComboBox.Items.Add(d.Title);
            }
            if (CurrentWorker.FirstName == null)
            {
                ContentTextBlock.Text = "Добавить";
            }
            else
            {
                DivisionComboBox.SelectedValue = DivisionList.ToList().First(d => d.Id == CurrentWorker.Division.Id).Title;
            }


            DataContext = CurrentWorker;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(FirstNameTextBox.Text == "" || LastNameTextBox.Text == "" || AgeTextBox.Text == "" || EmailTextBox.Text == "")
                {
                    MessageBox.Show("Пожалуйста, заполните поля имени, фамилии, возраста и почты.");
                    return;
                }
                Worker worker = new Worker();
                worker.FirstName = FirstNameTextBox.Text;
                worker.LastName = LastNameTextBox.Text;
                worker.Phone = PhoneTextBox.Text;
                worker.Email = EmailTextBox.Text;
                worker.DivisionId = database.Division.ToList().FirstOrDefault(d => d.Title == DivisionComboBox.SelectedItem as string).Id;
                worker.Age = Convert.ToInt32(AgeTextBox.Text);
                worker.Photo = FullFileName;
                if (ContentTextBlock.Text.ToString() == "Сохранить")
                {
                    Worker oldWorker = database.Worker.First(w => w.Id == CurrentWorker.Id);
                    database.Worker.Remove(oldWorker);
                }
                database.Worker.Add(worker);
                database.SaveChanges();

                MessageBox.Show("Успешное сохранение данных");
                ViewEmployees window = new ViewEmployees();
                window.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка сохранения данных");
            }
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    Filter = "Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
                };

                if (fileDialog.ShowDialog() == true)
                {
                    FullFileName = fileDialog.FileName;
                    PhotoImage.Source =new BitmapImage(new Uri(FullFileName));
                }
                else
                    MessageBox.Show("Выбор картинки был отменён");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            ViewEmployees nextWindow = new ViewEmployees();
            nextWindow.Show();
            this.Close();
        }
    }
}
