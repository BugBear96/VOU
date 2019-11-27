using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOU.BinaryObjects;
using VOU.Branch.Dto;
using VOU.Dto;
using VOU.Voucher;
//using VOU.Voucher.Dto;

namespace VOU.Branch
{
    public class BranchAppService : VOUAppServiceBase, IBranchAppService
    {

        private readonly IStateManager _stateManager;
        private readonly ILocationManager _locationManager;
        private readonly IVoucherPlatformManager _voucherPlatformManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public BranchAppService(
            IStateManager stateManager,
            ILocationManager locationManager,
            IVoucherPlatformManager voucherPlatformManager,
            IBinaryObjectManager binaryObjectManager)
        {
            _stateManager = stateManager;
            _locationManager = locationManager;
            _voucherPlatformManager = voucherPlatformManager;
            _binaryObjectManager = binaryObjectManager;
    }

        public async Task<ListResultDto<StateListDto>> GetStates(GetStateInput input)
        {
            var items = await _stateManager.States
                .Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.StateName.Contains(input.Keyword) : true)
                .Select(x => new StateListDto
                {
                    Id = x.Id,
                    StateName = x.StateName,
                    Cities = x.Cities.Select(y => new CityDto
                    {
                        CityName = y.CityName
                    }).ToList()
                })
                .OrderBy(x => x.StateName)
                .ToListAsync();

            return new ListResultDto<StateListDto>(items);

        }

        public async Task<StateEditDto> GetStateForEdit(EntityDto input)
        {
            var state = await _stateManager.FindAsync(input.Id);
            if (state == null)
                throw new UserFriendlyException(L("InvalidState"));

            return ObjectMapper.Map<StateEditDto>(state);

        }

        public async Task<EntityDto> CreateOrUpdateState(StateEditDto input)
        {
            var isEdit = input.Id > 0;

            State state;
            if (isEdit)
            {
                state = await _stateManager.FindAsync(input.Id);
                if (state == null)
                    throw new UserFriendlyException(L("InvalidState"));
            }
            else state = new State(input.StateName);

            state.UpdateState(input.StateName);

            state.ClearCities();
            foreach (var item in input.Cities)
                state.AddCity(item.CityName);

            if (!isEdit)
                await _stateManager.CreateAsync(state);

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto(state.Id);
        }


        public async Task<ListResultDto<LocationListDto>> GetLocations(GetLocationInput input)
        {

            var items = await _locationManager.Locations
                .Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.Name.Contains(input.Keyword) : true)
                .Select(x => new LocationListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Postcode = x.Postcode,
                    State = new StateDto {
                        Id = x.State.Id,
                        StateName = x.State.StateName
                    },
                    City = new CityDto
                    {
                        CityName = x.City.CityName
                    },
                    CoverPictureId = x.CoverPictureId ?? 0,
                    Remarks = x.Remarks
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return new ListResultDto<LocationListDto>(items);
            
        }

        public async Task<LocationEditDto> GetLocationForEdit(EntityDto input)
        {
            var location = await _locationManager.FindLocationAsync(input.Id);
            if (location == null)
                throw new UserFriendlyException(L("InvalidLocation"));

            var dto = ObjectMapper.Map<LocationEditDto>(location);
            dto.TimeTableJson = location.GetSettings();
            //var items = ObjectMapper.Map<LocationEditDto>(location)

            return dto;
        }

        public async Task<EntityDto> CreateOrUpdateLocation(LocationEditDto input)
        {
            var isEdit = input.Id > 0;

            Location location;
            if (isEdit)
            {
                location = await _locationManager.FindLocationAsync(input.Id);
                if (location == null)
                    throw new UserFriendlyException(L("InvalidLocation"));
            }
            else location = new Location(input.Name);

            //location.VoucherPlatforms = new List<BranchWithVoucherPlatform>();
            location.UpdateName(input.Name);
            location.UpdateAddress(input.Address, input.Postcode);
            location.UpdateSettings(input.TimeTableJson);

            State state = state = await _stateManager.FindAsync(input.State.Id);
            City city = state.Cities.Where(x => x.CityName == input.City.CityName).FirstOrDefault();

            // if state city null
            location.UpdateState(state);
            location.UpdateCity(city);


            if (!isEdit)
                await _locationManager.CreateLocationAsync(location);

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto(location.Id);
        }

