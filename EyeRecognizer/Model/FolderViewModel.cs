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

        //public SpecificType Type { get; set; }

        public bool IsFolder { get; set; }

        public FolderViewModel(string shortPath, string name = null)
        {
            ShortPath = shortPath;
            Name = name;

            if (name.Contains(".jpg") || name.Contains(".png"))
                IsFolder = false;
            
            else
                IsFolder = true;
            
            Metadata = new PhotoMetadata
            {
                Name = name,
                Directory = ShortPath,
                Side = "L",
                Phase = 2,
                Size = "20 kb"
            };
        }

        public PhotoMetadata Metadata { get; set; }

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

public enum SpecificType
{
    Folder,
    Photo
}
