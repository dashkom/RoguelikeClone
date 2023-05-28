using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomConroller : MonoBehaviour
{
    public static RoomConroller instance;
    string curWorldName = "House";
    RoomInfo curLoadRoomData;
    Room curRoom;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRoom = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        instance = this;
        UpdateRoomQueue();
        UpdateEnemyState();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }
        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnedBossRoom());

            }
            else if (spawnedBossRoom && !updatedRoom)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRoom = true;
            }
            return;
        }
        curLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(curLoadRoomData));

    }

    private Room endRoom;

    IEnumerator SpawnedBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);

            LoadRoom("End", tempRoom.X, tempRoom.Y);

            yield return StartCoroutine(LoadRoomRoutine(new RoomInfo() { name = "End", X = tempRoom.X, Y = tempRoom.Y }));

            // Найдите комнату "End"
            endRoom = FindRoom(tempRoom.X, tempRoom.Y);
        }
    }
    

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y) == true)
        {
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = curWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(curLoadRoomData.X, curLoadRoomData.Y))
        {
            room.transform.position = new Vector3(curLoadRoomData.X * room.Width, curLoadRoomData.Y * room.Height, 0);

            room.X = curLoadRoomData.X;
            room.Y = curLoadRoomData.Y;
            room.name = curWorldName + "-" + curLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.curRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.curRoom = room;
        curRoom = room;

        // Проверить, если все враги в текущей комнате убиты, то открыть двери
        if (room.aliveEnemyCount == 0)
        {
            foreach (Door door in room.doors)
            {
                door.OpenDoor();
            }
        }
        else
        {
            StartCoroutine(DelayedCloseDoors(room));
        }
    }
    private IEnumerator DelayedCloseDoors(Room room)
    {
        // Задержка в 1 секунду
        yield return new WaitForSeconds(1f);

        foreach (Door door in room.doors)
        {
            door.CloseDoor();
        }
    }

    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }



    public void UpdateEnemyState()
    {
        bool isEndRoom = (curRoom != null && curRoom.name.Contains("End"));

        foreach (Room room in loadedRooms)
        {
            bool isInRoom = (room == curRoom);
            bool isEndRoomExcluded = (room != endRoom);

            EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();

            foreach (EnemyController enemy in enemies)
            {
                enemy.notInRoom = (!isInRoom || (isEndRoom && isEndRoomExcluded));
            }
        }
    }

    public void UpdateRooms()
    {
        foreach (Room room in loadedRooms)
        {
            bool isInRoom = (room == curRoom);

            EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();

            foreach (EnemyController enemy in enemies)
            {
                enemy.notInRoom = !isInRoom;
            }
        }

        Debug.Log("endRoom in loadedRooms: " + loadedRooms.Contains(endRoom));
    }

}