# Введение
Для того чтобы понять как работает c-sharp создадим программы без IDE в обычном
блокноте.

# Компиляция
Создадим файл `comp.cmd`
С таким содержимиым, если нужно замените путь к компилятору

```powershell
SET csharp=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe
%csharp% /target:winexe main.cs
%csharp% /target:winexe main2.cs
%csharp% /target:winexe main3.cs
%csharp% /target:winexe main4.cs
```
