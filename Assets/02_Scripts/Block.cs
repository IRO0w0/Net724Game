using UnityEngine;

public class Block : MonoBehaviour
{
    private int hitCount = 0; // 블록이 밟힌 횟수
    public int hitsToDestroy = 2; // 블록이 사라지기 위해 필요한 밟힘 횟수

    public void HitByPlayer()
    {
        hitCount++;
        if (hitCount >= hitsToDestroy)
        {
            Destroy(gameObject); // 블록 제거
            Debug.Log("블록이 사라졌습니다.");
        }
        else
        {
            Debug.Log("블록이 밟혔습니다. 현재 횟수: " + hitCount);
        }
    }
}
