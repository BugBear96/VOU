
using Abp.Application.Services.Dto;

namespace VOU.Dto
{
    public class PagedResultInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
