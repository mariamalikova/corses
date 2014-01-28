using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace mobile
{
    public partial class Form1 : Form
    {
        string path_file;       // путь к файлу с текстом
        string path_voc;        // путь к файлу словаря        

        public Form1()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            InitializeComponent();
           
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
                      
            saveFileDialog1.Filter = "html files (*.html)|*.html";
            saveFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
        }

       // запоминаем путь к файлу с текстом
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path_file = openFileDialog1.FileName;
                    System.IO.FileInfo file = new System.IO.FileInfo(path_file);

                    // проверяем допустимый размер файла
                    if (file.Length > 2097152)
                    {
                        throw new Exception("Размер файла превышает допустимый");
                    }
                    textBox1.Text = path_file + "  Размер файла  " + file.Length.ToString() + " байт";

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: Невозможно создать файл html. " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // запоминаем путь к файлу со словарем
        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path_voc = openFileDialog1.FileName;
                    System.IO.FileInfo file = new System.IO.FileInfo(path_voc);
                    
                    //проверяем допустимый размер файла
                    if (file.Length > 2097152)
                    {
                        throw new Exception("Размер файла превышает допустимый");
                    }                  
                    textBox2.Text = path_voc + "  Размер файла  " + file.Length.ToString() + " байт";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: Невозможно создать файл html. " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // обработываем файлы
        private void button3_Click_1(object sender, EventArgs e)
        {            
            try
            {
                // проверяем все ли пути к файлам есть
                if ((path_file == null)||(path_voc==null))
                {
                    throw new Exception("Не все необходимые файлы были загружены!");
                }

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string path = saveFileDialog1.FileName;
                    // создаем объект класса myHTML
                    myHTML html = new myHTML(path_file, path_voc, path, int.Parse(numericUpDown1.Value.ToString()));
                    // заполняем словарь
                    html.Create_Voc(path_voc);

                    if (File.Exists(path)) { File.Delete(path); }

                    html.Create_Thread(path);
                    html.Create_HTML_head();

                    using (StreamReader sr = new StreamReader(path_file, Encoding.GetEncoding(1251)))
                    {
                        while (sr.Peek() >= 0)
                        {
                            // считали строку, передаем ее на обработку
                            html.Create_String(sr.ReadLine());
                        }
                    }
                    html.Close_HTML();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( "Ошибка: Невозможно создать файл html. " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
