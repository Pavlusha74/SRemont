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
    public partial class Form1 : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        static public string userName;
        static public string dostup;
        private string cellval; 
        private string cellval1;
        private string cellval2;
        private double sum = 0;

        public Form1(string u, string d)
        {
            InitializeComponent();
            SelfRef = this;
            userName = u;
            dostup = d;
            toolStripStatusLabel1.Text = d + ": " + u; 
        }

        //свойство для вызова методов из другой формы
        public static Form1 SelfRef
        {
            get;
            set;
        } 

        //построение сетки
        public void ViewData()
        {            
            var q1 = from a in db.View_4
                    select a;
            dataGridView1.DataSource = q1;
            GetDet();
            GetRab();            
        }

        //загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            if (dostup == "Администратор")
                сотрудникиToolStripMenuItem.Enabled = true;
            else if (dostup == "Пользователь")
                сотрудникиToolStripMenuItem.Enabled = false;
            this.comboBox1.DataSource = from a in db.TipUstr
                                        select new
                                        {
                                            pID = a.id_tu,
                                            pName = a.naim_tu
                                        };
            this.comboBox1.DisplayMember = "pName";
            this.comboBox1.ValueMember = "pID";
            this.comboBox1.Text = "";
            this.comboBox2.DataSource = from a in db.ModelUstr
                                        select new
                                        {
                                            pID = a.id_mu,
                                            pName = a.naim_mu
                                        };
            this.comboBox2.DisplayMember = "pName";
            this.comboBox2.ValueMember = "pID";
            this.comboBox2.Text = "";
            this.comboBox3.DataSource = from a in db.Klient
                                        select new
                                        {
                                            pID = a.id_kl,
                                            pName = a.fio_kl
                                        };
            this.comboBox3.DisplayMember = "pName";
            this.comboBox3.ValueMember = "pID";
            this.comboBox3.Text = "";
            this.comboBox4.DataSource = from a in db.Sotrudnik
                                        select new
                                        {
                                            pID = a.id_sotr,
                                            pName = a.fio_sotr
                                        };
            this.comboBox4.DisplayMember = "pName";
            this.comboBox4.ValueMember = "pID";
            this.comboBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";            
            dateTimePicker1.Checked = false;
            dateTimePicker2.Checked = false;
            dateTimePicker3.Checked = false;
            dateTimePicker4.Checked = false;
            checkBox1.Checked = false;
            ViewData();
        }

        //вызов формы Клиенты
        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kli form;
            form = Kli.GetInstance();
            form.Show();
        }

        //вызов формы Детали
        private void деталиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Det form;
            form = Det.GetInstance();
            form.Show();
        }

        //вызов формы Работы
        private void работыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rab form;
            form = Rab.GetInstance();
            form.Show();
        }

        //вызов справочника Группы деталей
        private void группыДеталейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GT form;
            form = GT.GetInstance();
            form.Show();
        }

        //вызов справочника Типы устройств
        private void типыУстройствToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TU form;
            form = TU.GetInstance();
            form.Show();
        }

        //вызов справочника Модели устройств
        private void моделиУстройствToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MU form;
            form = MU.GetInstance();
            form.Show();
        }

        //вызов справочника Сотрудники
        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sotr form;
            form = Sotr.GetInstance();
            form.Show();
        }

        //нажатие кнопки Выход
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string message = "Вы действительно хотите покинуть приложение?";
            const string caption = "Выход";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }            
        }

        //нажатие кнопки Поиск заказов
        private void button1_Click(object sender, EventArgs e)
        {
            //DataClasses1DataContext db = new DataClasses1DataContext();
            var i = from a in db.View_4
                    select a;

            if (textBox2.Text != "")
            {

                var q = from a in i
                        where a.id_zak == Convert.ToInt32(textBox2.Text)
                        select a;
                i = q;
            }
            if (textBox1.Text != "")
            {

                var q = from a in i
                        where a.ser == textBox1.Text
                        select a;
                i = q;
            }
            if (comboBox1.Text != "")
            {

                var q = from a in i
                        where a.naim_tu == comboBox1.Text
                        select a;
                i = q;
            }
            if (comboBox2.Text != "")
            {

                var q = from a in i
                        where a.naim_mu == comboBox2.Text
                        select a;
                i = q;
            }
            if (comboBox3.Text != "")
            {

                var q = from a in i
                        where a.fio_kl == comboBox3.Text
                        select a;
                i = q;
            }
            if (comboBox4.Text != "")
            {

                var q = from a in i
                        where a.fio_sotr == comboBox4.Text
                        select a;
                i = q;
            }
            if (dateTimePicker1.Checked)
            {
                var q = from a in i
                        where a.date_p >= Convert.ToDateTime
                        (dateTimePicker1.Value)
                        select a;
                i = q;
            }

            if (dateTimePicker3.Checked)
            {
                var q = from a in i
                        where a.date_p <= Convert.ToDateTime
                        (dateTimePicker3.Value)
                        select a;
                i = q;
            }
            if (dateTimePicker2.Checked)
            {
                var q = from a in i
                        where a.date_v >= Convert.ToDateTime
                        (dateTimePicker2.Value)
                        select a;
                i = q;
            }

            if (dateTimePicker4.Checked)
            {
                var q = from a in i
                        where a.date_v <= Convert.ToDateTime
                        (dateTimePicker4.Value)
                        select a;
                i = q;
            }
            if (checkBox1.Checked)
            {
                var q = from a in i
                        where a.oplata == true                        
                        select a;
                i = q;
            }
            dataGridView1.DataSource = i;   
        }

        //показать все заказы
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
            dateTimePicker1.Checked = false;
            dateTimePicker2.Checked = false;
            dateTimePicker3.Checked = false;
            dateTimePicker4.Checked = false;
            checkBox1.Checked = false;
            ViewData();
        }

        //показать поступившие заказы
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            var q = from a in db.View_4
                    where a.naim_sz == "Поступившие"                    
                    select a;
            dataGridView1.DataSource = q; 
        }

        //показать заказы ожидающие зачастей
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            var q = from a in db.View_4
                    where a.naim_sz == "Ожидают з/ч"
                    select a;
            dataGridView1.DataSource = q; 
        }

        //показать заказы в работе
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            var q = from a in db.View_4
                    where a.naim_sz == "В ремонте"
                    select a;
            dataGridView1.DataSource = q; 
        }

        //подказать готовые заказы
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            var q = from a in db.View_4
                    where a.naim_sz == "Готовые"
                    select a;
            dataGridView1.DataSource = q; 
        }

        //нажатие кнопки добавить заказ
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Zak form;
            form = Zak.GetInstance();
            form.Show();
        }


        //нажатие кнопки изменить заказ
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Zak form;
            form = Zak.GetInstance();
            form.Show();
            form.Izm(cellval);
        }

        //нажатие кнопки удалить заказ
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы действительно хотите удалить заказ?",
                    "Удаление записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Zakaz
                         where a.id_zak == Convert.ToInt32(cellval)
                         select a).SingleOrDefault();
                try
                {
                    db.Zakaz.DeleteOnSubmit(q);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewData();
            }
        }

        //нажатие кнопки экспотр в Excel
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        //нажатие кнопки добавить деталь
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DD form;
            form = DD.GetInstance();
            form.Show();
            form.DobD(Convert.ToInt32(cellval));
        }

        //нажатие кнопки удалить деталь
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        //нажатие кнопки добавить работу
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DU form;
            form = DU.GetInstance();
            form.Show();
            form.DobD(Convert.ToInt32(cellval));
        }

        //нажатие кнопки удалить работу
        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        //форматирование сетки 1
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                /*var q = (from a in db.View_4
                         where a.id_zak == Convert.ToInt32(e.Value)
                         select a).SingleOrDefault();
                if (q.naim_sz == "Поступившие")
                {
                    e.CellStyle.BackColor = Color.LightBlue;                    
                }
                else if (q.naim_sz == "Ожидают з/ч")
                {
                    e.CellStyle.BackColor = Color.LightPink;
                }
                else if (q.naim_sz == "В ремонте")
                {
                    e.CellStyle.BackColor = Color.LightYellow;
                }
                else if (q.naim_sz == "Готовые")
                {
                    e.CellStyle.BackColor = Color.LimeGreen;
                }
                //e.Value = e.RowIndex + 1;
                ViewSum();*/
            }
        }

        //событие при изменении выбора ячейки сетки 1 
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                cellval = Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
                GetDet();
                GetRab();  
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        //подсчет общей суммы
        public void ViewSum()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                sum = sum + Convert.ToDouble(dataGridView1.Rows[i].Cells[11].Value);
            }
            toolStripStatusLabel2.Text = "Общая сумма по заказам: " + sum + " руб.";
            sum = 0;
        }

        //экспорт заказов в Excel
        private void ExportToExcel()
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
            //создание книги
            ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);
            //создание таблицы
            ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                ExcelApp.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    ExcelApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
                }
            }
            //вызов Excel
            ExcelApp.Visible = true;
            ExcelApp.UserControl = true;
        }

        //построение сетки 2
        public void GetDet()
        {
            var q = from a in db.View_2
                    where a.id_zak == Convert.ToInt32(cellval)
                    select a;            
            dataGridView2.DataSource = q;
        }

        //построение сетки 3
        public void GetRab()
        {
            var q = from a in db.View_3
                    where a.id_zak == Convert.ToInt32(cellval)
                    select a;
            dataGridView3.DataSource = q;
        }

        //форматирование сетки 2
        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
        }

        //событие при изменении выбора ячейки сетки 2 
        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            cellval1 = Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
        }

        //форматирование сетки 3
        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
        }

        //событие при изменении выбора ячейки сетки 3
        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            cellval2 = Convert.ToString(dataGridView1[0, dataGridView1.CurrentRow.Index].Value);
        }
    }
}
