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
        Color[] pallete = new Color[39464];
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(input.Text)) return;
            var searchStrings = input.Text.Split(' ');
            if (cloth.IsChecked == true) Palettes.CLOTH.CopyTo(pallete,0);
            if (cloth_bright.IsChecked == true) Palettes2.CLOTH_BRIGHT.CopyTo(pallete, 0);
            if (leather.IsChecked == true) Palettes3.LEATHER.CopyTo(pallete, 0);
            if (silk.IsChecked == true) Palettes.SILK.CopyTo(pallete, 0);
            if (metal.IsChecked == true) Palettes4.METAL.CopyTo(pallete, 0);
            if (weapon.IsChecked == true) Palettes.WEAPON.CopyTo(pallete, 0);
            output.Items.Clear();
            foreach(var str in searchStrings)
            {
                if (string.IsNullOrWhiteSpace(str)) continue;
                if (Regex.IsMatch(str, @"[0-9a-fA-F]{6}$"))
                {
                    var s = str;
                    if (s[0] == '#') s = s.Substring(1);
                    Color target = Color.FromRgb(byte.Parse(s.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(s.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                    output.Items.Add(new TextBlock() { Background = new SolidColorBrush(target), Text = "目标颜色", Foreground = CalculateForeground(target), Width = 128, TextAlignment = TextAlignment.Center });
                    Array.Sort(pallete, (a, b) =>
                        (int)(2 * Math.Pow(a.R - target.R, 2) + 4 * Math.Pow(a.G - target.G, 2) + 3 * Math.Pow(a.B - target.B, 2) - 
                        (2 * Math.Pow(b.R - target.R, 2) + 4 * Math.Pow(b.G - target.G, 2) + 3 * Math.Pow(b.B - target.B, 2))));
                }
                else MessageBox.Show("早期草稿版，只支持颜色代码搜索，无法筛选RGB值。");
            }

            {
                int n = 0;
                foreach (var c in pallete)
                {
                    output.Items.Add(new TextBox() { Background = new SolidColorBrush(c), Text = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2"), Foreground = CalculateForeground(c), Width = 128, TextAlignment = TextAlignment.Center, IsReadOnly = true, BorderThickness = new Thickness() });
                    n++;
                    if (n > 900) break;
                }
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            if (hyperlink != null)
            {
                string navigateUri = "mailto:snowyyang@outlook.com"; // 替换为你想要导航的 URL
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
            input.Text = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(null, null);
            }
        }

        private void radio_Checked(object sender, RoutedEventArgs e)
        {
            img.Source = new BitmapImage(new Uri("/" + (sender as RadioButton).Name + ".png", UriKind.Relative));
        }
    }
}
