using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserPointersController : AbstractPointscontroll
{
    public UserPointersController(Tool.PointMode mode) : base(mode)
    {
        selfpointmode = Tool.PointMode.UserImport;
    }

    public override void HandleEvent(QMsg msg)
    {
        PointMsg pm = msg as PointMsg;
        switch (pm.controllmode)
        {
            case PointControll_E.Ido:
            case PointControll_E.Edit:
            case PointControll_E.SaveMatchingpoint:
            case PointControll_E.Cancle:
                ControllPoint(msg);
                break;
            case PointControll_E.AutoNext:
                break;
            case PointControll_E.Load:
                LoadPointer(pm.parent,pm.pointsmap);
                break;
            case PointControll_E.ChoiseGroup:
            case PointControll_E.AllDisplay:
                RefreshDisplay(pm.controllmode,pm);
                break;
            case PointControll_E.SetCamra:
                this.selfcamera = pm.selfcamra;
                break;
            default:
                break;
        }

    }

    //点控制
    void ControllPoint(QMsg msg)
    {
        PointMsg pm = msg as PointMsg;
        PointControll_E pce = pm.controllmode;
        currentmode = pce;
        switch (pce)
        {
            case PointControll_E.Edit:
                CheckLastEdit(currentgroup,currentindex);
                this.currentgroup = pm.group;
                this.currentindex = pm.index;
                break;
            case PointControll_E.SaveMatchingpoint:
                SaveGizmo();
                break;
            case PointControll_E.Cancle:
                CancleGizmo();
                break;
            case PointControll_E.AutoNext:
                break;
            default:
                break;
        }
    }

    Ray ray;
    RaycastHit hit;
    void Update()
    {
        if (CanEditGizmo())
        {
            ray = selfcamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit, 100))
            {
                if (hit.collider.tag == Tool.userimporttag)
                {
                    SpawnGizmo(currentgroup,currentindex,hit.point,transform);
                }
            }
        }
    }


    bool CanEditGizmo()
    {
        return Input.GetMouseButtonDown(0) && currentmode == PointControll_E.Edit && !EventSystem.current.IsPointerOverGameObject();
    }


    /// <summary>
    /// 编辑生成调用
    /// </summary>
    /// <param name="group">组</param>
    /// <param name="index">序号</param>
    /// <param name="pos">坐标</param>
    /// <param name="parent">父物体</param>
    protected override GameObject SpawnGizmo(int group,int index,Vector3 pos,Transform parent)
    {
        AbstractGizmoPointer abspoint = CheckChoise(group, index);
        GameObject gizmo = null;
        UserGizmoPointer userpointer = null;

        if (abspoint)
        {
            gizmo = abspoint.gameObject;
            userpointer = abspoint as UserGizmoPointer;
        }
        else {
            gizmo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            userpointer = gizmo.AddComponent<UserGizmoPointer>();
            if (!pointmap.ContainsKey(group))
            {
                pointmap.Add(group, new Dictionary<int, AbstractGizmoPointer>());
            }
            if (!pointmap[group].ContainsKey(index))
            {
                pointmap[group].Add(index, userpointer);
            }
        }

        gizmo.transform.SetParent(parent);

        userpointer.Init(group, index, Vector3.one * 3);

        gizmo.transform.position = pos;

        return gizmo;
    }

    /// <summary>
    /// 保存调用
    /// </summary>
   protected override void SaveGizmo()
    {
        AbstractGizmoPointer abspoint = CheckChoise(currentgroup, currentindex);
        if (abspoint)
        {
            abspoint.Saveme();

        }
    }

    /// <summary>
    /// 取消调用
    /// </summary>
    void CancleGizmo()
    {
        AbstractGizmoPointer abspoint = CheckChoise(currentgroup, currentindex);
        if (abspoint)
        {
            abspoint.EditDone(ReleaseGizmo);
        }

    }

    /// <summary>
    /// 回调释放掉不保存的点
    /// </summary>
    /// <param name="group">组</param>
    /// <param name="index">序号</param>
    void ReleaseGizmo(int group, int index)
    {
        if (pointmap.ContainsKey(group) && pointmap[group].ContainsKey(index))
        {
            pointmap[group].Remove(index);
        }
    }


    /// <summary>
    /// 当下一编辑命令到来时检查上一个编辑的点状态
    /// </summary>
    void CheckLastEdit(int group,int index)
    {
        AbstractGizmoPointer abspoint = CheckChoise(group, index);
        if (abspoint)
        {
            abspoint.EditDone(ReleaseGizmo);
        }
    }

}
