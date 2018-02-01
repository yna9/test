using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        private PrintDialog printDialog;

        public MainWindow()
        {
            InitializeComponent();

            printDialog = new PrintDialog();


            //ImageBrush myImageBrush = new ImageBrush(bitmap);

            //Canvas myCanvas = new Canvas();
            //myCanvas.Width = bitmap.PixelWidth;
            //myCanvas.Height = bitmap.PixelHeight;
            //myCanvas.Background = myImageBrush;

            //MainCanvas.Children.Add(myCanvas);



        }


        private void PrintImage(BitmapImage bitmap)
        {
            var area = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket).PageImageableArea;

            var ratio = bitmap.Width / bitmap.Height;
            var imageCanv = new Canvas();
            imageCanv.Width = area.ExtentWidth;
            imageCanv.Height = (imageCanv.Width / ratio);

            ImageBrush myImageBrush = new ImageBrush(bitmap);
            imageCanv.Background = myImageBrush;

            var canv = new Canvas()
            {
                Width = area.ExtentWidth + area.OriginWidth,
                Height = area.ExtentHeight + area.OriginHeight,
            };

            canv.Children.Add(imageCanv);
            Canvas.SetLeft(imageCanv, area.OriginWidth);
            Canvas.SetTop(imageCanv, area.OriginHeight);

            var page = new FixedPage()
            {
                Width = area.ExtentWidth + area.OriginWidth,
                Height = area.ExtentHeight + area.OriginHeight,
            };
            page.Children.Add(canv);

            var cont = new PageContent{ Child = page };

            var doc = new FixedDocument();
            doc.Pages.Add(cont);

            printDialog.PrintDocument(doc.DocumentPaginator, "Print");

        }

        private void ConfigureAndPrint(BitmapImage bitmap)
        {
            // 印刷ダイアログを表示して、プリンタ選択と印刷設定を行う。
            if (printDialog.ShowDialog() == true)
            {
                PrintImage(bitmap);
            }
        }

        private void Print(BitmapImage bitmap)
        {
            PrintImage(bitmap);
        }

        private BitmapImage LoadImage(string filepath)
        {
            var bitmap = new BitmapImage();
            var stream = File.OpenRead(filepath);
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            stream.Close();
            stream.Dispose();

            return bitmap;

        }

        private void PrintTest(object sender, RoutedEventArgs e)
        {
            var filepath = System.Environment.CurrentDirectory + "\\test.bmp";

            var bitmap = LoadImage(filepath);

            ConfigureAndPrint(bitmap);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var filepath = System.Environment.CurrentDirectory + "\\test.bmp";

            var bitmap = LoadImage(filepath);

            Print(bitmap);
        }
    }
}
