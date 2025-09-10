using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace ChipSys.Common.Helpers;

public class IPHelper
{
    /// <summary>
    /// �Ƿ�Ϊip
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public static bool IsIP(string ip)
    {
        return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
    }

    /// <summary>
    /// ���IP��ַ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string GetIP(HttpRequest request)
    {
        if (request == null)
        {
            return "";
        }

        string ip = request.Headers["X-Real-IP"].FirstOrDefault();
        if (ip.IsNull())
        {
            ip = request.Headers["X-Forwarded-For"].FirstOrDefault();
        }
        if (ip.IsNull())
        {
            ip = request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }
        if (ip.IsNull())
        {
            ip = ip.Split(":")?.FirstOrDefault();
        }
        if (ip.IsNull() || !IsIP(ip))
        {
            ip = "127.0.0.1";
        }

        return ip;
    }
}
