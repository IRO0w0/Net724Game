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
        // 적의 레이를 시각화
        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 0.3f);

        // 레이캐스트로 플레이어를 감지
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxDistance))
        {
            if (hit.collider.gameObject == player)
            {
                // 플레이어가 레이 범위 안에 있음
                playerInSight = true;

                // 플레이어가 일정 거리 이상으로 움직였을 때만 적이 이동
                Vector3 playerCurrentPosition = player.transform.position;
                if (Vector3.Distance(playerCurrentPosition, playerLastPosition) > movementThreshold)
                {
                    Debug.Log("Player is moving in range!");
                    // 이동 완료 시 플레이어의 위치 업데이트
                    playerLastPosition = playerCurrentPosition;
                }
            }
            else
            {
                playerInSight = false; // 플레이어가 레이 범위를 벗어남
            }
        }
        else
        {
            playerInSight = false; // 레이 범위 내에 아무것도 감지되지 않음
        }

        // 적이 목표 위치에 도달할 때까지 부드럽게 이동 (이동 속도 반영)
        if (playerInSight && transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        // 적이 목표 위치에 도달하면 애니메이션 종료
        if (transform.position == targetPosition)
        {
            animator.SetBool("IsRun", false);
        }
    }
}
