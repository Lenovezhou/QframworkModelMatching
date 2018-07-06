using QFramework;
using System.Collections.Generic;
using UnityEngine;

public class AbstractPointscontroll : MonoBehaviour
{
    /// <summary>
    /// 可添加的pointergizmo类型
    /// </summary>
    protected Tool.PointMode selfpointmode;

    /// <summary>
    /// 点数据
    /// </summary>
    protected Dictionary<int, Dictionary<int, AbstractGizmoPointer>> pointmap;

    /// <summary>
    /// 构造函数决定自己添加gizmopointer类型
    /// </summary>
    /// <param name="mode"></param>
    public AbstractPointscontroll(Tool.PointMode mode)
    {
        selfpointmode = mode;
    }

    /// <summary>
    /// 加载模型
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="pointsmap"></param>
    protected virtual void LoadPointer(Transform parent, Dictionary<int, Dictionary<int, Vector3>> pointsmap)
    {
        transform.SetParent(parent);
        if (null == pointmap)
        {
            pointmap = new Dictionary<int, Dictionary<int, AbstractGizmoPointer>>();
        }
        foreach (KeyValuePair<int, Dictionary<int, Vector3>> item in pointsmap)
        {
            Dictionary<int, Vector3> templist = item.Value;
            Dictionary<int, AbstractGizmoPointer> tempdic = new Dictionary<int, AbstractGizmoPointer>();
            List<Transform> tempgos = new List<Transform>();

            foreach (KeyValuePair<int, Vector3> it in item.Value)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //g.transform.localScale = Vector3.one * 3;
                g.transform.SetParent(transform,false);
                g.transform.localPosition = it.Value;
                //g.SetActive(false);
                int group = item.Key;
                int index = it.Key;
                float s = 0.3f / 0.1f;
                Vector3 finalscaler = Vector3.one * s;

                AbstractGizmoPointer p = null;
                switch (selfpointmode)
                {
                    case Tool.PointMode.Normal:
                        p = g.AddComponent<NormalGizmoPointer>();
                        break;
                    case Tool.PointMode.UserImport:
                        p = g.AddComponent<UserGizmoPointer>();
                        break;
                    default:
                        break;
                }
                p.Init(group, index, finalscaler);
                Destroy(g.GetComponent<Collider>());
                tempgos.Add(g.transform);
                tempdic.Add(index, p);
            }
            pointmap.Add(item.Key, tempdic);
        }
    }

    /// <summary>
    /// 接收到通知
    /// </summary>
    /// <param name="msg">事件数据</param>
    public virtual void HandleEvent(QMsg msg) { }

    /// <summary>
    /// 查找选中的点
    /// </summary>
    /// <param name="group">组</param>
    /// <param name="index">序号</param>
    private AbstractGizmoPointer CheckChoise(int group, int index)
    {
        foreach (KeyValuePair<int, Dictionary<int, AbstractGizmoPointer>> item in pointmap)
        {
            foreach (KeyValuePair<int, AbstractGizmoPointer> it in item.Value)
            {
                if (it.Value.IsMe(group, index))
                {
                    return it.Value;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 释放该点
    /// </summary>
    /// <param name="group">组</param>
    /// <param name="index">序号</param>
    public void ReleasePointe(int group, int index)
    {
        AbstractGizmoPointer p = CheckChoise(group, index);
        p.ReleseMe();
    }

    protected virtual void RefreshDisplay(PointControll_E pce,QMsg msg)
    {
        PointMsg pm = msg as PointMsg;
        switch (pce)
        {
            case PointControll_E.ChoiseGroup:
                Refreshdis(pm.group);
                break;
            case PointControll_E.AllDisplay:
                Refreshdis(-1);
                break;
            default:
                break;
        }
    }

    private void Refreshdis(int index)
    {
        foreach (KeyValuePair<int, Dictionary<int, AbstractGizmoPointer>> item in pointmap)
        {
            foreach (KeyValuePair<int, AbstractGizmoPointer> it in item.Value)
            {
                if (it.Value.IsMe(index, it.Key))
                {
                    it.Value.Display();
                }
                else {
                    it.Value.Hidden();
                }
            }
        }
    }
}
