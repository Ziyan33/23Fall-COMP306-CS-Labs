using _301133339_Liu_Lab1.Helpers;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Win32;
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

namespace _301133339_Liu_Lab1
{
    /// <summary>
    /// Interaction logic for objectOperation.xaml
    /// </summary>
    public partial class objectOperation : Window
    {
       /* public class Object
        {
            public string objectName;
            public int objectSize;
        } */
        public objectOperation()
        {
            InitializeComponent();
        }

        public void backToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private async void cbBucket_Loaded(object sender, RoutedEventArgs e)
        {
            ObjectOps objectOps = new ObjectOps();
            List<string> bucketNames = await objectOps.LoadBucketNames();

            cbBucket.ItemsSource = bucketNames;
        }

        private async void cbBucket_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (cbBucket.SelectedItem != null)
            {
                string selectedBucket = cbBucket.SelectedItem.ToString();
                ObjectOps objectOps = new ObjectOps();
                List<S3Object> objects = await objectOps.ListObjects(selectedBucket);
                ObjectList.ItemsSource = objects;
            }
        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {
            string selectedFileName = txtFileName.Text;
            try
            {
                ObjectOps objectOps = new ObjectOps();
                //await objectOps.UploadObject(selectedBucket, selectedFileName);

                //List<S3Object> objects = await objectOps.ListObjects(selectedBucket);
                //ObjectList.ItemsSource = objects;

                //MessageBox.Show($"File '{selectedFileName}' uploaded successfully to '{selectedBucket}'.");
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show($"Error uploading file: {ex.Message}");
            }
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();
            if (newDialog.ShowDialog() == true) 
            {
                string selectedFileName = newDialog.FileName;

                txtFileName.Text = selectedFileName;
            }
        }

      
    }
}
