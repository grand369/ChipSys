using ChipSys.Admin.Core.Consts;
using ChipSys.Common.Helpers;
using DataType = FreeSql.DataType;

namespace ChipSys.Admin.Core.Configs;

/// <summary>
/// ���ݿ�����
/// </summary>
public class DbConfig
{
    /// <summary>
    /// ���ݿ�ע���
    /// </summary>
    public string Key { get; set; } = DbKeys.AdminDb;

    private string[] _assemblyNames;
    
    /// <summary>
    /// ��������
    /// </summary>
    public string[] AssemblyNames
    {
        get => _assemblyNames;
        set
        {
            var expandedNames = new List<string>();
            if (value != null)
            {
                foreach (var name in value)
                {
                    expandedNames.Add(name);

                    if (!name.EndsWith(".Contracts"))
                    {
                        string contractsName = name + ".Contracts";
                        if (AssemblyHelper.Exists(contractsName) && !expandedNames.Contains(contractsName))
                        {
                            expandedNames.Add(contractsName);
                        }
                    }
                }
            }
            
            _assemblyNames = [.. expandedNames];
        }
    }

    /// <summary>
    /// ָ��ʵ�����ݿ��б�
    /// </summary>
    public string[] IncludeEntityDbs { get; set; }

    /// <summary>
    /// �ų�ʵ�����ݿ��б�
    /// </summary>
    public string[] ExcludeEntityDbs { get; set; }

    /// <summary>
    /// ���ݿ�����
    /// </summary>
    public DataType Type { get; set; } = DataType.Sqlite;

    /// <summary>
    /// ���ݿ��ַ���
    /// </summary>
    public string ConnectionString { get; set; } = "Data Source=|DataDirectory|\\admindb.db; Pooling=true;Min Pool Size=1";

    /// <summary>
    /// ָ������
    /// </summary>
    public string ProviderType { get; set; }

    /// <summary>
    /// ��������
    /// </summary>
    public bool GenerateData { get; set; } = false;

    /// <summary>
    /// ͬ���ṹ
    /// </summary>
    public bool SyncStructure { get; set; } = false;

    /// <summary>
    /// ͬ���ṹ�ű�
    /// </summary>
    public bool SyncStructureSql { get; set; } = false;

    private int _syncStructureEntityBatchSize = 1;

    /// <summary>
    /// ͬ���ṹ����ʵ����
    /// </summary>
    public int SyncStructureEntityBatchSize
    {
        get => _syncStructureEntityBatchSize <= 1 ? 1 : _syncStructureEntityBatchSize;
        set => _syncStructureEntityBatchSize = value;
    }

    /// <summary>
    /// ͬ�����ݷ��������С
    /// </summary>
    public int SyncDataBatchSize { get; set; } = 500;

    /// <summary>
    /// ͬ������
    /// </summary>
    public bool SyncData { get; set; } = false;

    /// <summary>
    /// ͬ�����ݼ���Curd����
    /// </summary>
    public bool SyncDataCurd { get; set; } = false;

    /// <summary>
    /// ͬ����������
    /// </summary>
    [Obsolete("�����DbConfig.SyncUpdateData����")]
    public bool SysUpdateData { get; set; } = false;

    /// <summary>
    /// ͬ����������
    /// </summary>
    public bool SyncUpdateData { get; set; } = false;

    /// <summary>
    /// ͬ�����ݵ�ַ
    /// </summary>
    public string SyncDataPath { get; set; } = "InitData/Admin";

    /// <summary>
    /// ͬ�����ݰ������б�
    /// </summary>
    public string[] SyncDataIncludeTables { get; set; }

    /// <summary>
    /// ͬ�������ų����б�
    /// </summary>
    public string[] SyncDataExcludeTables { get; set; }

    /// <summary>
    /// ͬ�����ݲ����û�
    /// </summary>
    public SyncDataUser SyncDataUser { get; set; } = new SyncDataUser { Id = 161223411986501, UserName = "admin", TenantId = 161223412138053 };

    /// <summary>
    /// ����
    /// </summary>
    public bool CreateDb { get; set; } = false;

    /// <summary>
    /// ���������ַ���
    /// </summary>
    public string CreateDbConnectionString { get; set; }

    /// <summary>
    /// ����ű�
    /// </summary>
    public string CreateDbSql { get; set; }

    /// <summary>
    /// ����ű��ļ�
    /// </summary>
    public string CreateDbSqlFile { get; set; } = "ConfigCenter/createdbsql.txt";

    /// <summary>
    /// �������в���
    /// </summary>
    public bool MonitorCommand { get; set; } = false;

    /// <summary>
    /// ����Curd����
    /// </summary>
    public bool Curd { get; set; } = false;

    /// <summary>
    /// ����ʱ�䣨�֣���Ĭ��10���ӣ�����idleTime=0���Զ�����
    /// </summary>
    public int? IdleTime { get; set; } = 10;

    /// <summary>
    /// �����ݿ�
    /// </summary>
    public DbConfig[] Dbs { get; set; }

    /// <summary>
    /// ��д����ӿ��б�
    /// </summary>
    public SlaveDb[] SlaveList { get; set; }

    /// <summary>
    /// ǿ�Ƹ���
    /// </summary>
    public bool ForceUpdate { get; set; } = false;
}

/// <summary>
/// ��д����ӿ�
/// </summary>
public class SlaveDb
{
    /// <summary>
    /// ���ݿ�����
    /// </summary>
    public int Weight { get; set; } = 1;

    /// <summary>
    /// ���ݿ������ַ���
    /// </summary>
    public string ConnectionString { get; set; }
}

/// <summary>
/// ͬ�����ݲ����û�
/// </summary>
public class SyncDataUser
{
    /// <summary>
    /// �û�Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// �˺�
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// �⻧Id
    /// </summary>
    public long TenantId { get; set; }
}
