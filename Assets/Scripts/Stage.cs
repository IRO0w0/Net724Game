using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2 : MonoBehaviour
{
    // 전환할 씬의 이름 (여기서는 첫 번째 페이지인 "Stage"로 이동)
    public string sceneToLoad = "Stage";

    // 페이드 아웃을 처리할 CanvasGroup 참조 (FadePanel에 할당해야 함)
    public CanvasGroup fadeCanvasGroup;

    // 페이드 아웃 지속 시간 (여기서는 0.5초로 설정)
    public float fadeDuration = 0.5f;

    // 씬 로드 여부를 체크하기 위한 플래그
    private bool isSceneLoading = false;

    // 매 프레임마다 호출
    void Update()
    {
        // 터치 또는 마우스 클릭을 감지
        if (Input.GetMouseButtonDown(0) && !isSceneLoading)
        {
            // 씬 로드를 한 번만 진행하도록 플래그 설정
            isSceneLoading = true;

            // 페이드 아웃 후 씬 로드 진행
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    // 페이드 아웃 후 씬 로드 코루틴
    private System.Collections.IEnumerator FadeOutAndLoadScene()
    {
        // 페이드 아웃 (화면이 서서히 검은색으로 덮임)
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            float elapsedTime = Time.time - startTime;
            float percentageComplete = elapsedTime / fadeDuration;

            // 알파 값 변경 (0 -> 1)
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, percentageComplete);

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f; // 최종적으로 알파 값을 1로 설정하여 화면을 완전히 덮음

        // 씬 로드
        SceneManager.LoadScene(sceneToLoad);
    }
}
