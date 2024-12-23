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


    //房间列表
    public List<GameObject> rooms = new List<GameObject>();
    private void Start()
    {
        //循环创建列表
        for (int i = 0; i < roomCount; i++)
        {
            rooms.Add(Instantiate(roomPrefab, pointCreat.position, Quaternion.identity));

            //更改房间位置
            ChangePointPos();
        }

        //为始、末房间设置颜色
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;

        //为尾房间设初始值
        endRoom = rooms[0];
        foreach (var room in rooms)
        {
            if (room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)       //sqrMagnitude:向量的长度的平方(x*x + y*y + z*z)
            {
                endRoom = room;

            }
        }
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
}
