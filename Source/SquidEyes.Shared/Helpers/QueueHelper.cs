using System;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace SquidEyes.Shared
{
    public static class QueueHelper
    {
        private static Regex queueNameRegex = new Regex(
            "^[a-z]([a-z0-9]|((?<!-)-))*[a-z0-9]$");

        private const int STORAGETIMEOUT = 15;
        private const int MAXRETRIES = 3;
        private const int SECSBETWEENRETRIES = 1;

        public static bool IsQueueName(string value)
        {
            return queueNameRegex.IsMatch(value);
        }

        public static CloudQueue GetQueue(string connString,
            string queueName)
        {
            var account = CloudStorageAccount.Parse(connString);

            var client = account.CreateCloudQueueClient();

            client.Timeout = TimeSpan.FromSeconds(STORAGETIMEOUT);

            client.RetryPolicy = RetryPolicies.Retry(MAXRETRIES,
                TimeSpan.FromSeconds(SECSBETWEENRETRIES));

            return client.GetQueueReference(queueName);
        }

        public static bool GetQueueExists(string connString,
            string queueName)
        {
            return GetQueue(connString, queueName).Exists();
        }
    }
}