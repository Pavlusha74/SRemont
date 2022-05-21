using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SRemont
{
    public partial class Kli : Form
    {
        private static Kli instance = null;
        private string cellval; //значение столбца1, выбранной строки

        public Kli()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static Kli GetInstance()
        {
            if (instance == null)
                instance = new Kli();
            return instance;
        }

        //построение сетки
        public void ViewKli()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from a in db.Klient
                    select a;
            dataGridView1.DataSource = q;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            
        }        

        //нажатие кнопки добавить
        private void button1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {
                bool b = true;
                if (textBox1.Text != "")
                {
                    foreach (var v in db.Klient)
                    {
                        if (v.fio_kl == textBox1.Text) b = false;
                    }
                    if (b)
                    {
                        Klient item = new Klient();
                        item.fio_kl = textBox1.Text;
                        item.adres = textBox2.Text;
                        item.email = textBox3.Text;
                        item.tel = textBox4.Text;
                        db.Klient.InsertOnSubmit(item);
                        db.SubmitChanges();
                        ViewKli();
                    }
                    else MessageBox.Show
                        ("Повторный ввод данных", "Ошибка");
                }
                else MessageBox.Show
                  ("Введите данные для добавления", "Ошибка");
            }
            catch (Exception ex)
            {
                throw ex;
            }     
        }

        //нажатие кнопки изменить
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Изменить данные о клиенте: " + cellval + " ?",
                    "Изменение записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Klient
                         where a.fio_kl == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {

                    if (textBox1.Text != "")
                    {
                        q.fio_kl = textBox1.Text;
                        q.adres = textBox2.Text;
                        q.email = textBox3.Text;
                        q.tel = textBox3.Text;
                        db.SubmitChanges();
                        ViewKli();
                    }
                    else MessageBox.Show("Введите ФИО клиента", "Ошибка");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }            
        }

        //нажатие кнопки удалить
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Удалить клиента: " + cellval + " ?",
                    "Удаление записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Klient
                         where a.fio_kl == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {
                    db.Klient.DeleteOnSubmit(q);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewKli();
            }
        }

        //нажатие кнопки отмена
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //событие при изменении выбора ячейки
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                cellval = Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
                textBox1.Text = Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
                textBox2.Text = Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
                textBox3.Text = Convert.ToString(dataGridView1[4, dataGridView1.CurrentRow.Index].Value);
                textBox4.Text = Convert.ToString(dataGridView1[3, dataGridView1.CurrentRow.Index].Value);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //добавление номера записи
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
        }        

        //закрытие формы
        private void Klient_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null; 
        }

        //загрузка формы
        private void Klient_Load(object sender, EventArgs e)
        {
            ViewKli();   
        }
    }
}
