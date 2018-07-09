using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class NormalpointersController : AbstractPointscontroll
{
    public NormalpointersController(Tool.PointMode mode) : base(mode)
    {
        selfpointmode = Tool.PointMode.Normal;
    }

    public override void HandleEvent(QMsg msg)
    {
        PointMsg pm = msg as PointMsg;

        switch (pm.controllmode)
        {
            case PointControll_E.Edit:
                break;
            case PointControll_E.SaveMatchingpoint:
                break;
            case PointControll_E.Cancle:
                break;
            case PointControll_E.AutoNext:
                break;
            case PointControll_E.Load:
                LoadPointer(pm.parent, pm.pointsmap);
                break;
            case PointControll_E.ChoiseGroup:
            case PointControll_E.AllDisplay:
                RefreshDisplay(pm.controllmode, pm);
                break;
            default:
                break;
        }
    }

    protected override GameObject SpawnGizmo(int group, int index, Vector3 pos, Transform parent)
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        g.transform.SetParent(transform, false);
        g.transform.localPosition = pos;
        float s = 0.3f / 0.1f;
        Vector3 finalscaler = Vector3.one * s;

        AbstractGizmoPointer p = null;
        p = g.AddComponent<NormalGizmoPointer>();

        p.Init(group, index, finalscaler);
        Destroy(g.GetComponent<Collider>());

        return g;
    }
}
