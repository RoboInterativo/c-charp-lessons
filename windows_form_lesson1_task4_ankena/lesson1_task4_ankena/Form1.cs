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
           // собираем в строку анкетные данные
           string strMessage = "ФИО:" + lastname.Text + " " 
                + firstname.Text + " " 
                + lastname.Text
                + "\n"; 
            strMessage += "Место проживания:" 
                + country.Text 
                + ", г." 
                + city.Text + "\n"; 
            strMessage += "Телефон: " 
                + phone.Text + "\n"; 
            strMessage += "Дата рождения: " 
                + dateTimePicker1.Value.ToLongDateString() + "\n"; 
           if (male.Checked == true) { 
                strMessage += "Пол: мужской"; 
            } else { 
                strMessage += "Пол: женский"; 
            }
            //выводим анкетные данные в диалоговое окно
            MessageBox.Show(strMessage, "Анкетные данные");
        }
    }
}
