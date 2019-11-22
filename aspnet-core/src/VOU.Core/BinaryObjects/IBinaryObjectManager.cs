using Abp.Domain.Services;
using System.Threading.Tasks;

namespace VOU.BinaryObjects
{
    public interface IBinaryObjectManager : IDomainService
    {
        Task<long> SaveAsync(string type, byte[] content, string contentType);

        Task<BinaryObject> GetAsync(long id, string type);

        Task DeleteAsync(BinaryObject binaryObject);

        Task DeleteAsync(long id, string type);
    }
}