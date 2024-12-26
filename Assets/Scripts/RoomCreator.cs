using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Random
using Random = UnityEngine.Random;
//重新加载场景
using UnityEngine.SceneManagement;

public class RoomCreator : MonoBehaviour
{
    //方向
    public enum Direction
    {
        up,
        down,
        left,
        right
    }
    public Direction direction;

    [Header("房间信息")]
    //预制体
    public GameObject roomPrefab;
    //房间数
    public int roomCount;
    //颜色
    public Color startColor, endColor;

    //PointCreat的位置
    [Header("房间位置控制")]
    public Transform pointCreat;
    public float xOffset;
    public float yOffset;

    //图层检测
    [Header("图层检测")]
    public LayerMask roomLayer;

    //尾房间检测
    public GameObject endRoom;

    List<GameObject> farRooms = new List<GameObject>();    //最远房间 列表
    List<GameObject> lessFarRooms = new List<GameObject>();   //最远房间的前一个房间 列表
    List<GameObject> oneDoorRooms = new List<GameObject>();     //最远房间 与上一房间 间只有一个门的房间列表

    public int maxStemp;     //最远房间的步数

    //房间列表
    public List<Room> rooms = new List<Room>();
    private void Start()
    {
        //循环创建列表
        for (int i = 0; i < roomCount; i++)
        {
            rooms.Add(Instantiate(roomPrefab, pointCreat.position, Quaternion.identity).GetComponent<Room>());

            //更改房间位置
            ChangePointPos();
        }

        //为始、末房间设置颜色
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        //为尾房间设初始值
        endRoom = rooms[0].gameObject;
        foreach (var room in rooms)
        {
            //判断最远房间位置
            //if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)       //sqrMagnitude:向量的长度的平方(x*x + y*y + z*z)
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
        //重新加载场景
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //随机生成房间位置
    private void ChangePointPos()
    {
        //利用枚举(方向)和switch随机切换
        direction = (Direction)Random.Range(0, 4);

        do
        {
            //针对不同枚举及逆行不同位置修改
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

    //检测对应方向是否有房间并生成门
    public void SetupDoor(Room newRoom, Vector3 roomPosition)
    {
        //上方
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        //下方
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        //左方
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        //右方
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

        newRoom.RoomUpdate();

    }

    public void FindEndRoom()
    {
        //遍历所有房间,求出最远房间的步数
        for (int i = 0; i < rooms.Count; i++)
        {
            if(maxStemp <= rooms[i].stempToStart)
            {
                maxStemp = rooms[i].stempToStart;
            }
        }

        foreach (var room in rooms)
        {
            //最远房间添加至列表
            if (room.stempToStart == maxStemp)            
                farRooms.Add(room.gameObject);            
            //最远房间的前一个房间添加至列表
            if (room.stempToStart == maxStemp - 1)            
                lessFarRooms.Add(room.gameObject);           
        }

        //最远房间 与上一房间 间只有一个门的房间添加至列表
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
        
        //有单门房间则从单门房间中随机        
        if(oneDoorRooms.Count != 0)
        {
            endRoom = oneDoorRooms[Random.Range(0, oneDoorRooms.Count)];
        }
        //如果没有单门的房间,则在最远房间的前一个房间中随机选一个房间
        else
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }
    }
} 
