using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

/// <summary>
/// 标准件模型及用户导入模型控制
/// </summary>
public enum Model_E
{
    Begin = QMgrID.Model,
    Normal,
    UserImport,
    LoadModel,
    Hidden,
    SetCamera,
    Display,
    translate,
    Rotate,
    Scaler,
    Ido
}

/// <summary>
/// 标准件及用户导入模型Transfrom控制
/// </summary>
public enum ModelTranslate_E
{
    Begin = Model_E.Scaler,
    Up,
    Down,
    Left,
    Right,
    Scaler,
    ScalerX,
    ScalerY,
    ScalerZ,
    Rotate,
    Last
}


/// <summary>
/// 模型控制事件参数
/// </summary>
public class ModelMsg : QMsg
{
    public PlayerData pdata;
    public Dictionary<int, Dictionary<int, Vector3>> pointmap;
    public Model_E modelevent;
    public ModelTranslate_E translateevent;
    /// <summary>
    /// 移动旋转指令打开或关闭
    /// </summary>
    public bool active;
    /// <summary>
    /// 控制模型时的该变量
    /// </summary>
    public float Changdir;
    /// <summary>
    /// 设置观看摄像机
    /// </summary>
    public Camera selfcamera;
}


public class ModelManager : QMgrBehaviour, ISingleton
{
    //用户模型控制
    private AbstractModelMain userimportcontroller;
    //标准模型控制
    private AbstractModelMain normalmodelcontroller;

    public void OnSingletonInit()
    {
        RegisterEvent(Model_E.UserImport);
        RegisterEvent(Model_E.Normal);

        GameObject userimprot = new GameObject("UserImport",typeof(UserImportController));
        GameObject normal = new GameObject("Normal",typeof(NormalModelController));

        userimprot.transform.SetParent(transform);
        normal.transform.SetParent(transform);

        userimportcontroller = userimprot.GetComponent<AbstractModelMain>();
        normalmodelcontroller = normal.GetComponent<AbstractModelMain>();

        userimportcontroller.Init(Tool.PointMode.UserImport);
        normalmodelcontroller.Init(Tool.PointMode.Normal);
    }


    protected override void ProcessMsg(int eventId, QMsg msg)
    {
        ModelMsg mm = msg as ModelMsg;
        switch (eventId)
        {
            case (int)Model_E.Normal:
                normalmodelcontroller.HandlerEvent(mm);
                break;
            case (int)Model_E.UserImport:
                userimportcontroller.HandlerEvent(mm);
                break;
            default:
                break;
        }
    }


    protected override void SetupMgrId()
    {
        mMgrId = QMgrID.Model;
    }



    private ModelManager() { }

    public static ModelManager Instance
    {
        get { return MonoSingletonProperty<ModelManager>.Instance; }
    }

}
