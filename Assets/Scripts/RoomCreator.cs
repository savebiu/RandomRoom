using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Random
using Random = UnityEngine.Random;
//���¼��س���
using UnityEngine.SceneManagement;

public class RoomCreator : MonoBehaviour
{
    //����
    public enum Direction
    {
        up,
        down,
        left,
        right
    }
    public Direction direction;

    [Header("������Ϣ")]
    //Ԥ����
    public GameObject roomPrefab;
    //������
    public int roomCount;
    //��ɫ
    public Color startColor, endColor;

    //PointCreat��λ��
    [Header("����λ�ÿ���")]
    public Transform pointCreat;
    public float xOffset;
    public float yOffset;

    //ͼ����
    [Header("ͼ����")]
    public LayerMask roomLayer;

    //β������
    public GameObject endRoom;


    //�����б�
    public List<GameObject> rooms = new List<GameObject>();
    private void Start()
    {
        //ѭ�������б�
        for (int i = 0; i < roomCount; i++)
        {
            rooms.Add(Instantiate(roomPrefab, pointCreat.position, Quaternion.identity));

            //���ķ���λ��
            ChangePointPos();
        }

        //Ϊʼ��ĩ����������ɫ
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        //Ϊβ�������ʼֵ
        endRoom = rooms[0];
        foreach (var room in rooms)
        {
            if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)       //sqrMagnitude:�����ĳ��ȵ�ƽ��(x*x + y*y + z*z)
            {
                endRoom = room;

            }
        }
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
    }
    private void Update()
    {
        //Test
        //���¼��س���
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //������ɷ���λ��
    private void ChangePointPos()
    {
        //����ö��(����)��switch����л�
        direction = (Direction)Random.Range(0, 4);

        do
        {
            //��Բ�ͬö�ټ����в�ͬλ���޸�
            switch (direction)
            {
                case Direction.up:
                    pointCreat.position += new Vector3(0, yOffset, 0);
                    break;

                case Direction.down:
                    pointCreat.position += new Vector3(0, -yOffset, 0);
                    break;

                case Direction.left:
                    pointCreat.position += new Vector3(-xOffset, 0, 0);
                    break;

                case Direction.right:
                    pointCreat.position += new Vector3(xOffset, 0, 0);
                    break;

                default:
                    break;
            }
        } while (Physics2D.OverlapCircle(pointCreat.position, 0.2f, roomLayer));
       
    }
}
