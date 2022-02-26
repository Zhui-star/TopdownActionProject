using UnityEngine;
using System.Collections.Generic;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// Debug tools support log information,warning,error. support different color, bold, italic font style
    /// </summary>
    public static class Debugs
    {
        private static Dictionary<string,string> _colorDicts=new Dictionary<string, string>()
        {
            {"RGBA(0.000, 1.000, 1.000, 1.000)","cyan"},
            {"RGBA(0.000, 0.000, 0.000, 0.000)","clear"},
            {"RGBA(0.500, 0.500, 0.500, 1.000)","grey"},
            {"RGBA(0.010, 0.000, 0.010, 1.000)","magenta"},
            {"RGBA(1.000, 0.000, 0.000, 1.000)","red"},
            {"RGBA(1.000, 0.922, 0.016, 1.000)","yellow"},
            {"RGBA(0.000, 0.000, 0.000, 1.000)","black"},
            {"RGBA(1.000, 1.000, 1.000, 1.000)","white"},
            {"RGBA(0.000, 1.000, 0.000, 1.000)","green"},
            {"RGBA(0.000, 0.000, 1.000, 1.000)","blue"},

        };
        public static void LogInformation(string debugStr, Color color, bool bold = true, bool italic = true)
        {
            string strColor = color.ToString("#0.000");
            strColor=_colorDicts.TryGet<string,string>(strColor);
            string boldStr = bold ? "b" : "";
            string italicStr = italic ? "i" : "";

            Debug.Log("<color="+strColor+"><"+boldStr+"><"+italicStr+"> " + debugStr + " </i> </b></color>");
        }

        public static void LogWarning(string debugStr, Color color, bool bold = true, bool italic = true)
        {
            string strColor = color.ToString("#0.000");
            strColor=_colorDicts.TryGet<string,string>(strColor);
            string boldStr = bold ? "b" : "";
            string italicStr = italic ? "i" : "";

          Debug.LogWarning("<color="+strColor+"><"+boldStr+"><"+italicStr+"> " + debugStr + " </i> </b></color>");
        }

        public static void LogError(string debugStr, Color color, bool bold = true, bool italic = true)
        {
            string strColor = color.ToString("#0.000");
            strColor=_colorDicts.TryGet<string,string>(strColor);
            string boldStr = bold ? "b" : "";
            string italicStr = italic ? "i" : "";

            Debug.LogError("<color="+strColor+"><"+boldStr+"><"+italicStr+"> " + debugStr + " </i> </b></color>");
        }
    }

}
