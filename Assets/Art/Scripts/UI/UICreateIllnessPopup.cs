using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;

namespace QFramework.Example
{
	public class UICreateIllnessPopupData : UIPanelData
	{
        // TODO: Query Mgr's Data
        public PlayerData playerdata;
	}

    public partial class UICreateIllnessPopup : UIPanel
	{
		protected override void InitUI(IUIData uiData = null)
		{
			mData = uiData as UICreateIllnessPopupData ?? new UICreateIllnessPopupData();
            //please add init code here
            injury_positionDropdown.InitDropDown(Tool.InjuryPosition);
            protector_shapeDropdown.InitDropDown(Tool.protector_shape);
            directionDropdown.InitDropDown(Tool.directionill);
            NewDataRefresh(mData.playerdata);

            #region 初始化UI事件
            //取消
            transform.Find("Panel/CancleButton").GetComponent<Button>().onClick.AddListener(() => 
            {
                UIMgr.OpenPanel<UIFirstPage>(prefabName: "Resources/UIFirstPage");
                CloseSelf();
            });



            //部位
            injury_positionDropdown.onValueChanged.AddListener((index) =>
            {
                mData.playerdata.injury_position = Tool.InjuryPosition[index];
            });
            //外形
            protector_shapeDropdown.onValueChanged.AddListener((index) =>
            {
                mData.playerdata.protector_shape = (PlayerData.ProtectorShape)index;
            });
            //方向
            directionDropdown.onValueChanged.AddListener((index) =>
            {
                mData.playerdata.position = (PlayerData.Direction)index;
            });

            //病例名称
            TitleInputField.onEndEdit.AddListener((str) =>
            {
                mData.playerdata.title = str;
            });

            //病情描述
            descriptioninputfiled.onEndEdit.AddListener((str) =>
            {
                mData.playerdata.description = str;
            });

            //备注
            noteinputfiled.onEndEdit.AddListener((str) =>
            {
                mData.playerdata.note = str;
            });

            //选择模型
            ShowFileButton.onClick.AddListener(() => 
            {
                mData.playerdata.LocalUserModelPath = Tool.OpenFileDisplay();
                mData.playerdata.LocalUserModelPath = Tool.CopyFileAndRename(mData.playerdata.LocalUserModelPath, mData.playerdata.ID);
                mData.playerdata.md5 = Tool.ReadMD5(mData.playerdata.LocalUserModelPath);
            });


            #endregion

        }

        protected override void ProcessMsg (int eventId,QMsg msg)
		{
			throw new System.NotImplementedException ();
		}

		protected override void RegisterUIEvent()
		{

        }

        protected override void OnShow()
        {

        }


        protected void NewDataRefresh(PlayerData data)
		{
           PlayerData CurrentPlayerdata = data != null ? data : new PlayerData()
           {
                ID = -1,
                injury_position = "手",
                protector_shape = 0,
                position = 0,
                title = "",
                description = "",
                note = ""
            };
            //currentillnessID = CurrentPlayerdata.ID;
            injury_positionDropdown.value = Tool.InjuryPosition.IndexOf(CurrentPlayerdata.injury_position);
            protector_shapeDropdown.value = (int)CurrentPlayerdata.protector_shape;
            directionDropdown.value = (int)CurrentPlayerdata.position;
            TitleInputField.text = CurrentPlayerdata.title;
            descriptioninputfiled.text = CurrentPlayerdata.description;
            noteinputfiled.text = CurrentPlayerdata.note;


            //点击确定发送请求到服务器
            CommitButton.onClick.RemoveAllListeners();
            if (null == mData.playerdata)
            {
                CommitButton.onClick.AddListener(AddIllNess);
                mData.playerdata = CurrentPlayerdata;
            }
            else
            {
                CommitButton.onClick.AddListener(() =>
                {

                    string url = Tool.refreshillnessdatasimplepath + CurrentPlayerdata.ID.ToString();

                    string json = JsonHelper.MergPlayerdataJson(CurrentPlayerdata);

                    QMsg msg = new WebMsg
                    {
                        EventID = (int)Web_E.PUT,
                        url = url,
                        message = json,
                        callback = (success, str) =>
                        {
                            if (success)
                            {
                                UIMgr.OpenPanel<UICreateIllnessPopup>(canvasLevel: UILevel.PopUI, prefabName: "Resources/UIFirstPage");
                                CloseSelf();
                            }
                            else
                            {
                                Log.E(url + "  <----createIllnessPopUp-->> " + str +"   " + json);
                                //ShowPage<UINotice>(Tool.FaleToConnect);
                            }
                        }
                    };
                    SendMsg(msg);
                });
            }
        }

        /// <summary>
        /// 添加新用户数据
        /// </summary>
        void AddIllNess()
        {
            if (CheckDataAllright())
            {
                //TODO ----->>>>>>>>SendRequest To PHP
                string Url = Tool.addillnessdatasimplepath;
                string s = JsonHelper.MergPlayerdataJson(mData.playerdata);

                QMsg msg = new WebMsg
                {
                    EventID = (int)Web_E.POST,
                    url = Url,
                    message = s,
                    callback = (success, str) =>
                    {
                        if (success)
                        {
                            mData.playerdata.ID = JsonHelper.ParseNewIllDataID("data", str);
                            PlayerDataCenter.Instance.PlayerDatalist.Add(mData.playerdata);

                            //Tool.CopyFileAndRename(mData.playerdata.LocalUserModelPath, mData.playerdata.ID);
                            UIMgr.OpenPanel<UIFirstPage>(prefabName: "Resources/UIFirstPage");
                            CloseSelf();
                        }
                        else
                        {
                            //ShowPage<UINotice>(Tool.FaleToConnect);
                        }
                    }
                };
                SendMsg(msg);

            }
            else
            {
                string result = "请选择路径!!!";
                LoadModelResult(result);
            }

        }

        /// <summary>
        /// 检查必选框内容
        /// </summary>
        /// <returns></returns>
        bool CheckDataAllright()
        {
            if (string.IsNullOrEmpty(mData.playerdata.LocalUserModelPath) || string.IsNullOrEmpty(mData.playerdata.title))
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        protected override void OnHide()
		{
			base.OnHide();
		}

		protected override void OnClose()
		{
			base.OnClose();
		}

		void ShowLog(string content)
		{
			Debug.Log("[ UICreateIllnessPopup:]" + content);
		}

        /// <summary>
        /// 选择模型路径时文字提示
        /// </summary>
        /// <param name="result"></param>
        void LoadModelResult(string result)
        {
            string str = "请正确上传患者模型";
            if (result.Split('*').Length > 1)
            {
                str = result.Split('*')[1];
            }
            Sequence seq = DOTween.Sequence();
            seq.Append(WarmingText.DOColor(Color.red, 0.5f).SetEase(Ease.Linear).SetLoops(3));
            //seq.Append(this.transform.DOMove(vecPosition, time).SetEase(Ease.Linear));
            seq.OnComplete(delegate
            {
                WarmingText.color = Color.black;
            });
            WarmingText.text = str;
        }



        UICreateIllnessPopupData mData = null;
	}
}