using System; //пространство для Windows.Forms
using System.Windows.Forms;
namespace HelloWinFormsWorld
{
     //пользовательский класс
  class MyForm : Form
    { //конструктор класса
      public MyForm(string caption)
        {
          //задаём заголовок формы
          Text = caption;
          //закрепляем обработчик события
          Click+=new EventHandler(ClickHandler);
        }
        public void ClickHandler(Object sender,EventArgs e)
          {
              MessageBox.Show("Click");     
          }
    }
  class FirstWinFormApp
    {
        public static void Main()
          {
         //создание объекта пользовательского класса формы
         //запуск обработки очереди сообщений
         //и отображение формы
         Application.Run(new MyForm("Hello, world!!!"));
          }
    }
}
