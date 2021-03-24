using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace НИР
{
    public class ViewClass : DependencyObject
    {        
        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства FileName
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(ViewClass), new PropertyMetadata(null));

        public int Max
        {
            get { return (int)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства Max
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(ViewClass), new PropertyMetadata(1));

        public int Val
        {
            get { return (int)GetValue(ValProperty); }
            set { SetValue(ValProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства Val
        public static readonly DependencyProperty ValProperty =
            DependencyProperty.Register("Val", typeof(int), typeof(ViewClass), new PropertyMetadata(1));

        public List<BlobParameters> BlobList
        {
            get { return (List<BlobParameters>)GetValue(BlobListProperty); }
            set { SetValue(BlobListProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства BlobList
        public static readonly DependencyProperty BlobListProperty =
            DependencyProperty.Register("BlobList", typeof(List<BlobParameters>), typeof(ViewClass), new PropertyMetadata(null));

        public BitmapImage BlobImage
        {
            get { return (BitmapImage)GetValue(BlobImageProperty); }
            set { SetValue(BlobImageProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства BlobImage
        public static readonly DependencyProperty BlobImageProperty =
            DependencyProperty.Register("BlobImage", typeof(BitmapImage), typeof(ViewClass), new PropertyMetadata(null));

        public PointCollection Graphici
        {
            get { return (PointCollection)GetValue(GraphiciProperty); }
            set { SetValue(GraphiciProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства Graphici
        public static readonly DependencyProperty GraphiciProperty =
            DependencyProperty.Register("Graphici", typeof(PointCollection), typeof(ViewClass), new PropertyMetadata(null));

        public PointCollection GraphiciNoice
        {
            get { return (PointCollection)GetValue(GraphiciNoiceProperty); }
            set { SetValue(GraphiciNoiceProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства GraphiciNoice
        public static readonly DependencyProperty GraphiciNoiceProperty =
            DependencyProperty.Register("GraphiciNoice", typeof(PointCollection), typeof(ViewClass), new PropertyMetadata(null));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        // Использование DependencyProperty в качестве резервного хранилища для свойства MaxValue
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(ViewClass), new PropertyMetadata(0));
    }
}
