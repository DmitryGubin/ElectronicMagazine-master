using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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

namespace ElectronicMagazine.Magazine
{
    /// <summary>
    /// Логика взаимодействия для Journal.xaml
    /// </summary>
    public partial class JournalPage : Page
    {
        JournalEntities entities = new JournalEntities();
        string discipline;
        int group;
        private List<Students> students;
        int _dis;

        public JournalPage(int group1, string discipline, int dis)
        {
            InitializeComponent();
            _dis = dis;
            this.discipline = discipline;
            this.group = group1;

            Manager.MainFrame = Manager.MainFrame;

            var matches = from attr in entities.Students
                          where attr.Id_Класса == group1
                          orderby attr.Фамилия, attr.Имя
                          select attr;

            foreach (var student in matches)
            {
                ListBoxStudens.Items.Add(student);
            }

            this.students = matches.ToList();
            DataTable dataTable = GetDataTable(matches.ToList(), _dis);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
        private DataTable GetDataTable(List<Students> students, int selectedDiscipline)
        {
            DataTable table = new DataTable();

            DataColumn orderColumn = new DataColumn("Номер строки", typeof(string));
            orderColumn.MaxLength = 100;
            table.Columns.Add(orderColumn);

            DataColumn cl = new DataColumn("Студент", typeof(string));
            cl.MaxLength = 100;
            table.Columns.Add(cl);

            DataColumn averageColumn = new DataColumn("Средний бал", typeof(double));
            table.Columns.Add(averageColumn);

            var dateGrades = students.Select(s => s.Grades
                .Where(g => g.Id_Дисциплины == selectedDiscipline) 
                .Select(g => g.Дата_оценки.Value.Date)
                .ToList())
                .SelectMany(g => g)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            dateGrades.ForEach(d =>
            {
                DataColumn dateColumn = new DataColumn(d.ToShortDateString().Replace('.', '-'), typeof(string));
                dateColumn.MaxLength = 100;
                table.Columns.Add(dateColumn);
            });

            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                DataRow rw = table.NewRow();
                table.Rows.Add(rw);
                rw["Номер строки"] = i + 1;
                rw["Студент"] = s.Фамилия + ' ' + s.Имя;

                var grades = dateGrades.Select(d =>
                {
                    var grade = s.Grades
                        .Where(g => g.Id_Дисциплины == selectedDiscipline && g.Дата_оценки.Value.Date.Equals(d))
                        .FirstOrDefault();

                    return grade?.Оценка?.ToString() ?? "";
                }).ToArray();

                var item = new object[] { i + 1, s.Фамилия + ' ' + s.Имя };

                double averageGrade = Math.Round(s.Grades
                    .Where(g => g.Id_Дисциплины == selectedDiscipline && g.Оценка.HasValue)
                    .Average(g => g.Оценка.Value), 1);

                item = item.Concat(new object[] { averageGrade }).Concat(grades).ToArray();
                rw.ItemArray = item;
            }

            orderColumn.ReadOnly = true;
            cl.ReadOnly = true;
            averageColumn.ReadOnly = true;
            return table;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ListBoxStudens.SelectedItem as Students;

            DateTime date = (DateTime)(selected.Дата_рождения);

            string dateBirth = date.ToString("dd.MM.yyyy");

            StudentsProf.FirstName = selected.Имя;
            StudentsProf.SecondName = selected.Фамилия;
            StudentsProf.Birthday = dateBirth;
            StudentsProf.Class = selected.Id_Класса.ToString();


            StudentId.IdStudent = selected.Id.ToString();
            StudentId.IdClasses = selected.Id_Класса.ToString();
            StudentId.IdDiscipline = selected.Grades.ToString();





            var linq = (from attr in entities.Discipline where attr.Дисциплина == discipline select attr).Single();
            Manager.MainFrame.Navigate(new StudentsAssessment(linq, (ListBoxStudens.SelectedItem as Students)));

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = (e.OriginalSource as DataGrid).SelectedItem;
            var b = (e.OriginalSource as DataGrid).SelectedIndex;
            var c = (e.OriginalSource as DataGrid).CurrentItem;
            if (a is DataRowView && b > 2)
            {
                var newGrade = (a as DataRowView)[b];
                var d = 0;
            }
        }

        private void dataGrid_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void dataGrid_CurrentCellChanged_1(object sender, EventArgs e)
        {
            var d = 0;
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    int rowIndex = e.Row.GetIndex();
                    var el = e.EditingElement as TextBox;
                    var ci = new CultureInfo("ru-RU");

 
                    if (!int.TryParse(el.Text, out int newGrade))
                    {
                        MessageBox.Show("Пожалуйста, введите корректное числовое значение.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true; 
                        return;
                    }


                    if (newGrade < 1 || newGrade > 5)
                    {
                        MessageBox.Show("Пожалуйста, введите значение от 1 до 5.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true; 
                        return;
                    }

                    var date = DateTime.ParseExact(column.Header.ToString(), "dd-MM-yyyy", ci);
                    var student = students[rowIndex];

                    MessageBox.Show($"{student.Фамилия} {student.Имя} {newGrade} {date}");

                    using (var context = new JournalEntities())
                    {
 
                        var gradeToUpdate = context.Grades.FirstOrDefault(g => g.Id_Студента == student.Id && g.Дата_оценки == date);

                        if (gradeToUpdate != null)
                        {

                            gradeToUpdate.Оценка = newGrade;
                        }
                        else
                        {
 
                            var newGradeEntry = new Grades
                            {
                                Id_Студента = student.Id,
                                Id_Дисциплины = _dis,
                                Дата_оценки = date,
                                Оценка = newGrade
                            };
                            context.Grades.Add(newGradeEntry);
                        }

                        MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}

