using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalModelController : AbstractModelMain
{
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

    protected override void LoadModel(ModelMsg obj)
    {
        ReleaseOld();
        totle++;
        ModelMsg mm = obj;

        string stlpath = Tool.LocalNormalModelPath + mm.pdata.md5 + Tool.STLfiledir;// mm.pdata.Normalmodelpath;//

        GameObject realmodel = new GameObject("NormalModel" + totle, typeof(STL));

        realmodel.transform.SetParent(transform);

        STL stlloalder = realmodel.GetComponent<STL>();
        stlloalder.CreateInstance(stlpath, str => { }, go =>
        {
            AdjustmentChild(go, PlayerDataCenter.Instance.MaterialMap[Tool.MatarialsUse.NormalModel]);
            LoadGizmopointer(mm.pointmap, go.transform,SelfPointMode);
            go.transform.SetParent(realmodel.transform);
            go.transform.localScale = Vector3.one * Tool.UserImportScaler;
            selforiginscaler = transform.localScale.x;
        });


        Transform cameratarget = new GameObject("Camratarget").transform;
        cameratarget.SetParent(realmodel.transform);
        cameratarget.localPosition = Vector3.zero;

        QMsg msg = new CameraControllMsg()
        {
            EventID = (int)Camera_E.Begin,
            cameraevent = Camera_E.Target,
            camerarotatetarget = cameratarget
        };
       CameraManager.Instance.SendMsg(msg);


        lastrealmodel = realmodel;
    }

    protected override void ScalerCallback(ModelTranslate_E escaler, float value)
    {
        Vector3 scaler = transform.localScale;
        switch (escaler)
        {
            case ModelTranslate_E.Scaler:
                scaler = new Vector3(value, value, value) * selforiginscaler;
                break;
            case ModelTranslate_E.ScalerX:
                scaler.x = value * selforiginscaler;
                break;
            case ModelTranslate_E.ScalerY:
                scaler.y = value * selforiginscaler;
                break;
            case ModelTranslate_E.ScalerZ:
                scaler.z = value * selforiginscaler;
                break;
            default:
                break;
        }
        transform.localScale = scaler;

    }


    protected override void ResetPreferenceSetting()
    {
    }


}
