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
using Pen = System.Drawing.Pen;
using Rectangle = System.Windows.Shapes.Rectangle;
using Color = System.Drawing.Color;

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

        public static string SelectColor = "Black";
        public static SolidColorBrush SelectedColor = Brushes.Black;
        public static SolidColorBrush SelectedPaintColor = Brushes.White;
        public static Color color = Color.Black;

        bool start = false;

        Line newLine = null;
        Ellipse newEllipse = null;
        Rectangle newRectangle = null;
        
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
            Status.Add(Circle);
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
                    newLine.Fill = SelectedPaintColor;
                    newLine.Stroke = SelectedColor;

                    Canvas.SetZIndex(newLine, 10);
                    canvas.Children.Add(newLine);
                    break;

                case "Pipette":
                    break;

                case "Select":
                    break;

                case "Square":
                    newRectangle = new Rectangle();

                    startPoint = new Point();
                    startPoint.X = clickPoint.X;
                    startPoint.Y = clickPoint.Y;

                    newRectangle.Fill = SelectedPaintColor;
                    newRectangle.Stroke = SelectedColor;

                    Canvas.SetZIndex(newRectangle, 10);
                    Canvas.SetLeft(newRectangle, clickPoint.X);
                    Canvas.SetTop(newRectangle, clickPoint.Y);

                    canvas.Children.Add(newRectangle);
                    break;

                case "Triangle":
                    break;

                case "Circle":
                    newEllipse = new Ellipse();

                    startPoint = new Point();
                    startPoint.X = clickPoint.X;
                    startPoint.Y = clickPoint.Y;

                    newEllipse.Fill = SelectedPaintColor;
                    newEllipse.Stroke = SelectedColor;

                    Canvas.SetZIndex(newEllipse, 10);
                    Canvas.SetLeft(newEllipse, clickPoint.X);
                    Canvas.SetTop(newEllipse, clickPoint.Y);

                    canvas.Children.Add(newEllipse);
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
                        newLine.Stroke = SelectedColor;
                        newLine.StrokeThickness = 2;


                        canvas.Children.Add(newLine);
                        Canvas.SetZIndex(newLine, 10);
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
                        if (newRectangle != null)
                        {
                            double width = startPoint.X - nowPoint.X;
                            double height = startPoint.Y - nowPoint.Y;

                            if (startPoint.X < nowPoint.X)
                            {
                                width *= -1;
                            }

                            if (startPoint.Y < nowPoint.Y)
                            {
                                height *= -1;
                            }                            

                            if (nowPoint.X < startPoint.X)
                            {
                                if (nowPoint.Y < startPoint.Y)
                                {
                                    Canvas.SetLeft(newRectangle, nowPoint.X);
                                    Canvas.SetTop(newRectangle, nowPoint.Y);
                                }
                                else
                                {
                                    Canvas.SetLeft(newRectangle, nowPoint.X);
                                }
                            }
                            else
                            {
                                if (nowPoint.Y < startPoint.Y)
                                {
                                    Canvas.SetTop(newRectangle, nowPoint.Y);

                                }
                            }
                            newRectangle.Height = height;
                            newRectangle.Width = width;
                        }
                            break;

                    case "Triangle":
                        break;

                    case "Circle":
                        if(newEllipse != null)
                        {
                            double width = startPoint.X - nowPoint.X;
                            double height = startPoint.Y - nowPoint.Y;

                            if (startPoint.X < nowPoint.X)
                            {                                
                                width *= -1;
                            }

                            if (startPoint.Y < nowPoint.Y)
                            {
                                height *= -1;
                            }

                            if (nowPoint.X < startPoint.X)
                            {
                                //왼쪽 위 확장
                                if (nowPoint.Y < startPoint.Y)
                                {
                                    Canvas.SetLeft(newEllipse, nowPoint.X);
                                    Canvas.SetTop(newEllipse, nowPoint.Y);
                                }
                                
                                //왼쪽 아래 확장
                                else
                                {
                                    Canvas.SetLeft(newEllipse, nowPoint.X);
                                }
                            }
                            else
                            {
                                //오른쪽 위 확장
                                if (nowPoint.Y < startPoint.Y)
                                {
                                    Canvas.SetTop(newEllipse, nowPoint.Y);
                                }

                            }
                            newEllipse.Width = width;
                            newEllipse.Height = height;

                        }
                        

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
            newEllipse = null;
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

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            SelectColor = button.Name;


            if (CurrentStatus == "Paint")
            {
                PaintColor.Name = button.Name;
                PaintColor.Background = button.Background;

                switch (SelectColor)
                {
                    case "WhiteColor":
                        SelectedPaintColor = Brushes.White;
                        break;

                    case "RedColor":
                        SelectedPaintColor = Brushes.Red;
                        break;

                    case "BlueColor":
                        SelectedPaintColor = Brushes.Blue;
                        break;

                    case "GreenColor":
                        SelectedPaintColor = Brushes.Green;
                        break;

                    case "YellowColor":
                        SelectedPaintColor = Brushes.Yellow;
                        break;

                    case "PurpleColor":
                        SelectedPaintColor = Brushes.Purple;
                        break;

                    case "BlackColor":
                        SelectedPaintColor = Brushes.Black;
                        break;
                }
            }
            else
            {
                PencilColor.Name = button.Name;
                PencilColor.Background = button.Background;

                switch (SelectColor)
                {
                    case "WhiteColor":
                        SelectedColor = Brushes.White;
                        break;

                    case "RedColor":
                        SelectedColor = Brushes.Red;
                        break;

                    case "BlueColor":
                        SelectedColor = Brushes.Blue;
                        break;

                    case "GreenColor":
                        SelectedColor = Brushes.Green;
                        break;

                    case "YellowColor":
                        SelectedColor = Brushes.Yellow;
                        break;

                    case "PurpleColor":
                        SelectedColor = Brushes.Purple;
                        break;

                    case "BlackColor":
                        SelectedColor = Brushes.Black;
                        break;
                }
            }

        }
    }
}
