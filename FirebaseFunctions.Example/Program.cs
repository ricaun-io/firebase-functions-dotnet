using Firebase.Auth;
using System;
using System.Threading.Tasks;

namespace Firebase.Functions.Example
{
    public class Program
    {
        public static string FirebaseApiKey = "###########";
        public static string FirebaseFunctions = "###########.cloudfunctions.net";
        public static string FirebaseCallFunction = "Test";

        static void Main(string[] args)
        {
            var task = Task.Run(FirebaseFunctionsTest);
            task.GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static async void FirebaseFunctionsTest()
        {
            Console.WriteLine($"FirebaseFunctionsTest");
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));

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
            Console.WriteLine($"Response: {response}");

            Console.WriteLine($"DeleteUser");
            await authProvider.DeleteUserAsync(auth.FirebaseToken);
        }
    }
}
