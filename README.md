# FirebaseFunctions.net

[![Visual Studio 2019](https://img.shields.io/badge/Visual%20Studio%202019-16.11.7+-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Publish](../../actions/workflows/Publish.yml/badge.svg)](../../actions)
[![Develop](../../actions/workflows/Develop.yml/badge.svg)](../../actions)
[![Release](https://img.shields.io/nuget/v/FirebaseFunctions.net?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/FirebaseFunctions.net)

Firebase functions library for C# to work with [`https.onCall`](https://firebase.google.com/docs/functions/callable-reference).

For Authenticating with Firebase checkout the [Firebase Authentication library](https://github.com/step-up-labs/firebase-authentication-dotnet).

## Installation
```csharp
// Install release version
Install-Package FirebaseFunctions.net
```

## Supported frameworks
* .NET Standard 1.1 - see https://github.com/dotnet/standard/blob/master/docs/versions.md for compatibility matrix.

## Supported scenarios
* Firbase Functions [`https.onCall`](https://firebase.google.com/docs/functions/callable-reference)

## Usage

### Firebase.Functions

```csharp
string FirebaseApiKey = "###########";
string FirebaseFunctions = "###########.cloudfunctions.net";
string FirebaseCallFunction = "Test";

var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseApiKey));
var auth = await authProvider.SignInAnonymouslyAsync();

var functions = new FirebaseFunctions(FirebaseFunctions,
    new FirebaseFunctionsOptions()
    {
        AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken),
    });

var function = functions.GetHttpsCallable(FirebaseCallFunction);

var response = await function.CallAsync();
Console.WriteLine($"Response: {response}");
```

### Firebase Functions Test

```js
exports.Test = functions.https.onCall((data, context) => {
    // check request is made by an admin
    if (context.auth.token.admin !== true) {
        return { message: 'This user is not admin.' }
    }
    return {
        message: `Admin success!`
    }
});

```

## Release

* [Latest release](../../releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](../../stargazers)!