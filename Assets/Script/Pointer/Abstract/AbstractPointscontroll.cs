using QFramework;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPointscontroll : MonoBehaviour
{
    /// <summary>
    /// 可添加的pointergizmo类型
    /// </summary>
    protected Tool.PointMode selfpointmode;

    /// <summary>
    /// 观察相机
    /// </summary>
    protected Camera selfcamera;

    /// <summary>
    /// 当前控制状态
    /// </summary>
    protected  PointControll_E currentmode;

    /// <summary>
    /// 点数据
    /// </summary>
    protected Dictionary<int, Dictionary<int, AbstractGizmoPointer>> pointmap;


    /// <summary>
    /// 当前控制组
    /// </summary>
    protected int currentgroup = -1;

    /// <summary>
    /// 当前控制序号
    /// </summary>
    protected int currentindex = -1;


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
        pointmap = new Dictionary<int, Dictionary<int, AbstractGizmoPointer>>();
        foreach (KeyValuePair<int, Dictionary<int, Vector3>> item in pointsmap)
        {
            Dictionary<int, Vector3> templist = item.Value;
            Dictionary<int, AbstractGizmoPointer> tempdic = new Dictionary<int, AbstractGizmoPointer>();
            List<Transform> tempgos = new List<Transform>();
            foreach (KeyValuePair<int, Vector3> it in item.Value)
            {
                int group = item.Key;
                int index = it.Key;
                float s = 0.3f / 0.1f;
                Vector3 finalscaler = Vector3.one * s;
                GameObject g = SpawnGizmo(group, index, it.Value, transform);

                Destroy(g.GetComponent<Collider>());
                tempgos.Add(g.transform);
                tempdic.Add(index, g.GetComponent<AbstractGizmoPointer>());

                SaveGizmo();

            }
            pointmap.Add(item.Key, tempdic);
        }
    }

    /// <summary>
    /// 生成gizmo
    /// </summary>
    /// <param name="group">组</param>
    /// <param name="index">序号</param>
    /// <param name="pos">坐标</param>
    /// <param name="parent">父级</param>
    /// <returns></returns>
    protected abstract GameObject SpawnGizmo(int group, int index, Vector3 pos, Transform parent);

    /// <summary>
    /// 保存当前点
    /// </summary>
    protected virtual void SaveGizmo() { }


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
    protected AbstractGizmoPointer CheckChoise(int group, int index)
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
                Refreshdis(allactive: true);
                break;
            default:
                break;
        }
    }

    private void Refreshdis(int index = -1,bool allactive = false)
    {
        foreach (KeyValuePair<int, Dictionary<int, AbstractGizmoPointer>> item in pointmap)
        {
            foreach (KeyValuePair<int, AbstractGizmoPointer> it in item.Value)
            {
                if (!allactive)
                {
                    if (it.Value.IsMe(index, it.Key))
                    {
                        it.Value.Display();
                    }
                    else
                    {
                        it.Value.Hidden();
                    }
                }
                else {
                    it.Value.Display();
                }
            }
        }
    }
}
