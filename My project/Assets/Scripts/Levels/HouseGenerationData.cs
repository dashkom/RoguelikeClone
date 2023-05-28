using UnityEngine;

[CreateAssetMenu(fileName = "HouseGenerationData.asset", menuName = "HouseGenerationData/House Data")]
public class HouseGenerationData : ScriptableObject
{
    public int numberOfCrawlers;
    public int iterMin;
    public int iterMax;
    public float stoneRoomChance = 0.6f;
}
