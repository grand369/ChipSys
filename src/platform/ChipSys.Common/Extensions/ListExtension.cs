using ChipSys.Common.Helpers;

namespace ChipSys.Common.Extensions;

public static class ListExtension
{
    /// <summary>
    /// ���б�ת��Ϊ���νṹ
    /// </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="list">����</param>
    /// <param name="rootWhere">������</param>
    /// <param name="childsWhere">�ڵ�����</param>
    /// <param name="addChilds">����ӽڵ�</param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static List<T> ToTree<T>(this List<T> list, Func<T, T, bool> rootWhere, Func<T, T, bool> childsWhere, Action<T, IEnumerable<T>> addChilds, T entity = default)
    {
        var treelist = new List<T>();
        //����
        if (list == null || list.Count == 0)
        {
            return treelist;
        }
        if (!list.Any(e => rootWhere(entity, e)))
        {
            return treelist;
        }

        //����
        if (list.Any(e => rootWhere(entity, e)))
        {
            treelist.AddRange(list.Where(e => rootWhere(entity, e)));
        }

        //��Ҷ
        foreach (var item in treelist)
        {
            if (list.Any(e => childsWhere(item, e)))
            {
                var nodedata = list.Where(e => childsWhere(item, e)).ToList();
                foreach (var child in nodedata)
                {
                    //����Ӽ�
                    var data = list.ToTree(childsWhere, childsWhere, addChilds, child);
                    addChilds(child, data);
                }
                addChilds(item, nodedata);
            }
        }

        return treelist;
    }

    /// <summary>
    /// ����Ӽ��б�ƽ���б�
    /// </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="list">ƽ���б�</param>
    /// <param name="getChilds">����Ӽ��б�ķ���</param>
    /// <param name="entity">�Ӽ�����</param>
    public static void AddListWithChilds<T>(List<T> list, Func<T, List<T>> getChilds, T entity = default)
    {
        var childs = getChilds(entity);
        if (childs != null && childs.Count > 0)
        {
            list.AddRange(childs);
            foreach (var child in childs)
            {
                AddListWithChilds(list, getChilds, child);
            }
        }
    }

    /// <summary>
    /// �������б�ת��Ϊƽ���б�
    /// tree.ToPlainList((a) => a.Children);
    /// </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="tree">�����б�</param>
    /// <param name="getChilds">����Ӽ��б�ķ���</param>
    /// <param name="entity">���ݶ���</param>
    /// <returns></returns>
    public static List<T> ToPlainList<T>(this List<T> tree, Func<T, List<T>> getChilds, T entity = default)
    {
        var list = new List<T>();
        if (tree == null || tree.Count == 0)
        {
            return list;
        }

        foreach (var item in tree)
        {
            list.Add(item);
            AddListWithChilds(list, getChilds, item);
        }

        return list;
    }

    /// <summary>
    /// ��ȿ�¡
    /// </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="list">�б�</param>
    /// <returns></returns>
    public static List<T> Clone<T>(this List<T> list)
    {
        return JsonHelper.Deserialize<List<T>>(JsonHelper.Serialize(list));
    }
}
