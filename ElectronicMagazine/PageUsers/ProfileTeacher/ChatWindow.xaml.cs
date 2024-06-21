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

namespace ElectronicMagazine.PageUsers.ProfileTeacher
{
    /// <summary>
    /// Логика взаимодействия для ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private Students _student;
        int _idTeacher;

        public ChatWindow(Students student, int idTeacher)
        {
            InitializeComponent();
            _idTeacher = idTeacher;
            _student = student;
            StudentNameTextBlock.Text = $"Чат {_student.Имя}";
            LoadChatHistory();
        }

        private void LoadChatHistory()
        {
            
            using (var context = new JournalEntities())
            {
                var messages = context.Message
                    .Where(m => m.Id_Student == _student.Id)
                    .OrderBy(m => m.Date)
                    .ToList();

                ChatListView.ItemsSource = messages;
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageText = MessageTextBox.Text;
            if (!string.IsNullOrEmpty(messageText))
            {
                using (var context = new JournalEntities())
                {
                    var message = new Message
                    {
                        Id_Student = _student.Id,
                        Id_Teacher = _idTeacher, 
                        Report = messageText,
                        Author = "Teacher", 
                        Date = DateTime.Now
                    };
                    context.Message.Add(message);
                    context.SaveChanges();
                }
                LoadChatHistory(); 
                MessageTextBox.Clear();
            }
        }
    }
}
