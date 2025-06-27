using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class PlatformService : IPlatformService
    {
        public List<EnumValueDTO> GetRoleEnums()
        {
            return Enum.GetValues<UserRole>().Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"Role_{e}"))).ToList();
        }
    }
}
