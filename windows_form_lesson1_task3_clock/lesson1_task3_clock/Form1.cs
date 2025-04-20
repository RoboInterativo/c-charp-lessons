namespace lesson1_task3_clock
{
    public partial class Form1 : Form
    {
        private void ShowTime(object vObj, EventArgs e)
        {       //преобразование к строке
            label1.Text = DateTime.Now.ToLongTimeString();  
            
        }
        public Form1()
        {

            InitializeComponent();
            //преобразование к строке
            label1.Text = DateTime.Now.ToLongTimeString();       
            //закрепление обработчика
            timer1.Tick += new EventHandler(ShowTime);       
            //установка интервала времени
            timer1.Interval = 500;
            timer1.Start();
        }
    }
}
