using System.ComponentModel;

namespace ChipSys.Admin.Core.Enums;

/// <summary>
/// ״̬��ö��
/// </summary>
public enum StatusCodes
{
    /// <summary>
    /// ����ʧ��
    /// </summary>
    [Description("����ʧ��")]
    Status0NotOk = 0,

    /// <summary>
    /// �����ɹ�
    /// </summary>
    [Description("�����ɹ�")]
    Status1Ok = 1,

    /// <summary>
    /// δ��¼����Ҫ���µ�¼��
    /// </summary>
    [Description("δ��¼")]
    Status401Unauthorized = 401,

    /// <summary>
    /// Ȩ�޲���
    /// </summary>
    [Description("Ȩ�޲���")]
    Status403Forbidden = 403,

    /// <summary>
    /// ��Դ������
    /// </summary>
    [Description("��Դ������")]
    Status404NotFound = 404,

    /// <summary>
    /// ϵͳ�ڲ����󣨷�ҵ���������ʽ�׳����쳣�������������ݲ���ȷ���¿�ָ���쳣�����ݿ��쳣�ȵȣ�
    /// </summary>
    [Description("ϵͳ�ڲ�����")]
    Status500InternalServerError = 500
}
