using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Pointer> pointers = new List<Pointer>();
        private const string srlz_path = "E:\\Lab\\4 курс\\Визуалка\\Лаба3\\WindowsFormsApp1\\Serialization\\";
        private const string txt_path = "E:\\Lab\\4 курс\\Визуалка\\Лаба3\\WindowsFormsApp1\\file.txt";
        public Form1()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].HeaderText = "Слово";
            dataGridView1.Columns[1].HeaderText = "Страницы";
        }

        private void DGV_Refresh()
        {
            dataGridView1.Rows.Clear();
            if (pointers.Count > 0)
            {
                foreach (var p in pointers)
                {
                    string word;
                    List<int> pages;
                    p.Return_fields(out word, out pages);

                    string pgs = "";
                    for (int i = 0; i < pages.Count - 1; i++)
                    {
                        pgs += pages[i].ToString() + ", ";
                    }
                    pgs += pages.Last().ToString();

                    dataGridView1.Rows.Add(word, pgs);
                }
            }

            textBox1.Clear();
            textBox2.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string word = textBox1.Text;
            string[] pages = textBox2.Text.Split(',');
            if (pages.Length < 10)
            {
                List<int> int_pages = new List<int>();
                foreach (string page in pages)
                    int_pages.Add(Convert.ToInt32(page));
                pointers.Add(new Pointer(word, int_pages));
                MessageBox.Show("Указатель добавлен");
                DGV_Refresh();
            }
            else
                MessageBox.Show("Количество страниц не должно превышать 10");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(srlz_path + "pointer.dat", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, pointers);
                }

                MessageBox.Show("Сохранение завершено");
            }
            catch
            {
                MessageBox.Show("Не удалось сериализовать, попробуйте ещё раз");
            }

            pointers.Clear();
            DGV_Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pointer[]));
            try
            {
                using (FileStream fs = new FileStream(srlz_path + "pointer.xml", FileMode.OpenOrCreate))
                {
                    Pointer[] _pointers = pointers.ToArray();
                    serializer.Serialize(fs, _pointers);
                }

                MessageBox.Show("Сохранение завершено");
            }
            catch
            {
                MessageBox.Show("Не удалось сериализовать, попробуйте ещё раз");
            }

            pointers.Clear();
            DGV_Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (FileStream fs = new FileStream(srlz_path + "pointer.dat", FileMode.OpenOrCreate))
                {
                    pointers = (List<Pointer>)formatter.Deserialize(fs);
                }

                MessageBox.Show("Загрузка завершена");
                DGV_Refresh();
            }
            catch
            {
                MessageBox.Show("Не удалось десериализовать, попробуйте ещё раз");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pointer[]));

            try
            {
                using (FileStream fs = new FileStream(srlz_path + "pointer.xml", FileMode.OpenOrCreate))
                {
                    Pointer[] _pointers = serializer.Deserialize(fs) as Pointer[];
                    pointers = _pointers.ToList();
                }

                MessageBox.Show("Загрузка завершена");
                DGV_Refresh();
            }
            catch
            {
                MessageBox.Show("Не удалось десериализовать, попробуйте ещё раз");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pointers.Clear();
            try
            {
                StreamReader reader = new StreamReader(txt_path, Encoding.UTF8);
                using (reader)
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] splitline = line.Split(':');
                        string word = splitline[0];
                        string[] numbers = splitline[1].Split(',');
                        List<int> pages = new List<int>();
                        foreach (var n in numbers)
                            pages.Add(Convert.ToInt32(n));
                        pointers.Add(new Pointer(word, pages));
                    }
                }

                MessageBox.Show("Успешно загружено");
                DGV_Refresh();
            }
            catch
            {
                MessageBox.Show("Что-то пошло не по плану, проверьте содержимое файла");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pointers.RemoveAt(dataGridView1.SelectedRows[0].Index);
            DGV_Refresh();
        }
    }
}
