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
            // Настраиваем форму
            this.Text = "Черепашья графика";
            this.ClientSize = new Size(800, 600);
            this.DoubleBuffered = true;

            // Инициализируем черепаху
            Turtle.Init(this);

            // Создаём кнопку для рисования
            var drawButton = new Button
            {
                Text = "Нарисовать фигуру",
                Location = new Point(120, 20),
                Size = new Size(120, 30)
            };
            drawButton.Click += DrawButton_Click;
            this.Controls.Add(drawButton);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            // Сбрасываем состояние черепахи
            Turtle.Reset();
            Turtle.PenColor = Color.Blue;
            Turtle.Delay = 100; // Задержка для анимации (в миллисекундах)

            // Рисуем простую фигуру: звезду
            for (int i = 0; i < 5; i++)
            {
                Turtle.Forward(100);
                Turtle.Rotate(144); // Угол для звезды (720/5 = 144)
            }
        }
    }
}