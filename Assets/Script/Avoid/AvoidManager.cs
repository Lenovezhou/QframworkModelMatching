using QFramework;
using UnityEngine;

public enum Avoid_E
{
    Begin = QMgrID.Avoid,
}

public enum AvoidControll_E
{
    Ido,
    Edit,
    Save,
    Cancle,
    Remove,
    HiddenSingle,
    HiddenAll,
    DisplaySingle,
    DisplayAll
}


public class AvoidMsg : QMsg
{
    public AvoidControll_E controllmode;
    public int index;
}


public class AvoidManager : QMgrBehaviour, ISingleton
{

    UserAvoidController useravoidcontroller;

    public void OnSingletonInit()
    {
        RegisterEvent(Avoid_E.Begin);

        GameObject useravoid = new GameObject("useravoidparent", typeof(UserAvoidController));
        useravoid.transform.SetParent(transform);
        useravoidcontroller = useravoid.GetComponent<UserAvoidController>();
        useravoidcontroller.Init(Tool.PointMode.UserImport);
    }



    protected override void SetupMgrId()
    {
        mMgrId = QMgrID.Avoid;
    }

    protected override void ProcessMsg(int eventId, QMsg msg)
    {
        switch (eventId)
        {
            case (int)Avoid_E.Begin:
                useravoidcontroller.HandleEvent(msg);
                break;
            default:
                break;
        }
    }

    private AvoidManager() { }

    public static AvoidManager Instance
    {
        get { return MonoSingletonProperty<AvoidManager>.Instance; }
    }
}
