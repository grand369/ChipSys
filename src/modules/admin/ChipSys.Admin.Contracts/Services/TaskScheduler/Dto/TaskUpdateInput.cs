using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Services.TaskScheduler.Dto;

/// <summary>
/// �޸�
/// </summary>
public partial class TaskUpdateInput : TaskAddInput
{
    /// <summary>
    /// ����Id
    /// </summary>
    [Required(ErrorMessage = "��ѡ������")]
    public string Id { get; set; }
}
