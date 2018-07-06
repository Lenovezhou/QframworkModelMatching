using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserImportController : AbstractModelMain
{

    private float mouseoffsetx = 0;
    private float mouseoffsety = 0;


    /// <summary>
    /// 根据发送来的消息回调刷新
    /// </summary>
    /// <param name="obj"></param>
    public override void HandlerEvent(QMsg obj)
    {
        ModelMsg mm = obj as ModelMsg;
        //主模型过程控制
        Model_E modelx = mm.modelevent;
        //modletranslate方式控制
        ModelTranslate_E mte = mm.translateevent;
        //传来的数据
        float value = mm.Changdir;
        //模型状态使用
        switch (modelx)
        {
            case Model_E.LoadModel:
                LoadModel(mm);
                break;
            case Model_E.Hidden:
                Hidden();
                break;
            case Model_E.Display:
                Display();
                break;
            case Model_E.SetCamera:
                selfcamera = mm.selfcamera;
                break;
            case Model_E.translate:
            case Model_E.Rotate:
            case Model_E.Scaler:
                modelcontrollmode = modelx;
                break;
            default:
                break;
        }

        //调整(位移旋转缩放)使用
        tranlatmode = mte;
        switch (mte)
        {
            case ModelTranslate_E.Up:
            case ModelTranslate_E.Down:
            case ModelTranslate_E.Left:
            case ModelTranslate_E.Right:
                TranslateCallback(mte);
                break;
            case ModelTranslate_E.Scaler:
            case ModelTranslate_E.ScalerX:
            case ModelTranslate_E.ScalerY:
            case ModelTranslate_E.ScalerZ:
                ScalerCallback(mte, value);
                break;
            case ModelTranslate_E.Rotate:
                RotateCallback(value);
                break;
            default:
                break;
        }

        //pointer使用


    }


    private void Update()
    {
        switch (modelcontrollmode)
        {
            case Model_E.translate:
                MoveUpdate();
                break;
            case Model_E.Rotate:
                RotateUpdata();
                break;
            case Model_E.Scaler:
                break;
            case Model_E.Ido:
                break;
            default:
                break;
        }
    }

    protected override void LoadModel(ModelMsg obj)
    {
        ReleaseOld();
        totle++;
        ModelMsg mm = obj;

        string stlpath =Tool.LocalModelonSavePath + mm.pdata.ID + Tool.STLfiledir;// mm.pdata.LocalUserModelPath;//
        if (!Tool.CheckFileExist(stlpath))
        {
            Debug.LogError("根据ID查找模型 不存在，加载默认模型");
        }
        GameObject realmodel = new GameObject("UserImprot" + totle, typeof(STL));

        realmodel.transform.SetParent(transform);

        STL stlloalder = realmodel.GetComponent<STL>();
        stlloalder.CreateInstance(stlpath, str => { }, go =>
        {
            AdjustmentChild(go, PlayerDataCenter.Instance.MaterialMap[Tool.MatarialsUse.UserimportModel]);
            LoadGizmopointer(mm.pointmap, go.transform, SelfPointMode);
            go.transform.SetParent(realmodel.transform);
            go.transform.localScale = Vector3.one * Tool.UserImportScaler;
        });
        lastrealmodel = realmodel;
    }


    protected override void RotateCallback(float _endvalue)
    {
        float end = _endvalue;
        float rotatedir = end - lastvalue;
        transform.Rotate(selfcamera.transform.forward, rotatedir);
        lastvalue = end;
        Vector3 euler = transform.localEulerAngles;
        //PlayerDataCenter.UpdataUserData(PlayerDataCenter.LoclaUserData.DataKey.Eulerangel, euler);
    }

    protected override void TranslateCallback(ModelTranslate_E _direction)
    {
        Vector3 dir = Vector3.zero;
        switch (_direction)
        {
            case ModelTranslate_E.Up:
                dir = selfcamera.transform.up;
                break;
            case ModelTranslate_E.Down:
                dir = -selfcamera.transform.up;
                break;
            case ModelTranslate_E.Left:
                dir = -selfcamera.transform.right;
                break;
            case ModelTranslate_E.Right:
                dir = selfcamera.transform.right;
                break;
            default:
                break;
        }

        transform.localPosition += dir * 0.02f;
        Vector3 v = transform.localPosition;
        //PlayerDataCenter.UpdataUserData(PlayerDataCenter.LoclaUserData.DataKey.Postion, v);
    }

    protected override void ResetPreferenceSetting()
    {

    }


   protected override void MoveUpdate()
    {
        if (CanLeftMouseRotate())
        {
            mouseoffsetx = Input.GetAxis("Mouse X") * 0.01f;
            mouseoffsety = Input.GetAxis("Mouse Y") * 0.01f;
            Vector3 offset = mouseoffsetx * selfcamera.transform.right + mouseoffsety * selfcamera.transform.up;
            transform.localPosition += offset;
            Vector3 v = transform.localPosition;
            //PlayerDataCenter.UpdataUserData(PlayerDataCenter.LoclaUserData.DataKey.Postion, v);
        }
    }
   protected override void RotateUpdata()
    {
        if (CanLeftMouseRotate())
        {
            mouseoffsetx = Input.GetAxis("Mouse X") * 4f;
            mouseoffsety = Input.GetAxis("Mouse Y") * 0.04f;

            transform.Rotate(Vector3.Cross(selfcamera.transform.right, selfcamera.transform.up), -mouseoffsetx, Space.World);
            Vector3 euler = transform.localEulerAngles;
            //PlayerDataCenter.UpdataUserData(PlayerDataCenter.LoclaUserData.DataKey.Eulerangel, euler);

        }
    }
   bool CanLeftMouseRotate()
    {
        return Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject();
    }

}
