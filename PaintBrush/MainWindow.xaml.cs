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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Media;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

namespace PaintBrush
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string CurrentStatus = "Pencil";
        //string으로 처리시 처리가 안됨 List<Button>형식으로만 처리가능 왜지...?
        public static List<Button> Status = new List<Button>();

        bool start = false;

        Line newLine = null;
        Point startPoint;
        public MainWindow()
        {
            InitializeComponent();
            Status.Add(Pencil);
            Status.Add(Paint);
            Status.Add(Eraser);
            Status.Add(Line);
            Status.Add(Pipette);
            Status.Add(Select);
            Status.Add(Square);
            Status.Add(Triangle);
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = (Point)e.GetPosition(canvas);
            start = true;
            switch (CurrentStatus)
            {
                case "Pencil":
                    startPoint = new Point();
                    startPoint.X = clickPoint.X;
                    startPoint.Y = clickPoint.Y;
                    break;

                case "Paint":
                    break;

                case "Eraser":
                    break;

                case "Line":
                    newLine = new Line();
                    newLine.X1 = clickPoint.X;
                    newLine.Y1 = clickPoint.Y;
                    newLine.Fill = Brushes.Green;
                    newLine.Stroke = Brushes.Blue;

                    Canvas.SetZIndex(newLine, 10);
                    canvas.Children.Add(newLine);
                    break;

                case "Pipette":
                    break;

                case "Select":
                    break;

                case "Square":
                    break;

                case "Triangle":
                    break;

                default:
                    MessageBox.Show("잘못 선택하셨습니다.");
                    break;
            }

        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point nowPoint = (Point)e.GetPosition(canvas);

            if (start)
            {
                switch (CurrentStatus)
                {
                    case "Pencil":
                        newLine = new Line();
                        newLine.X1 = startPoint.X;
                        newLine.X2 = nowPoint.X;
                        newLine.Y1 = startPoint.Y;
                        newLine.Y2 = nowPoint.Y;
                        newLine.Stroke = Brushes.Blue;
                        newLine.StrokeThickness = 2;
                        canvas.Children.Add(newLine);
                        startPoint = nowPoint;
                        break;

                    case "Paint":
                        break;

                    case "Eraser":
                        break;

                    case "Line":
                        if (newLine != null)
                        {
                            newLine.X2 = nowPoint.X;
                            newLine.Y2 = nowPoint.Y;
                        }
                        break;

                    case "Pipette":
                        break;

                    case "Select":
                        break;

                    case "Square":
                        break;

                    case "Triangle":
                        break;

                    default:
                        MessageBox.Show("잘못 선택하셨습니다.");
                        break;
                }
            }
        }
        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            newLine = null;
            start = false;
            //switch (CurrentStatus)
            //{
            //    case "Pencil":
            //        break;

            //    case "Paint":
            //        break;

            //    case "Eraser":
            //        break;

            //    case "Line":

            //    case "Pipette":
            //        break;

            //    case "Select":
            //        break;

            //    case "Square":
            //        break;

            //    case "Triangle":
            //        break;

            //    default:
            //        MessageBox.Show("잘못 선택하셨습니다.");
            //        break;
            //}
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            CurrentStatus = button.Name;

            foreach (Button button1 in Status)
            {
                if (button1.Name == CurrentStatus)
                {
                    button1.Background = Brushes.White;
                }

                else
                {
                    button1.Background = Brushes.LightGray;
                }
            }

        }

    }
}
