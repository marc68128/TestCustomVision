using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkidivingVideoCut.Domain
{
    public class Video
    {
        public Video(string path, int frameRate, long frameCount)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);

            FrameRate = frameRate;
            FrameCount = frameCount;
        }

        public string Path { get; }
        public string Name { get; }
        public int FrameRate { get; }
        public long FrameCount { get; }
    }
}
