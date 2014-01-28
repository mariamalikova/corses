using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace mobile
{
    class myHTML
    {
        StreamWriter sw;
        Hashtable vocabulary = new Hashtable();
        char[] charsToTrim = { ',', '*', ' ', '?', '!', '.', '\'' };

        string path_file;       // путь к файлу с текстом        
        int max_row_count;      // максимально допустимое количество строк в HTML - файле 
        int file_count;         // количество созданных файлов
        int row_count;          // текущее кол-во строк в текущем файле
        string path_html;       // путь к файлу html
        
        // конструктор
        public myHTML(string file, string voc, string html, int count)
        {
            this.path_file = file;            
            this.max_row_count = count;
            this.path_html = html;
            this.file_count = 0; 
        }

        // создаем словарь
        public void Create_Voc(string path_voc)
        {            
            using (StreamReader sr = new StreamReader(path_voc, Encoding.GetEncoding(1251)))
                {
                    while (sr.Peek() >= 0)
                    {
                        string word = sr.ReadLine();
                        foreach (char i in charsToTrim)
                        {
                            if (word.Contains(i))
                            {
                                throw new Exception("Неверный формат файла");
                            }
                        }
                        if (!vocabulary.ContainsKey(word))
                            vocabulary.Add(word.ToLower(), null);
                    }
                } 
        }

        // создаем поток для записи
        public void Create_Thread(string path)
        {
            sw = new StreamWriter(path);
        }

        // пишем шапку файла
        public void Create_HTML_head()
        {
            //row_count = 0;            
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html> <head> <title> Мой HTML </title> ");
            sw.WriteLine("<meta charset =\"utf-8\"> </head>");
            sw.WriteLine("<body> ");
        }

        // обрабатываем строку
        public void Create_String(string str)
        {
            if (str.Length > 0)
            {
                if (row_count >= max_row_count)
                {
                    // пишем в новый файл
                    file_count++;
                    // закрываем текущий файл
                    Close_HTML();
                    // создаем имя следующего файла
                    string pth = path_html.Remove(path_html.Length - 5, 5) + "_" + file_count + ".html";
                    Create_Thread(pth);
                    // создаем новый файл                    
                    Create_HTML_head();
                    row_count = 0;
                }
                // разбиваем на массив строк
                string[] st_mass = str.Split(' ');
                sw.Write("<p>");
                foreach (var k in st_mass)
                {
                    // отбрасываем ненужные символы справа
                    Match result = Regex.Match(k, @"\w+", RegexOptions.RightToLeft);
                    // проверяем наличие слова в словаре
                    if (vocabulary.ContainsKey(result.Value.ToLower()))
                        sw.Write("<strong> <em> {0} </strong> </em>  ", k);
                    else sw.Write(" {0}", k);
                }
                sw.Write("<p>\r\n");
                row_count++;
            }
        }
        // закрываем html-файл и поток
        public void Close_HTML()
        {
            sw.WriteLine("</body> </html>");
            sw.Close();
        }

    }
}

