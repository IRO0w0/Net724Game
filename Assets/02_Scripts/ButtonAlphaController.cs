using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject settingsCanvas;

    private void Start()
    {
        // 게임 시작 시 설정 캔버스를 숨깁니다.
        settingsCanvas.SetActive(false);
    }

    // 시작 버튼을 클릭했을 때 씬 이동
    public void StartGame()
    {
        Debug.Log("게임 씬으로 이동합니다.");
        SceneManager.LoadScene("Stage_1"); // 이동할 씬 이름을 입력합니다.
    }

    // 설정 버튼을 클릭했을 때 설정 캔버스를 표시
    public void OpenSettings()
    {
        settingsCanvas.SetActive(true);
        Debug.Log("설정 메뉴가 열렸습니다.");
    }

    // 나가기 버튼을 클릭했을 때 게임 종료
    public void ExitGame()
    {
        Debug.Log("게임을 종료합니다.");
        Application.Quit();
    }
}
