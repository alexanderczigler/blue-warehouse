using System;
using System.IO;
using BlueWarehouse.Model;
using Ionic.Zip;

namespace BlueWarehouse.Service.Packaging
{
    /// <summary>
    /// Implementation using IonicZip - handles zip files.
    /// </summary>
    public class ZipPackageManager : IPackageManager
    {
        public bool DeployArtifact(Artifact artifact)
        {
            try
            {
                // Make sure the file data is read from the beginning. Ionic.Zip does not do this.
                artifact.FileStream.Seek(0, new SeekOrigin());

                /*
                 * Unzip.
                 */
                using (var zipFile = ZipFile.Read(artifact.FileStream))
                {
                    foreach (var zipEntry in zipFile)
                    {
                        zipEntry.Extract(artifact.TargetDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
