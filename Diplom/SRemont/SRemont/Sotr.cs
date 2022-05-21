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
    public partial class Sotr : Form
    {
        private static Sotr instance = null;
        private string cellval; //значение столбца1, выбранной строки

        public Sotr()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static Sotr GetInstance()
        {
            if (instance == null)
                instance = new Sotr();
            return instance;
        }

        //построение сетки
        public void ViewSotr()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from a in db.Sotrudnik
                    select a;
            dataGridView1.DataSource = q;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
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
                    foreach (var v in db.Sotrudnik)
                    {
                        if (v.fio_sotr == textBox1.Text) b = false;
                    }
                    if (b)
                    {
                        Sotrudnik item = new Sotrudnik();
                        item.fio_sotr = textBox1.Text;
                        item.login = textBox2.Text;
                        item.pass = textBox3.Text;
                        item.dost = comboBox1.Text;
                        db.Sotrudnik.InsertOnSubmit(item);
                        db.SubmitChanges();
                        ViewSotr();
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
                "Изменить данные о сотруднике: " + cellval + " ?",
                    "Изменение записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Sotrudnik
                         where a.fio_sotr == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {

                    if (textBox1.Text != "")
                    {
                        q.fio_sotr = textBox1.Text;
                        q.login = textBox2.Text;
                        q.pass = textBox3.Text;
                        q.dost = comboBox1.Text;
                        db.SubmitChanges();
                        ViewSotr();
                    }
                    else MessageBox.Show("Введите ФИО сотрудника", "Ошибка");
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
                "Удалить сотрудника: " + cellval + " ?",
                    "Удаление записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Sotrudnik
                         where a.fio_sotr == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {
                    db.Sotrudnik.DeleteOnSubmit(q);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewSotr();
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
                textBox3.Text = Convert.ToString(dataGridView1[3, dataGridView1.CurrentRow.Index].Value);
                comboBox1.Text = Convert.ToString(dataGridView1[4, dataGridView1.CurrentRow.Index].Value);
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
        
        //загрузка формы
        private void Sotr_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Администратор");
            comboBox1.Items.Add("Пользователь");          
            ViewSotr();   
        }

        //закрытие формы
        private void Sotr_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;  
        }
    }
}