        public async Task<EntityDto> CreateOrUpdateVoucherPlatform(BranchWithVoucherPlatformEditDto input)
        {
            var isEdit = input.Id > 0;

            BranchWithVoucherPlatform platform;
            Location location;
            VoucherPlatform voucherPlatform;
            if (isEdit)
            {
                platform = await _locationManager.FindVoucherPlatformAsync(input.Id);
                if (platform == null)
                    throw new UserFriendlyException(L("InvalidVoucherPlatform"));
            }
            else
            {
                location = await _locationManager.FindLocationAsync(input.LocationId);
                voucherPlatform = await _voucherPlatformManager.FindAsync(input.VoucherPlatformId);
                platform = new BranchWithVoucherPlatform(location, voucherPlatform);
            }

            //location.VoucherPlatforms = new List<BranchWithVoucherPlatform>();
            platform.UpdateTime(input.Start, input.End);


            if (!isEdit)
                await _locationManager.CreateBranchWithVoucherPlatformAsync(platform);

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto(platform.Id);
        }

        public async Task<ListResultDto<BranchWithVoucherPlatformListDto>> GetVoucherPlatformByBranch(EntityDto input)
        {
            var items = await _locationManager.VoucherPlatforms
                .Where(x => x.Location.Id == input.Id)
                .Select(x => new BranchWithVoucherPlatformListDto
                {
                    Id = x.Id,
                    Start = x.Start,
                    End = x.End,
                    VoucherPlatform = new VOU.Voucher.Dto.VoucherPlatformListDto
                    {
                        Id = x.VoucherPlatform.Id,
                        Name = x.VoucherPlatform.Name,
                        Description = x.VoucherPlatform.Description,
                        ArchivedTime = x.VoucherPlatform.ArchivedTime,
                        CoverPictureId = x.VoucherPlatform.CoverPictureId ?? 0
                    }

                })
                .OrderBy(x => x.Start)
                .ToListAsync();

            return new ListResultDto<BranchWithVoucherPlatformListDto>(items);
        }

        public async Task<BranchWithVoucherPlatformEditDto> GetVoucherPlatformForEdit(EntityDto input)
        {
            var platform = await _locationManager.FindVoucherPlatformAsync(input.Id);
            if (platform == null)
                throw new UserFriendlyException(L("InvalidVoucherPlatform"));

            return ObjectMapper.Map<BranchWithVoucherPlatformEditDto>(platform);
        }


        //[AbpAuthorize(PermissionNames.Administration_Facilities_Manage)]
        public async Task<UpdateCoverPictureOutput> UpdateCoverPicture(UpdateCoverPictureInput input)
        {
            var location = await _locationManager.FindLocationAsync(input.TargetId);
            if (location == null)
                throw new UserFriendlyException(L("InvalidAction"));

            byte[] data;
            using (var ms = new MemoryStream())
            {
                var newWidth = input.Width;
                var newHeight = input.Height;

                using (var newImg = new Bitmap(newWidth, newHeight))
                {
                    var fullPath = Path.Combine(Path.GetTempPath(), input.FileName);
                    using (var img = Image.FromFile(fullPath))
                    using (var g = Graphics.FromImage(newImg))
                    {
                        g.DrawImage(img,
                            new Rectangle(0, 0, newWidth, newHeight),
                            new Rectangle(input.X, input.Y, input.Width, input.Height),
                            GraphicsUnit.Pixel);
                    }
                    newImg.Save(ms, ImageFormat.Jpeg);
                }

                data = ms.ToArray();
            }

            var binaryObjectId = await _binaryObjectManager.SaveAsync(BinaryObjectTypes.BranchCoverPicture, data, "image/jpeg");

            if (location.CoverPictureId != null)
                await _binaryObjectManager.DeleteAsync(location.CoverPictureId.Value, BinaryObjectTypes.BranchCoverPicture);

            location.UpdateCoverPicture(binaryObjectId);
            await _locationManager.UpdateBranchAsync(location);

            return new UpdateCoverPictureOutput
            {
                TargetId = location.Id,
                CoverPictureId = location.CoverPictureId
            };
        }



    }
}
