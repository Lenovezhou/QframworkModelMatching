using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class AbstractGizmoPointer : MonoBehaviour
{

    [SerializeField]
    /// <summary>
    /// 点类型
    /// </summary>
    protected Tool.PointMode pointmode;

    [SerializeField]
    /// <summary>
    /// 所在组
    /// </summary>
    protected int group;

    [SerializeField]
    /// <summary>
    /// 所在组的第几个点
    /// </summary>
    protected int index;

    /// <summary>
    /// 正常时使用的材质
    /// </summary>
    protected Material originmatrial;

    /// <summary>
    /// 高亮时使用的材质
    /// </summary>
    protected Material highlightmatrial;

    /// <summary>
    /// 自身meshrenderer
    /// </summary>
    protected MeshRenderer selfmeshrender;


    /// <summary>
    /// 初始化gizmopointer
    /// </summary>
    /// <param name="dic">材质列表</param>
    /// <param name="group">组</param>
    /// <param name="index">序号</param>
    /// <param name="pm">点类型</param>
    /// <param name="localsclaer">本地缩放</param>
    public void Init(int group, int index, Vector3 localsclaer)
    {
        Dictionary<Tool.MatarialsUse, Material> dic = PlayerDataCenter.Instance.MaterialMap;
        this.group = group;
        this.index = index;
        //this.pointmode = pm;

        switch (pointmode)
        {
            case Tool.PointMode.Normal:
                originmatrial = dic[Tool.MatarialsUse.NormalPoint_origin];
                highlightmatrial = dic[Tool.MatarialsUse.NormalPoint_hight];
                break;
            case Tool.PointMode.UserImport:
                originmatrial = dic[Tool.MatarialsUse.UserImportpoint_origin];
                highlightmatrial = dic[Tool.MatarialsUse.UserImportpoint_hight];
                break;
            default:
                break;
        }
        transform.localScale = localsclaer;

        selfmeshrender = gameObject.GetComponent<MeshRenderer>();
        selfmeshrender.material = originmatrial;
    }

    /// <summary>
    /// 恢复材质到正常状态
    /// </summary>
    public void SetMaterialOrigin()
    {
        selfmeshrender.material = originmatrial;
    }

    /// <summary>
    /// 高亮材质
    /// </summary>
    public void SetMaterialHighlight()
    {
        //MSGCenter.Execute(Enums.ViewMode.ChangeTargetPos.ToString(), transform.position.ToString());
        selfmeshrender.material = highlightmatrial;
    }

    /// <summary>
    /// 自己被按下
    /// </summary>
    public void OnMouseDown()
    {
        SetMaterialHighlight();
        //string type = Enums.MatchigPointGizmoControll.PointerDown.ToString();
        //string message = MSGCenter.FormatMessage(group.ToString(), index.ToString(), transform.position.ToString());
        //MSGCenter.Execute(type, message);
    }


    public bool IsMe(int group,int index)
    {
        return this.group == group && this.index == index;
    }


    public virtual void ReleseMe()
    {
        Destroy(gameObject);
    }

    public virtual void Hidden() { gameObject.SetActive(false); }

    public virtual void Display() { gameObject.SetActive(true); }

    protected AbstractGizmoPointer(Tool.PointMode pm) { this.pointmode = pm; }

}
