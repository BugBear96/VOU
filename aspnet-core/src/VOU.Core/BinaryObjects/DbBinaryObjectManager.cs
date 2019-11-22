using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Abp.UI;
using System.Threading.Tasks;

namespace VOU.BinaryObjects
{
    public class DbBinaryObjectManager : DomainService, IBinaryObjectManager
    {
        private readonly IAbpSession _abpSession;
        private readonly IRepository<BinaryObject, long> _binaryObjectRepository;

        public DbBinaryObjectManager(
            IAbpSession abpSession,
            IRepository<BinaryObject, long> binaryObjectRepository)
        {
            _abpSession = abpSession;
            _binaryObjectRepository = binaryObjectRepository;

            LocalizationSourceName = VOUConsts.LocalizationSourceName;
        }

        public async Task DeleteAsync(long id, string type)
        {
            var binaryObject = await GetAsync(id, type);
            if (binaryObject != null)
                await DeleteAsync(binaryObject);
        }

        public async Task DeleteAsync(BinaryObject binaryObject)
        {
            await _binaryObjectRepository.DeleteAsync(binaryObject);
        }

        public async Task<BinaryObject> GetAsync(long id, string type)
        {
            var binaryObject = await _binaryObjectRepository.FirstOrDefaultAsync(id);
            if (binaryObject?.Type == type)
                return binaryObject;
            return null;
        }

        public async Task<long> SaveAsync(string type, byte[] content, string contentType)
        {
            var binaryObject = new BinaryObject
            {
                TenantId = _abpSession.TenantId,
                Type = type,
                ContentType = contentType,
                Content = content
            };

            switch (type)
            {
                case BinaryObjectTypes.ProfilePicture:
                    if (content.Length >= VOUConsts.MaxProfilePictureByteLength)
                        throw new UserFriendlyException(L("ProfilePicture_Change_Info"));
                    if (!string.Equals(contentType, "image/jpeg", System.StringComparison.InvariantCultureIgnoreCase))
                        throw new UserFriendlyException(L("ProfilePicture_Warn_FileType"));
                    break;

                case BinaryObjectTypes.TenantProfilePicture:
                case BinaryObjectTypes.VoucherPlatformCoverPicture:
                case BinaryObjectTypes.BranchCoverPicture:
                    if (!string.Equals(contentType, "image/jpeg", System.StringComparison.InvariantCultureIgnoreCase))
                        throw new UserFriendlyException(L("JpegPicture_Warn_FileType"));
                    break;

                default:
                    throw new AbpException("Unrecognized binary object types");
            }

            return await _binaryObjectRepository.InsertAndGetIdAsync(binaryObject);
        }
    }
}