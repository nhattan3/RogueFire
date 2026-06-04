using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private float damage=10f;
    [SerializeField] GameObject bloodPrefabs;

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
{
    // Kiểm tra xem đối tượng va chạm có Tag là "Enemy" hay không
    if (collision.CompareTag("Enemy"))
    {
        // Lấy component Enemy từ đối tượng va chạm
        Enemy enemy = collision.GetComponent<Enemy>();

        // Nếu tìm thấy component Enemy, thực hiện trừ máu kẻ địch
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            GameObject blood =Instantiate(bloodPrefabs,transform.position,Quaternion.identity);
            Destroy(blood,1f);
        }

        // Tự hủy đối tượng này (ví dụ: viên đạn) sau khi va chạm
        Destroy(gameObject);
    }
}
}