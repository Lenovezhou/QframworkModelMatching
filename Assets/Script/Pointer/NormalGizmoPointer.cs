using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGizmoPointer : AbstractGizmoPointer
{
    protected NormalGizmoPointer(Tool.PointMode pm) : base(pm)
    {
        pointmode = Tool.PointMode.Normal;
    }
}
