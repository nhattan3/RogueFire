using UnityEngine;
using UnityEngine.UI;
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float enemyMoveSpeed = 1f;
    [SerializeField] protected float maxHp=50f;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDamage=10f;
    [SerializeField] protected float stayDamage =1f;
    protected float currentHp;
    protected Player player;

    protected virtual void Start()
    {
        // Tìm kiếm đối tượng Player trong Scene
        player = FindAnyObjectByType<Player>();
        currentHp = maxHp ;
        UpdateHpBar();
    }

    protected virtual void Update()
    {
        // Gọi hàm di chuyển liên tục mỗi khung hình
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (player != null)
        {
            // Di chuyển vị trí kẻ địch về phía Player
            transform.position = Vector2.MoveTowards(
                transform.position, 
                player.transform.position, 
                enemyMoveSpeed * Time.deltaTime
            );

            // Xử lý quay mặt kẻ địch
            FlipEnemy();
        }
    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            // Nếu Player ở bên trái kẻ địch, lật Scale X sang -1, ngược lại là 1
            float direction = player.transform.position.x < transform.position.x ? -1f : 1f;
            transform.localScale = new Vector3(direction, 1f, 1f);
        }
    }
    public void TakeDamage(float damage)
    {
       currentHp -= damage;
       currentHp =Mathf.Max(currentHp,0);
       UpdateHpBar();
       if(currentHp<=0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    protected void UpdateHpBar()
    {
        if(hpBar != null)
        {
            hpBar.fillAmount= currentHp/maxHp;
        }
    }
}