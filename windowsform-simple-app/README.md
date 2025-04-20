# Введение
Для того чтобы понять как работает c-sharp создадим программы без IDE в обычном
блокноте.

# Компиляция
Создадим файл `comp.cmd`
С таким содержимым, если нужно замените путь к компилятору

```powershell
SET csharp=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe
%csharp% /target:winexe main.cs
%csharp% /target:winexe main2.cs
%csharp% /target:winexe main3.cs
%csharp% /target:winexe main4.cs
```
# main.cs
Простое создание экземпляра класса windows form

# main2.cs
Вынесем класс MyForm и создадим конструктор класса

# main3.cs
создание объекта пользовательского класса формы

запуск обработки очереди сообщений

и отображение формы


# main4.cs
Добавим обработчик события Click
