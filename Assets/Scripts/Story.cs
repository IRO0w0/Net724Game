using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ImageSequenceHandler : MonoBehaviour
{
    // 표시할 RawImage 배열
    public RawImage[] images = new RawImage[6]; // RawImage 배열 크기를 6으로 설정

    // 현재 이미지 인덱스
    private int currentImageIndex = 0;

    // 페이드 지속 시간 (빠르게 설정)
    public float fadeDuration = 0.1f; // 0.1초로 설정하여 빠른 페이드 효과

    // 전환할 씬의 이름
    public string sceneToLoad = "stage_1";

    // 클릭 제한 시간
    private float clickCooldown = 1.0f;
    private bool canClick = true;

    void Start()
    {
        // 모든 RawImage를 비활성화하고 첫 번째 이미지만 활성화
        foreach (RawImage img in images)
        {
            img.gameObject.SetActive(false);
        }
        if (images.Length > 0)
        {
            images[0].gameObject.SetActive(true);
            images[0].canvasRenderer.SetAlpha(1.0f); // 처음 이미지는 완전하게 보이도록 설정
        }
    }

    void Update()
    {
        // 마우스 왼쪽 클릭을 감지하고, 클릭 제한 시간 동안 클릭 불가
        if (Input.GetMouseButtonDown(0) && canClick)
        {
            StartCoroutine(FadeInAndOutImages());
        }
    }

    IEnumerator FadeInAndOutImages()
    {
        canClick = false; // 클릭 불가 설정

        // 현재 이미지 인덱스가 배열 범위를 초과하지 않도록 체크
        if (currentImageIndex >= images.Length)
        {
            yield break; // 배열 범위를 초과한 경우, 메서드 종료
        }

        // 현재 이미지 (기존 이미지)
        RawImage currentImage = images[currentImageIndex];

        // 다음 이미지 인덱스와 이미지
        int nextIndex = currentImageIndex + 1;
        if (nextIndex < images.Length)
        {
            // 다음 이미지 활성화 및 초기 설정
            RawImage nextImage = images[nextIndex];
            nextImage.gameObject.SetActive(true);
            nextImage.canvasRenderer.SetAlpha(0f); // 투명하게 설정
            nextImage.CrossFadeAlpha(1f, fadeDuration, false); // 페이드 인
        }

        // 페이드 아웃이 완료될 때까지 대기
        if (nextIndex < images.Length)
        {
            // 다음 이미지가 완전히 나타날 때까지 대기
            yield return new WaitForSeconds(fadeDuration);

            // 현재 이미지 페이드 아웃
            currentImage.CrossFadeAlpha(0f, fadeDuration, false);

            // 페이드 아웃이 완료될 때까지 대기
            yield return new WaitForSeconds(fadeDuration);

            // 현재 이미지 비활성화
            currentImage.gameObject.SetActive(false);
        }
        else
        {
            // 모든 이미지가 끝나면 대기 후 모든 이미지를 비활성화
            yield return new WaitForSeconds(fadeDuration);

            foreach (RawImage img in images)
            {
                img.gameObject.SetActive(false); // 모든 이미지를 비활성화
            }

            // 마지막 이미지가 사라진 후 씬 전환
            SceneManager.LoadScene(sceneToLoad);
        }

        // 인덱스 증가
        currentImageIndex++;

        yield return new WaitForSeconds(clickCooldown); // 클릭 제한 시간 대기
        canClick = true; // 클릭 가능 상태로 변경
    }
}
