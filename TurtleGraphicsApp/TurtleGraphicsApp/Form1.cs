using System;
using System.Drawing;
using System.Windows.Forms;
using Nakov.TurtleGraphics;

namespace TurtleGraphicsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // ����������� �����
            this.Text = "��������� �������";
            this.ClientSize = new Size(800, 600);
            this.DoubleBuffered = true;

            // �������������� ��������
            Turtle.Init(this);

            // ������ ������ ��� ���������
            var drawButton = new Button
            {
                Text = "���������� ������",
                Location = new Point(120, 20),
                Size = new Size(120, 30)
            };
            drawButton.Click += DrawButton_Click;
            this.Controls.Add(drawButton);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            // ���������� ��������� ��������
            Turtle.Reset();
            Turtle.PenColor = Color.Blue;
            Turtle.Delay = 100; // �������� ��� �������� (� �������������)

            // ������ ������� ������: ������
            for (int i = 0; i < 5; i++)
            {
                Turtle.Forward(100);
                Turtle.Rotate(144); // ���� ��� ������ (720/5 = 144)
            }
        }
    }
}