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
    public partial class MU : Form
    {
        private static MU instance = null;
        private string cellval; //значение столбца1, выбранной строки

        public MU()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static MU GetInstance()
        {
            if (instance == null)
                instance = new MU();
            return instance;
        }

        //построение сетки
        public void ViewMU()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from a in db.View_5
                    select a;
            dataGridView1.DataSource = q;
            textBox1.Text = "";
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
                    foreach (var v in db.ModelUstr)
                    {
                        if (v.naim_mu == textBox1.Text) b = false;
                    }
                    if (b)
                    {
                        ModelUstr item = new ModelUstr();
                        item.naim_mu = textBox1.Text;
                        item.id_tu = Convert.ToInt32(comboBox1.SelectedValue);
                        db.ModelUstr.InsertOnSubmit(item);
                        db.SubmitChanges();
                        ViewMU();
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
                "Изменить данные о модели устройства: " + cellval + " ?",
                    "Изменение записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.ModelUstr
                         where a.naim_mu == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {

                    if (textBox1.Text != "")
                    {
                        q.naim_mu = textBox1.Text;
                        q.id_tu = Convert.ToInt32(comboBox1.SelectedValue);
                        db.SubmitChanges();
                        ViewMU();
                    }
                    else MessageBox.Show("Введите название модели устройства", "Ошибка");
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
                "Удалить модель устройства: " + cellval + " ?",
                    "Удаление записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.ModelUstr
                         where a.naim_mu == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {
                    db.ModelUstr.DeleteOnSubmit(q);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewMU();
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
                comboBox1.Text = Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
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
        private void MU_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }

        //загрузка формы
        private void MU_Load(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            this.comboBox1.DataSource = from a in db.TipUstr
                                        select new
                                        {
                                            pID = a.id_tu,
                                            pName = a.naim_tu
                                        };
            this.comboBox1.DisplayMember = "pName";
            this.comboBox1.ValueMember = "pID";
            this.comboBox1.Text = "";
            ViewMU();   
        }
    }
}
