using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
};
public class HouseCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up },
        {Direction.left, Vector2Int.left },
        {Direction.down, Vector2Int.down },
        {Direction.right, Vector2Int.right }
    };

    public static List<Vector2Int> GenerateRooms(HouseGenerationData generationData)
    {
        List<HouseCrawler> houseCrawlers = new List<HouseCrawler>();
        
        for (int i = 0; i < generationData.numberOfCrawlers; i++)
        {
            houseCrawlers.Add(new HouseCrawler(Vector2Int.zero));
        }

        int iters = Random.Range(generationData.iterMin, generationData.iterMax);

        for (int i = 0; i < iters; i++)
        {
            foreach(HouseCrawler c in houseCrawlers)
            {
                Vector2Int newPos = c.Move(directionMovementMap);
                positionsVisited.Add(newPos); 
            }
        }

        return positionsVisited;
    }
}
