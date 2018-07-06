using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class LenovoScenedMiddlePanelData : UIPanelData
	{
        // TODO: Query Mgr's Data
        public int Length;
	}

	public partial class LenovoScenedMiddlePanel : UIPanel
	{
		protected override void InitUI(IUIData uiData = null)
		{
			mData = uiData as LenovoScenedMiddlePanelData ?? mData;
			//please add init code here
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
			base.OnShow();
            ScrollView.GetComponentInChildren<Image>().color = "#00FFFFFF".HtmlStringToColor();
            ScrollView1.GetComponentInChildren<Image>().color = "#00FGFFFF".HtmlStringToColor();
            ScrollView2.GetComponentInChildren<Image>().color = "#00FFFGFF".HtmlStringToColor();
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
			Debug.Log("[ LenovoScenedMiddlePanel:]" + content);
		}

		LenovoScenedMiddlePanelData mData = null;
	}
}