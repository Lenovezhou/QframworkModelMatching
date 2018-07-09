using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataCenter :MonoBehaviour,ISingleton
{
    #region 全局数据声明
    private PlayerData currentplayerdata;

    private Dictionary<int, Dictionary<int, Vector3>> currentuserpointmap = new Dictionary<int, Dictionary<int, Vector3>>();

    private Dictionary<int, Dictionary<int, Vector3>> currentnormalpointmap = new Dictionary<int, Dictionary<int, Vector3>>();

    private Dictionary<Tool.MatarialsUse, Material> materialmap;


    private List<PlayerData> playerDatalist = new List<PlayerData>();
    #endregion

    #region 只对外公布获取接口

    /// <summary>
    /// 用户点（当前和服务器总）
    /// </summary>
    public Dictionary<int, Dictionary<int, Vector3>> Currentuserpointmap
    {
        get
        {
            return currentuserpointmap;
        }
    }

    /// <summary>
    /// 保存单个点调用
    /// </summary>
    /// <param name="group">所在组</param>
    /// <param name="index">所在序列号</param>
    /// <param name="localposition">本地坐标</param>
    public void FillCurrentuserpointmap(int group,int index,Vector3 localposition)
    {
        if (!currentuserpointmap.ContainsKey(group))
        {
            currentuserpointmap.Add(group, new Dictionary<int, Vector3>());
        }
        if (!currentuserpointmap[group].ContainsKey(index))
        {
            currentuserpointmap[group].Add(index, localposition);
        }
        currentuserpointmap[group][index] = localposition;
    }
    
    /// <summary>
    /// 标准点
    /// </summary>
    public Dictionary<int, Dictionary<int, Vector3>> Currentnormalpointmap
    {
        get
        {
            return currentnormalpointmap;
        }
    }

    /// <summary>
    /// 材质列表
    /// </summary>
    public Dictionary<Tool.MatarialsUse, Material> MaterialMap
    {
        get
        {
            if (null == materialmap)
            {
                Material origin = Instantiate(Resources.Load<Material>("Materials/origin_normal"));
                Material highlight = Instantiate(Resources.Load<Material>("Materials/highlight_normal"));
                Material highlight_userimport = Instantiate(Resources.Load<Material>("Materials/highlight_userimport"));
                Material origin_userimport = Instantiate(Resources.Load<Material>("Materials/origin_userimprot"));

                Material indicatoreffect = Instantiate(Resources.Load<Material>("Materials/indicatoreffect"));
                Material indicatornormal = Instantiate(Resources.Load<Material>("Materials/indicatornormal"));
                Material Normalmodel = Instantiate(Resources.Load<Material>("Materials/NormalModel"));
                Material UserImportModel = Instantiate(Resources.Load<Material>("Materials/UserImportModel"));

                materialmap = new Dictionary<Tool.MatarialsUse, Material>();

                materialmap.Add(Tool.MatarialsUse.Indicator_Effect, indicatoreffect);
                materialmap.Add(Tool.MatarialsUse.Indicator_origin, indicatornormal);
                materialmap.Add(Tool.MatarialsUse.NormalModel, Normalmodel);
                materialmap.Add(Tool.MatarialsUse.UserimportModel, UserImportModel);
                materialmap.Add(Tool.MatarialsUse.NormalPoint_origin, origin);
                materialmap.Add(Tool.MatarialsUse.NormalPoint_hight, highlight);
                materialmap.Add(Tool.MatarialsUse.UserImportpoint_origin, origin_userimport);
                materialmap.Add(Tool.MatarialsUse.UserImportpoint_hight, highlight_userimport);
            }

            return materialmap;
        }
    }


    /// <summary>
    /// 全部保存并提交到服务器，准备获取下载模型
    /// </summary>
    public void SaveAll()
    {
        //发送所有点数据到服务器，记录本地偏好设置到json文件中

    }


    #endregion

    /// <summary>
    /// 当前用户 修改当前用户的时候 自动填充新的用户点和标准点
    /// </summary>
    public PlayerData Currentplayerdata
    {
        get
        {
            return currentplayerdata;
        }

        set
        {
            currentplayerdata = value;
            //填充用户点
            currentuserpointmap = JsonHelper.ParseJsonToMatchingpointroot(currentplayerdata.matching_point);
            if (string.IsNullOrEmpty(currentplayerdata.md5))
            {
                Log.E("md5为空，加载默认模型");
                currentplayerdata.md5 = Tool.DefultMd5;
            }
            //填充标准点
            string normalpointsjson = Tool.ReadLocalJson(Tool.LocalNormalpointsJsonPath + currentplayerdata.md5+".json");
            currentnormalpointmap = JsonHelper.ParseMatchingpointsJosn(normalpointsjson);

            //load标准模型
            QMsg normalmsg = new ModelMsg
            {
                EventID = (int)Model_E.Normal,
                pdata = currentplayerdata,
                pointmap = currentnormalpointmap,
                modelevent = Model_E.LoadModel
            };
            QMsgCenter.Instance.SendMsg(normalmsg);

            //load用户模型
            QMsg usermsg = new ModelMsg
            {
                EventID = (int)Model_E.UserImport,
                pdata = currentplayerdata,
                pointmap = currentuserpointmap,
                modelevent = Model_E.LoadModel
            };
            QMsgCenter.Instance.SendMsg(usermsg);
        }
    }

    /// <summary>
    /// 用户列表
    /// </summary>
    public List<PlayerData> PlayerDatalist
    {
        get
        {
            return playerDatalist;
        }

        set
        {
            playerDatalist = value;
        }
    }

    /// <summary>
    /// 服务器发送来的数据，转化为用户列表
    /// </summary>
    /// <param name="checkIllDatalist"></param>
    /// <param name="Call"></param>
    public void RevertToNormal(IllDatalistRoot checkIllDatalist, Action<List<PlayerData>> Call)
    {
        for (int i = 0; i < checkIllDatalist.data.Count; i++)
        {
            PlayerData ind = new PlayerData();

            DataItem di = checkIllDatalist.data[i];
            ind.ID = di.case_id;
            ind.title = di.title;
            try
            {
                ind.protector_shape = (PlayerData.ProtectorShape)Enum.Parse(typeof(PlayerData.ProtectorShape), di.protector_shape);
            }
            catch (Exception)
            {
                ind.protector_shape = PlayerData.ProtectorShape.Long;
            }

            ind.position = di.potsition ? PlayerData.Direction.Right : PlayerData.Direction.Left;

            ind.note = di.note;
            ind.description = di.description;
            ind.illcreatetime = di.created_at;
            ind.injury_position = di.injury_position;
            
            playerDatalist.Add(ind);
        }

        Call.InvokeGracefully(playerDatalist);
    }

    public void OnSingletonInit()
    {
        //throw new NotImplementedException();
    }

    private PlayerDataCenter() { }

    public static PlayerDataCenter Instance
    {
        get { return MonoSingletonProperty<PlayerDataCenter>.Instance; }
    }


}

