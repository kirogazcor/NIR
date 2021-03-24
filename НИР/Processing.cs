using AForge.Imaging.Filters;
using AForge.Imaging;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace НИР
{
    public delegate void OnExceptionHandler(string text);

    // Класс обработки изображений
    public class Processing
    {
        private List<string> filenames; // Список обрабатываемых файлов
        private string selectedPath;    // Текущий каталог с файлами
        private List<BlobParameters> blobsParameters;
        private List<AnalizeResult> analizeResults;

        #region Public properties
        public List<string> Filenames
        { get { return filenames; } }
        public string SelectedPath
        { get { return selectedPath; } }
        public List<BlobParameters> BlobsParameters
        { get { return blobsParameters; } }
        public List<AnalizeResult> AnalizeResults
        { get { return analizeResults; } }
        #endregion

        // Конструктор
        public Processing(string path)
        {
            filenames = Directory.GetFiles(path, "*.png").ToList();
            selectedPath = path;            
        }

        // Метод запуска обработки изображений
        public void Run()
        {   
            // Создание каталогов для бинарных изображений и контуров
            string path = selectedPath + "\\Binary";
            Directory.CreateDirectory(path);
            path = selectedPath + "\\Contour";
            Directory.CreateDirectory(path);
            path = selectedPath;
            
            try
            {
                foreach (string filename in filenames)
                {
                    if (!File.Exists(filename.Replace(selectedPath, selectedPath + "\\Binary")) &&
                        !File.Exists(filename.Replace(selectedPath, selectedPath + "\\Contour")))
                    {
                        // Создание объектов фильтров
                        Grayscale filterGray = new Grayscale(0.2125, 0.7154, 0.0721);
                        GrayscaleToRGB filterRGB = new GrayscaleToRGB();
                        IterativeThreshold binary = new IterativeThreshold();
                        HomogenityEdgeDetector contour = new HomogenityEdgeDetector();
                        // Считывание изображения из файла
                        Bitmap image = new Bitmap(filename);
                        // Получение бинарного изображений
                        Bitmap binaryImage = binary.Apply(filterGray.Apply(image));
                        // Получение контура
                        Bitmap contourImage = contour.Apply(binaryImage);

                        // Сохранение бинарного изображения
                        path = selectedPath + "\\Binary";
                        image = filterRGB.Apply(binaryImage);
                        binaryImage.Dispose();
                        image.Save(filename.Replace(selectedPath, path),
                            System.Drawing.Imaging.ImageFormat.Png);
                        // Сохранение контура
                        path = selectedPath + "\\Contour";
                        image = filterRGB.Apply(contourImage);
                        contourImage.Dispose();
                        image.Save(filename.Replace(selectedPath, path),
                            System.Drawing.Imaging.ImageFormat.Png);
                        image.Dispose();
                        // Периодическая очистка памяти
                        if (GC.GetTotalMemory(true) > 1000000000) GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex.Message);
            }
        }

        // Метод создания каталога изображений с шумом
        public string GetNoiseImages()
        {
            // Создание каталога для изображений с шумом
            string path = selectedPath + "\\Noise";
            Directory.CreateDirectory(path);            
            try
            {
                foreach (string filename in filenames)
                {                    
                    string newfilename = filename.Replace(selectedPath, path);
                    if (!File.Exists(newfilename))
                    {
                        // Создание объектов фильтров
                        Grayscale filterGray = new Grayscale(0.2125, 0.7154, 0.0721);
                        GrayscaleToRGB filterRGB = new GrayscaleToRGB();
                        AdaptiveSmoothing noise = new AdaptiveSmoothing();
                        // Считывание изображения из файла
                        Bitmap image = new Bitmap(filename);
                        // Получение изображения с шумом
                        image = filterRGB.Apply(noise.Apply(filterGray.Apply(image)));
                        // Сохранение изображения с шумом
                        image.Save(newfilename, System.Drawing.Imaging.ImageFormat.Png);
                        image.Dispose();
                        // Периодическая очистка памяти
                        if (GC.GetTotalMemory(true) > 1000000000) GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex.Message);
            }
            return path;
        }

        // Получение параметров контуров
        public void GetBlobs(string filename)
        {
            // Создание объектов фильтров
            BlobCounter bc = new BlobCounter();
            Grayscale filterGray = new Grayscale(0.2125, 0.7154, 0.0721);
            try
            {
                // Считывание изображения из файла
                Bitmap image = new Bitmap(filename);
                image = filterGray.Apply(image);
                bc.ProcessImage(image);
                Blob[] blobs = bc.GetObjects(image, true);
                blobsParameters = new List<BlobParameters>();
                foreach (Blob blob in blobs)
                {
                    blobsParameters.Add(new BlobParameters(blob, bc));
                }
                image.Dispose();
            }
            catch (Exception ex)
            {
                OnException(ex.Message);
            }
        }

        // Анализ параметров контуров
        public void GetAnalize ()
        {
            string path = selectedPath + "\\Contour";
            analizeResults = new List<AnalizeResult>();
            try
            {
                foreach (string _filename in filenames)
                {
                    // Создание объектов фильтров
                    BlobCounter bc = new BlobCounter();
                    Grayscale filterGray = new Grayscale(0.2125, 0.7154, 0.0721);
                    string filename = _filename.Replace(selectedPath, path);
                    // Считывание изображения из файла
                    Bitmap image = new Bitmap(filename);
                    image = filterGray.Apply(image);
                    bc.ProcessImage(image);
                    Blob[] blobs = bc.GetObjects(image, true);
                    image.Dispose();
                    int sumAreas = 0;
                    int sumPerimeters = 0;
                    foreach (Blob blob in blobs)
                    {
                        sumAreas += blob.Area;
                        sumPerimeters += bc.GetBlobsEdgePoints(blob).Count;
                    }                    
                    analizeResults.Add(new AnalizeResult(blobs.Length, sumAreas, sumPerimeters));
                    // Периодическая очистка памяти
                    if (GC.GetTotalMemory(true)>1000000000) GC.Collect();
                }
            }
            catch (Exception ex)
            {
                OnException(ex.Message);
            }
        }

        // Очистка данных о параметрах контуров
        public void ClearStat()
        {
            blobsParameters = null;
        }
        
        // Событие ошибки в программе
        public event OnExceptionHandler OnException;
    }

    // Класс параметров контура
    public class BlobParameters
    {
        private BitmapImage bmp;
        private int area;
        private int perimeter;
        private AForge.Point centerOfGravity;
        private double fullness;
        private int width;
        private int height;
        private int x;
        private int y;

        #region Открытые свойства
        public BitmapImage Bmp
        { get { return bmp; } }
        public int Area
        { get { return area; } }
        public int Perimeter
        { get { return perimeter; } }
        public AForge.Point CenterOfGravity
        { get { return centerOfGravity; } }
        public double Fullness
        { get { return fullness; } }
        public int Width
        { get { return width; } }
        public int Height
        { get { return height; } }
        public int X
        { get { return x; } }
        public int Y
        { get { return y; } }
        #endregion

        // Конструктор
        public BlobParameters(Blob blob, BlobCounter bc)
        {
            Bitmap bmpBlob = blob.Image.ToManagedImage();
            bmp = BitmapToImageSource(bmpBlob);     
            area = blob.Area;
            perimeter = bc.GetBlobsEdgePoints(blob).Count;
            centerOfGravity = blob.CenterOfGravity;
            fullness = blob.Fullness;
            width = blob.Rectangle.Width;
            height = blob.Rectangle.Height;
            x = blob.Rectangle.X;
            y = blob.Rectangle.Y;
        }

        // Преобразование изображения из Bitmap в BitmapImage
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }

    // Класс параметров изображения для графического анализа
    public class AnalizeResult
    {
        private int contoursCount;  // Количество контуров
        private int sumAreas;       // Сумма площадей контуров
        private int sumPerimeters;  // Сумма периметров контуров

        #region Открытые свойства
        public int ContoursCount
        { get { return contoursCount; } }
        public int SumAreas
        { get { return sumAreas; } }
        public int SumPerimeters
        { get { return sumPerimeters;} }
        #endregion
        // Конструктор
        public AnalizeResult(int _count, int _sumAreas, int _sumPerim)
        {
            contoursCount = _count;
            sumAreas = _sumAreas;
            sumPerimeters = _sumPerim;
        }
    }
}
