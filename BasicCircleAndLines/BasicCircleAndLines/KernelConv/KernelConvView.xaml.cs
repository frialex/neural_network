using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Threading.Tasks.Dataflow;

namespace BasicCircleAndLines.KernelConv
{
    
    public partial class KernelConvView : Window
    {
        private List<KernelWindow> DataList { get; set; } = new List<KernelWindow>();

        public KernelConvView()
        {
            BatchBlock<KernelWindow> bb = new BatchBlock<KernelWindow>(4);

            InitializeComponent();

            

            var vm = new KernelConvViewModel(bb);
            this.DataContext = vm;

            OutputList.ItemsSource = DataList;
            
            Task.Run(() => WatchForNewData(bb));
        }

        private async Task WatchForNewData(BatchBlock<KernelWindow> bb)
        {
            do
            {
                var data = await bb.ReceiveAsync();

                Dispatcher.Invoke(() =>
                {
                    DataList.AddRange(data);
                    OutputList.Items.Refresh();
                    OutputList.ScrollIntoView(data.Last());
                });

                await Task.Delay(50);
                
            } while (true);
        }    
        
    }
}
