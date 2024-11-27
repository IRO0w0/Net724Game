using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager 인스턴스가 없습니다!");
            }
            return _instance;
        }
    }

    [Header("플레이어 설정")]
    public GameObject playerPrefab; // 리스폰 시 사용할 플레이어 프리팹
    public static Vector3 respawnPoint; // 리스폰 위치
    public int maxHealth = 100;
    public static int playerHP;

    [Header("이동 설정")]
    public float moveDistance = 1f; // 한 번에 이동할 거리
    public float moveSpeed = 5f; // 이동 속도

    [Header("UI 설정")]
    public RawImage[] deathImages;
    public Canvas deathCanvas;
    public float fadeDuration = 0.5f;
    public float respawnDelay = 3.0f;
    public Button UpButton;
    public Button DownButton;
    public Button LeftButton;
    public Button RightButton;

    private Vector3 targetPosition; // 이동 목표 위치
    private bool isMoving = false;  // 이동 중인지 여부
    private bool isFirstDeath = true;
    private bool isDead = false; // 사망 상태 확인 플래그

    private Quaternion initialRotation; // 초기 방향 저장
    private Animator animator;

    public static event Action OnPlayerMoveComplete; // 이동 완료 이벤트

    private Coroutine firstDeathCoroutine;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    private void Start()
    {
        playerHP = maxHealth;
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        initialRotation = transform.rotation; // 초기 방향 저장
        firstDeathCoroutine = null;

        if (respawnPoint == Vector3.zero)
            respawnPoint = transform.position; // 초기 리스폰 위치 설정

        UpButton.onClick.AddListener(MoveUp);
        DownButton.onClick.AddListener(MoveBack);
        LeftButton.onClick.AddListener(MoveLeft);
        RightButton.onClick.AddListener(MoveRight);
    }

    private void Update()
    {
        if (isDead) return; // 사망 중에는 업데이트 중지

        if (!isMoving && playerHP > 0) // 사망하지 않았을 때만 이동 입력 처리
        {
            HandleMovementInput();
        }
        // Y좌표가 -1 이하로 떨어지면 체력을 0으로 설정
        if (transform.position.y <= -1f)
        {
            playerHP = 0;
            Die();
        }

        if (playerHP <= 0) // 체력이 0 이하이면 사망 처리
        {
            Die();
        }

        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    public void MoveUp()
    {
        Move(Vector3.forward);
    }

    public void MoveBack()
    {
        Move(Vector3.back);
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    private void HandleMovementInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.back);
        else if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right);
    }

    private void Move(Vector3 direction)
    {
        targetPosition = transform.position + direction * moveDistance;
        transform.rotation = Quaternion.LookRotation(direction);

        if (!CheckCollisionAtTargetPosition())
        {
            isMoving = true;
            GameManager.Instance.IncrementTurn();
        }
    }

    private void MoveTowardsTarget()
    {
        animator.SetBool("IsRun", true);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            if (!CheckCollisionAtTargetPosition())
            {
                transform.position = targetPosition;
                isMoving = false;

                animator.SetBool("IsRun", false);
                OnPlayerMoveComplete?.Invoke();
            }
        }
    }

    private bool CheckCollisionAtTargetPosition()
    {
        Collider[] colliders = Physics.OverlapSphere(targetPosition, 0.1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("TransparentBlock"))
                return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Block block = collision.gameObject.GetComponent<Block>();
            block?.HitByPlayer();
        }

        GameManager.Instance.HandleCollision(collision.gameObject);
    }

    public static void TakeDamage(int damage)
    {
        playerHP -= damage;
    }

    private void Die()
    {
        if (isDead) return; // 이미 사망 처리 중이라면 중복 실행 방지
        isDead = true;

        Debug.Log("플레이어가 사망했습니다.");
        if (isFirstDeath)
        {
            firstDeathCoroutine = StartCoroutine(ShowDeathImagesAndRespawn());
        }
        else
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    IEnumerator ShowDeathImagesAndRespawn()
    {
        isFirstDeath = false;
        deathCanvas.gameObject.SetActive(true);

        foreach (RawImage img in deathImages)
        {
            img.gameObject.SetActive(false);
        }

        for (int i = 0; i < deathImages.Length; i++)
        {
            RawImage currentImage = deathImages[i];
            currentImage.gameObject.SetActive(true);
            currentImage.canvasRenderer.SetAlpha(0f);
            StartCoroutine(FadeIn(currentImage));
            yield return new WaitForSeconds(1f - fadeDuration);
            StartCoroutine(FadeOut(currentImage));
            yield return new WaitForSeconds(1f);
            currentImage.gameObject.SetActive(false);
        }

        deathCanvas.gameObject.SetActive(false);
        yield return RespawnAfterDelay();
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        Respawn();
        GameManager.Instance.ResetTurn();
        isDead = false;
    }

    private IEnumerator FadeIn(RawImage image)
    {
        image.CrossFadeAlpha(1f, fadeDuration, false); // 페이드인
        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator FadeOut(RawImage image)
    {
        image.CrossFadeAlpha(0f, fadeDuration, false); // 페이드아웃
        yield return new WaitForSeconds(fadeDuration);
    }

    public void Respawn()
    {
        Debug.Log("Player Respawned!");
        transform.position = respawnPoint;
        transform.rotation = initialRotation; // 초기 방향으로 설정
        playerHP = maxHealth;

        // 이동 상태 초기화
        isMoving = false;
        targetPosition = transform.position;

        // 애니메이션 초기화
        animator.Rebind();
        animator.Update(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            respawnPoint = other.transform.position; // 리스폰 지역 업데이트
            Debug.Log("새로운 리스폰 지역 설정됨: " + respawnPoint);
        }
    }
}
