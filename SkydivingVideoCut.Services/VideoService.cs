using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Video.FFMPEG;
using SkidivingVideoCut.Domain;

namespace SkydivingVideoCut.Services
{
    public class VideoService
    {
        const string ffmpegCmd = @"C:\FFMPEG\bin\ffmpeg.exe";

        public Video OpenVideo(string path)
        {
            var reader = new VideoFileReader();
            reader.Open(path);

            var frameCount = reader.FrameCount;
            var frameRate = Convert.ToInt32(reader.FrameRate.Value);

            return new Video(path, frameRate, frameCount);
        }

        public Task<Bitmap> GetFrame(Video video, long frame)
        {
            var reader = new VideoFileReader();
            reader.Open(video.Path);

            return Task.Run(() => reader.ReadVideoFrame(Convert.ToInt32(frame)));
        }

        public Task<string> GetFrames(Video video, double framePerSecond = 3)
        {
            return Task.Run(() =>
            {
                var outputDir = Path.Combine(Path.GetDirectoryName(video.Path), video.Name);
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = ffmpegCmd;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = $"-y -i {video.Path} -r {framePerSecond.ToString(CultureInfo.GetCultureInfo("en-GB"))} -qscale 0 {outputDir}\\%05d.jpg";
                startInfo.CreateNoWindow = true;


                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    return outputDir;
                }
            });
        }
    }
}
