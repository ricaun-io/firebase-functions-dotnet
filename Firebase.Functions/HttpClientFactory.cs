namespace Firebase.Functions
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// HttpClientFactory
    /// </summary>
    public static class HttpClientFactory
    {
        /// <summary>
        /// Creates a new <see cref="HttpClient"/> with authentication header when <see cref="FirebaseFunctions"/> is specified.
        /// </summary>
        /// <param name="firebaseFunctions"></param>
        /// <returns></returns>
        internal static async Task<HttpClient> CreateHttpClientAsync(this FirebaseFunctions firebaseFunctions)
        {
            var options = firebaseFunctions.Options;

            var client = new HttpClient();

            var uri = firebaseFunctions.Uri.Trim();
            if (!uri.StartsWith("http"))
                uri = "https://" + uri.Trim('/');

            client.BaseAddress = new Uri(uri);

            if (options.HttpClientTimeout != default(TimeSpan))
            {
                client.Timeout = options.HttpClientTimeout;
            }

            if (options.AuthTokenAsyncFactory != null)
            {
                var auth = await options.AuthTokenAsyncFactory().ConfigureAwait(false);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth}");
            }

            return client;
        }
    }
}