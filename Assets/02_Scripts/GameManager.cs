using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    private int turnCount = 0;
    private static GameManager instance;



    


    void Awake()
    {
        // Game Manager 인스턴스가 하나만 있는지 확인
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    public void ResetTurn()
    {
        turnCount = 0;
        Debug.Log("턴 수 초기화");
        Debug.Log("현재 턴: " + turnCount);
    }
    public void IncrementTurn()
    {
        turnCount++;
        Debug.Log("현재 턴: " + turnCount);
    }
    public int GetTurnCount()
    {
        return turnCount;
    }





   
    /// 총알이 충돌시
    public void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag("Bullet")) // 진짜 총알과 충돌했을 때
        {
            PlayerManager.TakeDamage(100); // HP를 0으로 설정하여 플레이어 사망 처리
            Destroy(collidedObject); // 총알 제거
        }
        else if (collidedObject.CompareTag("FakeBullet")) // 가짜 총알과 충돌했을 때
        {
            // 가짜 총알과의 충돌은 플레이어의 HP에 영향을 주지 않음
            Destroy(collidedObject); // 가짜 총알만 제거
        }
    }
}