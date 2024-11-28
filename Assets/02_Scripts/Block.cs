using UnityEngine;

public class Block : MonoBehaviour
{
    private int hitCount = 0; // 블록이 밟힌 횟수
    public int hitsToDestroy = 2; // 블록이 사라지기 위해 필요한 밟힘 횟수

    // 플레이어와 충돌했을 때 호출되는 함수
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 객체가 "Player" 태그를 가진 객체인지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 블록을 밟았을 때 hitCount 증가
            HitByPlayer();
        }
    }

    // 플레이어가 블록을 밟았을 때 호출되는 함수
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

    private void Update()
    {
        // 플레이어 HP가 0이면 적 초기화
        if (PlayerManager.playerHP <= 0)
        {
            ResetEnemyPositionAndRotation();
        }
    }

    private void ResetEnemyPositionAndRotation()
    {
        hitCount = 0; // hitCount 초기화
    }
}