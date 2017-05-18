using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EyeRecognizer.Model
{
    public class FolderViewModel
    {
        public string Name { get; set; }
        public string ShortPath { get; set; }

        public FolderViewModel(string shortPath, string name = null)
        {
            ShortPath = shortPath;
            Name = name;

            Metadata = new PhotoMetadata
            {
                Name = name,
                Directory = ShortPath,
                Side = "L",
                Phase = 2,
                Size = "20 kb"
            };
        }

        public PhotoMetadata Metadata {get; set; }

        public FolderViewModel()
        {

        }
    }

    public class PhotoMetadata
    {
        public string Directory { get; set; }
        public string Name { get; set; }
        public string Side { get; set; }
        public byte Phase { get; set; }

        public string Histogram { get; set; }

        public string Size { get; set; }
    }
}
