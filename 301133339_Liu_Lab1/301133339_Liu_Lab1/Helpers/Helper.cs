using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301133339_Liu_Lab1.Helpers
{
    public static class Helper
    {
        public readonly static IAmazonS3 s3Client;

        static Helper()
        {
            s3Client = GetS3Client();
        }

        private static IAmazonS3 GetS3Client()
        {
            string awsAccessKey = "AKIARZWD54YKDCET4RUR";
            string awsSecretKey = "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";
            return new AmazonS3Client(awsAccessKey, awsSecretKey, Amazon.RegionEndpoint.CACentral1);
        }
    }
}
