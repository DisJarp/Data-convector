using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace TestAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        String ferstNameError = "", middleNameError = "", lastNameError = "";
        String[] lineResult;
        int allCount = 0;
        bool flagValid = true;
        List<User> result = new List<User>();
        public MainWindow()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = "c:\\";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

        }


        private void b_Write_Click(object sender, RoutedEventArgs e)
        {
            TaskAsync_Write(sender, e);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ;
        }

        private async Task TaskAsync_Write(object sender, RoutedEventArgs e)
        {
            openFileDialog1.ShowDialog();
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string fileText = System.IO.File.ReadAllText(filename);
            l_Steam.Content = filename;
            MessageBox.Show("Файл открыт");

            try
            {
                using (StreamReader sr = new StreamReader(filename, true))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        addItemTable(validator(line));

                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
            dg_ViewList.ItemsSource = result;
        }


        public String[] validator(String line) {
            String ferstName = "", middleName = "", lastName = "", number = "", replace = "";
            int count = 0;
            number = checkNumber(Regex.Replace(line, @"[^\d]", ""));
            String[] lineAll = line.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lineAll.Length; i++)
            {
                replace = Regex.Replace(lineAll[i], @"[^А-Яа-я]+", String.Empty);

                switch (count)
                {
                    case 0:
                        if (!Regex.IsMatch(replace, "^[^а-яА-Я]+$"))
                        {
                            
                            lastName = replace;
                            if (lastName == "") {
                                lastName = "Поле не может быть пустым";
                                flagValid = false;
                            }
                            replace = "";
                        }
                        else
                        {
                            replace = "";
                        }
                        count++;
                        break;
                    case 1:
                        if (!Regex.IsMatch(replace, "^[^а-яА-Я]+$"))
                        {
                            ferstName = replace;
                            if (ferstName == "")
                            {
                                ferstName = "Поле не может быть пустым";
                                flagValid = false;
                            }
                            replace = "";
                        }
                        else
                        {
                            replace = "";
                        }
                        count++;
                        break;
                    case 2:
                        if (!Regex.IsMatch(replace, "^[^а-яА-Я]+$"))
                        {
                            middleName = replace;
                            replace = "";
                        }
                        else
                        {
                            replace = "";
                        }
                        count++;
                        break;
                }
            }

            String[] lines = { lastName, ferstName, middleName, number };

            return lines;
        }

        public void addItemTable(string [] line) {
            result.Add(new User(allCount, line[0], line[1], line[2], line[3]));
            allCount++;
            
        }

        private void b_Save_Click(object sender, RoutedEventArgs e)
        {
            saveJsonFile();
            MessageBox.Show("Фаил успешно сохранен!");
        }

        public String checkNumber(string number) {
            if (number.Length <= 11 & number.Length >= 10)
            {
                if (number[0] == '8')
                {
                    var array = number.ToCharArray();
                    array[0] = '7';
                    number = new string(array);
                }
                if (number[0] == '9') {
                    number = "7" + number;
                }
            }
            else
            {
                number = "Неправельный формат номера";
                flagValid = false;
                return number;
            }
            return number;
        }

        public void saveJsonFile() {
            var jsonStringOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            saveFileDialog1.ShowDialog();
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;

            var final = new Contacts {
                Contact = result,
                Count = result.Count
            };
           
            string jsonString = JsonSerializer.Serialize(final, jsonStringOptions);
            File.WriteAllText(filename, jsonString);
        }
    }
}
