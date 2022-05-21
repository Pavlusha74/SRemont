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
    public partial class DD : Form
    {
        private static DD instance = null;
        private int nz;

        public DD()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static DD GetInstance()
        {
            if (instance == null)
                instance = new DD();
            return instance;
        }

        //нажатие кнопки ок
        private void button1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {
                ZakDet item = new ZakDet();
                item.id_zak = nz;
                item.id_det = Convert.ToInt32(comboBox2.SelectedValue);
                item.col = (int)numericUpDown1.Value;
                db.ZakDet.InsertOnSubmit(item);
                db.SubmitChanges();
                Form1.SelfRef.GetDet();
                Form1.SelfRef.ViewData();
                this.Close();                

            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }

        //нажатие кнопки отмена
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //закрытие формы
        private void DD_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }

        //закгрузка формы
        private void DD_Load(object sender, EventArgs e)
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
            this.comboBox2.DataSource = from a in db.Detal
                                        select new
                                        {
                                            pID = a.id_det,
                                            pName = a.naim_det
                                        };
            this.comboBox2.DisplayMember = "pName";
            this.comboBox2.ValueMember = "pID";
            this.comboBox2.Text = "";
        }

        //выбор товара соответствующей группы
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboModel();
        }

        //выбор товара соответствующей группы
        private void comboModel()
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            this.comboBox2.DataSource = from m in db.Detal
                                        where
                                        (m.id_gdet ==
                                        Convert.ToInt32(comboBox1.SelectedValue))
                                        select new
                                        {
                                            mID = m.id_det,
                                            mName = m.naim_det
                                        };
            this.comboBox2.DisplayMember = "mName";
            this.comboBox2.ValueMember = "mID";
            this.comboBox2.Text = "";
        }

        //получение номера заказа
        public void DobD(int n)
        {
            nz = n;
        }
    }
}
