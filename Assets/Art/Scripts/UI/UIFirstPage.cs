using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{


	public class UIFirstPageData : UIPanelData
	{
		// TODO: Query Mgr's Data
	}

	public partial class UIFirstPage : UIPanel
	{

        Dictionary<int, GameObject> allhereilldatas = new Dictionary<int, GameObject>();


        protected override void InitUI(IUIData uiData = null)
		{
			mData = uiData as UIFirstPageData ?? new UIFirstPageData();
			//please add init code here
		}

		protected override void ProcessMsg (int eventId,QMsg msg)
		{
			throw new System.NotImplementedException ();
		}

		protected override void RegisterUIEvent()
		{
            AddPlayerButton.onClick.AddListener(() => 
            {
                UIMgr.OpenPanel<UICreateIllnessPopup>(canvasLevel: UILevel.PopUI,prefabName: "Resources/UICreateIllnessPopup");
                CloseSelf();
            });
		}

		protected override void OnShow()
		{
            //Get请求服务器  获取数据刷新
            string url = Tool.illnessdatasimplepath;

            QMsg msg = new WebMsg
            {
                EventID = (int)Web_E.GET,
                url = Tool.illnessdatasimplepath,
                callback = (success, str) =>
                {
                    if (success)
                    {
                        //json解析
                        IllDatalistRoot cd = JsonHelper.ParseJsonToNeed<IllDatalistRoot>(str);
                        //根据还原的数据进行本地还原,刷新到UI上
                        PlayerDataCenter.Instance.RevertToNormal(cd,CallbackRefresh);
                    }
                    else
                    {
                        Log.E("-->>firstpage get erro-->>  " + str);
                    }
                }
            }; 

            SendMsg(msg);
        }


        /// <summary>
        /// 回调刷新
        /// </summary>
        /// <param name="illarray"></param>
        public void CallbackRefresh(List<PlayerData> illarray)
        {
            if (null == illarray)
            {
                return;
            }
            //根据id从大到小,冒泡排序
            int length = illarray.Count;
            PlayerData temp = null;
            for (int i = length; i > 0; i--)
            {
                for (int j = 0; j < i - 1; j++)
                {
                    if (illarray[j].ID < illarray[j + 1].ID)
                    {
                        temp = illarray[j];
                        illarray[j] = illarray[j + 1];
                        illarray[j + 1] = temp;
                    }
                }
            }


            for (int i = 0; i < illarray.Count; i++)
            {
                SpawnIllData(illarray[i]);
            }

        }

        /// <summary>
        /// 生成playerdata 物体
        /// </summary>
        /// <param name="da"></param>
        void SpawnIllData(PlayerData da)
        {
            PlayerData _d = da;
            GameObject go = null;
            if (!allhereilldatas.ContainsKey(_d.ID))
            {
                go = GameObject.Instantiate(Item.gameObject);
                allhereilldatas.Add(_d.ID, go);
            }
            go = allhereilldatas[_d.ID];
            go.GetComponent<PlayerItemButton>().Init(Item.gameObject, _d);
            /*========================以下功能将放在按钮中执行=======================
             * ======================================================================
             * ======================================================================
             * ①将所有信息都存储到本地（ID）.json （MD5，MatchingPoint,PlayerSetting）
             * ②成功后回调
             * ③根据Md5加载normalmodel和usermodel（也可能是下载得来）
             * ④成功回调设置摄像机旋转中心
             * ⑤打开第二界面
             * ⑥刷新UI列表
             * ⑦设置默认界面
             */
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
			Debug.Log("[ UIFirstPage:]" + content);
		}

		UIFirstPageData mData = null;
	}
}