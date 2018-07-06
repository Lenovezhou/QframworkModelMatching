using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{



	public class UIThirdPageData : UIPanelData
	{
        public PlayerData pdata;

		// TODO: Query Mgr's Data
        
        //进入该页面将要默认刷新到哪个界面
        

	}

	public partial class UIThirdPage : UIPanel
	{
        private Dictionary<Button, GameObject> guidemap = new Dictionary<Button, GameObject>();
        protected Button lastkey;

        protected override void InitUI(IUIData uiData = null)
		{
			mData = uiData as UIThirdPageData ?? new UIThirdPageData();
            InfoPanel.GetComponent<InfoPanelControl>().Init(mData.pdata);
            FillGuideMap();
            ChoisePanel(UserGuide);
        }

        void FillGuideMap()
        {
            if (guidemap.Count == 0)
            {
                guidemap.Add(MatchingpointButton, MatchingpointPanel);
                //guidemap.Add(MainviewmenuToggle, MainviewmenuPanel);
                guidemap.Add(AvoidvacancyButton, AvoidvacancyPanel);
                guidemap.Add(DecorativepatternButton, DecorativepatternPanel.gameObject);
                guidemap.Add(BandageButton, BandagePanel.gameObject);
                guidemap.Add(OtherButton, OtherPanel.gameObject);
                //guidemap.Add(MainmenuButton, MainMenuPanel);
                guidemap.Add(ResetRotationBut, ResetNotice);
                guidemap.Add(UserGuide, UserGuidePanel);

            }
        }

		protected override void ProcessMsg (int eventId,QMsg msg)
		{
		}

		protected override void RegisterUIEvent()
		{
            //三UI显示面板
            MainmenuButton.onClick.AddListener(() =>
            {
                //ChoisePanel(showmainUIbutton);
                //ExportDropdown.Show();
                MainMenuPanel.gameObject.SetActive(!MainMenuPanel.gameObject.activeSelf);
            });
            
            //重设角度
            ResetRotationBut.onClick.AddListener(() =>
            {
                ChoisePanel(ResetRotationBut);
            });
            //匹配点
            MatchingpointButton.onClick.AddListener(() =>
            {
                ChoisePanel(MatchingpointButton);
            });

            //避空位
            AvoidvacancyButton.onClick.AddListener(() =>
            {
                ChoisePanel(AvoidvacancyButton);
            });

            //花纹
            DecorativepatternButton.onClick.AddListener(() =>
            {
                ChoisePanel(DecorativepatternButton);
            });


            //扎带
            BandageButton.onClick.AddListener(() =>
            {
                ChoisePanel(BandageButton);
            });

            //其它
            OtherButton.onClick.AddListener(() =>
            {
                ChoisePanel(OtherButton);
            });
            //使用说明
            UserGuide.onClick.AddListener(() =>
            {
                ChoisePanel(UserGuide);
            });

            //提交
            CommitButton.onClick.AddListener(() =>
            {
                //①序列化当前数据
                //PointHelper.GetInstance().SaveAll();

                //②将现在的所有数据提交到web


            });
        }



        protected virtual void ChoisePanel(Button bu)
        {
            if (bu == lastkey)
            {
                return;
            }
            if (null != lastkey)
            {
                if (guidemap.ContainsKey(lastkey))
                {
                    lastkey.GetComponent<Image>().color = Color.white;
                    IPanelItem ipi = guidemap[lastkey].gameObject.GetComponent<IPanelItem>();
                    if (null != ipi)
                    {
                        ipi.OnLeveThisPage();
                    }
                    guidemap[lastkey].gameObject.SetActive(false);
                }
            }
            if (guidemap.ContainsKey(bu))
            {
                bu.GetComponent<Image>().color = Color.green;
                guidemap[bu].gameObject.SetActive(true);
                titletext.text = bu.GetComponentInChildren<Text>().text;
                IPanelItem ipi = guidemap[bu].gameObject.GetComponent<IPanelItem>();
                if (null != ipi)
                {
                    ipi.OnEnterThisPage();
                }

            }
            lastkey = bu;
        }




        protected override void OnShow()
		{
			base.OnShow();
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
			Debug.Log("[ UIThirdPage:]" + content);
		}

		UIThirdPageData mData = null;
	}
}