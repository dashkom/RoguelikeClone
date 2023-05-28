using UnityEngine;

public class WallController : MonoBehaviour
{
    public BoxCollider2D passageCollider;
    public GameObject doorObject;

    private void Update()
    {
        bool hasDoor = doorObject != null && doorObject.activeSelf;

        if (hasDoor)
        {
            passageCollider.isTrigger = true; // Установите триггер для прохода
        }
        else
        {
            passageCollider.isTrigger = false; // Установите коллизию для прохода
        }
    }
}
