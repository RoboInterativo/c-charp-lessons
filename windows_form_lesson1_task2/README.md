# Создание приложения таймера
![1](media/lessson1_task2_002.PNG)
Выберете дизайнер форм, и кликните на панель элементов
В приложении 2 кнопки, таймер, метка для вывода надписи, и выбор цифры

Вот названия элементов
* numericUpDown1
* timer1
* button1
* button2
* label1тк

Через обозреватель проекта и значок событий, можно создать обработчики событий
кнопок.
Остальное можно определить непосредственно в Form1.cs

```csharp
//Добавляем фунцию события таймера вручную
private void ShowTimer(object vObject, EventArgs e)
{          //останавливаем таймер
    timer1.Stop();
    button2.Enabled = false;
    MessageBox.Show("Таймер отработал!", "Таймер");
}
public Form1()
{
    InitializeComponent();
    //Добавляем после инициализации компонента формы

    button2.Enabled = false;
    //определяем обработчик события для таймера
    timer1.Tick += new EventHandler(ShowTimer);
}
```
