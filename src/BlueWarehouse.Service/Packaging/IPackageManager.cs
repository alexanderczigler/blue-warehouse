using BlueWarehouse.Model;

namespace BlueWarehouse.Service.Packaging
{
    public interface IPackageManager
    {
        /// <summary>
        /// Unpacks the artifact into its Target folder, using whatever implementation that fits the package type.
        /// </summary>
        /// <param name="artifact"></param>
        /// <returns></returns>
        bool DeployArtifact(Artifact artifact);
    }
}