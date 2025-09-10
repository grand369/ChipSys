using System.ComponentModel.DataAnnotations;

namespace ChipSys.Admin.Core.Validators;

/// <summary>
/// ָ�����ԡ��ֶΡ���������
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class ValidateRequiredAttribute : ValidationAttribute
{
    public ValidateRequiredAttribute() : base("{0} Ϊ������") { }

    public ValidateRequiredAttribute(string errorMessage) : base(errorMessage) { }

    public override bool IsValid(object value)
    {
        if (value is null)
        {
            return false;
        }

        var valid  = value switch
        {
            Guid guid => guid != Guid.Empty,
            long longValue => longValue > 0,
            int intValue => intValue > 0,
            string strValue => strValue.NotNull(),
            _ => true
        };

        return valid;
    }
}
