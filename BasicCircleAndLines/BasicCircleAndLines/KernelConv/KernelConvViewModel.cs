using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace BasicCircleAndLines.KernelConv
{
    //TODO: There is a class from prisim 5 to inherit from to make change tracking easier?
    public class KernelConvViewModel
    {
        
        public ObservableCollection<KernelWindow> KernelBuffer { get; set; } = new ObservableCollection<KernelWindow>();

        public KernelConvViewModel()
        {
            Task.Run(AddStuffToBuffer);
        }
        
        private async Task AddStuffToBuffer()
        {
            var r = new Random();

            Func<byte> b = () => Convert.ToByte(r.Next(0, 255));
            Func<Brush> color = () => new SolidColorBrush(Color.FromRgb(b(), b(), b()));
            var MAX_ITERATION = 100;
            var iteration = 0;
            do
            {
                var frame = new KernelWindow
                {
                    Color = color(),
                    Height = r.NextDouble() * 100,
                    Width = r.NextDouble() * 100,
                    Rotation  = r.NextDouble() * 100
                };

                Application.Current.Dispatcher.InvokeAsync(() => KernelBuffer.Add(frame));
                

                await Task.Delay(800);

            } while (iteration++ < MAX_ITERATION);            
        }
    }

    public class KernelWindow
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double Rotation { get; set; }

        public Brush Color { get; set; }
    }
}
