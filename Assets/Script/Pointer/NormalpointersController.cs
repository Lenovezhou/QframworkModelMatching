﻿using System.Collections;
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
                break;
            default:
                break;
        }
    }

}
