using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using SquidEyes.Generic;

namespace SquidEyes.Shared
{
    public static class BlobHelper
    {
        private const int STORAGETIMEOUT = 30;
        private const int MAXRETRIES = 3;
        private const int SECSBETWEENRETRIES = 1;

        public static CloudBlobContainer GetContainer(
            string connString, string containerName)
        {
            var account = CloudStorageAccount.Parse(connString);

            var client = account.CreateCloudBlobClient();

            var container =
                client.GetContainerReference(containerName);

            var permissions = container.GetPermissions();

            permissions.PublicAccess =
                BlobContainerPublicAccessType.Off;

            container.SetPermissions(permissions);

            return container;
        }

        public static void AssertContainerExists(string connString,
            string containerName)
        {
            const string DOESNOTEXIST =
                "The \"{0}\" container does not exist!";

            var container = GetContainer(connString, containerName);

            try
            {
                container.FetchAttributes();
            }
            catch (Exception error)
            {
                throw new ApplicationException(string.Format(
                    DOESNOTEXIST, containerName), error);
            }
        }

        public static bool IsContainerName(string value)
        {
            return value.IsMatch(@"^[a-z]([a-z0-9]|[\-][^\-])*(?<!\-)$");
        }
    }
}
