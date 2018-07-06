
using QFramework;
using QFramework.Example;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 第一界面显示的playerdatalist
/// </summary>
public class PlayerItemButton : MonoBehaviour {

    [SerializeField]
   private PlayerData selfplayerdata;

        /*======================================================================
        * ①将所有信息都存储到本地（ID）.json （MD5，MatchingPoint,PlayerSetting）
        * ②成功后回调
        * ③根据Md5加载normalmodel和usermodel（也可能是下载得来）
        * ④成功回调设置摄像机旋转中心
        * ⑤打开第二界面
        * ⑥刷新UI列表
        * ⑦设置默认界面
        ======================================================================*/

    public void Init(GameObject illnessprefabe, PlayerData data)
    {
        transform.SetParent(illnessprefabe.transform.parent);
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;

        transform.Find("Text (1)").GetComponent<Text>().text = data.note;
        transform.Find("Text (2)").GetComponent<Text>().text = data.illcreatetime;
        AssigneEvents(data);
        SavePlayerData(data);
    }


    /// <summary>
    /// 将网络同步数据(加工后),保存到客户端
    /// </summary>
    /// <param name="data"></param>
    void SavePlayerData(PlayerData data)
    {
        //由于服务器不存储md5及localuserdata只能从本地获取
        //① 拿取本地保存的json文件与服务器数据进行比较
        //playerdata部分此时不可修改

        string localsavedata = Tool.ReadLocalJson(Tool.LocalJsonSavePath + data.ID + ".json");

        PlayerData localsaveplayer = JsonHelper.ParseJsonToNeed<PlayerData>(localsavedata);

        if (string.IsNullOrEmpty(localsavedata) || null == localsaveplayer)
        {
            localsaveplayer = new PlayerData();
        }

        localsaveplayer.ID = data.ID;
        localsaveplayer.title = data.title;
        localsaveplayer.md5 = string.IsNullOrEmpty(data.md5) ? localsaveplayer.md5 : data.md5;
        localsaveplayer.matching_point = data.matching_point;
        localsaveplayer.position = data.position;
        localsaveplayer.injury_position = data.injury_position;
        localsaveplayer.protector_shape = data.protector_shape;
        localsaveplayer.note = data.note;
        localsaveplayer.description = data.description;

        localsaveplayer.LocalUserModelPath = string.IsNullOrEmpty(data.LocalUserModelPath) ? localsaveplayer.LocalUserModelPath : data.LocalUserModelPath;
        localsaveplayer.LocalUserJsonPath = string.IsNullOrEmpty(data.LocalUserJsonPath) ? localsaveplayer.LocalUserJsonPath : data.LocalUserJsonPath;

        localsaveplayer.NormalJsonpath = string.IsNullOrEmpty(data.NormalJsonpath) ? localsaveplayer.NormalJsonpath : data.NormalJsonpath;
        localsaveplayer.Normalmodelpath = string.IsNullOrEmpty(data.Normalmodelpath) ? localsaveplayer.Normalmodelpath : data.Normalmodelpath;

        selfplayerdata = localsaveplayer;

        string json = JsonHelper.ParseObjectToJson(localsaveplayer);
        Tool.UpdateLocaluserdataFiles(localsaveplayer.ID, json);
    }



    /// <summary>
    /// UI赋值事件
    /// </summary>
    /// <param name="_data">绑定的数据</param>
    void AssigneEvents(PlayerData _data)
    {
        Text titlename = transform.Find("Text").GetComponent<Text>();
        titlename.text = _data.title;

        //点击显示/隐藏
        transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener(iSon =>
        {
            titlename.text = iSon ? _data.title : "***";
        });

        //点击编辑
        transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            UICreateIllnessPopupData _d = new UICreateIllnessPopupData();
            _d.playerdata = selfplayerdata;
            UIMgr.OpenPanel<UICreateIllnessPopup>(canvasLevel: UILevel.PopUI, prefabName: "Resources/UICreateIllnessPopup",uiData: _d);
            QUIManager.Instance.HideUI<UIFirstPage>();
        });
        //点击病例
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //TODO ---->>>>> NextScene
            //当前选择用户 并 赋值好normalpoints 及 userpoints
            PlayerDataCenter.Instance.Currentplayerdata = selfplayerdata;

            UIPanelData paneldata = new UIThirdPageData() { pdata = selfplayerdata };

            UIMgr.ClosePanel("Resources/UIFirstPage");

            UIMgr.OpenPanel<UIThirdPage>(prefabName: "Resources/UIThirdPage", uiData:paneldata);

            //* ①将所有信息都存储到本地（ID）.json （MD5，MatchingPoint,PlayerSetting）
            //* ②成功后回调
            //* ③根据Md5加载normalmodel和usermodel（也可能是下载得来）
            //* ④成功回调设置摄像机旋转中心
            //* ⑤打开第二界面
            //* ⑥刷新UI列表
            //* ⑦设置默认界面


            //打开第二界面

        });

    }

}
