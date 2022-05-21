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
    public partial class DU : Form
    {
        private static DU instance = null;
        private int nz;

        public DU()
        {
            InitializeComponent();
        }

        //запрет вызова второй копии окна
        public static DU GetInstance()
        {
            if (instance == null)
                instance = new DU();
            return instance;
        }

        //нажатие кнопки ок
        private void button1_Click(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();
            try
            {
                ZakRab item = new ZakRab();
                item.id_zak = nz;
                item.id_rab = Convert.ToInt32(comboBox1.SelectedValue);                
                db.ZakRab.InsertOnSubmit(item);
                db.SubmitChanges();
                Form1.SelfRef.GetRab();
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
        
        //получение номера заказа
        public void DobD(int n)
        {
            nz = n;
        }

        //закрытие формы
        private void DU_FormClosing(object sender, FormClosingEventArgs e)
        {
            instance = null;
        }

        //закгрузка формы
        private void DU_Load(object sender, EventArgs e)
        {
            DataClasses1DataContext db = new DataClasses1DataContext();

            this.comboBox1.DataSource = from a in db.Rabota
                                        select new
                                        {
                                            pID = a.id_rab,
                                            pName = a.naim_rab
                                        };
            this.comboBox1.DisplayMember = "pName";
            this.comboBox1.ValueMember = "pID";
            this.comboBox1.Text = "";            
        }
    }
}
