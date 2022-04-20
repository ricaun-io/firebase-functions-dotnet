namespace Firebase.Functions
{
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// FirebaseFunctionsOptions
    /// </summary>
    public class FirebaseFunctionsOptions
    {
        /// <summary>
        /// FirebaseFunctionsOptions
        /// </summary>
        public FirebaseFunctionsOptions()
        {
            this.JsonSerializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Gets or sets the method for retrieving auth tokens. Default is null.
        /// </summary>
        public Func<Task<string>> AuthTokenAsyncFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Timeout of the <see cref="HttpClient"/>. Default is 100s.
        /// </summary>
        public TimeSpan HttpClientTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the json serializer settings.
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings
        {
            get;
            set;
        }

    }
}