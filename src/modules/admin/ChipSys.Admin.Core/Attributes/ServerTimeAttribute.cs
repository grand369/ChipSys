namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// �����ʱ��
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ServerTimeAttribute : Attribute
{
    /// <summary>
    /// �������ø��ֶη�������ʱ�䣬Ĭ��ֵfalse��ָ��Ϊtrue����ʱ����
    /// </summary>
    public bool CanUpdate { get; set; } = false;

    /// <summary>
    /// �������ø��ֶη�������ʱ�䣬Ĭ��ֵtrue��ָ��Ϊfalse����ʱ������
    /// </summary>
    public bool CanInsert { get; set; } = true;
}
