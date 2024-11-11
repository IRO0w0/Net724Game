using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject settingsCanvas;

    private void Start()
    {
        // ���� ���� �� ���� ĵ������ ����ϴ�.
        settingsCanvas.SetActive(false);
    }

    // ���� ��ư�� Ŭ������ �� �� �̵�
    public void StartGame()
    {
        Debug.Log("���� ������ �̵��մϴ�.");
        SceneManager.LoadScene("Stage_1"); // �̵��� �� �̸��� �Է��մϴ�.
    }

    // ���� ��ư�� Ŭ������ �� ���� ĵ������ ǥ��
    public void OpenSettings()
    {
        settingsCanvas.SetActive(true);
        Debug.Log("���� �޴��� ���Ƚ��ϴ�.");
    }

    // ������ ��ư�� Ŭ������ �� ���� ����
    public void ExitGame()
    {
        Debug.Log("������ �����մϴ�.");
        Application.Quit();
    }
}
