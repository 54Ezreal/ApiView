using ApiView.OpenApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ApiView.Code
{
    public class ApiHelper
    {
        public static List<Api> AllApis = new List<Api>();
        public static List<DllInfo> AllDllInfo = new List<DllInfo>();
        public static List<Api> OldAllApis = new List<Api>();
        public static DateTime lastupdatetime = new DateTime();
        private static object _lock = new object();

        public static void Load()
        {
            lock (_lock)
            {
                foreach (var p in DllPaths())
                {
                    AllApis.AddRange(GetApis(p));
                }
                lastupdatetime = DateTime.Now;
            }
        }

        public static List<DllInfo> DllPaths()
        {
            if (AllDllInfo == null || AllDllInfo.Count == 0)
            {
                var rs = new List<DllInfo>();
                foreach (var r in System.Configuration.ConfigurationManager.AppSettings.AllKeys)
                {
                    if (r.Contains("DllPath"))
                    {
                        var dllinfo = new DllInfo(System.Configuration.ConfigurationManager.AppSettings[r] as string);
                        rs.Add(dllinfo);
                    }
                }
                AllDllInfo = rs;
            }
            return AllDllInfo;
        }

        /// <summary>
        /// 反射加载程序集
        /// </summary>
        public static List<Api> GetApis(DllInfo info)
        {
            var assembly = Assembly.LoadFrom(info.Path);
            var rs = new List<Api>();
            foreach (var t in (assembly.GetTypes()))
            {
                foreach (var m in t.GetMethods())
                {
                    var find = GetOpenApiAttribute(m);
                    if (find != null)
                    {
                        rs.Add(new Api() { Method = m, OpenDll = info, ID = Guid.NewGuid().ToString() });
                    }
                }
            }
            return rs;
        }

        public static OpenDocAttribute GetOpenApiAttribute(MethodInfo m)
        {
            return (
                from a in m.GetCustomAttributes(false)
                where a is OpenDocAttribute
                select a as OpenDocAttribute)
                .SingleOrDefault();
        }

        public static bool IsComplexParameterType(MethodInfo m)
        {
            var pars = m.GetParameters();
            foreach (var p in pars)
            {
                if (p.ParameterType.BaseType != typeof(ValueType) && p.ParameterType != typeof(string) && p.ParameterType  != typeof(HttpPostedFileBase) )
                {
                    var properties = p.ParameterType.GetProperties();
                    foreach (var item in properties)
                    {
                        if (item.PropertyType.BaseType == typeof(object) && item.PropertyType != typeof(string)  && item.PropertyType != typeof(HttpPostedFileBase))
                        {
                            //层级超过两层且参数类型为object 认定为复杂的参数类型
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 高亮显示搜索文本
        /// </summary>
        /// <returns></returns>
        public static string HighLightKeyword(string str, string keyword)
        {
            int index;
            var startIndex = 0;
            const string highLightBegin = "<span class='keyWord'>";
            const string highLightEnd = "</span>";
            var length = highLightBegin.Length + keyword.Length;
            var lengthHighlight = length + highLightEnd.Length;

            while ((index = str.IndexOf(keyword, startIndex, StringComparison.OrdinalIgnoreCase)) > -1)
            {
                str = str.Insert(index, highLightBegin).Insert(index + length, highLightEnd);
                startIndex = index + lengthHighlight;
            }
            return str;
        }

    }

}