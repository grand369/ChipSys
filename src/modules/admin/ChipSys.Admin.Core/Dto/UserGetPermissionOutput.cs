namespace ChipSys.Admin.Services.User.Dto;

/// <summary>
/// �û�Ȩ��
/// </summary>
public class UserGetPermissionOutput
{
    public static class Models
    {
        /// <summary>
        /// �ӿ�
        /// </summary>
        public class ApiModel
        {
            /// <summary>
            /// ���󷽷�
            /// </summary>
            public string HttpMethods { get; set; }

            /// <summary>
            /// �����ַ
            /// </summary>
            public string Path { get; set; }
        }
    }

    /// <summary>
    /// �ӿ��б�
    /// </summary>
    public List<Models.ApiModel> Apis {  get; set; }

    /// <summary>
    /// Ȩ�޵�����б�
    /// </summary>
    public List<string> Codes { get; set; }
}
