using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOU
{
    public abstract class VOUDomainServiceBase : DomainService
    {
        public VOUDomainServiceBase()
        {
            LocalizationSourceName = VOUConsts.LocalizationSourceName;
        }
    }
}
