using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MabinogiDyeColors
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Brush CalculateForeground(Color background)
        {
            return 0.2126 * background.R + 0.7152 * background.G + 0.0722 * background.B > 128 ? Brushes.Black : Brushes.White;
        }

        int i = 0;
        int moreColorsTimes = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var searchStrings = input.Text.Split(' ');
            Color[] pallete = new Color[] { };
            List<Color> results = new List<Color>();
            if (cloth.IsChecked == true) pallete = Palettes.CLOTH;
            foreach(Color c in pallete)
            {
                results.Add(c);
            }
            output.Children.Clear();
            foreach(var str in searchStrings)
            {
                if (Regex.IsMatch(str, @"[0-9a-f]{6}$"))
                {
                    var s = str;
                    if (s[0] == '#') s = s.Substring(1);
                    Color target = Color.FromRgb(byte.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                    output.Children.Add(new TextBlock() { Background = new SolidColorBrush(target), Text = "目标颜色", Foreground = CalculateForeground(target) });
                    results.Sort((a, b) =>
                        (int)(2 * Math.Pow(a.R - target.R, 2) + 4 * Math.Pow(a.G - target.G, 2) + 3 * Math.Pow(a.B - target.B, 2) - 
                        (2 * Math.Pow(b.R - target.R, 2) + 4 * Math.Pow(b.G - target.G, 2) + 3 * Math.Pow(b.B - target.B, 2))));
                }
            }
            if (results.Count == 0)
            {
                output.Children.Add(new TextBlock() { Text = "无匹配颜色" });
            }
            else
            {
                int n = 0;
                foreach (var c in results)
                {
                    output.Children.Add(new TextBox() { Background = new SolidColorBrush(c), Text = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2"), Foreground = CalculateForeground(c) });
                    n++;
                    if (n > 300) break;
                }
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            if (hyperlink != null)
            {
                string navigateUri = "https://snowyyang.me/mabinogi/dye-colors"; // 替换为你想要导航的 URL
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = navigateUri,
                    UseShellExecute = true
                });
            }
        }

        private void colorPicker_Click(object sender, RoutedEventArgs e)
        {
            //使用Windows.Forms.ColorDialog
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.ShowDialog();
            var color = colorDialog.Color;
            input.Text += "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
