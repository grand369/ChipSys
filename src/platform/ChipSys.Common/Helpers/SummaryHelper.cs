using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace ChipSys.Common.Helpers;

/// <summary>
/// �ĵ�˵��������
/// </summary>
public class SummaryHelper
{
    static int _CodeBaseNotSupportedException = 0;

    /// <summary>
    /// ���ö������˵���б�
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetEnumSummaryList(Type type)
    {
        return LocalGetComment(type, 0);

        Dictionary<string, string> LocalGetComment(Type localType, int level)
        {
            if (localType.Assembly.IsDynamic) return null;
            //��̬���ɵĳ��򼯣����ʲ��� Assembly.Location/Assembly.CodeBase
            var regex = new Regex(@"\.(dll|exe)", RegexOptions.IgnoreCase);
            var xmlPath = regex.Replace(localType.Assembly.Location, ".xml");
            if (File.Exists(xmlPath) == false)
            {
                if (_CodeBaseNotSupportedException == 1) return null;
                try
                {
                    if (string.IsNullOrEmpty(localType.Assembly.Location)) return null;
                }
                catch (NotSupportedException) //NotSupportedException: CodeBase is not supported on assemblies loaded from a single-file bundle.
                {
                    Interlocked.Exchange(ref _CodeBaseNotSupportedException, 1);
                    return null;
                }

                xmlPath = regex.Replace(localType.Assembly.Location, ".xml");
                if (xmlPath.StartsWith("file:///") && Uri.TryCreate(xmlPath, UriKind.Absolute, out var tryuri))
                    xmlPath = tryuri.LocalPath;
                if (File.Exists(xmlPath) == false) return null;
            }

            var dic = new Dictionary<string, string>();
            StringReader sReader = null;
            try
            {
                sReader = new StringReader(File.ReadAllText(xmlPath));
            }
            catch
            {
                return dic;
            }
            using (var xmlReader = XmlReader.Create(sReader))
            {
                XPathDocument xpath = null;
                try
                {
                    xpath = new XPathDocument(xmlReader);
                }
                catch
                {
                    return null;
                }
                var xmlNav = xpath.CreateNavigator();

                var className = (localType.IsNested ? (localType.DeclaringType != null && localType.DeclaringType.DeclaringType != null &&
                    localType.DeclaringType.DeclaringType.FullName.NotNull() ? $"{localType.DeclaringType.DeclaringType.FullName}.{localType.DeclaringType.Name}.{localType.Name}" :
                    $"{localType.Namespace}.{localType.DeclaringType.Name}.{localType.Name}") :
                    $"{localType.Namespace}.{localType.Name}").Trim('.');

                var node = xmlNav.SelectSingleNode($"/doc/members/member[@name='T:{className}']/summary");
                if (node != null)
                {
                    var comment = node.InnerXml.Trim(' ', '\r', '\n', '\t');
                    if (string.IsNullOrEmpty(comment) == false) dic.Add("", comment); //classע��
                }

                if (localType.IsEnum)
                {
                    var fields = Enum.GetValues(localType).Cast<Enum>().Select(x => x.ToString()).ToList();
                    foreach (var field in fields)
                    {
                        node = xmlNav.SelectSingleNode($"/doc/members/member[@name='F:{className}.{field}']/summary");
                        if (node != null)
                        {
                            var comment = node.InnerXml.Trim(' ', '\r', '\n', '\t');
                            if (string.IsNullOrEmpty(comment) == false) dic.Add(field, comment); //fieldע��
                        }
                    }
                }
            }
            return dic;
        }
    }
}
