using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Inject;

namespace PandoraPatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Pandora Patcher";

            if (args.Length != 1)
            {
                Console.WriteLine("Invalid parameter count!");
                return;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine($"Invalid Faeria install directory \"{args[0]}\"!");
                return;
            }

            string managedPath = Path.Combine(args[0], @"Faeria_Data\Managed");
            if (!Directory.Exists(managedPath))
            {
                Console.WriteLine($"Invalid Faeria install directory \"{managedPath}\"!");
                return;
            }

            string assemblyPath = Path.Combine(managedPath, "Assembly-CSharp.dll");
            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"Failed to find Faeria assembly \"{assemblyPath}\"!");
                return;
            }

            if (!File.Exists("PandoraPayload.dll"))
            {
                Console.WriteLine("Failed to find Pandora payload assembly!");
                return;
            }

            string assemblyOriginalPath = Path.Combine(managedPath, "Assembly-CSharp.dll.ORIGINAL");
            bool alreadyPatched = File.Exists(assemblyOriginalPath);

            AssemblyDefinition target = AssemblyLoader.LoadAssembly(alreadyPatched ? assemblyOriginalPath : assemblyPath);
            PatchTarget(target, AssemblyLoader.LoadAssembly("PandoraPayload.dll"));

            if (!alreadyPatched)
            {
                Console.WriteLine("Backing up original assembly...");
                File.Copy(assemblyPath, assemblyOriginalPath);

                Console.WriteLine("Moving public RSA key to install directory...");
                File.Copy("Client.pfx", Path.Combine(args[0], "Client.pfx"));
            }

            Console.WriteLine("Moving payload to install directory...");

            string payloadPath = Path.Combine(managedPath, "PandoraPayload.dll");
            File.Delete(payloadPath);
            File.Copy("PandoraPayload.dll", payloadPath);

            Console.WriteLine("Saving target binary to install directory....");
            target.Write(assemblyPath);

            Console.WriteLine("Finished!");
            Console.Read();
        }

        private static void PatchTarget(AssemblyDefinition target, AssemblyDefinition source)
        {
            MethodDefinition targetGetTokenSteamId = target.GetTypeDefinition("Abrakam.AuthenticationManager").GetMethodDefinition("GetTokenSteamId");
            targetGetTokenSteamId.Body.Instructions.Clear();

            ILProcessor il = targetGetTokenSteamId.Body.GetILProcessor();
            il.Append(il.Create(OpCodes.Ldstr, ""));
            il.Append(il.Create(OpCodes.Ret));

            Console.WriteLine("Patched Steam Id check");

            target.ReplaceString("https://api.abrakam.com/v20170407/api/faeria/token/", "http://localhost:2200/pandora/auth/");
            Console.WriteLine("Patched authentication API address");

            MethodDefinition targetNetworkAwake = target.GetTypeDefinition("Abrakam.NetworkManager").GetMethodDefinition("Awake");
            MethodDefinition sourceSetPublicKey = source.GetTypeDefinition("PandoraPayload.Cryptography.CryptoProvider").GetMethodDefinition("SetPublicKey");
            targetNetworkAwake.InjectWith(sourceSetPublicKey, codeOffset: -1);

            Console.WriteLine("Patched public RSA command key");

            TypeDefinition targetWelcomeLayout = target.GetTypeDefinition("Abrakam.WelcomeLayout");
            TypeDefinition sourceWelcomeLayout = source.GetTypeDefinition("PandoraPayload.Layout.WelcomeLayout");

            MethodDefinition targetSendSignUpMessage = targetWelcomeLayout.GetMethodDefinition("SendSignUpMessage");
            MethodDefinition sourceSendSignUpMessage = sourceWelcomeLayout.GetMethodDefinition("SendSignUpMessage");

            targetSendSignUpMessage.Body.Instructions.Clear();
            il = targetSendSignUpMessage.Body.GetILProcessor();
            il.Append(il.Create(OpCodes.Ret));

            targetSendSignUpMessage.InjectWith(sourceSendSignUpMessage);

            Console.WriteLine("Patched SignUp command");

            MethodDefinition targetSendSignInMessage = targetWelcomeLayout.GetMethodDefinition("SendTokenSignInMessage");
            MethodDefinition sourceSendSignInMessage = sourceWelcomeLayout.GetMethodDefinition("SendTokenSignInMessage");

            targetSendSignInMessage.Body.Instructions.Clear();
            il = targetSendSignInMessage.Body.GetILProcessor();
            il.Append(il.Create(OpCodes.Ret));

            targetSendSignInMessage.InjectWith(sourceSendSignInMessage);

            Console.WriteLine("Patched SignIn command");

            MethodDefinition targetOnNewAccountButton = targetWelcomeLayout.GetMethodDefinition("OnNewAccountButton");
            MethodDefinition sourceOnNewAccountButton = sourceWelcomeLayout.GetMethodDefinition("OnNewAccountButton");
            targetOnNewAccountButton.InjectWith(sourceOnNewAccountButton, flags: InjectFlags.ModifyReturn);

            Console.WriteLine("Patched account creation checks");

            MethodDefinition targetAwake = targetWelcomeLayout.GetMethodDefinition("Awake");
            MethodDefinition sourceAwake = sourceWelcomeLayout.GetMethodDefinition("Awake");
            targetAwake.InjectWith(sourceAwake);

            Console.WriteLine("Patched world server IP");
        }
    }
}
