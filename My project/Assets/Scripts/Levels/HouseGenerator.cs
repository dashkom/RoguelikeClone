using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGenerator : MonoBehaviour
{
    public HouseGenerationData generationData;
    private List<Vector2Int> houseRooms;
    private void Start()
    {
        houseRooms = HouseCrawlerController.GenerateRooms(generationData);
        SpawnRooms(houseRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomConroller.instance.LoadRoom("Start", 0, 0);
        foreach (Vector2Int roomLoaction in rooms)
        {
            string roomType = Random.value < generationData.stoneRoomChance ? "Stone" : "Empty";
            RoomConroller.instance.LoadRoom(roomType, roomLoaction.x, roomLoaction.y);
        }
    }
}
