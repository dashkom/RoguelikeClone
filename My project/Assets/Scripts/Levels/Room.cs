using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;
    public int aliveEnemyCount;
    private bool updatedDoors = false;
    public bool IsBossRoom { get; set; }
    //public RoomConroller roomConroller;

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public List<Door> doors = new List<Door>();

    // Start is called before the first frame update
    void Start()
    {
        if (RoomConroller.instance == null)
        {
            Debug.Log("Чет не то");
            return;
        }

        Door[] ds = GetComponentsInChildren<Door>();
        foreach (Door d in ds)
        {
            doors.Add(d);
            switch (d.doortype)
            {
                case Door.DoorType.right: rightDoor = d; break;
                case Door.DoorType.left: leftDoor = d; break;
                case Door.DoorType.bottom: bottomDoor = d; break;
                case Door.DoorType.top: topDoor = d; break;
            }
        }

        RoomConroller.instance.RegisterRoom(this);
    }


    void Update()
    {
        if (name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (Door d in doors)
        {
            switch (d.doortype)
            {
                case Door.DoorType.right:
                    if (GetRight() == null)
                        d.gameObject.SetActive(false);
                    rightDoor = d; break;
                case Door.DoorType.left:
                    if (GetLeft() == null)
                        d.gameObject.SetActive(false);
                    leftDoor = d; break;
                case Door.DoorType.bottom:
                    if (GetBottom() == null)
                        d.gameObject.SetActive(false);
                    bottomDoor = d; break;
                case Door.DoorType.top:
                    if (GetTop() == null)
                        d.gameObject.SetActive(false);
                    topDoor = d; break;
            }
        }
    }

    public Room GetRight()
    {
        if (RoomConroller.instance.DoesRoomExist(X + 1, Y))
        {
            return RoomConroller.instance.FindRoom(X + 1, Y);
        }
        return null;
    }

    public Room GetLeft()
    {
        if (RoomConroller.instance.DoesRoomExist(X - 1, Y))
        {
            return RoomConroller.instance.FindRoom(X - 1, Y);
        }
        return null;
    }

    public Room GetTop()
    {
        if (RoomConroller.instance.DoesRoomExist(X, Y + 1))
        {
            return RoomConroller.instance.FindRoom(X, Y + 1);
        }
        return null;
    }

    public Room GetBottom()
    {
        if (RoomConroller.instance.DoesRoomExist(X, Y - 1))
        {
            return RoomConroller.instance.FindRoom(X, Y - 1);
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3(X * Width, Y * Height);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RoomConroller.instance.OnPlayerEnterRoom(this);
        }
    }
}