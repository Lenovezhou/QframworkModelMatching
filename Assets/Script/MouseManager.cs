using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;


/// <summary>
/// 鼠标输入详情
/// </summary>
public enum Mouse_E
{
    Begin = QMgrID.Mouse,
    OnLeft,
    OnLeftDown,
    OnLeftUp,
    OnRight,
    OnRightDown,
    OnRightUp,
    OnMiddleDown,
    OnMiddleUp,
    OnMiddleScroll,
    Move
}

public class MouseMsg : QMsg
{
    public Mouse_E mouseaction;
    public float ScrollWheel;
}


public class MouseManager : QMgrBehaviour
{
    protected override void SetupMgrId()
    {
        mMgrId = QMgrID.Mouse;
    }


    private void Start()
    {
        var mousestatelit = new QFSMLite();
        QFSMState ido = new QFSMState(1);
        QFSMState leftdown = new QFSMState(2);
        QFSMState rightdown = new QFSMState(3);


        mousestatelit.AddState(ido.Name.ToString());
        mousestatelit.AddState(leftdown.Name.ToString());
        mousestatelit.AddState(rightdown.Name.ToString());
        mousestatelit.Start(ido.Name.ToString());
    }



    private void Update()
    {
        //鼠标事件外发
        if (Input.GetMouseButtonDown(0))
        {
        }
        if (Input.GetMouseButtonDown(1))
        {
        }
        if (Input.GetMouseButtonDown(2))
        {
        }
        if ( Input.GetAxis("Mouse ScrollWheel") != 0)
        {
        }

        if (Input.GetMouseButtonUp(0))
        {
        }
        if (Input.GetMouseButtonUp(1))
        {
        }
        if (Input.GetMouseButtonUp(2))
        {
        }

        //测试移动相机

        if (Input.GetKeyDown(KeyCode.U))
        {
            SendEvent(Camera_E.Up);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SendEvent(Camera_E.Down);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SendEvent(Camera_E.Left);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SendEvent(Camera_E.Right);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SendEvent(Camera_E.Forword);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SendEvent(Camera_E.Back);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SendEvent(Camera_E.ResetView);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SendEvent(Camera_E.ChangeTargetPos);
        }
    }
}
