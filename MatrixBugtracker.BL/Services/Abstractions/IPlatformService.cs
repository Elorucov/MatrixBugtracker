using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IPlatformService
    {
        List<EnumValueDTO> GetRoleEnums();
    }
}
