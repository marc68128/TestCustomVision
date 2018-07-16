using System;

namespace SkidivingVideoCut.Domain.Dto
{
    public class CustomVisionResult
    {
        public string id { get; set; }
        public string project { get; set; }
        public string iteration { get; set; }
        public DateTime created { get; set; }
        public Prediction[] predictions { get; set; }
    }

    public class Prediction
    {
        public float probability { get; set; }
        public string tagId { get; set; }
        public string tagName { get; set; }
    }
}
