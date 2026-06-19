using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace WpfApp3D
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartRotation();
        }

        private void StartRotation()
        {
            // Находим наш объект по имени (x:Name="RotationAxis")
            AxisAngleRotation3D rotation = this.RotationAxis;

            // Создаем анимацию
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 360;
            animation.Duration = new Duration(TimeSpan.FromSeconds(8));
            animation.RepeatBehavior = RepeatBehavior.Forever;

            // Запускаем
            rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);
        }
    }
}