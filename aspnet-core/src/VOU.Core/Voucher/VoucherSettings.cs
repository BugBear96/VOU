using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.Voucher
{

    [Serializable]
    public class VoucherSettings
    {
        public List<VoucherTermCondition> TermConditions { get; set; }
            = new List<VoucherTermCondition>();
    }
}
