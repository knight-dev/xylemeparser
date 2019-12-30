namespace LessonParser
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ErrorOutput = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.reverseObject = new System.Windows.Forms.CheckBox();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.vNumber = new System.Windows.Forms.RichTextBox();
            this.moduleNumber = new System.Windows.Forms.RichTextBox();
            this.lessonNumber = new System.Windows.Forms.RichTextBox();
            this.CourseAcronymInput = new System.Windows.Forms.TextBox();
            this.CourseCodeInput = new System.Windows.Forms.TextBox();
            this.folderInput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tempV4 = new System.Windows.Forms.TabPage();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tempV4.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.AllowDrop = true;
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.vNumber);
            this.tabPage1.Controls.Add(this.moduleNumber);
            this.tabPage1.Controls.Add(this.lessonNumber);
            this.tabPage1.Controls.Add(this.CourseAcronymInput);
            this.tabPage1.Controls.Add(this.CourseCodeInput);
            this.tabPage1.Controls.Add(this.folderInput);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(956, 485);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File Export";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.ErrorOutput);
            this.groupBox6.Location = new System.Drawing.Point(428, 331);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(524, 155);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Error Log:";
            // 
            // ErrorOutput
            // 
            this.ErrorOutput.BackColor = System.Drawing.SystemColors.WindowText;
            this.ErrorOutput.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorOutput.ForeColor = System.Drawing.Color.Red;
            this.ErrorOutput.Location = new System.Drawing.Point(5, 24);
            this.ErrorOutput.Name = "ErrorOutput";
            this.ErrorOutput.Size = new System.Drawing.Size(515, 126);
            this.ErrorOutput.TabIndex = 1;
            this.ErrorOutput.Text = "";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.Location = new System.Drawing.Point(428, 188);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(524, 137);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Exported Files:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Chartreuse;
            this.richTextBox1.Location = new System.Drawing.Point(5, 24);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(516, 108);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panel1);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.panel3);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.panel2);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(428, 20);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(512, 163);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Config Options";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton4);
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Location = new System.Drawing.Point(167, 54);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 104);
            this.panel1.TabIndex = 14;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton4.Location = new System.Drawing.Point(77, 10);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(55, 19);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "None";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3.Location = new System.Drawing.Point(16, 37);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(120, 19);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Chop Footer (4:3)";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(16, 62);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(127, 19);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Chop Footer (16:9)";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(16, 10);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(64, 19);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Default";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(188, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "Image Cropping:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.reverseObject);
            this.panel3.Controls.Add(this.radioButton10);
            this.panel3.Controls.Add(this.radioButton9);
            this.panel3.Controls.Add(this.radioButton8);
            this.panel3.Location = new System.Drawing.Point(338, 54);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(170, 104);
            this.panel3.TabIndex = 12;
            // 
            // reverseObject
            // 
            this.reverseObject.AutoSize = true;
            this.reverseObject.Checked = true;
            this.reverseObject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reverseObject.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reverseObject.Location = new System.Drawing.Point(11, 82);
            this.reverseObject.Margin = new System.Windows.Forms.Padding(2);
            this.reverseObject.Name = "reverseObject";
            this.reverseObject.Size = new System.Drawing.Size(146, 19);
            this.reverseObject.TabIndex = 3;
            this.reverseObject.Text = "Reverse Slide Objects";
            this.reverseObject.UseVisualStyleBackColor = true;
            this.reverseObject.CheckedChanged += new System.EventHandler(this.reverseObject_CheckedChanged);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton10.Location = new System.Drawing.Point(11, 58);
            this.radioButton10.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(109, 19);
            this.radioButton10.TabIndex = 2;
            this.radioButton10.Text = "No XML Output";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton9.Location = new System.Drawing.Point(11, 34);
            this.radioButton9.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(112, 19);
            this.radioButton9.TabIndex = 1;
            this.radioButton9.Text = "Single XML File";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Checked = true;
            this.radioButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton8.Location = new System.Drawing.Point(10, 10);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(127, 19);
            this.radioButton8.TabIndex = 0;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Multiple XML Files";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(344, 28);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "XML Export Options:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton7);
            this.panel2.Controls.Add(this.radioButton6);
            this.panel2.Controls.Add(this.radioButton5);
            this.panel2.Location = new System.Drawing.Point(27, 54);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(118, 104);
            this.panel2.TabIndex = 1;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton7.Location = new System.Drawing.Point(10, 67);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(84, 19);
            this.radioButton7.TabIndex = 2;
            this.radioButton7.Text = "3x Original";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton7_CheckedChanged);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton6.Location = new System.Drawing.Point(10, 38);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(84, 19);
            this.radioButton6.TabIndex = 1;
            this.radioButton6.Text = "2x Original";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Checked = true;
            this.radioButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton5.Location = new System.Drawing.Point(10, 10);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(84, 19);
            this.radioButton5.TabIndex = 0;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "1x Original";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(26, 29);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "Slide Image Scaling:";
            // 
            // vNumber
            // 
            this.vNumber.Location = new System.Drawing.Point(272, 123);
            this.vNumber.Name = "vNumber";
            this.vNumber.Size = new System.Drawing.Size(36, 33);
            this.vNumber.TabIndex = 10;
            this.vNumber.Text = "v2";
            this.vNumber.TextChanged += new System.EventHandler(this.vNumber_TextChanged);
            // 
            // moduleNumber
            // 
            this.moduleNumber.Location = new System.Drawing.Point(313, 123);
            this.moduleNumber.Name = "moduleNumber";
            this.moduleNumber.Size = new System.Drawing.Size(46, 33);
            this.moduleNumber.TabIndex = 8;
            this.moduleNumber.Text = "M01";
            this.moduleNumber.TextChanged += new System.EventHandler(this.moduleNumber_TextChanged);
            // 
            // lessonNumber
            // 
            this.lessonNumber.Location = new System.Drawing.Point(365, 123);
            this.lessonNumber.Name = "lessonNumber";
            this.lessonNumber.Size = new System.Drawing.Size(44, 33);
            this.lessonNumber.TabIndex = 9;
            this.lessonNumber.Text = "05";
            this.lessonNumber.TextChanged += new System.EventHandler(this.lessonNumber_TextChanged);
            // 
            // CourseAcronymInput
            // 
            this.CourseAcronymInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CourseAcronymInput.Location = new System.Drawing.Point(272, 20);
            this.CourseAcronymInput.Multiline = true;
            this.CourseAcronymInput.Name = "CourseAcronymInput";
            this.CourseAcronymInput.Size = new System.Drawing.Size(137, 27);
            this.CourseAcronymInput.TabIndex = 1;
            this.CourseAcronymInput.Text = "INFND";
            this.CourseAcronymInput.TextChanged += new System.EventHandler(this.CourseAcronymInput_TextChanged);
            // 
            // CourseCodeInput
            // 
            this.CourseCodeInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CourseCodeInput.Location = new System.Drawing.Point(272, 81);
            this.CourseCodeInput.Multiline = true;
            this.CourseCodeInput.Name = "CourseCodeInput";
            this.CourseCodeInput.Size = new System.Drawing.Size(137, 27);
            this.CourseCodeInput.TabIndex = 4;
            this.CourseCodeInput.Text = "1-0-0";
            // 
            // folderInput
            // 
            this.folderInput.Location = new System.Drawing.Point(22, 186);
            this.folderInput.Name = "folderInput";
            this.folderInput.ReadOnly = true;
            this.folderInput.Size = new System.Drawing.Size(219, 26);
            this.folderInput.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(65, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "Progress...";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(107, 362);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(201, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run Publisher";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(68, 281);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(291, 52);
            this.progressBar1.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(64, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "Selected File:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(272, 171);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 48);
            this.button2.TabIndex = 6;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Course Code";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Course Acronym";
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tempV4);
            this.tabControl1.ItemSize = new System.Drawing.Size(65, 25);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(964, 518);
            this.tabControl1.TabIndex = 7;
            this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(956, 485);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Convert Docx Lab";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(201, 23);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(148, 20);
            this.textBox2.TabIndex = 4;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(14, 21);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(181, 23);
            this.button5.TabIndex = 3;
            this.button5.Text = "Choose Source Directory";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(14, 67);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "Word File";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(95, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(139, 95);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 19);
            this.button3.TabIndex = 0;
            this.button3.Text = "Convert";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // tempV4
            // 
            this.tempV4.Controls.Add(this.textBox4);
            this.tempV4.Controls.Add(this.button8);
            this.tempV4.Controls.Add(this.button7);
            this.tempV4.Controls.Add(this.button6);
            this.tempV4.Controls.Add(this.textBox3);
            this.tempV4.Location = new System.Drawing.Point(4, 29);
            this.tempV4.Name = "tempV4";
            this.tempV4.Padding = new System.Windows.Forms.Padding(3);
            this.tempV4.Size = new System.Drawing.Size(956, 485);
            this.tempV4.TabIndex = 2;
            this.tempV4.Text = "Lesson Template v4.1";
            this.tempV4.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(258, 25);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(126, 20);
            this.textBox4.TabIndex = 6;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(56, 22);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(181, 23);
            this.button8.TabIndex = 5;
            this.button8.Text = "Choose Save Directory";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(56, 147);
            this.button7.Margin = new System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(328, 77);
            this.button7.TabIndex = 4;
            this.button7.Text = "Convert";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(56, 66);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(132, 43);
            this.button6.TabIndex = 3;
            this.button6.Text = "Load Word File";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(217, 66);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(167, 43);
            this.textBox3.TabIndex = 2;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 541);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "PowerPoint Image Media Publisher © 2016 TT Labs";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tempV4.ResumeLayout(false);
            this.tempV4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox folderInput;
        private System.Windows.Forms.TextBox CourseCodeInput;
        public System.Windows.Forms.TextBox CourseAcronymInput;
        public System.Windows.Forms.RichTextBox lessonNumber;
        public System.Windows.Forms.RichTextBox moduleNumber;
        public System.Windows.Forms.RichTextBox vNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RichTextBox ErrorOutput;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox reverseObject;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TabPage tempV4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBox4;
        //public System.Windows.Forms.Form renameDialog;
    }
}