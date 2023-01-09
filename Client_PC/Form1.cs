using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_PC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Count = 4;

            string Password = "537129";
            for (int i = 1; i < Count; i++)
            {
                label_Protect_Password.Text = "Счётчик пароля:" + i;

                if (textBox_Password.Text == Password)
                {
                    textBox_Password.BackColor = Color.Green;
                    textBox_Password.Visible = true;
                    textBox_Password.Text = "Пароль верный!!!";
                    this.Hide();
                    Form1 form1 = new Form1();
                    Form2 form2 = new Form2();
                    form2.ShowDialog();

                }
                else
                {
                    if (i < Count)
                    {
                       textBox_Password.Clear();

                    }
                    else if (i == Count)
                    {

                        Form1 form1 = new Form1();
                        label_Protect_Password.Visible = true;
                        label_Protect_Password.Text = "Вы ввели неправильный пароль " + Count + " раза!\nДоступ запрещён!";
                        form1.Size = new Size(421, 187);
                        button1.Location = new System.Drawing.Point(115, 86);
                        textBox_Password.Clear();
                        button1.Visible = false;
                        textBox_Password.ReadOnly = true;

                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }
    }
}
