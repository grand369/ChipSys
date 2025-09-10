using Microsoft.AspNetCore.Http;
using ChipSys.Admin.Core.Auth;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.User.Dto;
using ChipSys.Admin.Services.Auth.Dto;
using ChipSys.Admin.Services.User.Dto;

namespace ChipSys.Admin.Services.User;

/// <summary>
/// 用户接口
/// </summary>
public interface IUserService
{
    Task<UserGetOutput> GetAsync(long id);

    Task<PageOutput<UserGetPageOutput>> GetPageAsync(PageInput<UserGetPageInput> input);

    Task<AuthLoginOutput> GetLoginUserAsync(long id);

    Task<DataPermissionOutput> GetDataPermissionAsync(string? apiPath);

    Task<long> AddAsync(UserAddInput input);

    Task<long> AddMemberAsync(UserAddMemberInput input);

    Task UpdateAsync(UserUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task ChangePasswordAsync(UserChangePasswordInput input);

    Task<string> ResetPasswordAsync(UserResetPasswordInput input);

    Task SetManagerAsync(UserSetManagerInput input);

    Task UpdateBasicAsync(UserUpdateBasicInput input);

    Task<UserGetBasicOutput> GetBasicAsync();

    Task<UserGetPermissionOutput> GetPermissionAsync();

    Task<string> AvatarUpload(IFormFile file, bool autoUpdate = false);

    Task<TokenInfo> OneClickLoginAsync(string userName);
}
