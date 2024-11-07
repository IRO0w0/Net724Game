using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject character;               // 플레이어 오브젝트
    public Camera mainCamera;                  // 메인 카메라
    public Vector3 offset;                     // 카메라의 플레이어와의 거리 오프셋
    private bool shouldFollow = false;         // 카메라가 따라와야 하는지 여부
    private bool hasFollowed = false;          // 카메라가 이미 따라왔는지 여부를 추적

    public GameObject obstaclesGroup;          // 제거할 오브젝트 그룹
    public GameObject[] targetPositions;       // 스테이지별 목표 위치 오브젝트 배열
    private int currentStageIndex = 0;         // 현재 스테이지 인덱스

    void Start()
    {
        if (targetPositions.Length > 0)
        {
            // 초기 카메라 위치 설정 (첫 번째 목표 위치로)
            offset = mainCamera.transform.position - character.transform.position;
        }
        else
        {
            Debug.LogError("targetPositions 배열이 비어있습니다. 목표 위치를 설정해주세요.");
        }
    }

    private void FixedUpdate()
    {
        if (shouldFollow && !hasFollowed && currentStageIndex < targetPositions.Length)
        {
            // 현재 스테이지의 목표 위치로 카메라 이동
            Vector3 targetPos = targetPositions[currentStageIndex].transform.position;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, 5 * Time.deltaTime);

            // 카메라가 목표 위치에 거의 도달했는지 확인
            if (Vector3.Distance(mainCamera.transform.position, targetPos) < 0.1f)
            {
                GameManager.Instance.ResetTurn();
                hasFollowed = true;
                shouldFollow = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == character && !hasFollowed)
        {
            // 플레이어가 트리거 영역에 들어가면 카메라가 현재 목표 위치로 이동하게 설정
            shouldFollow = true;

            // 스테이지의 장애물 제거
            Destroy(obstaclesGroup);
        }
    }

    public void CompleteStage()
    {
        // 스테이지 완료 처리
        if (currentStageIndex < targetPositions.Length - 1)
        {
            currentStageIndex++;  // 다음 스테이지로 인덱스 증가
            hasFollowed = false;  // 다음 스테이지에서 카메라 이동을 다시 활성화
        }
        else
        {
            Debug.Log("모든 스테이지가 완료되었습니다.");
        }
    }
}
