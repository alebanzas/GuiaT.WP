using System;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace WebScraping
{
	/// <summary>
	/// This class opens a website and scraps the contents
	/// </summary>
	public class JsonResult<T> where T : class
	{
	    private readonly Encoding _encoding;
        
        public JsonResult(){}

        public JsonResult(Encoding encoding)
        {
            _encoding = encoding;
        }

	    public T GetNodes(Uri url)
        {
            try
            {
                // Create the WebRequest for the URL we are using
                var req = WebRequest.Create(url);

                // Get the stream from the returned web response
                var stream = _encoding != null ? new StreamReader(req.GetResponse().GetResponseStream(), _encoding) : new StreamReader(req.GetResponse().GetResponseStream());
                
                var serializer = new JavaScriptSerializer();

                return serializer.Deserialize<T>(stream.ReadToEnd());
		    }
            catch (WebException ex)
            {
                throw new Exception("Error llamando a " + url, ex);
            }
        }
	}
}
