using System.IO;

namespace BlueWarehouse.Model
{
    /// <summary>
    /// Represents an artifact.
    /// </summary>
    public class Artifact
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string TargetDirectory { get; set; }
        public MemoryStream FileStream { get; set; }
    }
}
