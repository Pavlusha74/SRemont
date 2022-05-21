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
    public partial class Vhod : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        static public string status;
        static public string userName;
        static public string dostup;

        public Vhod()
        {
            InitializeComponent();
            textBox1.Text = "1";
            textBox2.Text = "1";
        }

        //проверка логина и пароля
        private bool Verifying(string log, string pas)
        {
            bool param = false;
            var q = (from a in db.Sotrudnik
                     where a.login == log
                     select a).SingleOrDefault();
            if (q != null)
            {
                if (q.pass == pas)
                {
                    param = true;
                    userName = q.fio_sotr;
                    dostup = q.dost;
                    return param;
                }
                else return param;
            }
            else return param;
        }

        //нажатие кнопки ок
        private void button1_Click(object sender, EventArgs e)
        {
            if (Verifying(textBox1.Text, textBox2.Text))
            {
                status = "running";
                this.Close();
            }
            else
                MessageBox.Show("Неверный логин или пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);   
        }

        //нажатие кнопки отмена
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
