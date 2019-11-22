using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Dto
{
    public class UpdateCoverPictureInput
    {
        public int TargetId { get; set; }
        public string FileName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
