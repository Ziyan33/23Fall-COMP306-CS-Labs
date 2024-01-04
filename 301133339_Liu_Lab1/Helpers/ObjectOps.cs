using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace _301133339_Liu_Lab1.Helpers
{
    public class ObjectOps
    {

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public async Task<List<string>> LoadBucketNames()
        {
            ListBucketsResponse response = await Helper.s3Client.ListBucketsAsync();
            List<string> bucketNames = new List<string>();

            foreach (S3Bucket bucket in response.Buckets)
            {
                bucketNames.Add(bucket.BucketName);
            }

            return bucketNames;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public async Task<List<S3Object>> ListObjects(string bucketName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                MaxKeys = 10
            };

            ListObjectsV2Response response = await Helper.s3Client.ListObjectsV2Async(request);

            // Extract the list of objects from the response
            List<S3Object> objects = response.S3Objects;

            return objects;
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public Task UploadObject(string bucketName, string filePath)
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.CACentral1))
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    //Key = Path.GetFileName(filePath),
                    FilePath = filePath
                };
            }

            return Task.CompletedTask;
        }
    }
}
