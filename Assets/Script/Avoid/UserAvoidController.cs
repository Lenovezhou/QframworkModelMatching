using QFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserAvoidController:MonoBehaviour
{
    Dictionary<int, AbsAvoidGizmo> avoidmap = new Dictionary<int, AbsAvoidGizmo>();

    AvoidControll_E currentcontrollmode;

    int currentcontrollindex;

    Tool.PointMode selfpointmode;

    AbsAvoidGizmo lastedit;

    public void Init(Tool.PointMode pm)
    {
        selfpointmode = pm;
    }


    public void HandleEvent(QMsg msg)
    {
        AvoidMsg am = msg as AvoidMsg;

        AvoidControll_E ace = am.controllmode;

        currentcontrollmode = ace;

        currentcontrollindex = am.index;

        switch (currentcontrollmode)
        {
            case AvoidControll_E.Ido:
                break;
            case AvoidControll_E.Edit:
                OnEdit();
                break;
            case AvoidControll_E.Save:
                OnSave();
                break;
            case AvoidControll_E.Remove:
                OnRemove();
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (CanEditGizmo())
        {
            if (!avoidmap.ContainsKey(currentcontrollindex))
            {
                GameObject avoid = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                avoid.transform.localScale = Vector3.zero;
                avoid.transform.SetParent(transform);
                AbsAvoidGizmo useravoidgizmo = avoid.AddComponent<UserAvoidGizmo>();
                useravoidgizmo.Init(selfpointmode, currentcontrollindex);
                avoidmap.Add(currentcontrollindex, useravoidgizmo);
            }
            //TODO--->>>
            //avoidgizmo 的position 和scale

        }
    }

    void OnEdit()
    {
        AbsAvoidGizmo aag = GetCurrentGizmo(currentcontrollindex);
        aag.CheckMeNeedRelease();
        aag.OnEdit();
        lastedit = aag;
    }


    void OnSave()
    {
        AbsAvoidGizmo aag = GetCurrentGizmo(currentcontrollindex);
        aag.OnSave();
    }

    void OnRemove()
    {
        AbsAvoidGizmo aag = GetCurrentGizmo(currentcontrollindex);
        aag.OnRemove();
    }

    bool CanEditGizmo()
    {
        return Input.GetMouseButtonDown(0) && currentcontrollmode == AvoidControll_E.Edit && !EventSystem.current.IsPointerOverGameObject();
    }

    AbsAvoidGizmo GetCurrentGizmo(int index)
    {
        AbsAvoidGizmo aag = null;
        if (avoidmap.ContainsKey(index))
        {
            aag = avoidmap[index];
        }
        return aag;
    }

}