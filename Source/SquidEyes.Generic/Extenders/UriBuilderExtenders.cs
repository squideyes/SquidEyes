using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace SquidEyes.Generic
{
    public static partial class UriBuilderExtenders
    {
        public static UriBuilder SetQueryParam(this UriBuilder uri, string key,
            object value)
        {
            var collection = uri.ParseQuery();

            collection.Set(key, value.ToString());

            string query = collection.AsKeyValuePairs().ToConcatenatedString(pair =>
                pair.Key == null ? pair.Value : pair.Key + "=" + pair.Value, "&");

            uri.Query = query;

            return uri;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetQueryParams(
            this UriBuilder uri)
        {
            return uri.ParseQuery().AsKeyValuePairs();
        }

        private static IEnumerable<KeyValuePair<string, string>> AsKeyValuePairs(
            this NameValueCollection collection)
        {
            foreach (string key in collection.AllKeys)
                yield return new KeyValuePair<string, string>(key, collection.Get(key));
        }

        private static NameValueCollection ParseQuery(this UriBuilder uri)
        {
            return HttpUtility.ParseQueryString(uri.Query);
        }
    }
}
