using FreeSql.DataAnnotations;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;

namespace ChipSys.Admin.Domain.LoginLog;

/// <summary>
/// ��¼��־
/// </summary>
[Database(DbNames.Log)]
[Table(Name = DbConsts.TableNamePrefix + "login_log")]
public partial class LoginLogEntity : LogAbstract
{
}
