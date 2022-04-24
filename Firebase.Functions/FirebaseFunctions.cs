namespace Firebase.Functions
{
    /// <summary>
    /// FirebaseFunctions
    /// </summary>
    public class FirebaseFunctions
    {
        /// <summary>
        /// Creates an instance of <see cref="FirebaseFunctions"/> class.
        /// </summary>
        /// <param name="uri"> Google cloud functions url. E.g. 'your-application.cloudfunctions.net'. </param>
        /// <param name="options"> Optional settings. </param>
        public FirebaseFunctions(string uri, FirebaseFunctionsOptions options = null)
        {
            this.Uri = uri;
            this.Options = options ?? new FirebaseFunctionsOptions();
        }

        /// <summary>
        /// Gets the <see cref="FirebaseFunctionsOptions"/>.
        /// </summary>
        public FirebaseFunctionsOptions Options
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the google function uri.
        /// </summary>
        public string Uri
        {
            get;
            private set;
        }

        /// <summary>
        /// GetHttpsCallable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HttpsCallableReference GetHttpsCallable(string name)
        {
            return new HttpsCallableReference(this, name);
        }
    }
}
