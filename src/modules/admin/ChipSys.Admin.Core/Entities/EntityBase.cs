namespace ChipSys.Admin.Core.Entities;

/// <summary>
/// ʵ�����
/// </summary>
public class EntityBase<TKey> : EntityDelete<TKey> where TKey : struct
{
}

/// <summary>
/// ʵ�����
/// </summary>
public class EntityBase : EntityBase<long>
{
}
