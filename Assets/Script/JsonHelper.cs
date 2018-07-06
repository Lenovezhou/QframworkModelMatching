using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper : Singleton<JsonHelper> {


    /// <summary>
    /// 泛型解析出想要的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    static public T ParseJsonToNeed<T>(string json)
    {
        T t = JsonConvert.DeserializeObject<T>(json);
        return t;

    }

    /// <summary>
    /// 使用json 插件解析
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    static public string ParseObjectToJson<T>(T t)
    {
        return JsonConvert.SerializeObject(t);
    }


    /// <summary>
    /// 从json里拿出ID
    /// </summary>
    /// <param name="key"></param>
    /// <param name="json"></param>
    /// <returns></returns>
    static public int ParseNewIllDataID(string key, string json)
    {
        JObject o = JObject.Parse(json);

        return int.Parse(o[key].ToString());

    }


    /// <summary>
    /// 新增用户时，合并出 服务器所需的json格式
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    static public string MergPlayerdataJson(PlayerData data)
    {
        JObject requestobject = new JObject();
        JProperty title = new JProperty("title", data.title);
        JProperty injury_position = new JProperty("injury_position", data.injury_position.ToString());
        JProperty position = new JProperty("position", data.position);
        JProperty description = new JProperty("description", data.description.ToString());
        JProperty note = new JProperty("note", data.note);
        JProperty protector_shape = new JProperty("protector_shape", data.protector_shape.ToString());

        requestobject.Add(title);
        requestobject.Add(injury_position);
        requestobject.Add(position);
        requestobject.Add(description);
        requestobject.Add(note);
        requestobject.Add(protector_shape);

        return requestobject.ToString();
    }

    /// <summary>
    /// 根据数据中的matchingpoint字段得出点的列表
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    static public Dictionary<int, Dictionary<int, Vector3>> ParseJsonToMatchingpointroot(string json)
    {
        Dictionary<int, Dictionary<int, Vector3>> map = new Dictionary<int, Dictionary<int, Vector3>>();

        if (string.IsNullOrEmpty(json))
        {
            return map;
        }
        var v = JObject.Parse(json);
        if (!string.IsNullOrEmpty(v["data"].ToString()))
        {
            string _str = v["data"]["points"].ToString();
            if (!string.IsNullOrEmpty(_str))
            {
                string matchingstr = v["data"]["points"]["matching_point"].ToString();
                JProperty jp = new JProperty("matching_point", matchingstr);
                JObject jo = new JObject(jp);
                map = ParseMatchingpointsJosn(jo.ToString());
            }
        }
        return map;
    }

    /// <summary>
    /// 解析时normal或userimport时point序号都是从1开始
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    static public Dictionary<int, Dictionary<int, Vector3>> ParseMatchingpointsJosn(string json)
    {
        Dictionary<int, Dictionary<int, Vector3>> map = new Dictionary<int, Dictionary<int, Vector3>>();

        if (string .IsNullOrEmpty(json))
        {
            return map;
        }

        string datapointkey = Tool.datapointkey;
        //string poitmatchingpointkey = Tool.pointmatchingpoints;
        string key = "matching_point";

        JObject msgData = (JObject)JsonConvert.DeserializeObject(json);
        string channelStr = msgData.Property(key) != null ? msgData[key].ToString() : "error_noKey";
        //LogView.setViewText ("GameGlobal.cs,setChannelInfo(),channelStr=="+channelStr);
        JObject itemData = (JObject)JsonConvert.DeserializeObject(channelStr);

        int group = -1;

        foreach (KeyValuePair<string, JToken> item in itemData)
        {
            Dictionary<int, Vector3> dic = new Dictionary<int, Vector3>();
            //ABCD...I
            key = item.Key;
            group = Tool.CharToNumber(key) - 65;
            string itemchannelStr = itemData.Property(key) != null ? itemData[key].ToString() : "error_noKey";
            JObject itdata = (JObject)JsonConvert.DeserializeObject(itemchannelStr);

            int index = -1;
            foreach (KeyValuePair<string, JToken> it in itdata)
            {
                //0234...7
                key = it.Key;
                index = int.Parse(key);
                string posindex = itdata.Property(key) != null ? itdata[key].ToString() : "error_noKey";
                JObject posdata = (JObject)JsonConvert.DeserializeObject(posindex);
                Vector3 pos = Vector3.zero;
                //xyz
                foreach (KeyValuePair<string, JToken> p in posdata)
                {
                    key = p.Key;
                    switch (key)
                    {
                        case "x":
                            pos.x = float.Parse(p.Value.ToString());
                            break;
                        case "y":
                            pos.y = float.Parse(p.Value.ToString());
                            break;
                        case "z":
                            pos.z = float.Parse(p.Value.ToString());
                            break;
                        default:
                            break;
                    }
                }
                dic.Add(index - 1, pos);
            }
            map.Add(group, dic);
        }
        return map;
    }

}



#region 病例所需
public class DataItem
{
    public int case_id { get; set; }
    public string title { get; set; }
    public string injury_position { get; set; }
    public bool potsition { get; set; }
    public string description { get; set; }
    public string note { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public string protector_shape { get; set; }
}

public class IllDatalistRoot
{
    public string success { get; set; }
    public List<DataItem> data { get; set; }
}
#endregion