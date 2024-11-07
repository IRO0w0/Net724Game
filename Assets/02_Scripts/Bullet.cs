using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint; // 총알 발사 위치
    public float bulletSpeed = 10f; // 총알 속도

    private bool hasFired = false; // 총알이 발사되었는지 여부

    public AudioSource cannonAudio;     // 발사 소리를 담을 AudioSource
    public AudioClip fireClip;       // 발사 소리 파일

    public ParticleSystem muzzleFlash;  // 플래시 이펙트를 담을 Particle System

    void Start()
    {
        cannonAudio = GetComponent<AudioSource>();  // 오브젝트의 AudioSource 컴포넌트를 가져옴
    }

    void Update()
    {
        // GameManager에서 턴 수 가져오기
        int turnCount = GameManager.Instance.GetTurnCount();

        // 턴 수가 3의 배수이고 발사되지 않은 상태면 발사
        if (turnCount % 3 == 0 && turnCount != 0 && !hasFired)
        {
            Fire();
            hasFired = true;
        }

        // 다음 홀수 턴에서 다시 발사 가능하도록 설정
        if (turnCount % 3 != 0)
        {
            hasFired = false;
        }
    }

    // 발사 메서드
    void Fire()
    {
        // 총구 플래시 효과 재생
        if (muzzleFlash != null)
        {
            // 총구 플래시 이펙트를 직접 생성하고, 발사 위치 및 방향을 맞춤
            var flash = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
            flash.transform.forward = firePoint.forward;  // 이펙트의 방향을 발사 방향으로 설정
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration); // Particle System의 main.duration 시간 후 삭제
        }

        // 발사 소리 재생
        if (cannonAudio != null)
        {
            cannonAudio.PlayOneShot(fireClip);
        }

        // 총알 생성 및 발사
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange); // 총알 속도 설정
            Debug.Log("총알 발사!");
        }
        else
        {
            Debug.LogError("발사된 총알에 Rigidbody가 없습니다.");
        }

        // 총알 이동 관리 추가
        BulletMovement bulletMovement = bullet.AddComponent<BulletMovement>();
        bulletMovement.SetInitialPosition(bullet.transform.position);
    }


}

public class BulletMovement : MonoBehaviour
{
    private Vector3 initialPosition;

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }

    void Update()
    {
        // Z축 또는 X축 기준으로 2.5 이상 이동하면 총알 파괴
        if (Mathf.Abs(transform.position.z - initialPosition.z) >= 2.5f || Mathf.Abs(transform.position.x - initialPosition.x) >= 2.5f)
        {
            Destroy(gameObject);
            Debug.Log("총알이 2.5만큼 이동하여 파괴됨");
        }
    }
}
