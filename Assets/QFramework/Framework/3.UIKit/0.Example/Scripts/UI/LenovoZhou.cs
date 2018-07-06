using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class LenovoZhouData : UIPanelData
	{
        // TODO: Query Mgr's Data
        public int Length;
	}

	public partial class LenovoZhou : UIPanel
	{
		protected override void InitUI(IUIData uiData = null)
		{
            Log.I("InitUI");
            mData = uiData as LenovoZhouData ?? mData;
            
            //please add init code here
        }

		protected override void ProcessMsg (int eventId,QMsg msg)
		{
			throw new System.NotImplementedException ();
		}

		protected override void RegisterUIEvent()
		{
            Button1.onClick.AddListener(() =>
            {
            });
            Button2.onClick.AddListener(() => 
            {
                LenovoScenedMiddlePanelData middlepanelData = new LenovoScenedMiddlePanelData() { Length = mData.Length};
                UIMgr.OpenPanel<LenovoScenedMiddlePanel>(prefabName: "Resources/LenovoScenedMiddlePanel",uiData:middlepanelData);
                Hide();
            });
            Button3.onClick.AddListener(() => 
            {
                Hide();
            });
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
			Debug.Log("[ LenovoZhou:]" + content);
		}

		LenovoZhouData mData = null;
	}
}