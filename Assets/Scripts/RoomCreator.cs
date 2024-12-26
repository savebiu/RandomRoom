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

    List<GameObject> farRooms = new List<GameObject>();    //��Զ���� �б�
    List<GameObject> lessFarRooms = new List<GameObject>();   //��Զ�����ǰһ������ �б�
    List<GameObject> oneDoorRooms = new List<GameObject>();     //��Զ���� ����һ���� ��ֻ��һ���ŵķ����б�

    public int maxStemp;     //��Զ����Ĳ���

    //�����б�
    public List<Room> rooms = new List<Room>();
    private void Start()
    {
        //ѭ�������б�
        for (int i = 0; i < roomCount; i++)
        {
            rooms.Add(Instantiate(roomPrefab, pointCreat.position, Quaternion.identity).GetComponent<Room>());

            //���ķ���λ��
            ChangePointPos();
        }

        //Ϊʼ��ĩ����������ɫ
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        //Ϊβ�������ʼֵ
        endRoom = rooms[0].gameObject;
        foreach (var room in rooms)
        {
            //�ж���Զ����λ��
            //if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)       //sqrMagnitude:�����ĳ��ȵ�ƽ��(x*x + y*y + z*z)
            //{
            //    endRoom = room.gameObject;
            //}

            SetupDoor(room, room.transform.position);
        }
        FindEndRoom();
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

    //����Ӧ�����Ƿ��з��䲢������
    public void SetupDoor(Room newRoom, Vector3 roomPosition)
    {
        //�Ϸ�
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        //�·�
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        //��
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        //�ҷ�
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

        newRoom.RoomUpdate();

    }

    public void FindEndRoom()
    {
        //�������з���,�����Զ����Ĳ���
        for (int i = 0; i < rooms.Count; i++)
        {
            if(maxStemp <= rooms[i].stempToStart)
            {
                maxStemp = rooms[i].stempToStart;
            }
        }

        foreach (var room in rooms)
        {
            //��Զ����������б�
            if (room.stempToStart == maxStemp)            
                farRooms.Add(room.gameObject);            
            //��Զ�����ǰһ������������б�
            if (room.stempToStart == maxStemp - 1)            
                lessFarRooms.Add(room.gameObject);           
        }

        //��Զ���� ����һ���� ��ֻ��һ���ŵķ���������б�
        for(int i = 0; i<farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNumber == 1)
            {
                oneDoorRooms.Add(farRooms[i]);
            }            
        }
        for(int i = 0; i<lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNumber == 1)
            {
                oneDoorRooms.Add(lessFarRooms[i]);
            } 
        }
        
        //�е��ŷ�����ӵ��ŷ��������        
        if(oneDoorRooms.Count != 0)
        {
            endRoom = oneDoorRooms[Random.Range(0, oneDoorRooms.Count)];
        }
        //���û�е��ŵķ���,������Զ�����ǰһ�����������ѡһ������
        else
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }
    }
} 
