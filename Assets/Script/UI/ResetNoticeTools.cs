using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetNoticeTools : AbstractButtonOpenPanel,IPanelItem
{
    Button firstchoise;
    void Start ()
    {
       Button Rotate = transform.Find("RotateButton").GetComponent<Button>();
       Button translate = transform.Find("TranslateButton").GetComponent<Button>();
       Button scaler = transform.Find("ScalerButton").GetComponent<Button>();

        Rotate.onClick.AddListener(() => 
        {
            ChoisePanel(Rotate);
            QMsg modelmsg = new ModelMsg() { EventID = (int)Model_E.UserImport ,modelevent = Model_E.Rotate};
            ModelManager.Instance.SendMsg(modelmsg);
        });
        translate.onClick.AddListener(() =>
        {
            QMsg modelmsg = new ModelMsg() { EventID = (int)Model_E.UserImport, modelevent = Model_E.translate };
            ModelManager.Instance.SendMsg(modelmsg);
            ChoisePanel(translate);
        });
        scaler.onClick.AddListener(() => 
        {
            QMsg Usermodelmsg = new ModelMsg() { EventID = (int)Model_E.UserImport, modelevent = Model_E.Scaler};
            ModelManager.Instance.SendMsg(Usermodelmsg);
            QMsg Normalmodelmsg = new ModelMsg() { EventID = (int)Model_E.Normal, modelevent = Model_E.Scaler };
            ModelManager.Instance.SendMsg(Normalmodelmsg);
            ChoisePanel(scaler);
        });

        Transform RotatePanel = transform.Find("RotatePanel");
        Transform TranslatePanel = transform.Find("TranslatePanel");
        Transform ScalerPanel = transform.Find("ScalerPanel");


        RegestToMap(Rotate,RotatePanel.gameObject);
        RegestToMap(translate, TranslatePanel.gameObject);
        RegestToMap(scaler, ScalerPanel.gameObject);

        Slider S_rotate = RotatePanel.GetComponentInChildren<Slider>();
        InputField I_rotate = RotatePanel.GetComponentInChildren<InputField>();
        Slider S_XYZscaler = ScalerPanel.transform.Find("XYZPanel").GetComponentInChildren<Slider>();
        InputField I_XYZscaler = ScalerPanel.transform.Find("XYZPanel").GetComponentInChildren<InputField>();

        Slider S_Xscaler = ScalerPanel.transform.Find("XPanel").GetComponentInChildren<Slider>();
        InputField I_Xscaler = ScalerPanel.transform.Find("XPanel").GetComponentInChildren<InputField>();

        Slider S_Yscaler = ScalerPanel.transform.Find("YPanel").GetComponentInChildren<Slider>();
        InputField I_Yscaler = ScalerPanel.transform.Find("YPanel").GetComponentInChildren<InputField>();

        Slider S_Zscaler = ScalerPanel.transform.Find("ZPanel").GetComponentInChildren<Slider>();
        InputField I_Zscaler = ScalerPanel.transform.Find("ZPanel").GetComponentInChildren<InputField>();


        Button B_up = TranslatePanel.Find("UpButton").GetComponent<Button>();
        Button B_down = TranslatePanel.Find("DownButton").GetComponent<Button>();
        Button B_mid = TranslatePanel.Find("MidButton").GetComponent<Button>();
        Button B_right = TranslatePanel.Find("RightButton").GetComponent<Button>();
        Button B_left = TranslatePanel.Find("LeftButton").GetComponent<Button>();

        S_rotate.onValueChanged.AddListener((endvalue)=> 
        {
            OnRotateSliderChange(endvalue);
            I_rotate.text = endvalue.ToString();
        });
        I_rotate.onValueChanged.AddListener((str)=> 
        {
            try
            {
                float a = float.Parse(str);
                S_rotate.value =a;
            }
            catch (System.Exception)
            {
                I_rotate.text = S_rotate.value.ToString();
                throw;
            }
        });


        S_XYZscaler.onValueChanged.AddListener((endvalue) =>
        {
            OnScalerSliderChange(ModelTranslate_E.Scaler, endvalue);
            I_XYZscaler.text = endvalue.ToString();
        });
        I_XYZscaler.onValueChanged.AddListener((str) =>
        {
            try
            {
                float a = float.Parse(str);
                S_XYZscaler.value = a;
            }
            catch (System.Exception)
            {
                I_XYZscaler.text = S_XYZscaler.value.ToString();
                throw;
            }
        });

        S_Xscaler.onValueChanged.AddListener((endvalue)=> 
        {
            OnScalerSliderChange(ModelTranslate_E.ScalerX,endvalue);
            I_Xscaler.text = endvalue.ToString();
        });
        I_Xscaler.onValueChanged.AddListener((str) => 
        {
            try
            {
                float a = float.Parse(str);
                S_Xscaler.value = a;
            }
            catch (System.Exception)
            {
                I_Xscaler.text = S_Xscaler.value.ToString();
                throw;
            }
        });

        S_Yscaler.onValueChanged.AddListener((endvalue) =>
        {
            OnScalerSliderChange(ModelTranslate_E.ScalerY,endvalue);
            I_Yscaler.text = endvalue.ToString();
        });
        I_Yscaler.onValueChanged.AddListener((str) =>
        {
            try
            {
                float a = float.Parse(str);
                S_Yscaler.value = a;
            }
            catch (System.Exception)
            {
                I_Yscaler.text = S_Yscaler.value.ToString();
                throw;
            }
        });

        S_Zscaler.onValueChanged.AddListener((endvalue) =>
        {
            OnScalerSliderChange(ModelTranslate_E.ScalerZ, endvalue);
            I_Zscaler.text = endvalue.ToString();
        });
        I_Zscaler.onValueChanged.AddListener((str) =>
        {
            try
            {
                float a = float.Parse(str);
                S_Zscaler.value = a;
            }
            catch (System.Exception)
            {
                I_Zscaler.text = S_Zscaler.value.ToString();
                throw;
            }
        });


        B_up.onClick.AddListener(() => { OnTranslateChange(ModelTranslate_E.Up); });
        B_down.onClick.AddListener(() => { OnTranslateChange(ModelTranslate_E.Down); });
        B_right.onClick.AddListener(() => { OnTranslateChange(ModelTranslate_E.Right); });
        B_left.onClick.AddListener(() => { OnTranslateChange(ModelTranslate_E.Left); });

        firstchoise = Rotate;
        ChoisePanel(firstchoise);
    }

    ///旋转
    void OnRotateSliderChange(float endvalue)
    {
        QMsg modelmsg = new ModelMsg() { EventID = (int)Model_E.UserImport, translateevent = ModelTranslate_E.Rotate, Changdir = endvalue };
        ModelManager.Instance.SendMsg(modelmsg);
    }

    ///XYZ及等比缩放
    void OnScalerSliderChange(ModelTranslate_E mte,float endvalue)
    {
        QMsg modelmsg = new ModelMsg() { EventID = (int)Model_E.Normal, translateevent =mte, Changdir = endvalue };
        ModelManager.Instance.SendMsg(modelmsg);
    }

    /// 平移
    void OnTranslateChange(ModelTranslate_E mte)
    {
        QMsg modelmsg = new ModelMsg() { EventID = (int)Model_E.UserImport, translateevent = mte };
        ModelManager.Instance.SendMsg(modelmsg);
    }

    public void OnEnterThisPage()
    {
        ChoisePanel(firstchoise);
        QMsg modelmsg = new ModelMsg() { EventID = (int)Model_E.UserImport, modelevent = Model_E.Rotate };
        ModelManager.Instance.SendMsg(modelmsg);
    }

    public void OnLeveThisPage()
    {
        QMsg modelmsg = new ModelMsg() { EventID = (int)Model_E.UserImport, modelevent = Model_E.Ido };
        ModelManager.Instance.SendMsg(modelmsg);
    }

    QMsg CreatMsg(Model_E eventid,Model_E modleevent= Model_E.Begin,ModelTranslate_E modeltranslate = ModelTranslate_E.Begin,float valuedir = 0)
    {
        QMsg modelmsg = new ModelMsg() { EventID = (int)eventid, modelevent = modleevent,translateevent = modeltranslate,Changdir = valuedir};
        return modelmsg;
    }
}
