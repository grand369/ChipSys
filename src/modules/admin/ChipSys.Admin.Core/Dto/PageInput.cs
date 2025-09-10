namespace ChipSys.Admin.Core.Dto;

/// <summary>
/// ��ҳ��Ϣ����
/// </summary>
public class PageInput: QueryInput
{
    private int _currentPage;
    private int _pageSize;

    /// <summary>
    /// ��ǰҳ��
    /// </summary>
    public virtual int CurrentPage 
    {
        get => _currentPage < 1 ? 1 : _currentPage;
        set => _currentPage = value;
    }

    /// <summary>
    /// ÿҳ��С
    /// </summary>
    public virtual int PageSize 
    {
        get
        {
            if (_pageSize < 1) _pageSize = 1;
            //if (_pageSize > 1000) _pageSize = 1000;
            return _pageSize;
        }
        set => _pageSize = value;
    }
}

/// <summary>
/// ��ҳ��Ϣ����
/// </summary>
/// <typeparam name="T">��������</typeparam>
public class PageInput<T>: PageInput
{
    /// <summary>
    /// ��ѯ����
    /// </summary>
    public virtual T Filter { get; set; }
}
