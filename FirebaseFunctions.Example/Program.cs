using Firebase.Auth;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Firebase.Functions.Example
{
    public class Program
    {
        public static string FirebaseApiKey = "###########";
        public static string FirebaseFunctions = "###########.cloudfunctions.net";
        public static string FirebaseCallFunction = "Test";

        static void Main(string[] args)
        {
            Console.WriteLine("--------------------------");
            Console.WriteLine("Firebase.Functions.Example");
            Console.WriteLine("--------------------------");

            ReadFirebaseConfig();

            Console.WriteLine("--------------------------");

            var task = Task.Run(AsyncFirebaseFunctionsTest);
            task.GetAwaiter().GetResult();

            Console.ReadKey();
        }

        private static void ReadFirebaseConfig()
        {
            var firebaseJson = new FirebaseConfig();
            var firebaseConfigData = firebaseJson.GetFirebaseConfig();

            FirebaseApiKey = firebaseConfigData.FirebaseApiKey;
            FirebaseFunctions = firebaseConfigData.FirebaseFunctions;
            FirebaseCallFunction = firebaseConfigData.FirebaseCallFunction;

            Console.Write($"FirebaseApiKey({Hidden(FirebaseApiKey)}) = ");
            FirebaseApiKey = ReadLine(FirebaseApiKey);

            Console.Write($"FirebaseFunctions({Hidden(FirebaseFunctions)}) = ");
            FirebaseFunctions = ReadLine(FirebaseFunctions);

            Console.Write($"FirebaseCallFunction({FirebaseCallFunction}) = ");
            FirebaseCallFunction = ReadLine(FirebaseCallFunction);

            firebaseConfigData.FirebaseApiKey = FirebaseApiKey;
            firebaseConfigData.FirebaseFunctions = FirebaseFunctions;
            firebaseConfigData.FirebaseCallFunction = FirebaseCallFunction;

            firebaseJson.SetFirebaseConfig(firebaseConfigData);
        }


        private static async void AsyncFirebaseFunctionsTest()
        {
            try
            {
                Console.WriteLine($"FirebaseAuthProvider");
                var authProvider = new FirebaseAuthProvider(new Auth.FirebaseConfig(FirebaseApiKey));

                Console.WriteLine($"SignInAnonymously");
                var auth = await authProvider.SignInAnonymouslyAsync();

                var functions = new FirebaseFunctions(FirebaseFunctions,
                    new FirebaseFunctionsOptions()
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken),
                    });

                var function = functions.GetHttpsCallable(FirebaseCallFunction);
                Console.WriteLine($"CallAsync: {FirebaseCallFunction}");

                var response = await function.CallAsync();
                Console.WriteLine($"CallResponse: {response}");

                Console.WriteLine($"DeleteAnonymously");
                await authProvider.DeleteUserAsync(auth.FirebaseToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex}");
            }
        }

        private static string ReadLine(string text)
        {
            var content = Console.ReadLine();
            if (content != "") text = content;
            return text;
        }
        private static string Hidden(string content)
        {
            var len = 15;
            var str = "";
            if (content.Length > len)
            {
                str = content.Substring(0, len);
                for (int i = 0; i < content.Length - len; i++)
                    str += "#";
            }
            return str;
        }
    }

    public class FirebaseConfig
    {
        public string FileName => "FirebaseConfig.json";
        public string Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string GetFilePath() => Path.Combine(Location, FileName);

        public FirebaseConfigData GetFirebaseConfig()
        {
            var fileName = GetFilePath();
            if (File.Exists(fileName))
            {
                var content = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<FirebaseConfigData>(content);
            }
            return new FirebaseConfigData();
        }

        public void SetFirebaseConfig(FirebaseConfigData firebaseJsonData)
        {
            var fileName = GetFilePath();
            var content = JsonConvert.SerializeObject(firebaseJsonData);
            File.WriteAllText(fileName, content);
        }
        public class FirebaseConfigData
        {
            public string FirebaseApiKey { get; set; } = "";
            public string FirebaseFunctions { get; set; } = "";
            public string FirebaseCallFunction { get; set; } = "";
        }
    }
}
