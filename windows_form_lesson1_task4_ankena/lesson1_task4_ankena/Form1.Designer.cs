namespace lesson1_task4_ankena
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            female = new RadioButton();
            male = new RadioButton();
            label7 = new Label();
            dateTimePicker1 = new DateTimePicker();
            phone = new TextBox();
            city = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            country = new TextBox();
            secondname = new TextBox();
            firstname = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lastname = new TextBox();
            button1 = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(female);
            groupBox1.Controls.Add(male);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(dateTimePicker1);
            groupBox1.Controls.Add(phone);
            groupBox1.Controls.Add(city);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(country);
            groupBox1.Controls.Add(secondname);
            groupBox1.Controls.Add(firstname);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(lastname);
            groupBox1.Location = new Point(36, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(489, 426);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Анкета";
            // 
            // female
            // 
            female.AutoSize = true;
            female.Location = new Point(250, 338);
            female.Name = "female";
            female.Size = new Size(109, 29);
            female.TabIndex = 15;
            female.Text = "Женский";
            female.UseVisualStyleBackColor = true;
            // 
            // male
            // 
            male.AutoSize = true;
            male.Checked = true;
            male.Location = new Point(75, 338);
            male.Name = "male";
            male.Size = new Size(113, 29);
            male.TabIndex = 14;
            male.TabStop = true;
            male.Text = "Мужской";
            male.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(15, 295);
            label7.Name = "label7";
            label7.Size = new Size(137, 25);
            label7.TabIndex = 13;
            label7.Text = "Дата рождения";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(158, 289);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(300, 31);
            dateTimePicker1.TabIndex = 12;
            // 
            // phone
            // 
            phone.Location = new Point(109, 239);
            phone.Name = "phone";
            phone.Size = new Size(150, 31);
            phone.TabIndex = 11;
            // 
            // city
            // 
            city.Location = new Point(109, 202);
            city.Name = "city";
            city.Size = new Size(150, 31);
            city.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(27, 245);
            label4.Name = "label4";
            label4.Size = new Size(81, 25);
            label4.TabIndex = 9;
            label4.Text = "Телефон";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 205);
            label5.Name = "label5";
            label5.Size = new Size(63, 25);
            label5.TabIndex = 8;
            label5.Text = "Город";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(27, 168);
            label6.Name = "label6";
            label6.Size = new Size(69, 25);
            label6.TabIndex = 7;
            label6.Text = "Страна";
            // 
            // country
            // 
            country.Location = new Point(109, 165);
            country.Name = "country";
            country.Size = new Size(150, 31);
            country.TabIndex = 6;
            // 
            // secondname
            // 
            secondname.Location = new Point(109, 97);
            secondname.Name = "secondname";
            secondname.Size = new Size(150, 31);
            secondname.TabIndex = 5;
            // 
            // firstname
            // 
            firstname.Location = new Point(109, 60);
            firstname.Name = "firstname";
            firstname.Size = new Size(150, 31);
            firstname.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 103);
            label3.Name = "label3";
            label3.Size = new Size(88, 25);
            label3.TabIndex = 3;
            label3.Text = "Отчество";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 66);
            label2.Name = "label2";
            label2.Size = new Size(47, 25);
            label2.TabIndex = 2;
            label2.Text = "Имя";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 26);
            label1.Name = "label1";
            label1.Size = new Size(85, 25);
            label1.TabIndex = 1;
            label1.Text = "Фамилия";
            // 
            // lastname
            // 
            lastname.Location = new Point(109, 23);
            lastname.Name = "lastname";
            lastname.Size = new Size(150, 31);
            lastname.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(62, 373);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 1;
            button1.Text = "Печать анкеты";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox phone;
        private TextBox city;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox country;
        private TextBox secondname;
        private TextBox firstname;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox lastname;
        private RadioButton female;
        private RadioButton male;
        private Label label7;
        private DateTimePicker dateTimePicker1;
        private Button button1;
    }
}
