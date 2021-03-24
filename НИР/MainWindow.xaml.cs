using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;

namespace НИР
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Processing Proc, NoiseProc; // Объекты обработки изображений       

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewClass();
        }

        // Обработка выбора папки с файлами
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            // Выбор каталога с изображениями в диалоговом окне
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((ViewClass)DataContext).FileName = null;
                // Создание объекта обработки исходных изображений
                Proc = new Processing(FBD.SelectedPath);
                // Подписка на событие ошибки
                Proc.OnException += Proc_OnException;
                // Запуск обработки исходных изображений
                Proc.Run();
                // Создание каталога изображений с шумом
                string path = Proc.GetNoiseImages();
                // Создание объекта обработки изображений с шумом
                NoiseProc = new Processing(path);
                // Подписка на событие ошибки
                NoiseProc.OnException += Proc_OnException;
                // Запуск обработки изображений с шумом
                NoiseProc.Run(); 
                // Отображение первого изображения в каталоге
                ((ViewClass)DataContext).Val = 1; 
                ((ViewClass)DataContext).Max = Proc.Filenames.Count;                
                ((ViewClass)DataContext).FileName = Proc.Filenames[0];
                Image.IsSelected = true;
                OriginalImage.IsSelected = true;
                System.Windows.MessageBox.Show("Для просмотра всех изображений выберите " +
                    "соответствующую вкладку и перемещайте ползунок");
                Parameters.IsEnabled = true;
                Analiz.IsEnabled = true;
            }
        }

        // Обработка вычисления параметров контуров
        private void Parameters_Click(object sender, RoutedEventArgs e)
        {
            if (Image.IsSelected)
            {
                ContourImage.Focus();
                Proc.GetBlobs(((ViewClass)DataContext).FileName);
                ((ViewClass)DataContext).BlobList = Proc.BlobsParameters;
                Canvas can = (Canvas)ContourLabel.Template.FindName("MyCanvas", ContourLabel);
                DrawRectangles(can, Proc);
            }
            else
            {
                NoiceContourImage.Focus();
                NoiseProc.GetBlobs(((ViewClass)DataContext).FileName);
                ((ViewClass)DataContext).BlobList = NoiseProc.BlobsParameters;
                Canvas can = (Canvas)NoiceContourLabel.Template.FindName("MyCanvas", NoiceContourLabel);
                DrawRectangles(can, NoiseProc);
            }

        }
        
        // Метод рисования прямоугольника ограничивающего контур на холсте
        private void DrawRectangle(Canvas can, BlobParameters bp)
        {
            // Вычисление положения и размеров прямоугольника
            double ww = can.ActualWidth,
                   hw = can.ActualHeight,
                   wi = bp.Bmp.Width,
                   hi = bp.Bmp.Height,
                   x = 0,
                   y = 0,
                   w = 0,
                   h = 0;
            if (hw / ww >= hi / wi)
            {
                x = ww * bp.X / wi;
                y = (hw - ww * hi / wi) / 2 + ww * bp.Y / wi;
                w = ww * bp.Width / wi;
                h = ww * bp.Height / wi;
            }
            else
            {
                y = hw * bp.Y / hi;
                x = (ww - hw * wi / hi) / 2 + hw * bp.X / hi;
                w = hw * bp.Width / hi;
                h = hw * bp.Height / hi;
            }
            // Создание объекта прямоугольника
            Rectangle rect = new Rectangle()
            {
                Width = w,
                Height = h,
                Stroke = Brushes.Blue
            };
            // Помещение прямоугольника на холст
            can.Children.Add(rect);
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }

        // Метод рисования прямоугольников ограничивающих все контуры
        private void DrawRectangles(Canvas can, Processing proc)
        {
            if (can != null)
            {
                can.Children.Clear();
                if (proc.BlobsParameters != null)
                    foreach (BlobParameters bp in proc.BlobsParameters)
                    {
                        DrawRectangle(can, bp);
                    }
            }
        }

        #region Обработчики выбора вкладок
        
        private void Original_GotFocus(object sender, RoutedEventArgs e)
        {
            if(Proc!=null)
            ((ViewClass)DataContext).FileName =
                    Proc.Filenames[((ViewClass)DataContext).Val-1];
            dataGrid.SelectedIndex = -1;
            NoiceDataGrid.SelectedIndex = -1;
        }
        private void Binary_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Proc != null)
            {
                string path = Proc.SelectedPath + "\\Binary";
                ((ViewClass)DataContext).FileName =
                        Proc.Filenames[((ViewClass)DataContext).Val - 1]
                        .Replace(Proc.SelectedPath, path);
            }
            dataGrid.SelectedIndex = -1;
            NoiceDataGrid.SelectedIndex = -1;
        }
        private void Contour_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Proc != null)
            {
                string path = Proc.SelectedPath + "\\Contour";
                ((ViewClass)DataContext).FileName =
                        Proc.Filenames[((ViewClass)DataContext).Val - 1]
                        .Replace(Proc.SelectedPath, path);
                // Рисование прямоугольников
                Canvas can = (Canvas)ContourLabel.Template.FindName("MyCanvas", ContourLabel);
                DrawRectangles(can, Proc);
            }
        }
        private void Noice_Original_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NoiseProc != null)
                ((ViewClass)DataContext).FileName =
                    NoiseProc.Filenames[((ViewClass)DataContext).Val-1];
            dataGrid.SelectedIndex = -1;
            NoiceDataGrid.SelectedIndex = -1;
        }
        private void Noice_Binary_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NoiseProc != null)
            {
                string path = NoiseProc.SelectedPath + "\\Binary";
                ((ViewClass)DataContext).FileName =
                        NoiseProc.Filenames[((ViewClass)DataContext).Val - 1]
                        .Replace(NoiseProc.SelectedPath, path);
            }
            dataGrid.SelectedIndex = -1;
            NoiceDataGrid.SelectedIndex = -1;
        }
        private void Noice_Contour_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NoiseProc != null)
            {
                string path = NoiseProc.SelectedPath + "\\Contour";
                ((ViewClass)DataContext).FileName =
                        NoiseProc.Filenames[((ViewClass)DataContext).Val - 1]
                        .Replace(NoiseProc.SelectedPath, path);
                // Рисование прямоугольников
                Canvas can = (Canvas)NoiceContourLabel.Template.FindName("MyCanvas", NoiceContourLabel);
                DrawRectangles(can, NoiseProc);
            }
        }        
        private void Image_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Proc != null)
            {
                ((ViewClass)DataContext).BlobList = Proc.BlobsParameters;
                // Рисование прямоугольников
                Canvas can = (Canvas)ContourLabel.Template.FindName("MyCanvas", ContourLabel);
                DrawRectangles(can, Proc);
            }
        }
        private void Noice_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NoiseProc != null)
            {
                ((ViewClass)DataContext).BlobList = NoiseProc.BlobsParameters;
                // Рисование прямоугольников
                Canvas can = (Canvas)NoiceContourLabel.Template.FindName("MyCanvas", NoiceContourLabel);
                DrawRectangles(can, NoiseProc);
            }
        }
        #endregion

        // Обработка выбора контура без шума в таблице
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ViewClass)DataContext).BlobList != null && dataGrid.SelectedIndex >= 0)
            {
                ((ViewClass)DataContext).BlobImage = ((ViewClass)DataContext).BlobList[dataGrid.SelectedIndex].Bmp;
                Canvas can = (Canvas)ContourLabel.Template.FindName("MyCanvas", ContourLabel);
                if (can != null)
                {
                    can.Children.Clear();
                    if (Proc.BlobsParameters != null)
                        DrawRectangle(can, ((ViewClass)DataContext).BlobList[dataGrid.SelectedIndex]);
                }
            }
        }
        // Обработка выбора контура с шумом в таблице
        private void NoiceDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ViewClass)DataContext).BlobList != null && NoiceDataGrid.SelectedIndex >= 0)
            {
                ((ViewClass)DataContext).BlobImage = ((ViewClass)DataContext).BlobList[NoiceDataGrid.SelectedIndex].Bmp;
                Canvas can = (Canvas)ContourLabel.Template.FindName("MyCanvas", NoiceContourLabel);
                if (can != null)
                {
                    can.Children.Clear();
                    if (NoiseProc.BlobsParameters != null)
                        DrawRectangle(can, ((ViewClass)DataContext).BlobList[NoiceDataGrid.SelectedIndex]);
                }
            }            
        }

        // Обработка клика по окну с изображением
        // Сброс выбранного в таблице контура
        private void MyCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dataGrid.SelectedIndex = -1;
            NoiceDataGrid.SelectedIndex = -1;
            // Рисование прямоугольников
            if (Image.IsSelected)
            {
                if (Proc != null)
                {
                    Canvas can = (Canvas)ContourLabel.Template.FindName("MyCanvas", ContourLabel);
                    DrawRectangles(can, Proc);
                }
            }
            else
            {
                if (NoiseProc != null)
                {
                    Canvas can = (Canvas)NoiceContourLabel.Template.FindName("MyCanvas", NoiceContourLabel);
                    DrawRectangles(can, NoiseProc);
                }
            }

        }

        // Смена изображений на текущей вкладке при движении ползунка
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Image != null && ((ViewClass)DataContext).FileName != null)
            {
                string oldFile = ((ViewClass)DataContext).FileName;
                string newFile = Proc.Filenames[((ViewClass)DataContext).Val - 1];
                newFile = oldFile.Substring(0, oldFile.LastIndexOf('\\'))
                    + newFile.Substring(newFile.LastIndexOf('\\'));
                ((ViewClass)DataContext).FileName = newFile;
                Proc.ClearStat();
                NoiseProc.ClearStat();
                ((ViewClass)DataContext).BlobList = null;
                // Очистка холстов от прямоугольников
                Canvas can;
                can = (Canvas)ContourLabel.Template.FindName("MyCanvas", ContourLabel);
                if (can != null)
                    can.Children.Clear();
                can = (Canvas)NoiceContourLabel.Template.FindName("MyCanvas", NoiceContourLabel);
                if (can != null)
                    can.Children.Clear();                
            }
        }

        // Вызов окна графического анализа параметров контуров
        private void Analiz_Click(object sender, RoutedEventArgs e)
        {
            Proc.GetAnalize();
            NoiseProc.GetAnalize();
            Analize NewWindow = new Analize(Proc.AnalizeResults, NoiseProc.AnalizeResults)
            {
                Owner = this
            };
            NewWindow.ShowDialog();
        }

        // Обработка изменения размеров области для рисования контура без шума
        private void ContourLabel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(Image.IsSelected && ContourImage.IsSelected)
            {
                if (Proc != null)
                {
                    Canvas can = (Canvas)ContourLabel.Template.FindName("MyCanvas", ContourLabel);
                    if (dataGrid.SelectedIndex < 0)
                        DrawRectangles(can, Proc);
                    else
                    {
                        if (can != null)
                        {
                            can.Children.Clear();
                            if (Proc.BlobsParameters != null)
                                DrawRectangle(can, ((ViewClass)DataContext).BlobList[dataGrid.SelectedIndex]);
                        }
                    }
                }
            }
        }

        // Обработка изменения размеров области для рисования контура с шумом
        private void NoiceContourLabel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (NoiceImage.IsSelected && NoiceContourImage.IsSelected)
            {
                if (NoiseProc != null)
                {
                    Canvas can = (Canvas)NoiceContourLabel.Template.FindName("MyCanvas", NoiceContourLabel);
                    if (NoiceDataGrid.SelectedIndex < 0)
                        DrawRectangles(can, NoiseProc);
                    else
                    {
                        if (can != null)
                        {
                            can.Children.Clear();
                            if (NoiseProc.BlobsParameters != null)
                                DrawRectangle(can, ((ViewClass)DataContext).BlobList[NoiceDataGrid.SelectedIndex]);
                        }
                    }
                }
            }
        }

        // Обработка ошибок в объектах обработки изображений
        private void Proc_OnException(string Message)
        {
            System.Windows.MessageBox.Show(Message);
        }

    }
}
