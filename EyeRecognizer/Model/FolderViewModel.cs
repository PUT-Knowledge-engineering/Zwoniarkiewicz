using EyeRecognizer.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

            if (!IsFolder)
                Metadata = new PhotoMetadata
                {
                    Name = name,
                    Directory = ShortPath,
                    Side = "L",
                    Phase = 2,
                    Size = (new FileInfo("D:\\#EyePhoto" + "\\" + ShortPath).Length / 1024).ToString() + "kB"
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
