namespace lesson1_task2
{
    public partial class Form1 : Form
    {
        private void ShowTimer(object vObject, EventArgs e)
        {          //������������� ������
            timer1.Stop();
            button2.Enabled = false;
            MessageBox.Show("������ ���������!", "������");
        }
        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
            //���������� ���������� ������� ��� �������
            timer1.Tick += new EventHandler(ShowTimer);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //��������� ��������� ���������� ������           
            //��� �������
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("���������� ������    ������ ���� ������ 0!");
                return;
            }
            //��������� �������� ������
            button2.Enabled = true;
            //�������� �������� � ������������,            
            //������� ������� �������� �� 1000          
            //� ������ �������� �������
            timer1.Interval = Decimal.ToInt32(numericUpDown1.Value) * 1000;
            //������ �������
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //������������� ������
            timer1.Stop();         
            MessageBox.Show("������ �� ����� ����������!", "������");      
            button2.Enabled = false; 
        }
    }
}
