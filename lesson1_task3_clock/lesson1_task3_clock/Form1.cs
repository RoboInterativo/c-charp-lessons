namespace lesson1_task3_clock
{
    public partial class Form1 : Form
    {
        private void ShowTime(object vObj, EventArgs e)
        {       //�������������� � ������
            label1.Text = DateTime.Now.ToLongTimeString();  
            
        }
        public Form1()
        {

            InitializeComponent();
            //�������������� � ������
            label1.Text = DateTime.Now.ToLongTimeString();       
            //����������� �����������
            timer1.Tick += new EventHandler(ShowTime);       
            //��������� ��������� �������
            timer1.Interval = 500;
            timer1.Start();
        }
    }
}
