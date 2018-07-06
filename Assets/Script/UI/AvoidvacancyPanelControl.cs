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
        //Enums.AvoidvacancyControll ac = Enums.AvoidvacancyControll.InactiveAll;
        //MSGCenter.Execute(ac.ToString());

        //MSGCenter.Execute(Enums.LeftMouseButtonControl.AddAvoidGizmo.ToString(), "False");

    }

    void Start ()
    {
        GameObject prefab = transform.Find("Panel/Scroll View/Viewport/Content/AvoidvacancyItem").gameObject;
        Transform parent = prefab.transform.parent;
        transform.Find("Panel/AddButton").GetComponent<Button>().onClick.AddListener(() => 
        {
           GameObject go = SpawnChildren(prefab);
            int index = go.transform.parent.childCount - 2;

            //添加
            childmap.Add(index, go);
            //MSGCenter.Execute(Enums.AvoidvacancyControll.Add.ToString(), index.ToString());
            go.GetComponentInChildren<Text>().text = "第" + (index + 1) + "个避空位";
            //删除
            go.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                Destroy(go);
                childmap.Remove(index);
               // MSGCenter.Execute(Enums.AvoidvacancyControll.Remove.ToString(),index.ToString());
            });
            //编辑
            go.transform.Find("Button (1)").GetComponent<Button>().onClick.AddListener(() =>
            {
                transform.Find("Panel (2)").gameObject.SetActive(true);
                //MSGCenter.Execute(Enums.AvoidvacancyControll.Edit.ToString(), index.ToString());
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
               // Enums.AvoidvacancyControll ac = Enums.AvoidvacancyControll.Active;
                if (!iSon)
                {
                    //ac = Enums.AvoidvacancyControll.Inactive;
                }
                //MSGCenter.Execute(ac.ToString(), index.ToString());
            });
        });

        //隐藏/显示全部
        transform.Find("Panel/AllToggle").GetComponent<Toggle>().onValueChanged.AddListener((iSon) => 
        {
            //Enums.AvoidvacancyControll ac = Enums.AvoidvacancyControll.ActiveAll;
            if (!iSon)
            {
               // ac = Enums.AvoidvacancyControll.InactiveAll;
            }
            //MSGCenter.Execute(ac.ToString());
        });


        //保存
        transform.Find("Panel (2)/Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.Find("Panel (2)").gameObject.SetActive(false);
           // MSGCenter.Execute(Enums.AvoidvacancyControll.SaveAvoid.ToString());
        });
        //取消
        transform.Find("Panel (2)/Button (1)").GetComponent<Button>().onClick.AddListener(() =>
        {
            transform.Find("Panel (2)").gameObject.SetActive(false);
           // MSGCenter.Execute(Enums.AvoidvacancyControll.Remove.ToString());
        });
        //重置
        transform.Find("Panel (2)/Button (2)").GetComponent<Button>().onClick.AddListener(() => 
        {
            transform.Find("Panel (2)").gameObject.SetActive(false);
           // MSGCenter.Execute(Enums.AvoidvacancyControll.Reset.ToString());
        });


    }

}
