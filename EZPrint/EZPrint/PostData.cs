using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleCloudPrintServices.Support
{
    internal class PostData
    {
        private const String CRLF = "\r\n";

        public string Boundary { get; set; }
        private List<PostDataParam> _mParams;

        public List<PostDataParam> Params
        {
            get { return _mParams; }
            set { _mParams = value; }
        }

        public PostData()
        {
            // Get boundary, default is --AaB03x
            Boundary = "----CloudPrintFormBoundary" + DateTime.UtcNow;

            // The set of parameters
            _mParams = new List<PostDataParam>();
        }

        public string GetPostData()
        {
            var sb = new StringBuilder();
            foreach (var p in _mParams)
            {
                sb.Append("--" + Boundary).Append(CRLF);

                if (p.Type == PostDataParamType.File)
                {
                    sb.Append(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", p.Name, p.FileName)).Append(CRLF);
                    sb.Append("Content-Type: ").Append(p.FileMimeType).Append(CRLF);
                    sb.Append("Content-Transfer-Encoding: base64").Append(CRLF);
                    sb.Append("").Append(CRLF);
                    sb.Append(p.Value).Append(CRLF);
                }
                else
                {
                    sb.Append(string.Format("Content-Disposition: form-data; name=\"{0}\"", p.Name)).Append(CRLF);
                    sb.Append("").Append(CRLF);
                    sb.Append(p.Value).Append(CRLF);
                }
            }

            sb.Append("--" + Boundary + "--").Append(CRLF);

            return sb.ToString();
        }
    }

    public enum PostDataParamType
    {
        Field,
        File
    }

    public class PostDataParam
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FileMimeType { get; set; }
        public string Value { get; set; }
        public PostDataParamType Type { get; set; }

        public PostDataParam()
        {
            FileMimeType = "text/plain";
        }
    }
}