using Microsoft.AspNetCore.Mvc;
using ChipSys.Admin.Core.Attributes;
using ChipSys.Admin.Core.Consts;
using ChipSys.Admin.Core.Dto;
using ChipSys.Admin.Domain.Msg;
using ChipSys.Admin.Resources;
using ChipSys.DynamicApi;
using ChipSys.DynamicApi.Attributes;
using ChipSys.Admin.Repositories;
using ChipSys.Admin.Services.Msg.Dto;
using ChipSys.Admin.Domain.User;
using ChipSys.Admin.Core.Configs;
using ChipSys.Admin.Core;

namespace ChipSys.Admin.Services.Msg;

/// <summary>
/// ��Ϣ����
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class MsgService : BaseService, IDynamicApi
{
    private readonly AdminRepositoryBase<MsgEntity> _msgRep;
    private readonly AdminRepositoryBase<UserEntity> _userRep;
    private readonly AdminRepositoryBase<MsgUserEntity> _msgUserRep;
    private readonly AdminLocalizer _adminLocalizer;

    public MsgService(
        AdminRepositoryBase<MsgEntity> msgRep,
        AdminRepositoryBase<UserEntity> userRep,
        AdminRepositoryBase<MsgUserEntity> msgUserRep,
        AdminLocalizer adminLocalizer
    )
    {
        _msgRep = msgRep;
        _userRep = userRep;
        _msgUserRep = msgUserRep;
        _adminLocalizer = adminLocalizer;
    }

    /// <summary>
    /// ��ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MsgGetOutput> GetAsync(long id)
    {
        var output = await _msgRep.Select
        .WhereDynamic(id)
        .ToOneAsync<MsgGetOutput>();

        return output;
    }

    /// <summary>
    /// ��ѯ��ҳ
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<MsgGetPageOutput>> GetPageAsync(PageInput<MsgGetPageInput> input)
    {
        var title = input.Filter?.Title;

        var list = await _msgRep.Select
        .WhereDynamicFilter(input.DynamicFilter)
        .WhereIf(title.NotNull(), a => a.Title.Contains(title))
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .Page(input.CurrentPage, input.PageSize)
        .ToListAsync(a => new MsgGetPageOutput { TypeName = a.Type.Name });

        var data = new PageOutput<MsgGetPageOutput>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// ��ѯ��Ϣ�û��б�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<List<MsgGetMsgUserListOutput>> GetMsgUserListAsync([FromQuery] MsgGetMsgUserListInput input)
    {
        var list = await _userRep.Select.From<MsgUserEntity>()
            .InnerJoin(a => a.t2.UserId == a.t1.Id)
            .Where(a => a.t2.MsgId == input.MsgId)
            .WhereIf(input.Name.NotNull(), a => a.t1.Name.Contains(input.Name))
            .OrderByDescending(a => a.t1.Id)
            .ToListAsync(a=> new MsgGetMsgUserListOutput
            {
                IsRead = a.t2.IsRead,
                ReadTime = a.t2.ReadTime,
            });

        return list;
    }

    /// <summary>
    /// �����Ϣ�û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddMsgUserAsync(MsgAddMsgUserListInput input)
    {
        var msgId = input.MsgId;
        var userIds = await _msgUserRep.Select.Where(a => a.MsgId == msgId).ToListAsync(a => a.UserId);
        var insertUserIds = input.UserIds.Except(userIds);

        if (insertUserIds != null && insertUserIds.Any())
        {
            var userMsgList = insertUserIds.Select(userId => new MsgUserEntity 
            { 
                UserId = userId, 
                MsgId = msgId 
            }).ToList();

            await _msgUserRep.InsertAsync(userMsgList);

            //������Ϣ
            var imConfig = AppInfo.GetOptions<ImConfig>();
            if (imConfig.Enable)
            {
                ImHelper.SendMessage(0, insertUserIds, new
                {
                    evts = new[]
                    {
                        new { name = "checkUnreadMsg", data = new { } }
                    }
                });
            }
        }
    }

    /// <summary>
    /// �Ƴ���Ϣ�û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemoveMsgUserAsync(MsgAddMsgUserListInput input)
    {
        var userIds = input.UserIds;
        if (userIds != null && userIds.Any())
        {
            await _msgUserRep.Where(a => a.MsgId == input.MsgId && input.UserIds.Contains(a.UserId)).ToDelete().ExecuteAffrowsAsync();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(MsgAddInput input)
    {
        var entity = Mapper.Map<MsgEntity>(input);
        await _msgRep.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// �޸�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task UpdateAsync(MsgUpdateInput input)
    {
        var entity = await _msgRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
        {
            throw ResultOutput.Exception(_adminLocalizer["��Ϣ������"]);
        }

        Mapper.Map(input, entity);
        await _msgRep.UpdateAsync(entity);
    }    

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        //ɾ����Ϣ�û�
        await _msgUserRep.DeleteAsync(a => a.MsgId == id);
        //ɾ����Ϣ
        await _msgRep.DeleteAsync(id);
    }

    /// <summary>
    /// ��������ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        //ɾ����Ϣ�û�
        await _msgUserRep.DeleteAsync(a => ids.Contains(a.MsgId));
        //ɾ����Ϣ
        await _msgRep.Where(a => ids.Contains(a.Id)).ToDelete().ExecuteAffrowsAsync();
    }

    /// <summary>
    /// ɾ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        await _msgUserRep.DeleteAsync(a => a.MsgId == id);
        await _msgRep.SoftDeleteAsync(id);
    }

    /// <summary>
    /// ����ɾ��
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        await _msgUserRep.DeleteAsync(a => ids.Contains(a.MsgId));
        await _msgRep.SoftDeleteAsync(a => ids.Contains(a.Id));
    }
}
