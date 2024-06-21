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
using System.Xml;

namespace ElectronicMagazine.AdminPanel
{
    /// <summary>
    /// Логика взаимодействия для EditPanelStudent.xaml
    /// </summary>
    public partial class EditPanelStudent : Page
    {
        private Students students = new Students();
        JournalEntities entities = new JournalEntities();
        public EditPanelStudent(Students students_)
        {
            InitializeComponent();

            var logins = entities.Users.Select(u => u.Login).ToList();

            foreach (var users in logins)
                ComboBox_User.Items.Add(users);

            foreach (var classes in entities.Classes)
                ComboClass.Items.Add(classes);

            if (students_ != null)
            {
                students = students_;
            }
            else
            {
                students = new Students();
            }

            DataContext = students;
        }
        /*
                private void SaveClick(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        if (students.Id == 0)
                        {
                            JournalEntities.GetContext().Students.Add(students);
                        }

                        students.Дата_рождения = DateTime.Parse(DataPicerBirthday.Text);
                        JournalEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные сохранены", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }*/

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, добавляем ли нового студента
                if (students.Id == 0)
                {
                    // Устанавливаем значения для студента, если они еще не установлены
                    students.Имя = TextFirstName.Text;
                    students.Фамилия = TextSecondName.Text;
                    students.Дата_рождения = DataPicerBirthday.SelectedDate; // Подразумевается, что у вас используется DatePicker для выбора даты
                    students.PhotoPath = TextPhoto.Text;

                    // Устанавливаем Id пользователя (по логину) и Id класса (по выбранному классу в ComboBox)
                    var selectedUserLogin = ComboBox_User.SelectedItem as string;
                    var selectedClass = ComboClass.SelectedItem as Classes;

                    if (selectedUserLogin != null)
                    {
                        var user = entities.Users.FirstOrDefault(u => u.Login == selectedUserLogin);
                        if (user != null)
                            students.Id_User = user.Id;
                    }

                    if (selectedClass != null)
                    {
                        students.Id_Класса = selectedClass.Id;
                    }

                    // Добавляем студента в контекст данных
                    JournalEntities.GetContext().Students.Add(students);
                }
                else
                {
                    // Иначе обновляем существующего студента
                    var existingStudent = JournalEntities.GetContext().Students.Find(students.Id);
                    if (existingStudent != null)
                    {
                        existingStudent.Имя = TextFirstName.Text;
                        existingStudent.Фамилия = TextSecondName.Text;
                        existingStudent.Дата_рождения = DataPicerBirthday.SelectedDate; // Подразумевается, что у вас используется DatePicker для выбора даты
                        existingStudent.PhotoPath = TextPhoto.Text;

                        // Устанавливаем Id пользователя (по логину) и Id класса (по выбранному классу в ComboBox)
                        var selectedUserLogin = ComboBox_User.SelectedItem as string;
                        var selectedClass = ComboClass.SelectedItem as Classes;

                        if (selectedUserLogin != null)
                        {
                            var user = entities.Users.FirstOrDefault(u => u.Login == selectedUserLogin);
                            if (user != null)
                                existingStudent.Id_User = user.Id;
                        }

                        if (selectedClass != null)
                        {
                            existingStudent.Id_Класса = selectedClass.Id;
                        }
                    }
                }

                // Сохраняем изменения
                JournalEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void PhotoAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
                    TextPhoto.Text = dlg.FileName.ToString();
                students.PhotoPath = dlg.FileName;
            }
            catch 
            {
                MessageBox.Show("Не верный формат", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
