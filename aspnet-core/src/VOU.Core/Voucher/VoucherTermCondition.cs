using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Voucher
{
   
    public class VoucherTermCondition
    {
        public VoucherTermCondition()
        {
            // EMPTY
        }

        public VoucherTermCondition(
           List<String> terms)
        {
            Terms = terms ?? new List<String>();
        }

        public List<String> Terms { get; set; }
            = new List<String>();

    }
}
