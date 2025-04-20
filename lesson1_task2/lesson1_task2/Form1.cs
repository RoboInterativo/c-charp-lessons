namespace lesson1_task2
{
    public partial class Form1 : Form
    {
        private void ShowTimer(object vObject, EventArgs e)
        {          //останавливаем таймер
            timer1.Stop();
            button2.Enabled = false;
            MessageBox.Show("Таймер отработал!", "Таймер");
        }
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            //определяем обработчик события для таймера
            timer1.Tick += new EventHandler(ShowTimer);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //проверяем введенное количество секунд           
            //для таймера
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Количество секунд    должно быть больше 0!");
                return;
            }
            //разрешаем прервать таймер
            button2.Enabled = true;
            //интервал задается в милисекундах,            
            //поэтому секунды умножаем на 1000          
            //и задаем интервал таймера
            timer1.Interval = Decimal.ToInt32(numericUpDown1.Value) * 1000;
            //запуск таймера
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //останавливаем таймер
            timer1.Stop();         
            MessageBox.Show("Таймер не успел отработать!", "Таймер");      
            button2.Enabled = false; 
        }
    }
}
