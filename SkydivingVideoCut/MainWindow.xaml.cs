using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SkidivingVideoCut.Domain.Dto;
using SkydivingVideoCut.Services;
using Path = System.IO.Path;

namespace SkydivingVideoCut
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Job();

            //List<Task> tasks = new List<Task>();
            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //for (int i = 1; i < video.FrameCount - 1; i++)
            //{
            //    var task = videoService.GetFrame(video, i).ContinueWith(taskBitmap =>
            //    {
            //        if (taskBitmap.IsCompleted)
            //            taskBitmap.Result.Dispose();
            //    });
            //    tasks.Add(task);
            //}

            //Task.WhenAll(tasks).ContinueWith(_ => TextBlock.Text = $"{sw.ElapsedMilliseconds} ms");
        }

        private async void Job()
        {
            var videoService = new VideoService();
            var customVisionService = new CustomVisionService();

            var video = videoService.OpenVideo(@"D:\Tmp\Saut2.MP4");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var imagesPath = await videoService.GetFrames(video, 0.5);
            TextBlock.Text = $"{sw.ElapsedMilliseconds} ms";

            var tasks = new List<Task<Tuple<CustomVisionResult, int>>>();

            foreach (var image in Directory.EnumerateFiles(imagesPath))
            {
                tasks.Add(customVisionService.Analyse(image).ContinueWith(task => new Tuple<CustomVisionResult, int>(task.Result, int.Parse(Path.GetFileNameWithoutExtension(image)))));
            }

            var res = await Task.WhenAll(tasks);

            TextBlock.Text =
                res
                    .Select(r => $"{r.Item2 / 0.5} : { r.Item1.predictions.First(p => p.tagName == "Plane").probability * 100:000.00}% ||| Jump : {r.Item1.predictions.First(p => p.tagName == "Jump").probability * 100:000.00}% ||| Canopy : {r.Item1.predictions.First(p => p.tagName == "Canopy").probability * 100:000.00}%")
                    .Aggregate((s, s1) => s + "\n" + s1);

        }
    }
}
