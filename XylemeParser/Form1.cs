using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LessonParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(trackSplitProgress);
            // Start the BackgroundWorker.
            backgroundWorker1.RunWorkerAsync();
        }

        private void CourseAcronymInput_TextChanged(object sender, EventArgs e)
        {
            Program.LayoutXML.CourseAcronym = CourseAcronymInput.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open PowerPoint Document",
                Filter = "Powerpoint Document|*.pptx;*.ppt;*.PPTX;*.PPT",
                InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString()
            };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    folderInput.Text = dialog.FileName;

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //textOutput.Text = Program.filesLengthList;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            //backgroundWorker1.DoWork += new DoWorkEventHandler(trackSplitProgress);
            Program.LayoutXML.CourseAcronym = CourseAcronymInput.Text;
            Program.LayoutXML.vNumber = vNumber.Text;
            Program.LayoutXML.moduleNumber = moduleNumber.Text;
            Program.LayoutXML.lessonNumber = lessonNumber.Text;
            
        }

        private void SetText(RichTextBox RichTextBox, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            if (RichTextBox.InvokeRequired)
            {
                RichTextBox.Invoke(new MethodInvoker(delegate {
                    // load the control with the appropriate data
                    RichTextBox.Text = text;
                }));
            }
            else
            {
                RichTextBox.Text = text;
            }
        }

        private void trackSplitProgress(object sender, DoWorkEventArgs e)
        {
            //Program.CheckFile
            if (CourseAcronymInput.Text.Length > 0 && CourseCodeInput.Text.Length > 0 && folderInput.Text.Length > 0)
            {
                try
                {
                    // run program
                    Program.RunSplit(CourseAcronymInput.Text, CourseCodeInput.Text, folderInput.Text);
                    //

                    if (Program.hasErrors)
                    {
                        // some error...
                        System.Windows.Forms.MessageBox.Show("The program has generated errors, check error log!");
                        //System.Windows.Forms.MessageBox.Show("Error: " + Program.ErrorList);
                        //ErrorOutput.Text = Program.ErrorList;
                        SetText(this.ErrorOutput, Program.ErrorList);
                    }
                    else
                    {
                        // complete
                        System.Windows.Forms.MessageBox.Show("Done! Check the files list.");
                        //this.richTextBox1.Text = Program.filesLengthList;
                        SetText(this.richTextBox1, Program.filesLengthList);
                    }

                }
                catch (Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show("Error: " + ex);
                    //ErrorOutput.Text = "Error: " + ex;
                    System.Windows.Forms.MessageBox.Show("Program has generated errors!");
                    SetText(this.ErrorOutput, ex.ToString());
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please fill out all fields!");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //trackSplitProgress(object sender, DoWorkEventArgs e);
        }

        private void backgroundWorker1_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar1.Value = e.ProgressPercentage;
        }

        private void renameDialog_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) System.Windows.Forms.MessageBox.Show(file);
        }

        private void vNumber_TextChanged(object sender, EventArgs e)
        {
            Program.LayoutXML.vNumber = vNumber.Text;
        }

        private void moduleNumber_TextChanged(object sender, EventArgs e)
        {
            Program.LayoutXML.moduleNumber = moduleNumber.Text;
        }

        private void lessonNumber_TextChanged(object sender, EventArgs e)
        {
            Program.LayoutXML.lessonNumber = lessonNumber.Text;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*var XmlDoc = new XmlDoc();
            XmlDoc.Start();*/

            
            if (textBox1.Text.Length > 0 && textBox2.Text.Length  > 0)
            {
                //var XParse = new XParseLesson();
                var XParse = new XParseLab();
                //var XParse = new XParseChallenge();
                XParse.SourceDirectory = @textBox2.Text + @"\";
                XParse.OpenDocument(@textBox1.Text);
                //Console.WriteLine(XParse.SourceDirectory);

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select a file and folder!");
            }

            //var XParse = new XParseChallenge();
            //XParse.OpenDocument(@"C:\Users\Niel\Source\Repos\projectx\SplitterReloaded\bin\Debug\challenge.docx");

            //var XParse = new XParseLab();
            //XParse.OpenDocument(@"C:\Users\Niel\Source\Repos\projectx\SplitterReloaded\bin\Debug\ANDMB_LAB01_Introduction to Hadoop.docx");
        }

        private void reverseObject_CheckedChanged(object sender, EventArgs e)
        {
            // reverse objects checkbox
            if (!reverseObject.Checked)
            {
                Program.reverseObjects = false;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open Word Document",
                Filter = "Word Document|*.docx;*.doc;*.DOCX;*.DOC"
            };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dialog.FileName;

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog { };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dialog.SelectedPath;

                }
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Program.SlideScale = 1;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            Program.SlideScale = 2;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Program.SlideScale = 3;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open Word Document",
                Filter = "Word Document|*.docx;*.doc;*.DOCX;*.DOC"
            };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = dialog.FileName;

                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                var XParse = new XParseLessonV4();
                //var XParse = new XParseLab();
                //var XParse = new XParseChallenge();
                XParse.SourceDirectory = @textBox4.Text + @"\";
                XParse.OpenDocument(@textBox3.Text);
                //Console.WriteLine(XParse.SourceDirectory);

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select a file and folder!");
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog { SelectedPath = Environment.CurrentDirectory };
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox4.Text = dialog.SelectedPath;

                }
            }
        }
    }
}
