using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

namespace lesson1_task4_ankena
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // �������� � ������ �������� ������
           string strMessage = "���:" + lastname.Text + " " 
                + firstname.Text + " " 
                + lastname.Text
                + "\n"; 
            strMessage += "����� ����������:" 
                + country.Text 
                + ", �." 
                + city.Text + "\n"; 
            strMessage += "�������: " 
                + phone.Text + "\n"; 
            strMessage += "���� ��������: " 
                + dateTimePicker1.Value.ToLongDateString() + "\n"; 
           if (male.Checked == true) { 
                strMessage += "���: �������"; 
            } else { 
                strMessage += "���: �������"; 
            }
            //������� �������� ������ � ���������� ����
            MessageBox.Show(strMessage, "�������� ������");
        }
    }
}
