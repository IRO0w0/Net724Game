using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour
{
    public Text tutorialText; // 튜토리얼 안내를 보여줄 UI 텍스트
    private int stepIndex = 0;
    private bool playerMoved = false;

    // 플레이어 이동 완료 이벤트 구독
    private void OnEnable()
    {
        PlayerManager.OnPlayerMoveComplete += PlayerMoved;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerMoveComplete -= PlayerMoved;
    }

    void Start()
    {
        StartTutorial(); // 튜토리얼 시작
    }

    void Update()
    {
        // 플레이어가 움직이지 않았을 때만 입력을 받음
        if (!playerMoved)
        {
            switch (stepIndex)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        tutorialText.text = "좋아요! 이제 S 키로 뒤로 이동해보세요.";
                        stepIndex++;
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        tutorialText.text = "잘했어요! 이제 A 키로 왼쪽으로 이동해보세요.";
                        stepIndex++;
                    }
                    break;
                case 2:
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        tutorialText.text = "훌륭해요! 이제 D 키로 오른쪽으로 이동해보세요.";
                        stepIndex++;
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        tutorialText.text = "완벽해요! 이동 방법을 배웠어요.";
                        stepIndex++;
                        FinishTutorial();
                    }
                    break;
            }
        }
    }

    // 튜토리얼 시작
    void StartTutorial()
    {
        tutorialText.text = "W 키를 눌러 앞으로 이동하세요.";
    }

    // 플레이어가 이동했을 때 호출되는 함수
    void PlayerMoved()
    {
        playerMoved = true; // 이동 완료 시 true 설정

        // 0.5초 후 다시 움직임 가능 상태로 설정
        Invoke(nameof(ResetMoveFlag), 0.5f);
    }

    void ResetMoveFlag()
    {
        playerMoved = false; // 다시 입력을 받을 수 있도록 설정
    }

    void FinishTutorial()
    {
        tutorialText.text = "튜토리얼이 완료되었습니다!";
    }
}
