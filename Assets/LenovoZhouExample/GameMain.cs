using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.Example;

public class GameMain : MonoBehaviour
{
	void Start ()
    {
        // ① 打开第一面板
        UIMgr.OpenPanel<UIFirstPage>(prefabName: "Resources/UIFirstPage");

        gameObject.AddComponent<UIManager>();

        //GameObject cameramanager = new GameObject("CameraManager",typeof(CameraManager));
        //GameObject modelmanager = new GameObject("ModelManager", typeof(ModelManager));
        //GameObject webmanager = new GameObject("WebManager", typeof(WebManager));
        //GameObject mousemanager = new GameObject("MouseManager", typeof(MouseManager));

        //cameramanager.GetComponent<CameraManager>().InitCameraManager();

        /*创建ModelManager
          包含
          1.normalmodel控制
          2.usermodel控制
          3.各自pointer控制
          
          与UI交互
          点击pointer需跳转到该UI对应的第几组的第几个button 高亮动画
          点击该UI时需要高亮pointer(标准的)和(用户的，如果有)

          点击UI编辑时需高亮pointer
         */

        /*创建CameraManager
         * 控制摄像机旋转
         *点击切换六视图，点击编辑pointer时视图中心切换到该点，退出匹配点模式时需回到原始中心点  
        */


        /*创建静态数据容器
          UI和modlemanager需根据数据加载 
         */

        /*创建webmanager
         */

        /*Tool 和 JsonHelper
         */
    }



    void RegistToQeventsystem()
    {

    }

}


public enum GameMsgID
{
    GameStart = QMgrID.Game
}
