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
    public partial class Det : Form
    {
        private static Det instance = null;
        private string cellval; //значение столбца1, выбранной строки

        public Det()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static Det GetInstance()
        {
            if (instance == null)
                instance = new Det();
            return instance;
        }

        //построение сетки
        public void ViewDet()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            var q = from a in db.View_1
                    select a;
            dataGridView1.DataSource = q;            
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            comboBox1.Text = "";
        }        

        //нажатие кнопки добавить
        private void button1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {
                bool b = true;
                if (textBox4.Text != "")
                {
                    foreach (var v in db.Detal)
                    {
                        if (v.naim_det == textBox4.Text) b = false;
                    }
                    if (b)
                    {
                        Detal item = new Detal();                        
                        item.cena = Convert.ToDecimal(textBox2.Text);
                        item.izg = textBox3.Text;
                        item.naim_det = textBox4.Text;
                        item.id_gdet = Convert.ToInt32(comboBox1.SelectedValue);
                        item.col = (int)numericUpDown1.Value;
                        item.garant = (int)numericUpDown2.Value;
                        db.Detal.InsertOnSubmit(item);
                        db.SubmitChanges();
                        ViewDet();
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
                "Изменить данные о детали: " + cellval + " ?",
                    "Изменение записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Detal
                         where a.naim_det == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {

                    if (textBox4.Text != "")
                    {                        
                        q.cena = Convert.ToDecimal(textBox2.Text);
                        q.izg = textBox3.Text;
                        q.naim_det = textBox4.Text;
                        q.id_gdet = Convert.ToInt32(comboBox1.SelectedValue);
                        q.col = (int)numericUpDown1.Value;
                        q.garant = (int)numericUpDown2.Value;
                        db.SubmitChanges();
                        ViewDet();
                    }
                    else MessageBox.Show("Введите название детали", "Ошибка");
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
                "Удалить деталь: " + cellval + " ?",
                    "Удаление записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Detal
                         where a.naim_det == Convert.ToString(cellval)
                         select a).SingleOrDefault();
                try
                {
                    db.Detal.DeleteOnSubmit(q);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewDet();
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
                textBox2.Text = Convert.ToString(dataGridView1[4, dataGridView1.CurrentRow.Index].Value);
                textBox3.Text = Convert.ToString(dataGridView1[5, dataGridView1.CurrentRow.Index].Value);
                textBox4.Text = Convert.ToString(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
                comboBox1.Text = Convert.ToString(dataGridView1[2, dataGridView1.CurrentRow.Index].Value);
                numericUpDown1.Value = Convert.ToInt32(dataGridView1[3, dataGridView1.CurrentRow.Index].Value);
                numericUpDown2.Value = Convert.ToInt32(dataGridView1[6, dataGridView1.CurrentRow.Index].Value);

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
        private void Det_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;  
        }

        //загрузка формы
        private void Det_Load(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            this.comboBox1.DataSource = from a in db.GDetal
                                        select new
                                        {
                                            pID = a.id_gdet,
                                            pName = a.naim_gdet
                                        };
            this.comboBox1.DisplayMember = "pName";
            this.comboBox1.ValueMember = "pID";
            this.comboBox1.Text = "";
            ViewDet();   
        }
    }
}
