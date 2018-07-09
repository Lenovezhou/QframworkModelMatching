using QFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbsAvoidGizmo:MonoBehaviour
{
    protected Tool.PointMode selfpointmode;

    protected int index;


    protected Vector3 savedposition;

    protected float savedscaler;


    protected bool hassaved = false;

    public void Init(Tool.PointMode pm,int index)
    {
        this.selfpointmode = pm;
        this.index = index;
    }



    public abstract void HandleEvent(QMsg msg);


    protected virtual void ControllMesh(bool active)
    {
        GetComponentInChildren<MeshRenderer>().enabled = active;
        GetComponentInChildren<Collider>().enabled = active;
    }


    public virtual bool IsMe(int index)
    {
        return this.index == index;
    }


    protected virtual void ReleaseMe()
    {
        Destroy(gameObject);
        hassaved = false;
    }


    public virtual void OnEdit() { }

    public virtual void OnSave()
    {
        hassaved = true;
        savedposition = transform.localPosition;
        savedscaler = transform.localScale.x;
    }

    public virtual void OnRemove()
    {
        ReleaseMe();
    }

    public virtual void CheckMeNeedRelease()
    {
        if (!hassaved)
        {
            ReleaseMe();
        }
    }

}