using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGizmoPointer : AbstractGizmoPointer
{
    protected UserGizmoPointer(Tool.PointMode pm) : base(pm)
    {
        pointmode = Tool.PointMode.UserImport;
    }
}
