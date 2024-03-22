using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Pipes;

namespace vattmetr_test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static async Task SetWattage(double watt)
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "woltmetrPipe", PipeDirection.Out))
            {
                await pipeClient.ConnectAsync(); // Подключаемся к именованному каналу

                // Отправляем данные о напряжении
                string wattageData = watt.ToString(); // Пример значения напряжения
                byte[] buffer = Encoding.UTF8.GetBytes(wattageData);
                pipeClient.Write(buffer, 0, buffer.Length);
            }
        }

        private void TextBox_WattageInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ числом
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true; // Если не число, отменяем ввод
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SetWattage(Convert.ToDouble(wattageText.Text));
        }
    }
}
