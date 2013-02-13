using System;
using System.IO;
using System.Net;
using BlueWarehouse.Model;
using BlueWarehouse.Model.Error;
using BlueWarehouse.Service.Client;

namespace BlueWarehouse.Service.Bamboo
{
    public class BambooClient : IHttpClient
    {
        private CookieContainer _cookieContainer;

        /// <summary>
        /// Downloads the artifact and saves the file data in memory.
        /// </summary>
        /// <param name="artifact"></param>
        public void FetchArtifact(ref Artifact artifact)
        {
            
            
            
            /*
             * Setup.
             */
            Stream remoteStream = null;
            WebResponse response = null;
            artifact.FileStream = new MemoryStream(); // Instantiate the artifacts' FileStream property.



            /*
             * Begin fetching the file from Bamboo.
             */
            try
            {
                // Create a request for the specified remote file name
                var request = (HttpWebRequest)WebRequest.Create(artifact.URL);

                if (_cookieContainer != null)
                    request.CookieContainer = _cookieContainer;

                // Send the request to the server and retrieve the WebResponse object 
                response = request.GetResponse();
                // Once the WebResponse object has been retrieved,
                // get the stream object associated with the response's data
                remoteStream = response.GetResponseStream();

                // Allocate a 1k buffer
                var buffer = new byte[1024];
                int bytesRead;

                // Simple do/while loop to read from stream until no bytes are returned
                do
                {
                    // Read data (up to 1k) from the stream
                    bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                    // Write the data to the local file
                    artifact.FileStream.Write(buffer, 0, bytesRead);
                } while (bytesRead > 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            finally
            {
                // Close the response and streams objects here 
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
            }
        }

        public void Authenticate(Credentials credentials)
        {
            if (credentials.AuthenticationType == AuthenticationType.Cookie)
            {
                const string authURL = "https://bamboonet.mogul.se/?os_username={0}&os_password={1}";

                var authRequest =
                    (HttpWebRequest)
                    WebRequest.Create(string.Format(authURL, credentials.Username, credentials.Password));
                authRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705)";
                authRequest.CookieContainer = new CookieContainer();
                
                authRequest.GetResponse();
                _cookieContainer = authRequest.CookieContainer;

                if (_cookieContainer != null)
                    return;

                throw new ClientAuthenticationFailure("Authentication failed, no cookie was set.");
            }

            throw new ClientAuthenticationFailure("Bamboo only supports cookie authentication, please reconfigure your Credentials.");
        }
    }
}
