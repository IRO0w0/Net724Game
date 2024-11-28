using UnityEngine;

public class Block : MonoBehaviour
{
    private int hitCount = 0; // ����� ���� Ƚ��
    public int hitsToDestroy = 2; // ����� ������� ���� �ʿ��� ���� Ƚ��

    // �÷��̾�� �浹���� �� ȣ��Ǵ� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� "Player" �±׸� ���� ��ü���� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾ ����� ����� �� hitCount ����
            HitByPlayer();
        }
    }

    // �÷��̾ ����� ����� �� ȣ��Ǵ� �Լ�
    public void HitByPlayer()
    {
        hitCount++;
        if (hitCount >= hitsToDestroy)
        {
            Destroy(gameObject); // ��� ����
            Debug.Log("����� ��������ϴ�.");
        }
        else
        {
            Debug.Log("����� �������ϴ�. ���� Ƚ��: " + hitCount);
        }
    }

    private void Update()
    {
        // �÷��̾� HP�� 0�̸� �� �ʱ�ȭ
        if (PlayerManager.playerHP <= 0)
        {
            ResetEnemyPositionAndRotation();
        }
    }

    private void ResetEnemyPositionAndRotation()
    {
        hitCount = 0; // hitCount �ʱ�ȭ
    }
}