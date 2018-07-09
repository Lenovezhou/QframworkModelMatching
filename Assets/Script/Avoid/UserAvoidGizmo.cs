using QFramework;
using UnityEngine;

public class UserAvoidGizmo : AbsAvoidGizmo
{
    public override void HandleEvent(QMsg msg)
    {
        AvoidMsg am = msg as AvoidMsg;

        AvoidControll_E ace = am.controllmode;

        switch (ace)
        {
            case AvoidControll_E.Edit:
                break;
            case AvoidControll_E.Save:
                break;
            case AvoidControll_E.Remove:
                break;
            case AvoidControll_E.HiddenSingle:
            case AvoidControll_E.DisplaySingle:
            case AvoidControll_E.HiddenAll:
            case AvoidControll_E.DisplayAll:
                DisplayControll(ace);
                break;
            default:
                break;
        }
    }


    void DisplayControll(AvoidControll_E ace)
    {
        switch (ace)
        {
            case AvoidControll_E.HiddenSingle:
            case AvoidControll_E.HiddenAll:
                ControllMesh(false);
                break;
            case AvoidControll_E.DisplaySingle:
            case AvoidControll_E.DisplayAll:
                ControllMesh(true);
                break;
            default:
                break;
        }
    }



}