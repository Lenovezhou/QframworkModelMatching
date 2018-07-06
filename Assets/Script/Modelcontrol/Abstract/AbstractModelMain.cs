using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractModelMain :MonoBehaviour
{
    protected int totle = -1;
    //多次导入新的模型，需控制最新的
    protected GameObject lastrealmodel;
    //摄像机
    protected Camera selfcamera;
    //能添加的点的类型
    protected Tool.PointMode SelfPointMode;
    //当前控制状态
    protected Model_E modelcontrollmode;
    //回调控制类型
    protected ModelTranslate_E tranlatmode;
    //初始缩放
    protected float selforiginscaler;

    /// <summary>
    /// 加载模型
    /// </summary>
    /// <param name="stlpath"></param>
    protected abstract void LoadModel(ModelMsg obj);

    /// <summary>
    /// 加载用户点
    /// </summary>
    protected virtual void LoadGizmopointer(Dictionary<int, Dictionary<int, Vector3>> _pointmap,Transform _parent,Tool.PointMode mode)
    {
        int eventid = -1;
        switch (mode)
        {
            case Tool.PointMode.Normal:
                eventid = (int)Point_E.Normal;
                break;
            case Tool.PointMode.UserImport:
                eventid = (int)Point_E.Userimport;
                break;
            default:
                break;
        }
        QMsg msg = new PointMsg() { EventID = eventid,controllmode = PointControll_E.Load, parent = _parent, pointsmap = _pointmap };
        PointerManager.Instance.SendMsg(msg);
    }

    /// <summary>
    /// 重置到上次设置的位置
    /// </summary>
    protected abstract void ResetPreferenceSetting();

    /// <summary>
    /// 隐藏
    /// </summary>
    protected virtual void Hidden() { lastrealmodel.SetActive(false); }

    /// <summary>
    /// 显示
    /// </summary>
    protected virtual void Display() { lastrealmodel.SetActive(true); }

    /// <summary>
    /// 删除旧模型
    /// </summary>
    protected virtual void ReleaseOld()
    {
        if (lastrealmodel)
        {
            lastrealmodel.SetActive(false);
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pm"></param>
    public void Init(Tool.PointMode pm,Camera _selfcamera =null)
    {
        this.SelfPointMode = pm;
    }


    /// <summary>
    /// 调整生成物体的子物体
    /// </summary>
    /// <param name="parent"></param>
    protected virtual void AdjustmentChild(GameObject go,Material mat)
    {
        MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = mat;
        }
    }

    /// <summary>
    /// 接收到通知
    /// </summary>
    /// <param name="msg">事件数据</param>
    public virtual void HandlerEvent(QMsg msg) { }

    /// <summary>
    /// 完成模型加载后通知pointermanager加载gizmopointer
    /// </summary>
    protected virtual void NotifyLoadPointer(Tool.PointMode mode,Dictionary<int,Dictionary<int,Vector3>> _pointsmap)
    {
    }


    #region 注册事件回调
    /// <summary>
    /// 记录上一帧的旋转值
    /// </summary>
    protected float lastvalue;

    /// <summary>
    /// 旋转控制回调
    /// </summary>
    /// <param name="_endvalue"></param>
    protected virtual void RotateCallback(float _endvalue) { }

    /// <summary>
    /// 移动控制回调
    /// </summary>
    /// <param name="_direction"></param>
    protected virtual void TranslateCallback(ModelTranslate_E _direction) { }

    /// <summary>
    /// 缩放控制回调
    /// </summary>
    /// <param name="escaler"></param>
    /// <param name="value"></param>
    protected virtual void ScalerCallback(ModelTranslate_E escaler, float value) { }

    /// <summary>
    /// 鼠标移动方法
    /// </summary>
    protected virtual void MoveUpdate() { }

    /// <summary>
    /// 鼠标旋转方法
    /// </summary>
    protected virtual void RotateUpdata() { }

    /// <summary>
    /// 编辑点
    /// </summary>
    protected virtual void EditPoint() { }

    /// <summary>
    /// 保存点
    /// </summary>
    protected virtual void SavePoint() { }

    /// <summary>
    /// 放弃编辑
    /// </summary>
    protected virtual void CancleEdit() { }


    #endregion


}
