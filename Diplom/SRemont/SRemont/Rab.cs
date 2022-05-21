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
    public partial class Rab : Form
    {
        private static Rab instance = null;
        private string cellval; //значение столбца1, выбранной строки

        public Rab()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static Rab GetInstance()
        {
            if (instance == null)
                instance = new Rab();
            return instance;
        }

        //построение сетки
        public void ViewRab()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from a in db.Rabota
                    select a;
            dataGridView1.DataSource = q;
            textBox1.Text = "";
            textBox2.Text = "";
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
                    foreach (var v in db.Rabota)
                    {
                        if (v.naim_rab == textBox1.Text) b = false;
                    }
                    if (b)
                    {
                        Rabota item = new Rabota();
                        item.naim_rab = textBox1.Text;
                        item.stoim = Convert.ToDecimal(textBox2.Text);
                        db.Rabota.InsertOnSubmit(item);
                        db.SubmitChanges();
                        ViewRab();
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
                "Изменить данные о работе: " + cellval + " ?",
                    "Изменение записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Rabota
                         where a.naim_rab == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {

                    if (textBox1.Text != "")
                    {
                        q.naim_rab = textBox1.Text;
                        q.stoim = Convert.ToDecimal(textBox2.Text);
                        db.SubmitChanges();
                        ViewRab();
                    }
                    else MessageBox.Show("Введите название работы", "Ошибка");
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
                "Удалить работу: " + cellval + " ?",
                    "Удаление записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Rabota
                         where a.naim_rab == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {
                    db.Rabota.DeleteOnSubmit(q);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewRab();
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
        private void Rab_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }

        //загрузка формы
        private void Rab_Load(object sender, EventArgs e)
        {
            ViewRab();
        }
    }
}
