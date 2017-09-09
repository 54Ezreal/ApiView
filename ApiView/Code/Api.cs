using System.Reflection;

namespace ApiView.Code
{
    public class Api
    {
        public Api() { }

        public MethodInfo Method;

        public DllInfo OpenDll { get; set; }

        public override string ToString()
        {
            return ApiHelper.GetOpenApiAttribute(Method).MethodName;
        }

        public string RelateUrl()
        {
            var url = Method.DeclaringType.Name.Replace("Controller", "") + "/" + Method.Name;
            return url;
        }

        public string ID { get; set; }

        //public string ID
        //{
        //    get
        //    {
        //        return Method.DeclaringType.FullName + "." + Method.Name;
        //    }
        //}

    }
}