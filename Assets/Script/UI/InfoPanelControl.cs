using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelControl : MonoBehaviour {

    private Text title;
    private Text position;
    private Text protector_shape;
    private Text injury_position;
    private Text description;
    private Text note;


    public void Init(PlayerData data)
    {
        title = transform.Find("titleText").GetComponent<Text>();
        position = transform.Find("positionText").GetComponent<Text>();
        protector_shape = transform.Find("protector_shapeText").GetComponent<Text>();
        injury_position = transform.Find("injury_positionText").GetComponent<Text>();
        description = transform.Find("descriptionText").GetComponent<Text>();
        note = transform.Find("noteText").GetComponent<Text>();

        RefreshInfo(data);
    }



    private void RefreshInfo(PlayerData data)
    {
        //名字
        title.text = data.title;
        //受伤部位
        injury_position.text = data.injury_position;
        //方向
        int direction = (int)data.position;
        position.text = Tool.directionill[direction];
        //护具外形
        int shape = (int)data.protector_shape;
        protector_shape.text = Tool.protector_shape[shape];
        //病情简述
        description.text = data.description;
        //备注
        note.text = data.note;
    }

}
