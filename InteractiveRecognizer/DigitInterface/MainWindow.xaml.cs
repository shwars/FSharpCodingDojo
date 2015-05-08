using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace DigitInterface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer dt = new DispatcherTimer();

        DigitReco.Classifier Cl = new DigitReco.Classifier();

        public MainWindow()
        {
            InitializeComponent();
            InitGrid();
            dt.Interval = TimeSpan.FromSeconds(5);
            dt.Tick += Dt_Tick;
            dt.Start();
            // Demo();
        }

        private async void Demo()
        {
            var d = Cl.GetData();
            foreach (var x in d)
            {
                Result.Text = x.Item1.ToString();
                Draw(x.Item2);
                await Dispatcher.Yield();
                Thread.Sleep(1000);
            }
        }

        private void Draw(int[] x)
        {
            var k = 0;
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                    Rects[i, j].Fill = x[k++] > 10 ? FillBrush : EmptyBrush;
        }

        private void Clear()
        {
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                {
                    Check[i, j] = false;
                    Rects[i, j].Fill = EmptyBrush;
                }
            ResetTimer();
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            var z = new int[28 * 28];
            var k = 0;
            Result.Text = "*";
            for (int i = 0; i < 28; i++)
                for (int j = 0; j < 28; j++)
                    z[k++] = Check[i, j] ? 255 : 0;
            var x = Cl.Classify(z);
            Result.Text = x.ToString();
            Clear();
        }

        SolidColorBrush EmptyBrush = new SolidColorBrush(Colors.LightGray);
        SolidColorBrush FillBrush = new SolidColorBrush(Colors.Black);

        Rectangle[,] Rects = new Rectangle[28, 28];
        bool[,] Check = new bool[28, 28];

        private void InitGrid()
        {
            for (int i=0;i<28;i++)
            {
                var sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                for (int j=0;j<28;j++)
                {
                    var x = new Rectangle();
                    x.Height = x.Width = 10;
                    x.Fill = EmptyBrush;
                    x.Tag = i * 100 + j;
                    x.MouseEnter += X_MouseEnter;
                    Rects[i, j] = x;
                    Check[i, j] = false;
                    sp.Children.Add(x);
                }
                GridPanel.Children.Add(sp);
            }
        }

        private void ResetTimer()
        {
            dt.Stop();
            dt.Interval = TimeSpan.FromSeconds(5);
            dt.Start();
        }

        private void X_MouseEnter(object sender, MouseEventArgs e)
        {
            var z = sender as Rectangle;
            if (e.LeftButton == MouseButtonState.Pressed && z!=null)
            {
                var i = (int)z.Tag / 100;
                var j = (int)z.Tag % 100;
                Mark(i, j);
            }
            ResetTimer();
        }

        private void Mark(int i, int j)
        {
            for (int i1=-1;i1<=1;i1++)
                for (int j1=-1;j1<=1;j1++)
                    CheckIt(i+i1, j+j1);
        }

        private void CheckIt(int i, int j)
        {
            if (i > 0 && i < 28 && j > 0 && j < 28)
            {
                Check[i, j] = true;
                Rects[i,j].Fill = FillBrush;
            }
        }
    }
}
