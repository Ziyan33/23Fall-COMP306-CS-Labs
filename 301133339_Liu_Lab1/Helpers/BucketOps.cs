using Amazon.Runtime;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301133339_Liu_Lab1.Helpers
{
    public class BucketOps
    {
       

        //+++++++++++++++++++++++++++++++++++++++
        //For displaying the bucketlist
        public async Task<List<Bucket>> GetBucketList()
        {
            ListBucketsResponse response = await Helper.s3Client.ListBucketsAsync();
            List<Bucket> bucketList=new List<Bucket>();

            foreach (S3Bucket bucket in response.Buckets)
            {
                //Console.Write(bucket.BucketName + " " + bucket.CreationDate.ToShortDateString());
                bucketList.Add(new Bucket
                {
                    BucketName = bucket.BucketName,
                    CreationDate = bucket.CreationDate
                });
            }
            return bucketList;
        }

        //++++++++++++++++++++++++++++++++
        //For Creating new bucket
        public async Task<AmazonWebServiceResponse> CreateBucket(string bucketName)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            var response = await Helper.s3Client.PutBucketAsync(putBucketRequest);

            return response;
        }

        //+++++++++++++++++++++++++++++++++++++++
        //For deleting the existing bucket
        public async Task<AmazonWebServiceResponse> DeleteBucket(string bucketName)
        {
            var deleteBucketRequest = new DeleteBucketRequest
            {
                BucketName = bucketName
            };

            var response = await Helper.s3Client.DeleteBucketAsync(deleteBucketRequest);

            return response;
        }


        //+++++++++++++++++++++++++++++++++++++++

        public class Program
        {
            public static async Task Main(string[] args)
            {
                BucketOps bucketOps = new BucketOps();

                // To list buckets
                await bucketOps.GetBucketList();
            }
        }
    }
}
