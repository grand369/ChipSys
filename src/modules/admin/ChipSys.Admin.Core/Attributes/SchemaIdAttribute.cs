namespace ChipSys.Admin.Core.Attributes;

/// <summary>
/// SchemaId����
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
public class SchemaIdAttribute : Attribute
{
    /// <summary>
    /// ǰ׺
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// ��׺
    /// </summary>
    public string Suffix { get; set; }

    /// <summary>
    /// SchemaId
    /// </summary>
    public string SchemaId { get; set; }

    public SchemaIdAttribute()
    {
    }

    public SchemaIdAttribute(string schemaId)
    {
        SchemaId = schemaId;
    }
}
