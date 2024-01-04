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

namespace _301133339_Liu_Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bucketOperation_Click(object sender, RoutedEventArgs e)
        {
            bucketOperation bucketWindow = new bucketOperation();
            bucketWindow.Show();

            this.Close();
        }

        private void objectOperation_Click(object sender, RoutedEventArgs e)
        {
           objectOperation o1=new objectOperation();
            o1.Show(); 
            this.Close();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
