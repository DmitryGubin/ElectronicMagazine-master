using ElectronicMagazine.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using System.Data.Entity;

namespace ElectronicMagazine.PageUsers.ProfileTeacher
{
    /// <summary>
    /// Логика взаимодействия для TeacherProf.xaml
    /// </summary>
    public partial class TeacherProf : Window
    {
        Message _message = new Message();
        JournalEntities entities = new JournalEntities();
        int _idTeacher;
        public TeacherProf(int idTeacher)
        {
            DataContext = _message;
            _idTeacher = idTeacher;
            InitializeComponent();
            ImagePhoto();
            Update();
            LoadMessages();

/*            foreach (var message in entities.Message)
            {
                MessageListBox.Items.Add(message);
            }*/
        }


        private void LoadMessages()
        {
            using (var context = new JournalEntities()) 
            {
                var messages = context.Message.Include(m => m.Students).ToList();

                MessageListBox.ItemsSource = messages;
            }
        }

        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageListBox.SelectedItem is Message selectedMessage)
            {
                OpenChatWindow(selectedMessage);
            }
        }

        private void MessageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Handle selection change if needed
        }

        private void OpenChatWindow(Message message)
        {
            ChatWindow chatWindow = new ChatWindow(message.Students, _idTeacher);
            chatWindow.Show();
        }

        public void Update()
        {

            var teachers = entities.Teachers.FirstOrDefault(s => s.Id == _idTeacher);

            if (teachers != null)
            {

                TitleName.Content = teachers.Имя_;
                TitleSecondName.Content = teachers.Фамилия;
            }
            else 
            {
                TitleName.Content = "";
                TitleSecondName.Content = "";
            }
        }

        public void ImagePhoto()
        {
            var student = entities.Teachers.FirstOrDefault(s => s.Id == _idTeacher);
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string imagePath = System.IO.Path.Combine(projectPath, "StudensPhoto", "user.png");

            if (student != null)
            {
                if (!string.IsNullOrEmpty(student.PhotoPath))
                {
                    string imageStudents = System.IO.Path.Combine(projectPath, student.PhotoPath);
                    ImageStudens.Source = new BitmapImage(new Uri(imageStudents));
                }
                else
                {
                    ImageStudens.Source = new BitmapImage(new Uri(imagePath));
                }
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var win = new Menu.MainMenu(_idTeacher);
            win.Show();
            this.Close();
        }
    }
}
