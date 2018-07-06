using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 相机控制详情
/// </summary>
public enum Camera_E {
    Begin = QMgrID.Camera,
    Target,
    Move,
    Forword,
    Left,
    Down,
    Right,
    Back,
    Up,
    ResetView,
    ChangeTargetPos
}
public class CameraControllMsg : QMsg
{
    public Camera_E cameraevent;
    public Transform camerarotatetarget;
}


public class CameraManager : QMgrBehaviour,ISingleton
{
    CameraRotateSphere camerarotatesphere;
    public void InitCameraManager()
    {
        GameObject camera = new GameObject("GameCamera", typeof(Camera));
        camerarotatesphere = camera.AddComponent<CameraRotateSphere>();
        camera.transform.SetParent(transform);

        QMsg Normalmsg = new ModelMsg() { EventID = (int)Model_E.Normal,modelevent = Model_E.SetCamera, selfcamera = camera.GetComponent<Camera>() };
        SendMsg(Normalmsg);
        QMsg Usermsg = new ModelMsg() { EventID = (int)Model_E.UserImport, modelevent = Model_E.SetCamera,selfcamera = camera.GetComponent<Camera>() };
        SendMsg(Usermsg);
        RegetsSelfEvent();
    }

    public void RegetsSelfEvent()
    {
        RegisterEvent(Camera_E.Begin);

        QEventSystem.RegisterEvent(Tool.GameEvent.E_Mouse, camerarotatesphere.MouseCallBack);

    }


    protected override void ProcessMsg(int eventId, QMsg msg)
    {
        switch (eventId)
        {
            case (int)Camera_E.Begin:
                camerarotatesphere.CallBack(msg);
                break;
            default:
                break;
        }
    }

    protected override void SetupMgrId()
    {
        mMgrId = QMgrID.Camera;
    }

    public void OnSingletonInit()
    {
        InitCameraManager();
    }
    #region 单例实现
    private CameraManager() { }

    public static CameraManager Instance
    {
        get { return MonoSingletonProperty<CameraManager>.Instance; }
    }

    #endregion

}
