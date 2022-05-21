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
    public partial class TU : Form
    {
        private static TU instance = null;
        private string cellval; //значение столбца1, выбранной строки

        public TU()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static TU GetInstance()
        {
            if (instance == null)
                instance = new TU();
            return instance;
        }

        //построение сетки
        public void ViewTU()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from a in db.TipUstr
                    select a;
            dataGridView1.DataSource = q;
            textBox1.Text = "";
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
                    foreach (var v in db.TipUstr)
                    {
                        if (v.naim_tu == textBox1.Text) b = false;
                    }
                    if (b)
                    {
                        TipUstr item = new TipUstr();
                        item.naim_tu = textBox1.Text;
                        db.TipUstr.InsertOnSubmit(item);
                        db.SubmitChanges();
                        ViewTU();
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
                "Изменить данные о типе устройств: " + cellval + " ?",
                    "Изменение записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.TipUstr
                         where a.naim_tu == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {

                    if (textBox1.Text != "")
                    {
                        q.naim_tu = textBox1.Text;                        
                        db.SubmitChanges();
                        ViewTU();
                    }
                    else MessageBox.Show("Введите название типа устройств", "Ошибка");
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
                "Удалить тип устройств: " + cellval + " ?",
                    "Удаление записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.TipUstr
                         where a.naim_tu == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {
                    db.TipUstr.DeleteOnSubmit(q);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewTU();
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
        private void TU_Load(object sender, EventArgs e)
        {
            ViewTU();   
        }

        //закрытие формы
        private void TU_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }
    }
}
