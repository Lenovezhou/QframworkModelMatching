using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvoidvacancyPanelControl : AbstractButtonOpenPanel,IPanelItem
{
    Dictionary<int, GameObject> childmap = new Dictionary<int, GameObject>();
    int lastchoise = -1;
    public void OnEnterThisPage()
    {
        transform.Find("Panel (2)").gameObject.SetActive(false);
        //Enums.AvoidvacancyControll ac = Enums.AvoidvacancyControll.ActiveAll;
        //MSGCenter.Execute(ac.ToString());

        //MSGCenter.Execute(Enums.LeftMouseButtonControl.AddAvoidGizmo.ToString(),"True");

    }

    public void OnLeveThisPage()
    {
        transform.Find("Panel (2)").gameObject.SetActive(false);
        UnspawnAll();
        //Enums.AvoidvacancyControll ac = Enums.AvoidvacancyControll.InactiveAll;
        //MSGCenter.Execute(ac.ToString());

        //MSGCenter.Execute(Enums.LeftMouseButtonControl.AddAvoidGizmo.ToString(), "False");

    }

    void Start ()
    {
        GameObject prefab = transform.Find("Panel/Scroll View/Viewport/Content/AvoidvacancyItem").gameObject;
        Transform parent = prefab.transform.parent;

        InitPool(10, prefab);

        transform.Find("Panel/AddButton").GetComponent<Button>().onClick.AddListener(() => 
        {
            GameObject go = SpawnChildren(SpawnChild.FirstPoolItem,parent);
            int index = go.transform.parent.childCount - 2;

            //添加(只表现在UI上)
            childmap.Add(index, go);
            go.GetComponentInChildren<Text>().text = "第" + (index + 1) + "个避空位";
            //删除
            go.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                Destroy(go);
                childmap.Remove(index);
                Send(AvoidControll_E.Remove, index);
            });
            //编辑
            go.transform.Find("Button (1)").GetComponent<Button>().onClick.AddListener(() =>
            {
                transform.Find("Panel (2)").gameObject.SetActive(true);
                Send(AvoidControll_E.Edit, index);
                if (lastchoise == index)
                {
                    return;
                }
                go.GetComponent<Image>().color = Color.green;
                if (childmap.ContainsKey(lastchoise))
                {
                    childmap[lastchoise].GetComponent<Image>().color = Color.gray;
                }
                lastchoise = index;
            });
            //显示/隐藏
            go.transform.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener((iSon) => 
            {
                AvoidControll_E ace = AvoidControll_E.DisplaySingle;
                if (!iSon)
                {
                    ace = AvoidControll_E.HiddenSingle;
                }
                Send(ace, index);
            });
        });

        //隐藏/显示全部
        transform.Find("Panel/AllToggle").GetComponent<Toggle>().onValueChanged.AddListener((iSon) => 
        {
            AvoidControll_E ace = AvoidControll_E.DisplayAll;
            if (!iSon)
            {
                ace = AvoidControll_E.HiddenAll;
            }
            Send(ace);
        });


        //保存
        transform.Find("Panel (2)/Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.Find("Panel (2)").gameObject.SetActive(false);
            Send(AvoidControll_E.Save);
        });
        //取消
        transform.Find("Panel (2)/Button (1)").GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.Find("Panel (2)").gameObject.SetActive(false);
            Send(AvoidControll_E.Cancle);
        });
        //重置
        transform.Find("Panel (2)/Button (2)").GetComponent<Button>().onClick.AddListener(() => 
        {
            transform.Find("Panel (2)").gameObject.SetActive(false);
           // MSGCenter.Execute(Enums.AvoidvacancyControll.Reset.ToString());
        });


    }



    void Send(AvoidControll_E ace,int _index = -1)
    {
        QMsg useravoidmsg = new AvoidMsg() { EventID = (int)Avoid_E.Begin,controllmode = ace,index = _index};
        AvoidManager.Instance.SendMsg(useravoidmsg);
    }

}
