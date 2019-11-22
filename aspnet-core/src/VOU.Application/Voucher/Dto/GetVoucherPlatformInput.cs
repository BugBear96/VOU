using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Voucher.Dto
{

    public class GetVoucherPlatformInput
    {
        public string Keyword { get; set; }
        public bool ShowArchived { get; set; }
    }
}
