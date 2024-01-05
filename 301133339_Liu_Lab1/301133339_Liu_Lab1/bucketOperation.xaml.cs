using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Amazon.S3;
using Amazon.S3.Model;
using _301133339_Liu_Lab1.Helpers;
using System.Windows.Controls;

namespace _301133339_Liu_Lab1
{
    /// <summary>
    /// Interaction logic for bucketOperation.xaml
    /// </summary>
    /// 
    public class Bucket
    {
        public string BucketName { get; set; }
        public DateTime CreationDate { get; set; }
    }
    
    public partial class bucketOperation : Window
    {
        private BucketOps bucketOps;
        public bucketOperation()
        {
            InitializeComponent();
            bucketOps = new BucketOps();
           
            LoadBucketList();
        }

        //+++++++++++++++++++++++++++++++++++++++++++
        //For Loading the Bucket in DataGrid
        private async void LoadBucketList()
        {
            try
            {
                List<Bucket> bucketList = await bucketOps.GetBucketList();
                BucketList.ItemsSource = bucketList;
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show($"Error loading bucket list: {ex.Message}");
            }
        }

        //+++++++++++++++++++++++++++++++++++++++++++


        private async void createBucket_Click(object sender, RoutedEventArgs e)
        {
            string newBucketName = txtBucketName.Text.Trim();
            ///Bucket.Add(new BucketOps());
            ///
            try
            {
                await bucketOps.CreateBucket(newBucketName);
                MessageBox.Show($"Bucket '{newBucketName}' created successfully!");

                // Reloading the bucket list to show the new bucket
                LoadBucketList(); 
            }
            catch (AmazonS3Exception ex)
            {
                MessageBox.Show($"Error creating bucket: {ex.Message}");
            }
            txtBucketName.Text = string.Empty;
        }

        //+++++++++++++++++++++++++++++++++++++++++++

        private async void deleteBucket_Click(object sender, RoutedEventArgs e)
        {
            if (BucketList.SelectedItem != null)
            {
                string selectedBucketName = ((Bucket)BucketList.SelectedItem).BucketName;
                try
                {
                    await bucketOps.DeleteBucket(selectedBucketName);
                    MessageBox.Show($"Bucket '{selectedBucketName}' deleted successfully!");
                    LoadBucketList();
                }
                catch (AmazonS3Exception ex)
                {
                    MessageBox.Show($"Error deleting bucket: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a bucket to delete.");
            }
        }

        //+++++++++++++++++++++++++++++++++++++++++++

        private void backToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }


    }
}
