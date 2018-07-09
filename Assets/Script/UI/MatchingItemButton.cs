using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingItemButton : MonoBehaviour {

    Button selfbutton;
    Text selfstatetext;
    [SerializeField]
    int group = -1;
    [SerializeField]
    int index = -1;
    string laststate;
    Color buttonorigincolor;
    Color nomatchingcolor = Color.red;
    Color matchingcolor = Color.yellow;
    Color matchingDoneColor = Color.green;
    string nomatchingstr = "未匹配";
    string matchingstr = "编辑中";
    string matchingdonestr = "匹配完成";


    Action<MatchingItemButton> callback;

    bool hassaved = false;

    private void Start()
    {
        if ( null == selfstatetext)
        {
            CheckChildAssigne();
        }
    }
    void CheckChildAssigne ()
    {
        selfbutton = GetComponentInChildren<Button>();
        buttonorigincolor = transform.GetComponent<Image>().color;
        selfstatetext = transform.Find("StateText").GetComponentInChildren<Text>();
        selfstatetext.text = nomatchingstr;
        selfstatetext.color = nomatchingcolor;
        laststate = selfstatetext.text;
        selfbutton.onClick.RemoveAllListeners();
        selfbutton.onClick.AddListener(OnEditHit);
	}


    private void OnEditHit()
    {
        if (null == selfstatetext)
        {
            CheckChildAssigne();
        }
        callback(this);
        DotweenColor(transform.GetComponent<Image>(), new Color(1, 1, 1, 0.4f), Color.red);
        selfstatetext.text = matchingstr;
        selfstatetext.color = matchingcolor;
        selfbutton.GetComponent<Image>().color = Color.black;

        

        //发送到pointmanager-->>>>
        Send(PointControll_E.Edit);
    }


    private void SaveMatchingpoint()
    {
        if (null == selfstatetext)
        {
            CheckChildAssigne();
        }
        hassaved = true;
        DotweenColor(transform.GetComponent<Image>(), Color.white, Color.red);
        selfstatetext.text = matchingdonestr;
        selfstatetext.color = matchingDoneColor;
    }

    private void Cancle()
    {
        hassaved = false;
        if (selfstatetext)
        {
            selfstatetext.text = nomatchingstr;
            selfstatetext.color = nomatchingcolor;
            selfbutton.GetComponent<Image>().color = Color.white;
        }
    }


    //封装发送
    private void Send(PointControll_E pcontroller)
    {
        QMsg normalmsg = new PointMsg() { EventID = (int)Point_E.Normal, controllmode = pcontroller, group = this.group, index = this.index };
        PointerManager.Instance.SendMsg(normalmsg);
        QMsg usermsg = new PointMsg() { EventID = (int)Point_E.Userimport, controllmode = pcontroller, group = this.group, index = this.index };
        PointerManager.Instance.SendMsg(usermsg);

    }


    public void CheckMeNeedCancle()
    {
        if (hassaved)
        {
            SaveMatchingpoint();
        }
        else
        {
            Cancle();
        }
    }


    public void Init(int _group,int _index,Action<MatchingItemButton> callback)
    {
        this.group = _group;
        this.index = _index;
        this.callback = callback;
    }


    void DotweenColor(Image _image, Color normal, Color worming)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_image.DOColor(worming, 0.5f).SetEase(Ease.Linear).SetLoops(2));

        seq.OnComplete(() => { _image.color = normal; });
    }


    public void HandleEvent(PointControll_E pcontroller)
    {
        switch (pcontroller)
        {
            case PointControll_E.SaveMatchingpoint:
                SaveMatchingpoint();
                break;
            case PointControll_E.Cancle:
                Cancle();
                break;
            case PointControll_E.AutoNext:
                break;
            default:
                break;
        }
    }
}
