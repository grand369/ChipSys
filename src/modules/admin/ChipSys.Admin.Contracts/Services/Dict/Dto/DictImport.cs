using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.Dict.Dto;

/// <summary>
/// �ֵ䵼����ͷɸѡ��
/// </summary>
public class DictImportHeaderFilter : IImportHeaderFilter
{
    public List<ImporterHeaderInfo> Filter(List<ImporterHeaderInfo> importerHeaderInfos)
    {
        foreach (var item in importerHeaderInfos)
        {
            if (item.PropertyName == "Enabled")
            {
                item.MappingValues = new Dictionary<string, dynamic>()
                    {
                        {"��", true },
                        {"��", false }
                    };
            }
        }
        return importerHeaderInfos;
    }
}

/// <summary>
/// �ֵ䵼��
/// </summary>
[ExcelImporter(IsLabelingError = true, ImportHeaderFilter = typeof(DictImportHeaderFilter))]
public class DictImport
{
    /// <summary>
    /// �ֵ�Id
    /// </summary>
    [ImporterHeader(Name = "�ֵ�Id", IsIgnore = true)]
    public long Id { get; set; }

    /// <summary>
    /// �ϼ��ֵ�Id
    /// </summary>
    [ImporterHeader(Name = "�ϼ��ֵ�Id", IsIgnore = true)]
    public long ParentId { get; set; }

    /// <summary>
    /// �ֵ�����Id
    /// </summary>
    [ImporterHeader(Name = "�ֵ�����Id", IsIgnore = true)]
    public long DictTypeId { get; set; }

    [ImporterHeader(Name = "�ֵ����")]
    [Required(ErrorMessage = "����Ϊ��")]
    public string DictTypeName { get; set; }

    [ImporterHeader(Name = "�ֵ�����", IsAllowRepeat = false)]
    [Required(ErrorMessage = "����Ϊ��")]
    public string Name { get; set; }

    [ImporterHeader(Name = "�ϼ��ֵ�����")]
    public string ParentName { get; set; }

    [ImporterHeader(Name = "�ֵ����", IsAllowRepeat = false)]
    public string Code { get; set; }

    [ImporterHeader(Name = "�ֵ�ֵ", IsAllowRepeat = false)]
    public string Value { get; set; }

    [ImporterHeader(Name = "����",IsInterValidation = false)]
    public bool Enabled { get; set; }

    [ImporterHeader(Name = "����")]
    public int Sort { get; set; }

    [ImporterHeader(Name = "˵��")]
    public string Description { get; set; }
}
