namespace Firebase.Functions
{
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// HttpsCallableReference
    /// </summary>
    public class HttpsCallableReference
    {
        private readonly FirebaseFunctions firebaseFunctions;
        private readonly string name;

        /// <summary>
        /// HttpsCallableReference
        /// </summary>
        /// <param name="firebaseFunctions"></param>
        /// <param name="name"></param>
        public HttpsCallableReference(FirebaseFunctions firebaseFunctions, string name)
        {
            this.firebaseFunctions = firebaseFunctions;
            this.name = name;
        }

        /// <summary>
        /// CallAsync
        /// </summary>
        /// <returns></returns>
        public async Task<string> CallAsync() => await CallAsync(null);

        /// <summary>
        /// CallAsync
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> CallAsync(object data)
        {
            var httpClient = await firebaseFunctions.CreateHttpClientAsync();
            var responseMessage = await httpClient.PostAsync(name, CreateHttpContent(data));
            var content = await responseMessage.Content.ReadAsStringAsync();
            var firebaseResult = JsonConvert.DeserializeObject<FirebaseResult>(content, firebaseFunctions.Options.JsonSerializerSettings);
            return firebaseResult.Result.ToString();
        }

        /// <summary>
        /// CallAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> CallAsync<T>() => await CallAsync<T>(null);

        /// <summary>
        /// CallAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> CallAsync<T>(object data)
        {
            var content = await CallAsync(data);
            return JsonConvert.DeserializeObject<T>(content, firebaseFunctions.Options.JsonSerializerSettings);
        }

        /// <summary>
        /// CreateHttpContent
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private HttpContent CreateHttpContent(object data)
        {
            var json = JsonConvert.SerializeObject(new FirebaseData(data), firebaseFunctions.Options.JsonSerializerSettings);
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return httpContent;
        }

        /// <summary>
        /// FirebaseData
        /// </summary>
        internal class FirebaseData
        {
            /// <summary>
            /// FirebaseData
            /// </summary>
            /// <param name="data"></param>
            internal FirebaseData(object data)
            {
                this.Data = data;
            }

            /// <summary>
            /// Data
            /// </summary>
            [JsonProperty("data")]
            public object Data { get; set; }
        }

        /// <summary>
        /// FirebaseResult
        /// </summary>
        internal class FirebaseResult
        {
            /// <summary>
            /// Result
            /// </summary>
            [JsonProperty("result")]
            public object Result { get; set; }
        }
    }
}
