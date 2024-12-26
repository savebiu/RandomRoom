using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{

    public GameObject door_Right, door_Left, door_Up, door_Down;

    public bool roomRight, roomLeft, roomUp, roomDown;

    //����ʼֵ�ľ���
    public int stempToStart;

    public Text text;

    public int doorNumber;

    private void Start()
    {
        door_Right.SetActive(roomRight);
        door_Left.SetActive(roomLeft);
        door_Up.SetActive(roomUp);
        door_Down.SetActive(roomDown);

    }

    //��ǰ�������ʼ������������
    public void RoomUpdate()
    {

        //��ǰ�����λ�ó���ƫ����, �����,���Ƿ�������������Ϊ����,Ҫȥ����ֵ
        stempToStart = (int)(Mathf.Abs(transform.position.x / 18 ) + Mathf.Abs(transform.position.y / 9));

        text.text = stempToStart.ToString();

        if (door_Up)       
            doorNumber++;
        if (door_Down)
            doorNumber++;
        if (door_Left)
            doorNumber++;
        if (door_Right)
            doorNumber++;
    }
}
