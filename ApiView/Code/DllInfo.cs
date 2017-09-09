namespace ApiView.Code
{
    public class DllInfo
    {
        public string Path { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DllInfo()
        {

        }
        public DllInfo(string infostr)
        {
            string[] ss = infostr.Trim(';').Split(';');
            Path = ss[0];
            Name = ss[1];
            Url = ss[2];
        }
    }
}