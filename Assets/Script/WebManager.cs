using QFramework;
using QFramework.Example;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public enum Web_E
{
    Begin = QMgrID.Network_WEB,
    PUT,
    POST,
    GET
}
public class WebMsg : QMsg
{
    public string url;
    public string message;
    public Action<bool, string> callback;
}


public class WebManager : QMgrBehaviour,ISingleton
{
    public void OnSingletonInit()
    {
        RegisterEvent(Web_E.GET);
        RegisterEvent(Web_E.POST);
        RegisterEvent(Web_E.PUT);
    }

    protected override void ProcessMsg(int eventId, QMsg msg)
    {
        WebMsg wm = msg as WebMsg;
        string url = wm.url;
        string message = wm.message;
        Action<bool, string> callback = wm.callback;
        switch (eventId)
        {
            case (int)Web_E.GET:
                Get(url, callback);
                break;
            case (int)Web_E.PUT:
                Put(url, message, callback);
                break;
            case (int)Web_E.POST:
                Post(url, message, callback);
                break;
            default:
                break;
        }
    }



    public void Get(string url, Action<bool, string> call)
    {
        string path = url;
        StartCoroutine(GetToPHP(path, call));

    }

    public void Put(string url, string message, Action<bool, string> call)
    {
        string path = url;
        StartCoroutine(PUTToPHP(path, message, call));
    }

    public void Post(string Url, string poststr, Action<bool, string> call)
    {
        StartCoroutine(PostToPHP(Url, poststr, call));
    }



    public void DownloadFileFromWed(string url, string filename, Action<bool, string> Finish = null)
    {
        StartCoroutine(DownloadAndSave(url, filename, Finish));
    }



    private IEnumerator PostToPHP(string url, string postData, Action<bool, string> callback)
    {
        using (UnityWebRequest postrequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            postrequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
            postrequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            postrequest.method = UnityWebRequest.kHttpVerbPOST;
            postrequest.SetRequestHeader("Content-Type", "application/json");
            postrequest.SetRequestHeader("X-Requested-With", "XMLHttpRequest");

            yield return postrequest.Send();
            if (postrequest.isNetworkError)
            {
                if (null != callback)
                {
                    callback(false, postrequest.error);
                }
            }
            else
            {
                // Show results as text    
                if (postrequest.responseCode == 200)
                {
                    if (null != callback)
                    {
                        //string s = Encoding.UTF8.GetString(postrequest.downloadHandler.text)
                        callback(true, postrequest.downloadHandler.text);
                    }
                }
                else
                {
                    callback(false, postrequest.responseCode.ToString());
                }
            }
        }
    }


    private IEnumerator PUTToPHP(string url, string postData, Action<bool, string> callback)
    {
        using (UnityWebRequest putrequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT))
        {
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            putrequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
            putrequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            putrequest.SetRequestHeader("Content-Type", "application/json");
            putrequest.SetRequestHeader("X-Requested-With", "XMLHttpRequest");

            yield return putrequest.Send();
            if (putrequest.isNetworkError)
            {
                if (null != callback)
                {
                    callback(false, putrequest.error);
                }
            }
            else
            {
                // Show results as text    
                if (putrequest.responseCode == 200)
                {
                    if (null != callback)
                    {
                        //string s = Encoding.UTF8.GetString(postrequest.downloadHandler.text)
                        callback(true, putrequest.downloadHandler.text);
                    }
                }
                else
                {
                    callback(false, putrequest.responseCode.ToString());
                }
            }
        }
    }
    private IEnumerator GetToPHP(string url, Action<bool, string> callback)
    {
        using (UnityWebRequest getrequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET))
        {
            //byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            //postrequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
            getrequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            getrequest.SetRequestHeader("Content-Type", "application/json");
            getrequest.SetRequestHeader("X-Requested-With", "XMLHttpRequest");

            yield return getrequest.Send();
            if (getrequest.isNetworkError)
            {
                if (null != callback)
                {
                    callback(false, getrequest.error);
                }
            }
            else
            {
                // Show results as text    
                if (getrequest.responseCode == 200)
                {
                    if (null != callback)
                    {
                        //string s = Encoding.UTF8.GetString(postrequest.downloadHandler.text)
                        callback(true, getrequest.downloadHandler.text);
                    }
                }
                else
                {
                    callback(false, getrequest.responseCode.ToString());
                }
            }
        }
    }





    #region 下载资源并保存到本地
    /// <summary>  
    /// 下载并保存资源到本地  
    /// </summary>  
    /// <param name="url"></param>  
    /// <param name="name"></param>  
    /// <returns></returns>  
    private static IEnumerator DownloadAndSave(string url, string name, Action<bool, string> Finish = null)
    {
        url = Uri.EscapeUriString(url);
        string Loading = string.Empty;
        bool b = false;
        WWW www = new WWW(url);
        if (www.error != null)
        {
            print("error:" + www.error);
        }
        while (!www.isDone)
        {
            Loading = (((int)(www.progress * 100)) % 100) + "%";
            if (Finish != null)
            {
                Finish(b, Loading);
            }
            yield return 1;
        }
        if (www.isDone)
        {
            Loading = "100%";
            byte[] bytes = www.bytes;
            //b = SaveAssets(Application.persistentDataPath, name, bytes);
            b = SaveAssets(Tool.SaveDownLoadFromWebPath, name, bytes);

            if (Finish != null)
            {
                Finish(b, Loading);
            }
        }
    }




    /// <summary>  
    /// 保存资源到本地  
    /// </summary>  
    /// <param name="path"></param>  
    /// <param name="name"></param>  
    /// <param name="info"></param>  
    /// <param name="length"></param>  
    private static bool SaveAssets(string path, string name, byte[] bytes)
    {
        Stream sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (t.Exists)
        {
            File.Delete(t.FullName);
        }
        try
        {
            sw = t.Create();
            sw.Write(bytes, 0, bytes.Length);
            sw.Close();
            sw.Dispose();
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion






    protected override void SetupMgrId()
    {
        mMgrId = QMgrID.Network_WEB;
    }


    private WebManager() { }

    public static WebManager Instance
    {
        get { return MonoSingletonProperty<WebManager>.Instance; }
    }
}