[Serializable]
//用户数据
public class PlayerData
{
    public enum InjuryPosition { Arm, Lowerleg, Shoulder, Ankle }
    public enum Direction { Left, Right }
    public enum ProtectorShape { Short, Long }


    //受伤方向
    public Direction position;
    //护具外形
    public ProtectorShape protector_shape;

    //ID
    public int ID;
    //本地保存用户模型地址
    public string LocalUserModelPath;
    //本地保存用户设置json地址
    public string LocalUserJsonPath;
    //本地保存标准点json地址
    public string NormalJsonpath;
    //本地保存标准件模型地址
    public string Normalmodelpath;
    //病例名称
    public string title;
    //病例描述
    public string description;
    //病例备注
    public string note;
    //病例创建时间
    public string illcreatetime;
    //受伤部位
    public string injury_position;
    //用户保存匹配点
    public string matching_point;
    //用户模型md5码
    public string md5;

    //用户偏好设置
    public LoclaUserData localuserdata;
}



[Serializable]
#region 本地修改(偏好设置)
public class LoclaUserData
{
    public enum DataKey { Postion, Eulerangel, Scale }
    //本地坐标，需双精度浮点数
    public float usermodellocalposX;
    public float usermodellocalposY;
    public float usermodellocalposZ;

    public float usermodellocaleulerangleX;
    public float usermodellocaleulerangleY;
    public float usermodellocaleulerangleZ;

    public float normalmodelscalerX;
    public float normalmodelscalerY;
    public float normalmodelscalerZ;
    //是否填充
    public bool isfill;
}
#endregion


