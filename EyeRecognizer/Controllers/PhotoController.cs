using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EyeRecognizer.Configuration;
using EyeRecognizer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenCvSharp;

namespace EyeRecognizer.Controllers
{
    public class PhotoController : Controller
    {
        private AppSettings AppSettings { get; set; }

        public PhotoController(IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value;
        }

        public IActionResult Index()
        {
            var folders = GetFolderList();
            return View(folders);
        }

        public IActionResult GetFoldersJson()
        {
            return Json(GetFolderList());
        }

        public IActionResult DisplayList(string path)
        {
            return View("Index", GetFolderList(path));
        }

        private List<FolderViewModel> GetFolderList(string path = null)
        {
            if (path == null)
                path = AppSettings.PhotoPath;
            else
                path = AppSettings.PhotoPath + @"\" + path;
            var folders = Directory.GetDirectories(path);
            var files = Directory.GetFiles(path);

            folders = folders.Concat(files).ToArray();
            var folderList = folders.Select(n => GetFolderViewModel(n));


            return folderList.ToList();
        }

        private FolderViewModel GetFolderViewModel(string n)
        {
            return new FolderViewModel(n.Replace(AppSettings.PhotoPath, ""), Path.GetFileName(n));
        }

        public IActionResult GetContent(string path)
        {
            if (path != null && path.Contains(".jpg"))
            {
                byte[] fileBytes = GetBytes(ref path);
                return File(fileBytes, "image/jpg", Path.GetFileName(path));
            }
            return Json(GetFolderList(path));
        }

        private byte[] GetBytes(ref string path)
        {
            path = AppSettings.PhotoPath + @"\" + path;
            var fileBytes = System.IO.File.ReadAllBytes(path);
            return fileBytes;
        }

        public IActionResult GetHistogram(string path)
        {
            return File(CreateHistogram(path), "image/png", "histogram.png");
        }

        private byte[] CreateHistogram(string path)
        {
            Mat src = Cv2.ImRead(AppSettings.PhotoPath + @"\" + path, ImreadModes.GrayScale);

            // Histogram view
            const int Width = 200, Height = 200;
            Mat render = new Mat(new Size(Width, Height), MatType.CV_8UC3, Scalar.All(255));

            // Calculate histogram
            Mat hist = new Mat();
            int[] hdims = { 256 }; // Histogram size for each dimension
            Rangef[] ranges = { new Rangef(0, 256), }; // min/max 
            Cv2.CalcHist(
                new Mat[] { src },
                new int[] { 0 },
                null,
                hist,
                1,
                hdims,
                ranges);

            // Get the max value of histogram
            double minVal, maxVal;
            Cv2.MinMaxLoc(hist, out minVal, out maxVal);

            Scalar color = Scalar.All(100);
            // Scales and draws histogram
            hist = hist * (maxVal != 0 ? Height / maxVal : 0.0);
            for (int j = 0; j < hdims[0]; ++j)
            {
                int binW = (int)((double)Width / hdims[0]);
                render.Rectangle(
                    new Point(j * binW, render.Rows),
                    new Point((j + 1) * binW, render.Rows - (int)(hist.Get<float>(j))),
                    color,
                    -1);
            }

            using (new Window("Image", WindowMode.AutoSize | WindowMode.FreeRatio, src))
            using (new Window("Histogram", WindowMode.AutoSize | WindowMode.FreeRatio, render))
                return render.ToBytes();

        }
    }
}