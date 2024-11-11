using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField]
    private float MaxDistance = 30f; // 레이 범위
    private Vector3 playerLastPosition; // 플레이어의 이전 위치 저장
    public GameObject player; // 플레이어 오브젝트
    public float moveDistance = 2f; // 적 이동 거리
    public float movementThreshold = 0.1f; // 플레이어 이동 감지 임계치
    private bool playerInSight = false; // 플레이어가 레이 범위 안에 있는지 여부
    private Vector3 targetPosition; // 적이 이동할 목표 위치
    public float speed = 5f; // 적 이동 속도 (초당 이동할 거리)

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // 플레이어의 초기 위치 저장
        playerLastPosition = player.transform.position;
        // 플레이어 이동 완료 이벤트 구독
        PlayerManager.OnPlayerMoveComplete += MoveEnemyOnPlayerMove;

        // 적의 초기 목표 위치 설정 (현재 위치)
        targetPosition = transform.position;
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        PlayerManager.OnPlayerMoveComplete -= MoveEnemyOnPlayerMove;
    }

    private void MoveEnemyOnPlayerMove()
    {
        // 플레이어가 레이 범위 안에 있을 때만 적이 이동
        if (playerInSight)
        {
            animator.SetBool("IsRun", true);
            // 이동할 목표 위치를 적이 바라보는 방향으로 설정
            targetPosition = transform.position + transform.forward * moveDistance;
        }
    }

    void Update()
    {
        // 레이를 캐릭터의 몸통 높이에서 시작하도록 설정
        Vector3 rayStartPosition = transform.position + Vector3.up * 1.5f; // 1.5f는 높이값으로 조정 가능

        // 적의 레이를 시각화
        Debug.DrawRay(rayStartPosition, transform.forward * MaxDistance, Color.blue, 0.3f);

        // 레이캐스트로 플레이어를 감지
        if (Physics.Raycast(rayStartPosition, transform.forward, out hit, MaxDistance))
        {
            if (hit.collider.gameObject == player)
            {
                playerInSight = true;
                Vector3 playerCurrentPosition = player.transform.position;
                if (Vector3.Distance(playerCurrentPosition, playerLastPosition) > movementThreshold)
                {
                    Debug.Log("Player is moving in range!");
                    playerLastPosition = playerCurrentPosition;
                }
            }
            else
            {
                playerInSight = false;
            }
        }
        else
        {
            playerInSight = false;
        }

        if (playerInSight && transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        if (transform.position == targetPosition)
        {
            animator.SetBool("IsRun", false);
        }
    }

}
