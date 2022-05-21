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
    public partial class Zak : Form
    {
        private string param, uname;
        private static Zak instance = null;
        DataClasses1DataContext db = new DataClasses1DataContext();

        public Zak()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static Zak GetInstance()
        {
            if (instance == null)
                instance = new Zak();
            return instance;
        }

        //свойство для вызова методов из другой формы
        public static Zak SelfRef
        {
            get;
            set;
        }


        //загрузка формы для изменения
        public void Izm(string str)
        {
            this.Text = "Изменение заявки № " + str;
            button1.Text = "Изменить";
            param = "izm";
            uname = str;
            var q = (from a in db.View_4
                     where a.id_zak == Convert.ToInt32(str)
                     select a).SingleOrDefault();
            textBox1.Text = q.ser;
            comboBox1.Text = q.naim_tu;
            comboBox2.Text = q.naim_mu;
            comboBox3.Text = q.fio_kl;
            comboBox4.Text = q.fio_sotr;
            comboBox5.Text = q.naim_sz;
            dateTimePicker1.Value = Convert.ToDateTime(q.date_p);            
            numericUpDown1.Value = (decimal)q.skidka;
            if (q.oplata == true) checkBox1.Checked = true;
            else checkBox1.Checked = false;
            var l = (from a in db.Zakaz
                     where a.id_zak == Convert.ToInt32(str)
                     select a).SingleOrDefault();
            richTextBox1.Text = l.neispr;
            richTextBox2.Text = l.kompl;
            richTextBox3.Text = l.prim;
            comboBox5.Enabled = true;
        }

        //добавление заявки
        private void AddZak()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {
                if ((comboBox3.Text != "") & (textBox1.Text != ""))
                {

                    Zakaz item = new Zakaz();
                    item.ser = textBox1.Text;
                    item.id_mu = Convert.ToInt32(comboBox2.SelectedValue);
                    item.id_kl = Convert.ToInt32(comboBox3.SelectedValue);
                    item.id_sotr = Convert.ToInt32(comboBox4.SelectedValue);
                    if (dateTimePicker2.Checked) item.date_v = dateTimePicker2.Value;
                    item.date_p = dateTimePicker1.Value;
                    item.id_sz = 1;
                    item.skidka = (int)numericUpDown1.Value;
                    if (checkBox1.Checked) item.oplata = true;
                    else item.oplata = false;
                    item.neispr = richTextBox1.Text;
                    item.kompl = richTextBox2.Text;
                    item.prim = richTextBox3.Text;
                    item.stoim_det = 0;
                    item.stoim_rab = 0;                    
                    db.Zakaz.InsertOnSubmit(item);
                    db.SubmitChanges();
                }
                else MessageBox.Show
                  ("Введите данные для добавления", "Ошибка");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.Close();
        }

        //изменение заявки
        private void IzmZak(string str)
        {
            DialogResult result = MessageBox.Show(
                "Изменить данные о заявке ?",
                    "Изменение записи", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                DataClasses1DataContext db = new DataClasses1DataContext();
                var q = (from a in db.Zakaz
                         where a.id_zak == Convert.ToInt32(str)
                         select a).SingleOrDefault();
                try
                {

                    if ((comboBox3.Text != "") & (textBox1.Text != ""))
                    {
                        q.ser = textBox1.Text;
                        q.id_mu = Convert.ToInt32(comboBox2.SelectedValue);
                        q.id_kl = Convert.ToInt32(comboBox3.SelectedValue);
                        q.id_sotr = Convert.ToInt32(comboBox4.SelectedValue);
                        if (dateTimePicker2.Checked) q.date_v = dateTimePicker2.Value;
                        q.date_p = dateTimePicker1.Value;
                        q.id_sz = Convert.ToInt32(comboBox5.SelectedValue);
                        q.skidka = (int)numericUpDown1.Value;
                        if (checkBox1.Checked) q.oplata = true;
                        else q.oplata = false;
                        q.neispr = richTextBox1.Text;
                        q.kompl = richTextBox2.Text;
                        q.prim = richTextBox3.Text;
                        db.SubmitChanges();
                    }
                    else MessageBox.Show("Введите данные для изменения", "Ошибка");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                this.Close();
            }
        }
        
        //загрузка формы
        private void Zak_Load(object sender, EventArgs e)
        {
            this.Text = "Новая заявка";
            button1.Text = "Добавить";
            param = "dob";
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
            this.comboBox2.DataSource = from a in db.ModelUstr
                                        select new
                                        {
                                            pID = a.id_mu,
                                            pName = a.id_tu
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
            this.comboBox5.DataSource = from a in db.StatZakaz
                                        select new
                                        {
                                            pID = a.id_sz,
                                            pName = a.naim_sz
                                        };
            this.comboBox5.DisplayMember = "pName";
            this.comboBox5.ValueMember = "pID";
            this.comboBox5.Text = "Поступившие";
            this.comboBox5.Enabled = false;            
            comboModel();
        }

        //закрытие формы
        private void Zak_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
            Form1.SelfRef.ViewData();
        }

        //изменение выбранного товара 
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboModel();
        }

        //выбор товара соответствующей группы
        private void comboModel()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            this.comboBox2.DataSource = from m in db.ModelUstr
                                        where
                                        (m.id_tu ==
                                        Convert.ToInt32(comboBox1.SelectedValue))
                                        select new
                                        {
                                            mID = m.id_mu,
                                            mName = m.naim_mu
                                        };
            this.comboBox2.DisplayMember = "mName";
            this.comboBox2.ValueMember = "mID";
            this.comboBox2.Text = "";
        }

        //нажатие кнопки ок
        private void button1_Click(object sender, EventArgs e)
        {
            if (param == "dob")
            {
                AddZak();
            }
            else if (param == "izm")
            {
                IzmZak(uname);
            }
            Form1.SelfRef.ViewData();
            this.Close();
        }

        //нажатие кнопки отмена
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
