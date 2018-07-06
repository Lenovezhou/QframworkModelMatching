
using DG.Tweening;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraRotateSphere : MonoBehaviour {

    #region 声明变量
    //六视图时摄像机对应的位置
    Vector3 up, down, right, left, forword, back;
    private bool isdotweenlerp = false;
    private Vector3 center;
	private Vector3 defaultCenter;
	private Vector3 defaultPositon;
    float distance = 6;
    float x, y;

    public float xSpeed = 200;
    public float ySpeed = 200;

    float speed = 400;
    public float mSpeed = 10;
    public float minDistance = 1;
    public float maxDistance = 10;
    float damping = 5.0f;
    public float yMinLimit = -50;
    public float yMaxLimit = 50;
    private bool needDamping = false;

    private bool isMiddlebuttonTranslat = false;

    //target初始位置
    private Vector3 targetoriginpos;
    public Transform target;
    #endregion
    public void Init(Transform _target)
	{
        Camera c = GetComponent<Camera>();
        c.tag = "MainCamera";
        c.rect = new Rect(-0.13f, 0, 1, 1);
        c.depth = 100;



        yMinLimit = -90;
        yMaxLimit = 90;

        target = _target;
        targetoriginpos = _target.transform.position;
        center = target.position;

		 up = target.transform.up * distance + center;
		 down = -target.transform.up * distance + center;
		 right = target.transform.right * distance + center;
		 left = -target.transform.right * distance + center;
		 forword = target.transform.forward * distance + center;
		 back = -target.transform.forward * distance + center;
    }

    private void MoveTo(Vector3 vecCenter, Vector3 vecPosition, float time, bool lookattarget = true)
	{
        // 设置缺省中心点
        center = vecCenter;
        defaultCenter = vecCenter;
        defaultPositon = vecPosition;
        distance = Vector3.Distance(defaultCenter, defaultPositon);

        isdotweenlerp = true;
        // 移动位置（用了DOTween做平滑）
        Sequence seq = DOTween.Sequence();
        //seq.Append(this.transform.DOMove(vecPosition, time).SetEase(Ease.Linear));
        if (lookattarget)
        {
            seq.Append(this.transform.DOJump(vecPosition, 2, 1, time).SetEase(Ease.Linear));
            seq.Play().OnUpdate(delegate
            {
                this.transform.LookAt(vecCenter);
            });
        }
        else {
            seq.Append(this.transform.DOMove(vecPosition, time).SetEase(Ease.Linear));
        }
        seq.OnComplete(() => { isdotweenlerp = false; });

        distance = Vector3.Distance(vecCenter, vecPosition);
        Vector3 direction = (vecCenter - vecPosition) / distance;
        Quaternion q2 = Quaternion.FromToRotation(Vector3.forward, direction);
        x = q2.eulerAngles.y;
        y = q2.eulerAngles.x;
    }
	// Update is called once per frame
	void Update ()
    {
        //CheckMoustButtonInput();
        if (CanRightMouseControlRotate())
        {
            if (CanRightMouseWithoutUIRotate())
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);
            }


            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * disVector + target.position;
            //adjust the camera  
            if (needDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }

        if (ScrollWhellInput())
        {
            distance -= Input.GetAxis("Mouse ScrollWheel") * mSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * disVector + target.position;
            //adjust the camera  
            if (needDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    //六视图回调
    public void CallBack(QMsg obj)
    {
        CameraControllMsg cmcc = obj as CameraControllMsg;
        Camera_E ce = cmcc.cameraevent;
        Vector3 endvilue = back;
        switch (ce)
        {
            case Camera_E.Forword:
                endvilue = forword;
                break;
            case Camera_E.Left:
                endvilue = left;
                break;
            case Camera_E.Down:
                endvilue = down;
                break;
            case Camera_E.Right:
                endvilue = right;
                break;
            case Camera_E.Back:
                endvilue = back;
                break;
            case Camera_E.Up:
                endvilue = up;
                break;
            case Camera_E.ResetView:
                endvilue = back;
                Replay();
                break;
            case Camera_E.ChangeTargetPos:
                break;
            case Camera_E.Target:
                Init(cmcc.camerarotatetarget);
                endvilue = back;
                break;
            default:
                break;
        }

        MoveTo(center, endvilue, 1);
    }

    //鼠标回调
    public void MouseCallBack(int key,object obj)
    {
        //Mouse_E me = (Mouse_E)obj.EventID;
        Debug.Log("-->" + key + "   " + obj);
    }


    void CheckMoustButtonInput()
    {
        if (Input.GetMouseButton(2))
        {
            if (!isMiddlebuttonTranslat)
            {
                isMiddlebuttonTranslat = true;
            }
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isMiddlebuttonTranslat = false;
        }
    }

    bool CanRightMouseControlRotate()
    {
        return target && !isdotweenlerp && !isMiddlebuttonTranslat;
    }

    bool CanRightMouseWithoutUIRotate()
    {
        return Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject();
    }


    bool CanMiddleMouseButtonControll()
    {
        return Input.GetMouseButton(2) && !EventSystem.current.IsPointerOverGameObject();
    }


    bool ScrollWhellInput()
    {
        return Input.GetAxis("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject();
    }


    void ChangeTargetPosCall(string message)
    {
        //Vector3 v = MSGCenter.ParseToVector(message);
        //if (v == Vector3.zero)
        //{
        //    v = targetoriginpos;
        //}
        //target.transform.position = v;

        //Vector3 vv = new Vector3(center.x, v.y, center.z);
        //Vector3 cameraendpos = vv + (v - vv).normalized * 2;
        //MoveTo(center,cameraendpos,1);
    }
    void HighLightPoint(string message)
    {

    }
    public void Replay()
    {
        target.transform.position = targetoriginpos;
        MoveTo(center, forword, 1);
    }


}
