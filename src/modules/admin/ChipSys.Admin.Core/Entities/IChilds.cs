namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// �Ӽ��ӿ�
/// </summary>
public interface IChilds<T>
{
    /// <summary>
    /// �Ӽ��б�
    /// </summary>
    List<T> Childs { get; set; }
}
