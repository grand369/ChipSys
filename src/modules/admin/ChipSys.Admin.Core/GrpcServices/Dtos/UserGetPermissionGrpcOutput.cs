using ProtoBuf;

namespace ChipSys.Admin.Core.GrpcServices.Dtos;

/// <summary>
/// �û�Ȩ��
/// </summary>
[ProtoContract(ImplicitFields = ImplicitFields.None)]
public class UserGetPermissionGrpcOutput
{
    public static class Models
    {
        /// <summary>
        /// �ӿ�
        /// </summary>
        [ProtoContract(ImplicitFields = ImplicitFields.None)]
        public class ApiModel
        {
            /// <summary>
            /// ���󷽷�
            /// </summary>
            [ProtoMember(1)]
            public string HttpMethods { get; set; }

            /// <summary>
            /// �����ַ
            /// </summary>
            [ProtoMember(2)]
            public string Path { get; set; }
        }
    }

    /// <summary>
    /// �ӿ��б�
    /// </summary>
    [ProtoMember(1)]
    public List<Models.ApiModel> Apis { get; set; }

    /// <summary>
    /// Ȩ�޵�����б�
    /// </summary>
    [ProtoMember(2)]
    public List<string> Codes { get; set; }
}
