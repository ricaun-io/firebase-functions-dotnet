using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Firebase.Functions.Example
{
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
