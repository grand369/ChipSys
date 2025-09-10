using System.ComponentModel.DataAnnotations;
using ChipSys.Admin.Core.Validators;

namespace ChipSys.Admin.Services.View.Dto;

/// <summary>
/// �޸�
/// </summary>
public class ViewUpdateInput : ViewAddInput
{
    /// <summary>
    /// ��ͼId
    /// </summary>
    [Required]
    [ValidateRequired("��ѡ����ͼ")]
    public long Id { get; set; }
}
