namespace lesson1_task1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private String CoordinatesToString(MouseEventArgs e)
        {
            return "���������� ����: �=" + e.X.ToString() + "; +y=" + e.Y.ToString();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //����������� ������� ��������� ����       
            //� ��������� ����
            Text = CoordinatesToString(e);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //��������� ����� ������ ���� ����� ������������
            String message = "";      
            if (e.Button == MouseButtons.Right)      
                {        
                    message = "�� ������ ������ ������ ����.";      
                }      
            if (e.Button == MouseButtons.Left)      
                {         
                    message = "�� ������ ����� ������ ����.";     
                }      
            message += "\n" + CoordinatesToString(e);      
            //������� ��������� � ���������� ����
            String caption = "���� ����";      
            MessageBox.Show(message, caption, MessageBoxButtons.OK,    MessageBoxIcon.Information);
        }
    }
}
