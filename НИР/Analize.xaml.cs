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
using System.Windows.Shapes;

namespace НИР
{
    /// <summary>
    /// Логика взаимодействия для Analize.xaml
    /// </summary>
    public partial class Analize : Window
    {
        List<AnalizeResult> Graph;
        List<AnalizeResult> NoiceGraph;
        public Analize(List<AnalizeResult> graph, List<AnalizeResult> noiceGraph)
        {
            InitializeComponent();
            Graph = new List<AnalizeResult>(graph);
            NoiceGraph = new List<AnalizeResult>(noiceGraph);
            DataContext = new ViewClass();
            NumCont.Focus();
        }        
        
        private void CanvasNum_Loaded(object sender, RoutedEventArgs e)
        {
            if (NumCont.IsSelected)
            {
                List<int> _value = new List<int>(Graph.Select(x => x.ContoursCount));
                List<int> _noiceValue = new List<int>(NoiceGraph.Select(x => x.ContoursCount));
                GetPoints(_value, _noiceValue, CanvasNum.ActualHeight, CanvasNum.ActualWidth);
            }
        }

        private void CanvasArea_Loaded(object sender, RoutedEventArgs e)
        {
            if (AreaCont.IsSelected)
            {
                List<int> _value = new List<int>(Graph.Select(x => x.SumAreas));
                List<int> _noiceValue = new List<int>(NoiceGraph.Select(x => x.SumAreas));
                GetPoints(_value, _noiceValue, CanvasArea.ActualHeight, CanvasArea.ActualWidth);
            }
        }

        private void CanvasPerimeter_Loaded(object sender, RoutedEventArgs e)
        {
            if (PerimCont.IsSelected)
            {
                List<int> _value = new List<int>(Graph.Select(x => x.SumPerimeters));
                List<int> _noiceValue = new List<int>(NoiceGraph.Select(x => x.SumPerimeters));
                GetPoints(_value, _noiceValue, CanvasPerimeter.ActualHeight, CanvasPerimeter.ActualWidth);
            }
        }

        private void GetPoints(List<int> _value, List<int> _noiceValue, double canH, double canW)
        {
            int maxG = _value.Max();
            int maxNG = _noiceValue.Max();
            double maxValue = maxG > maxNG ? maxG : maxNG;
            ((ViewClass)DataContext).MaxValue = (int)maxValue;
            double maxHeight = canH - 40.0;
            double maxWidth = canW;
            var _Graphici = new PointCollection();
            var _GraphiciNoice = new PointCollection();
            for (int i = 0; i < Graph.Count; i++)
            {
                _Graphici.Add(new Point(maxWidth * i / _value.Count, maxHeight + 40 - maxHeight * _value[i] / maxValue));
                _GraphiciNoice.Add(new Point(maxWidth * i / _noiceValue.Count, maxHeight + 40 - maxHeight * _noiceValue[i] / maxValue));
            }
            ((ViewClass)DataContext).Graphici = _Graphici;
            ((ViewClass)DataContext).GraphiciNoice = _GraphiciNoice;
        }
        
        private void TabCa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NumCont.IsSelected)
            {
                if (CanvasNum.Parent != null)
                {
                    var parent = (Panel)CanvasNum.Parent;
                    parent.Children.Remove(CanvasNum);
                    parent.Children.Add(CanvasNum);
                }
            }
            else if (AreaCont.IsSelected)
            {
                if (CanvasArea.Parent != null)
                {
                    var parent = (Panel)CanvasArea.Parent;
                    parent.Children.Remove(CanvasArea);
                    parent.Children.Add(CanvasArea);
                }
            }
            else
            {
                if (CanvasPerimeter.Parent != null)
                {
                    var parent = (Panel)CanvasPerimeter.Parent;
                    parent.Children.Remove(CanvasPerimeter);
                    parent.Children.Add(CanvasPerimeter);
                }
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (NumCont.IsSelected)
            {
                List<int> _value = new List<int>(Graph.Select(x => x.ContoursCount));
                List<int> _noiceValue = new List<int>(NoiceGraph.Select(x => x.ContoursCount));
                GetPoints(_value, _noiceValue, CanvasNum.ActualHeight, CanvasNum.ActualWidth);
            }
            else if (AreaCont.IsSelected)
            {
                List<int> _value = new List<int>(Graph.Select(x => x.SumAreas));
                List<int> _noiceValue = new List<int>(NoiceGraph.Select(x => x.SumAreas));
                GetPoints(_value, _noiceValue, CanvasArea.ActualHeight, CanvasArea.ActualWidth);
            }
            else
            {
                List<int> _value = new List<int>(Graph.Select(x => x.SumPerimeters));
                List<int> _noiceValue = new List<int>(NoiceGraph.Select(x => x.SumPerimeters));
                GetPoints(_value, _noiceValue, CanvasPerimeter.ActualHeight, CanvasPerimeter.ActualWidth);
            }
        }
    }
}
