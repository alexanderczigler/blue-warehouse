using System;
using System.Linq;
using BlueWarehouse.Model;
using BlueWarehouse.Model.Error;
using BlueWarehouse.Service.Bamboo;
using BlueWarehouse.Service.Client;
using BlueWarehouse.Service.Packaging;

namespace BlueWarehouse
{
    internal class Program
    {
        private static IHttpClient _httpClient;
        private static IPackageManager _packageManager;
        private static Credentials _credentials;

        private static string[] _args;

        private static void Main(string[] args)
        {
            _args = args;
            _packageManager = new ZipPackageManager();


            /*
             * Run!
             */
            Console.Clear();
            Console.WriteLine("BlueWarehouse");
            Console.WriteLine("=============");
            Console.WriteLine();
            SetupClient();


            /*
             * Credentials.
             */
            SetupCredentials();
            PromptForCredentials();
            Console.WriteLine();
            Console.WriteLine();
            Authenticate();



            /*
             * Download artifact.
             */
            Console.WriteLine();
            Console.WriteLine("Downloading artifact.");
            Console.WriteLine();

            Artifact artifact = AssembleArtifact();
            _httpClient.FetchArtifact(ref artifact);

            if (!_packageManager.DeployArtifact(artifact))
                Console.WriteLine("Failed to extract files.");

            // Done.
            Console.WriteLine("OK - bye!");
            Environment.Exit(0);
        }


        /// <summary>
        ///     Creates the artifact object.
        /// </summary>
        /// <returns></returns>
        private static Artifact AssembleArtifact()
        {
            return new Artifact
                       {
                           TargetDirectory = GetArgumentValue(Argument.UnpackFolder),
                           URL = GetArgumentValue(Argument.Artifact)
                       };
        }


        /// <summary>
        ///     If an authenticationtype is set - will attempty to authenticate with client.
        /// </summary>
        private static void Authenticate()
        {
            if (_credentials.AuthenticationType == AuthenticationType.None) return;

            try
            {
                Console.WriteLine();
                Console.WriteLine("Authenticating.");
                Console.WriteLine();

                _httpClient.Authenticate(_credentials);
            }
            catch (ClientAuthenticationFailure authenticationFailure)
            {
                Console.WriteLine("Authentication failure!");
                Console.WriteLine(authenticationFailure.Reason);
                Console.Read();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error enountered during authentication.");
                Console.WriteLine(ex.Message);
                Console.Read();
                Environment.Exit(1);
            }
        }


        /// <summary>
        ///     If asked to prompt for credentials - allows user to enter Username and Password.
        /// </summary>
        private static void PromptForCredentials()
        {
            if (string.IsNullOrEmpty(GetArgumentValue(Argument.AuthPrompt))) return;

            // Verify authentication data.
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = string.Empty;
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.KeyChar == 13)
                    break;

                password = password + key.KeyChar;

                if (key.KeyChar == 8)
                    password = password.Substring(0, ((password.Length - 2) >= 0) ? password.Length - 2 : 0);
            }

            _credentials.Username = username;
            _credentials.Password = password;
        }


        /// <summary>
        ///     Sets up the Credentials object.
        /// </summary>
        private static void SetupCredentials()
        {
            string authType = GetArgumentValue(Argument.AuthType);
            AuthenticationType authenticationType = (!string.IsNullOrEmpty(authType))
                                                        ? (AuthenticationType)
                                                          Enum.Parse(typeof (AuthenticationType), authType)
                                                        : AuthenticationType.None;

            _credentials = new Credentials
                               {
                                   AuthenticationType = authenticationType,
                                   Username = GetArgumentValue(Argument.Username),
                                   Password = GetArgumentValue(Argument.Password)
                               };
        }


        public static void SetupClient()
        {
            // TODO: This could be made prettier!
            switch (GetArgumentValue(Argument.Client))
            {
                case @"Bamboo":
                    _httpClient = new BambooClient();
                    break;
            }
        }


        /// <summary>
        ///     Helper method to fetch argument values.
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private static string GetArgumentValue(Argument argument)
        {
            foreach (
                string arg in _args.Where(arg => arg.StartsWith(string.Format("--{0}", argument.ToString().ToLower()))))
                return arg.Replace(string.Format("--{0}=", argument.ToString().ToLower()), string.Empty);

            // Default to empty string.
            return string.Empty;
        }



        /// <summary>
        /// The command line arguments used.
        /// </summary>
        private enum Argument
        {
            Client,
            Artifact,
            UnpackFolder,
            AuthType,
            AuthPrompt,
            Username,
            Password
        }
    }
}