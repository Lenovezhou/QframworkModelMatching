using QFramework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// point事件
/// </summary>
public enum Point_E
{
    Begin = QMgrID.Pointer,
    Normal,
    Userimport
}

public enum PointControll_E
{
    SaveMatchingpoint,
    Cancle,
    AutoNext,
    Load,
    ChoiseGroup,
    AllDisplay
}


/// <summary>
/// point事件参数
/// </summary>
public class PointMsg : QMsg
{
    public PointControll_E controllmode;
    public Transform parent;
    public Dictionary<int, Dictionary<int, Vector3>> pointsmap;
    public int group;
    public int index;
}




public class PointerManager : QMgrBehaviour, ISingleton
{
    [SerializeField]
    private AbstractPointscontroll userpcontroller;
    [SerializeField]
    private AbstractPointscontroll normalpcontroller;

    public void OnSingletonInit()
    {
        RegisterEvent(Point_E.Normal);
        RegisterEvent(Point_E.Userimport);

        GameObject useripoints = new GameObject("UserPointsParent", typeof(UserPointersController));
        GameObject normalpoints = new GameObject("NormalPointsParent", typeof(NormalpointersController));

        userpcontroller = useripoints.GetComponent<AbstractPointscontroll>();
        normalpcontroller = normalpoints.GetComponent<AbstractPointscontroll>();

    }

    protected override void ProcessMsg(int eventId, QMsg msg)
    {
        switch (eventId)
        {
            case (int)Point_E.Userimport:
                userpcontroller.HandleEvent(msg);
                break;
            case (int)Point_E.Normal:
                normalpcontroller.HandleEvent(msg);
                break;
            default:
                break;
        }
    }


    protected override void SetupMgrId()
    {
        mMgrId = QMgrID.Pointer;
    }



    private PointerManager() { }

    public static PointerManager Instance
    {
        get { return MonoSingletonProperty<PointerManager>.Instance; }
    }
}
