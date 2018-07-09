using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingpointPanelControl : AbstractButtonOpenPanel,IPanelItem
{
    //分组显示或全部显示
    int dropdownindex = 0;
    //选中组
    int currentgroup = 0;
    //选中序号
    int currentindex = 0;
    //组按钮预设button
    GameObject Groupbuttonitem;
    //点预设button
    GameObject Pointbuttonitem;
    //title上的button控制的所有panel父级
    GameObject toolpanel;
    //根据标准件生成的所有button
    Dictionary<int, Dictionary<int,MatchingItemButton>> buttonmap = new Dictionary<int, Dictionary<int, MatchingItemButton>>();


    //编辑点时保存和取消按钮
    Button savepoint, canceledit;
    //编辑点时自动下一点的toggle
    Toggle autonexe;

    //分组显示or全部显示dropdown
    Dropdown displaymodeDd;
    //上次选中的点的按钮
    private MatchingItemButton lastchoiseitem;

    //是否已经初始化过缓存池
    private bool hasinitpool = false;


    public void InitChild()
    {
        displaymodeDd = transform.Find("DisplaymodeDropdown").GetComponent<Dropdown>();
        Groupbuttonitem = transform.Find("ListScrollView/Viewport/Content/ListItem").gameObject;
        Pointbuttonitem = transform.Find("PointsScrollView/Viewport/Content/DecorativepatternItem").gameObject;

        toolpanel = transform.Find("Panel (1)").gameObject;
        savepoint = toolpanel.transform.Find("Button").GetComponent<Button>();
        canceledit = toolpanel.transform.Find("Button (1)").GetComponent<Button>();
        autonexe = toolpanel.transform.Find("Toggle").GetComponent<Toggle>();

        savepoint.onClick.RemoveAllListeners();
        canceledit.onClick.RemoveAllListeners();
        autonexe.onValueChanged.RemoveAllListeners();

        savepoint.onClick.AddListener(() =>
        {
            Send(PointControll_E.SaveMatchingpoint);
        });
        canceledit.onClick.AddListener(() =>
        {
            transform.Find("Panel (1)").gameObject.SetActive(false);
            Send(PointControll_E.Cancle);
        });
        autonexe.onValueChanged.AddListener((iSon) =>
        {
            Send(PointControll_E.AutoNext);
        });

        AssignDropdown(displaymodeDd);



        listmap.Clear();
        buttonmap.Clear();
        if (!hasinitpool)
        {
            InitPool(15,Groupbuttonitem,100,Pointbuttonitem);
            hasinitpool = true;
        }
    }


    /// <summary>
    /// 初始化("分组显示", "全部显示")下拉列表
    /// </summary>
    /// <param name="Dd"></param>
    void AssignDropdown(Dropdown Dd)
    {
        Dd.onValueChanged.RemoveAllListeners();
        List<string> namelist = new List<string>() { "分组显示", "全部显示" };
        Dd.InitDropDown(namelist);
        Dd.onValueChanged.AddListener(DropdownCallBack);
    }

    /// <summary>
    /// ("分组显示", "全部显示")下拉列表，事件定义
    /// </summary>
    /// <param name="index"></param>
    void DropdownCallBack(int index)
    {
        dropdownindex = index;
        QMsg usermsg = new PointMsg()
        {
            EventID = (int)Point_E.Userimport,
            group = currentgroup,
            controllmode = index == 0 ? PointControll_E.ChoiseGroup : PointControll_E.AllDisplay
        };
        PointerManager.Instance.SendMsg(usermsg);
        QMsg normalmsg = new PointMsg()
        {
            EventID = (int)Point_E.Normal,
            group = currentgroup,
            controllmode = index == 0 ? PointControll_E.ChoiseGroup : PointControll_E.AllDisplay
        };
        PointerManager.Instance.SendMsg(normalmsg);

        //Enums.PointControll pcontroll = (Enums.PointControll)index;
        //MSGCenter.Execute(pcontroll.ToString(),choiseindex.ToString());
    }


    /// <summary>
    /// 根据normalmodledata数据刷新两列button
    /// </summary>
    public void RefreshViewByNormalModleData()
    {
        Dictionary<int, Dictionary<int, Vector3>> normalpointmap = PlayerDataCenter.Instance.Currentnormalpointmap;
        if (null == normalpointmap || normalpointmap.Count == 0)
        {
            Debug.LogError("pointmap 为空！！！");
            return;
        }
        foreach (KeyValuePair<int, Dictionary<int, Vector3>> item in normalpointmap)
        {
            int group = item.Key;
            GameObject but = SpawnChildren(SpawnChild.FirstPoolItem,Groupbuttonitem.transform.parent);
            but.transform.SetAsLastSibling();
            Button b = but.GetComponent<Button>();
            string groupstr = Tool.NumberToChar(group + 1).ToUpper();
            b.GetComponentInChildren<Text>().text = groupstr;
            b.onClick.AddListener(() => 
            {
                ResetLastGroupItem(b,group);
                ChoisePanel(b);
            });
            Dictionary<int,Vector3> templist = item.Value;
            List<GameObject> tempgos = new List<GameObject>();
            Dictionary<int, MatchingItemButton> dicbuts = new Dictionary<int, MatchingItemButton>();

            foreach (KeyValuePair<int,Vector3> it in templist)
            {
                GameObject g = SpawnChildren(SpawnChild.SecendPoolItem, Pointbuttonitem.transform.parent);

                g.SetActive(false);
                int tempgroup = group;
                int tempindex = it.Key;
                g.transform.SetAsLastSibling();

                g.GetComponentInChildren<Text>().text = groupstr +" "+ (tempindex + 1).ToString();

                MatchingItemButton Matchingitem = g.GetComponent<MatchingItemButton>();
                Matchingitem.Init(tempgroup, tempindex, SetLastMatchingItem);
                tempgos.Add(g);
                dicbuts.Add(tempindex,Matchingitem);
            }
            listmap.Add(b, tempgos);
            buttonmap.Add(group, dicbuts);


            if (group == currentgroup)
            {
                ChoisePanel(b);
            }

        }
    }


    public void RefreshViewByUserModelData()
    {
        Dictionary<int, Dictionary<int, Vector3>> savedmap = PlayerDataCenter.Instance.Currentuserpointmap;

        foreach (KeyValuePair<int,Dictionary<int,Vector3>> item in savedmap)
        {
            foreach (KeyValuePair<int,Vector3> it in item.Value)
            {
                MatchingItemButton mib = GetMatchingItem(item.Key, it.Key);
                if (mib)
                {
                    mib.HandleEvent(PointControll_E.SaveMatchingpoint);
                }
            }
        }

    }

    MatchingItemButton GetMatchingItem(int group,int index)
    {
        MatchingItemButton mib = null;
        if (buttonmap.ContainsKey(group) && buttonmap[group].ContainsKey(index))
        {
            mib = buttonmap[group][index];
        }
        return mib;
    }

    void ResetLastGroupItem(Button currentlistbutton,int group)
    {
        if (lastkey)
        {
            lastkey.GetComponent<Image>().color = Color.white;
        }
        currentlistbutton.GetComponent<Image>().color = Color.green;

        //判断当前是分组显示
        if (dropdownindex == 0)
        {
            QMsg usermsg = new PointMsg()
            {
                EventID = (int)Point_E.Userimport,
                group = group,
                controllmode = PointControll_E.ChoiseGroup
            };
            PointerManager.Instance.SendMsg(usermsg);
            QMsg normalmsg = new PointMsg()
            {
                EventID = (int)Point_E.Normal,
                group = group,
                controllmode = PointControll_E.ChoiseGroup
            };
            PointerManager.Instance.SendMsg(normalmsg);
        }
        currentgroup = group;
       // lastkey = currentlistbutton;
    }


    void SetLastMatchingItem(MatchingItemButton item)
    {
        if (lastchoiseitem)
        {
            lastchoiseitem.CheckMeNeedCancle();
        }
        toolpanel.SetActive(true);
        this.lastchoiseitem = item;
    }

    /// <summary>
    /// 进入该页面的刷新
    /// </summary>
    public void OnEnterThisPage()
    {
        //每次进入该页面均需刷新
        InitChild();
        RefreshViewByNormalModleData();
        RefreshViewByUserModelData();
        toolpanel.SetActive(false);

        //TODO--->>>根据之前选择的[分组显示or全部显示]再次显示出对应点
        if (displaymodeDd)
        {
            displaymodeDd.onValueChanged.Invoke(dropdownindex);
        }
        if (lastkey)
        {
            lastkey.onClick.Invoke();
        }
    }

    /// <summary>
    /// 离开该页面的刷新
    /// </summary>
    public void OnLeveThisPage()
    {
        UnspawnAll();
        toolpanel.SetActive(false);
        //MSGCenter.Execute(Enums.LeftMouseButtonControl.AddMatchingGizmo.ToString(), "False");
        ////TODO--->>>隐藏所有点
        //MSGCenter.Execute(Enums.PointControll.ShutDownAll.ToString());
        ////摄像机的中心点恢复
        //MSGCenter.Execute(Enums.ViewMode.ChangeTargetPos.ToString(), Vector3.zero.ToString());
    }




    //封装发送
    private void Send(PointControll_E pcontroller)
    {
        QMsg normalmsg = new PointMsg() { EventID = (int)Point_E.Normal, controllmode = pcontroller};
        PointerManager.Instance.SendMsg(normalmsg);
        QMsg usermsg = new PointMsg() { EventID = (int)Point_E.Userimport, controllmode = pcontroller};
        PointerManager.Instance.SendMsg(usermsg);

        lastchoiseitem.HandleEvent(pcontroller);
        toolpanel.SetActive(pcontroller == PointControll_E.Edit);
    }


}
