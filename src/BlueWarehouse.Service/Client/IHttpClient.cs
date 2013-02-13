using BlueWarehouse.Model;

namespace BlueWarehouse.Service.Client
{
    public interface IHttpClient
    {
        /// <summary>
        /// Attempts to authenticate using supplied credentials, throws ClientAuthenticationFailure on error.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        void Authenticate(Credentials credentials);

        /// <summary>
        /// Fetches the artifact file data.
        /// </summary>
        /// <param name="artifact"></param>
        void FetchArtifact(ref Artifact artifact);
    }
}
