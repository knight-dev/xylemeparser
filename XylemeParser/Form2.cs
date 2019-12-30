using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LessonParser
{
    public partial class Form2 : Form
    {
        public string RenamedFile {
            get
            {
                return richTextBox1.Text;
            }

            set
            {
                richTextBox1.Text = value;
            }
        }

        string renamedFile;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // show length once dialog opens
            this.textBox1.Text = this.richTextBox1.Text.Length.ToString();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // show length in realtime
            this.textBox1.Text = this.richTextBox1.Text.Length.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // colour change to make things painfully obvious...
            if (this.richTextBox1.Text.Length < 100)
            {
                this.textBox1.ForeColor = System.Drawing.Color.Chartreuse;
            }
            else
            {
                this.textBox1.ForeColor = System.Drawing.Color.Red;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // one final check...
            if(this.richTextBox1.Text.Length < 100)
            {
                Program.renamedFile = this.richTextBox1.Text;
                Program.renameDialog.Close();
            }
            else {
                System.Windows.Forms.MessageBox.Show("File name still exceeds 99 chars!");
            }
        }
    }
}
