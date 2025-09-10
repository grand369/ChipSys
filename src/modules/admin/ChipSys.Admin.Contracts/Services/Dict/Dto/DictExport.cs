using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Magicodes.IE.Core;
using OfficeOpenXml.Table;
using System.Reflection;

namespace ChipSys.Admin.Services.Dict.Dto;

/// <summary>
/// ����ֵӳ��
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class BoolValueMappingAttribute : ValueMappingsBaseAttribute
{
    public override Dictionary<string, object> GetMappings(PropertyInfo propertyInfo)
    {
        var res = new Dictionary<string, object>
        {
            { "��", true },
            { "��", false }
        };
        return res;
    }
}

[ExcelExporter(Name = "�ֵ��б�")]
public class DictExportHeader
{
    [ExporterHeader(DisplayName = "�ֵ����", ColumnIndex = 3)]
    public string DictTypeName { get; set; }

    [ExporterHeader(DisplayName = "�ֵ�����", ColumnIndex = 4)]
    public string Name { get; set; }

    [ExporterHeader(DisplayName = "�ϼ��ֵ�", ColumnIndex = 5)]
    public string ParentName { get; set; }

    [ExporterHeader(DisplayName = "�ֵ����", ColumnIndex = 6)]
    public string Code { get; set; }

    [ExporterHeader(DisplayName = "�ֵ�ֵ", ColumnIndex = 7)]
    public string Value { get; set; }

    //[BoolValueMapping]
    //[ValueMapping(text: "��", true)]
    //[ValueMapping(text: "��", false)]
    [ExporterHeader(DisplayName = "����", ColumnIndex = 8)]
    public bool Enabled { get; set; }

    [ExporterHeader(DisplayName = "����", ColumnIndex = 9)]
    public int Sort { get; set; }

    [ExporterHeader(DisplayName = "˵��", ColumnIndex = 10)]
    public string Description { get; set; }
}

[ExcelExporter(Name = "�ֵ��б�", TableStyle = TableStyles.Light9, AutoFitAllColumn = true, AutoFitMaxRows = 5000)]
public class DictExport: DictExportHeader
{
    [ExporterHeader(DisplayName = "�ֵ���", Format = "0", ColumnIndex = 1)]
    public long Id { get; set; }

    [ExporterHeader(DisplayName = "�ϼ��ֵ���", Format = "0", ColumnIndex = 2)]
    public long ParentId { get; set; }

    [ExporterHeader(DisplayName = "������Ա", ColumnIndex = 11)]
    public string CreatedUserRealName { get; set; }

    [ExporterHeader(DisplayName = "����ʱ��", Format = "yyyy-MM-dd HH:mm:ss", Width = 20, ColumnIndex = 12)]
    public DateTime? CreatedTime { get; set; }

    [ExporterHeader(DisplayName = "�޸���Ա", ColumnIndex = 13)]
    public string ModifiedUserRealName { get; set; }

    [ExporterHeader(DisplayName = "�޸�ʱ��", Format = "yyyy-MM-dd HH:mm:ss", Width = 20, ColumnIndex = 14)]
    public virtual DateTime? ModifiedTime { get; set; }
}
