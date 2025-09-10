using System.Text.Json.Serialization;

namespace ChipSys.Admin.Services.Dict.Dto;

/// <summary>
/// �ֵ��б�
/// </summary>
public class DictGetListOutput
{
    /// <summary>
    /// �ֵ����ͱ���
    /// </summary>
    [JsonIgnore]
    public string DictTypeCode { get; set; }

    /// <summary>
    /// �ֵ���������
    /// </summary>
    [JsonIgnore]
    public string DictTypeName { get; set; }

    /// <summary>
    /// ����Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �ϼ�Id
    /// </summary>
    public long? ParentId { get; set; }

    public bool ShouldSerializeParentId()
    {
        return ParentId.HasValue && ParentId > 0;
    }

    /// <summary>
    /// �ֵ�����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �ֵ����
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// �ֵ�ֵ
    /// </summary>
    public string Value { get; set; }
}
