using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Threading.Tasks.Dataflow;

namespace BasicCircleAndLines.KernelConv
{
    //TODO: There is a class from prisim 5 to inherit from to make change tracking easier?
    public class KernelConvViewModel
    {
        
        public ObservableCollection<KernelWindow> KernelBuffer { get; set; } = new ObservableCollection<KernelWindow>();

        private BatchBlock<KernelWindow> BetterBuffer { get; set; }

        public KernelConvViewModel(BatchBlock<KernelWindow> bb)
        {
            BetterBuffer = bb;
            Task.Run(AddStuffToBuffer);
        }
        
        private async Task AddStuffToBuffer()
        {
            var r = new Random();

            Func<byte> b = () => Convert.ToByte(r.Next(0, 255));


            var MAX_ITERATION = 10000;
            var iteration = 0;
            do
            {
                var color = Color.FromRgb(b(), b(), b());

                //Brush is a class that is going to be referenced from the thread running XAML code.
                //In order for that thread to use this brush object, it must have been created on that same thread.
                //And so that is why we call the dispatcher to create a SolidColorBrush
                Brush brush = null;
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var brushObjectInUIThread = new SolidColorBrush(color);
                    brush = brushObjectInUIThread;
                });

                var frame = new KernelWindow
                {
                    Color = brush,
                    Height = r.NextDouble() * 100,
                    Width = r.NextDouble() * 100,
                    Rotation  = r.NextDouble() * 100
                };

                BetterBuffer.Post(frame);

                //await Task.Delay(200);

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
