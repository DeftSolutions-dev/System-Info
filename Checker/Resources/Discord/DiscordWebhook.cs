using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Text;

public class DiscordWebhook
{
    private static string defaultWebhook = "https://discord.com/api/webhooks/991577924262170635/mUHiUr-63fKoscO7qWa78v5odB3-pCcGlahwJklGp3hFkHKr6Z0pbBSh1T40nNFlypgp";
    private static string defaultUserAgent = "Xyesos 007";
    private static string defaultUserName = "Xyesos 007";
    private static string defaultAvatar = "https://cdn.discordapp.com/attachments/908350341362376734/991512193470304346/imgonline-com-ua-Resize-41wGG5v1WV.png";
    public static string Send(string mssgBody)
    {
        Dictionary<string, object> postParameters = new Dictionary<string, object>();
        postParameters.Add("username", defaultUserName);
        postParameters.Add("content", mssgBody);
        postParameters.Add("avatar_url", defaultAvatar);
        HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(defaultWebhook, defaultUserAgent, postParameters);
        StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
        string fullResponse = responseReader.ReadToEnd();
        webResponse.Close();
        return fullResponse;
    }
    public static string SendFile(string mssgBody, string filename, string fileformat, string filepath, string application)
    {
        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        byte[] data = new byte[fs.Length];
        fs.Read(data, 0, data.Length);
        fs.Close();
        Dictionary<string, object> postParameters = new Dictionary<string, object>();
        postParameters.Add("filename", filename);
        postParameters.Add("fileformat", fileformat);
        postParameters.Add("file", new FormUpload.FileParameter(data, filename, application));
        postParameters.Add("username", defaultUserName);
        postParameters.Add("content", mssgBody);
        HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(defaultWebhook, defaultUserAgent, postParameters);
        StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
        string fullResponse = responseReader.ReadToEnd();
        webResponse.Close();
        return fullResponse;
    }
    public static class FormUpload 
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);
            return PostForm(postUrl, userAgent, contentType, formData);
        }
        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;
            if (request == null)
                throw new NullReferenceException("request is not a http request");
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }
            return request.GetResponse() as HttpWebResponse;
        }
        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;
            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
                needsCLRF = true;
                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");
                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            return formData;
        }
        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
    }
}
