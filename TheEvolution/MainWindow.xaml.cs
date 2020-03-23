using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace TheEvolution
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected WriteableBitmap map;
        internal WorkPlace workPlace;
        public MainWindow()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            workPlace = new WorkPlace();
            workPlace.Start();
            new Thread(WorkPlaceUpdater).Start();
            /*image.Width = 1920;
            image.Height = 1080;*/
        }
        void WorkPlaceUpdater()
        {
            while(true)
            {
                workPlace.Update();
            }
        }
        public void SetEnvironment(Environment environment)
        {
            environment.window = this;
            map = new WriteableBitmap(environment.WORLD_WIDTH, environment.WORLD_HEIGHT, 96, 96, PixelFormats.Bgra32, null);
            this.Dispatcher.Invoke(() => {
                image.Source = map;
            });

        }
        public void drawPixel(int x, int y, byte r, byte g, byte b)
        {
            try
            {
                byte[] colorData = new byte[4] { b, g, r, 255 };
                Int32Rect rect = new Int32Rect(x, y, 1, 1);
                this.Dispatcher.Invoke(() =>
                {
                    map.WritePixels(rect, colorData, 4, 0);

                }, DispatcherPriority.Send);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            //Console.WriteLine("Drawing " + x + " " + y);
        }
    }
    public partial class Environment
    {
        public MainWindow window;
        public int WORLD_WIDTH { get; private set; }
        public int WORLD_HEIGHT { get; private set; }
        public Environment(int width, int height)
        {
            WORLD_WIDTH = width;
            WORLD_HEIGHT = height;
        }
        /*
         * Сразу поясню для тех, кто зашел слишком далеко, и у кого есть вопросы: да, я могу поставить это дело через MainWindow, но! Я делаю
         * сразу всё по-нормальному, ибо MainWindow не-пред-наз-на-чен для этого метода. Это просто нелогично. Взаимодействие должно идти
         * через отдельный класс, а не через ж*пу!
         * 
         * P.S. а еще не исключено, что этим буду пользоваться я сам, поэтому я сразу делаю так, чтобы было удобно, ибо я не мазохист :)
         * P.P.S. по всем структурно-рабочим и прочим вопросам ВК vitaly.megabyte, или то же самое в Телеграме.
         */
        public void drawPixel(int x, int y, byte r, byte g, byte b)
        {
            window.drawPixel(cycleX(x), cycleY(y), r, g, b);
        }
        public int cycleX(int x)
        {
            return (x + WORLD_WIDTH) % WORLD_WIDTH;
        }
        public int cycleY(int y)
        {
            return (y + WORLD_HEIGHT) % WORLD_HEIGHT;
        }
    }
}
