using System;
using System.IO;
using System.Net;

namespace Generator;

public class HttpHandler
{
    private const string Url = "/task";
    
    private static void PostRequest(string data, string task)
    {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create($"{Url}");
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(data);
            streamWriter.Write(task);
        }

        HttpWebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            string result = streamReader.ReadToEnd();
        }
    }
}