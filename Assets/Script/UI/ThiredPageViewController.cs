using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThiredPageViewController : MonoBehaviour {
    Button up, left, forward, right, back, down, resetview;
	void Start ()
    {
        up = transform.Find("UpButton").GetComponent<Button>();
        left = transform.Find("LeftButton").GetComponent<Button>();
        forward = transform.Find("ForwardButton").GetComponent<Button>();
        right = transform.Find("RightButton").GetComponent<Button>();
        back = transform.Find("BackButton").GetComponent<Button>();
        down = transform.Find("DownButton").GetComponent<Button>();

        resetview = transform.Find("ResetViewButton").GetComponent<Button>();

        up.onClick.AddListener(()=> { OnViewChange(Camera_E.Up); });
        left.onClick.AddListener(() => { OnViewChange(Camera_E.Left); });
        forward.onClick.AddListener(() => { OnViewChange(Camera_E.Forword); });
        right.onClick.AddListener(() => { OnViewChange(Camera_E.Right); });
        back.onClick.AddListener(() => { OnViewChange(Camera_E.Back); });
        down.onClick.AddListener(() => { OnViewChange(Camera_E.Down); });
        resetview.onClick.AddListener(() => { OnViewChange(Camera_E.ResetView); });


    }
    void OnViewChange(Camera_E _cameraevent)
    {
        QMsg msg = new CameraControllMsg()
        {
            EventID = (int)Camera_E.Begin,
            cameraevent = _cameraevent
        };

        CameraManager.Instance.SendMsg(msg);
    }


}
