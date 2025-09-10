using System.ComponentModel;

namespace ChipSys.Admin.Domain.Region;

/// <summary>
/// ��������
/// </summary>
public enum RegionLevel
{
    /// <summary>
    /// ʡ��
    /// </summary>
    Province = 1,

    /// <summary>
    /// ����
    /// </summary>
    City = 2,

    /// <summary>
    /// ��/��
    /// </summary>
    County = 3,

    /// <summary>
    /// ��/��/�ֵ�
    /// </summary>
    [Description("��/��")]
    Town = 4,

    /// <summary>
    /// ��/��ί��/����/��ί��
    /// </summary>
    [Description("��/����")]
    Vilage = 5
}