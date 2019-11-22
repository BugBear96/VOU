using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Partners.Dto
{
    public class UpdateProfilePictureInput
    {
        public int TenantId { get; set; }
        public string FileName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
