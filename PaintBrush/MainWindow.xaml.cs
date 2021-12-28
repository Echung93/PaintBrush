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
using System.Text.RegularExpressions;

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
        public static int ZIndex = 1;

        public static string SelectColor = "Black";
        public static SolidColorBrush SelectedColor = Brushes.Black;
        public static Color color = Color.Black;

        bool start = false;

        Line newLine = null;
        Line newPencil = null;
        Line newEraser = null;
        Ellipse newEllipse = null;
        Rectangle newRectangle = null;
        Shape newShape = null;

        Point startPoint;

        //움직이기
        bool dragCheck;   // drag 시작 여부     
        Thickness mgnStart;   // 도형 시작 Margin 값

        bool alterCheck;


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
            Status.Add(Circle);
            FontValue1.Text = "1";

        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
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
                        newShape = e.Source as Shape;
                        if (newShape != null)
                            newShape.Fill = PaintColor.Background;
                        break;

                    case "Eraser":
                        startPoint = new Point();
                        startPoint.X = clickPoint.X;
                        startPoint.Y = clickPoint.Y;
                        break;

                    case "Line":
                        newLine = new Line();
                        newLine.X1 = clickPoint.X;
                        newLine.Y1 = clickPoint.Y;
                        newLine.Fill = PaintColor.Background;
                        newLine.Stroke = SelectedColor;
                        newLine.StrokeThickness = FontValue.Value;

                        Canvas.SetZIndex(newLine, ZIndex);

                        canvas.Children.Add(newLine);

                        ZIndex++;
                        break;

                    case "Pipette":
                        newShape = e.Source as Shape;
                        if (newShape != null)
                            PaintColor.Background = newShape.Fill;
                        break;

                    case "Select":
                        startPoint = e.GetPosition(canvas);
                        newShape = e.Source as Shape;
                        double newX = Canvas.GetLeft(newShape);
                        double newY = Canvas.GetTop(newShape);

                        double newWidth = newShape.ActualWidth;
                        double newHeight = newShape.ActualHeight;

                        if (startPoint.X - newX > newWidth - 10)
                        {
                            if (startPoint.Y - newY > newHeight - 10)
                            {
                                alterCheck = true;
                            }

                            else 
                            {
                                dragCheck = true;
                                if (newShape != null)
                                    mgnStart = newShape.Margin;
                            }                          
                        }

                        else
                        {
                            dragCheck = true;
                            if (newShape != null)
                                mgnStart = newShape.Margin;
                        }                        
                        
                        break;

                    case "Square":
                        newRectangle = new Rectangle();

                        startPoint = new Point();
                        startPoint.X = clickPoint.X;
                        startPoint.Y = clickPoint.Y;

                        newRectangle.Fill = PaintColor.Background;
                        newRectangle.Stroke = SelectedColor;
                        newRectangle.StrokeThickness = FontValue.Value;

                        Canvas.SetZIndex(newRectangle, ZIndex);
                        Canvas.SetLeft(newRectangle, clickPoint.X);
                        Canvas.SetTop(newRectangle, clickPoint.Y);

                        canvas.Children.Add(newRectangle);
                        ZIndex++;
                        break;

                    case "Circle":
                        newEllipse = new Ellipse();

                        startPoint = new Point();
                        startPoint.X = clickPoint.X;
                        startPoint.Y = clickPoint.Y;

                        newEllipse.Fill = PaintColor.Background;
                        newEllipse.Stroke = SelectedColor;
                        newEllipse.StrokeThickness = FontValue.Value;

                        Canvas.SetZIndex(newEllipse, ZIndex);
                        Canvas.SetLeft(newEllipse, clickPoint.X);
                        Canvas.SetTop(newEllipse, clickPoint.Y);

                        canvas.Children.Add(newEllipse);

                        ZIndex++;
                        break;

                    default:
                        MessageBox.Show("잘못 선택하셨습니다.");
                        break;
                }
            }
            catch (Exception ex)
            {
            }

        }
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point nowPoint = (Point)e.GetPosition(canvas);

                if (start)
                {
                    switch (CurrentStatus)
                    {
                        case "Pencil":
                            newPencil = new Line();
                            newPencil.X1 = startPoint.X;
                            newPencil.X2 = nowPoint.X;
                            newPencil.Y1 = startPoint.Y;
                            newPencil.Y2 = nowPoint.Y;
                            newPencil.Stroke = SelectedColor;
                            newPencil.StrokeThickness = FontValue.Value;
                            newPencil.StrokeStartLineCap = PenLineCap.Round;
                            newPencil.StrokeEndLineCap = PenLineCap.Round;

                            canvas.Children.Add(newPencil);
                            Canvas.SetZIndex(newPencil, ZIndex);

                            startPoint = nowPoint;
                            ]

                            ZIndex++;
                            break;

                        case "Paint":
                            break;

                        case "Eraser":
                            newEraser = new Line();
                            newEraser.X1 = startPoint.X;
                            newEraser.X2 = nowPoint.X;
                            newEraser.Y1 = startPoint.Y;
                            newEraser.Y2 = nowPoint.Y;
                            newEraser.Stroke = Brushes.White;
                            newEraser.StrokeThickness = FontValue.Value;
                            newEraser.StrokeStartLineCap = PenLineCap.Round;
                            newEraser.StrokeEndLineCap = PenLineCap.Round;

                            canvas.Children.Add(newEraser);
                            Canvas.SetZIndex(newEraser, ZIndex);

                            startPoint = nowPoint;
                            ZIndex++;
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
                            if (dragCheck)
                            {
                                Point pt = e.GetPosition(canvas);

                                double moveX = (pt.X - startPoint.X);
                                double moveY = (pt.Y - startPoint.Y);
                                newShape.Margin = new Thickness(mgnStart.Left + moveX, mgnStart.Top + moveY, mgnStart.Right, mgnStart.Bottom);
                            }

                            else if (alterCheck)
                            {
                                if (newShape != null)
                                {
                                    Point point = Mouse.GetPosition(this.canvas);

                                    double newX = Canvas.GetLeft(newShape);
                                    double newY = Canvas.GetTop(newShape);

                                    double offsetX = newX - point.X;
                                    double offsetY = newY - point.Y;

                                    if (newX < point.X)
                                    {
                                        offsetX *= -1;
                                    }

                                    else
                                    {
                                        offsetX = int.Parse(FontValue1.Text) * int.Parse(FontValue1.Text);
                                    }

                                    if (newY < point.Y)
                                    {
                                        offsetY *= -1;
                                    }

                                    else
                                    {
                                        offsetY = int.Parse(FontValue1.Text) * int.Parse(FontValue1.Text);
                                    }

                                    Canvas.SetLeft(newShape, newX);
                                    Canvas.SetTop(newShape, newY);

                                    newShape.Width = offsetX;
                                    newShape.Height = offsetY;
                                }
                            }


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

                                newRectangle.Width = width;
                                newRectangle.Height = height;

                            }
                            break;

                        case "Circle":
                            if (newEllipse != null)
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
            catch (Exception ex)
            {
            }

        }
        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                start = false;

                switch (CurrentStatus)
                {
                    case "Pencil":
                        newPencil = null;
                        break;

                    case "Paint":
                        newShape = null;
                        break;

                    case "Eraser":
                        newEraser = null;
                        break;

                    case "Line":
                        newLine = null;
                        break;

                    case "Pipette":
                        break;

                    case "Select":
                        dragCheck = false;
                        alterCheck = false;
                        break;

                    case "Square":
                        //newRectangle = null;
                        break;

                    case "Circle":
                        newEllipse = null;
                        break;

                    default:
                        MessageBox.Show("잘못 선택하셨습니다.");
                        break;
                }
            }

            catch (Exception ex)
            {
            }
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
                        PaintColor.Background = Brushes.White;
                        break;

                    case "RedColor":
                        PaintColor.Background = Brushes.Red;
                        break;

                    case "BlueColor":
                        PaintColor.Background = Brushes.Blue;
                        break;

                    case "GreenColor":
                        PaintColor.Background = Brushes.Green;
                        break;

                    case "YellowColor":
                        PaintColor.Background = Brushes.Yellow;
                        break;

                    case "PurpleColor":
                        PaintColor.Background = Brushes.Purple;
                        break;

                    case "BlackColor":
                        PaintColor.Background = Brushes.Black;
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

        private void FontValue1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {

                if (numCheck(FontValue1.Text))
                {
                    int nums = int.Parse(FontValue1.Text);
                    if (0 <= nums && 100 >= nums)
                    {
                        FontValue1.Text = FontValue1.Text.Replace(" ", "");
                        FontValue.Value = int.Parse(FontValue1.Text);
                    }

                    else
                    {
                        MessageBox.Show($"0~100까지 숫자만 입력해주세요. \r\n기본값 2로 설정됩니다.");
                        FontValue1.Text = "2";
                        FontValue.Value = 2;
                    }

                }

                else
                {
                    FontValue1.Text = "0";
                    FontValue.Value = 0;
                }
            }
        }
        public bool numCheck(string num)
        {
            Regex regex = new Regex("[^0-9]+");
            Boolean boolean = regex.IsMatch(num);
            if (!boolean)
            {
                return true;
            }
            return false;
        }

    }
}
