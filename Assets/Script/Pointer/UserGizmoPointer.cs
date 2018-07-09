using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGizmoPointer : AbstractGizmoPointer
{
    protected UserGizmoPointer(Tool.PointMode pm) : base(pm)
    {
        pointmode = Tool.PointMode.UserImport;
    }



    public override void Saveme()
    {
        hassaved = true;
        savedposition = transform.localPosition;
        PlayerDataCenter.Instance.FillCurrentuserpointmap(group, index, savedposition);
    }


    /// <summary>
    /// 编辑后，开始编辑其他点，检查自身是否被保存，恢复状态
    /// </summary>
    public override void EditDone(Action<int,int> callback)
    {
        if (hassaved)
        {
            transform.localPosition = savedposition;
        }
        else
        {
            callback(this.group,this.index);
            ReleseMe();
        }
    }


}
