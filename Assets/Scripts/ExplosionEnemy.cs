using UnityEngine;

public class ExplosionEnemy : Enemy
{
    [SerializeField] private GameObject explosionPrefabs;
    private bool hasDied = false; // ← guard tránh gọi Die() 2 lần

    private void CreateExplosion()
    {
        if (explosionPrefabs != null)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
        }
    }

    protected override void Die()
    {
        if (hasDied) return; // ← chặn gọi lại
        hasDied = true;
        CreateExplosion();
        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasDied) return; // ← nếu đã chết thì bỏ qua

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player == null)
                player = collision.GetComponentInParent<Player>();

            if (player != null)
            {
                Explosion explosion = explosionPrefabs.GetComponent<Explosion>();
                float dmg = explosion != null ? explosion.damage : 25f;
                player.TakeDamage(dmg);
            }

            Die();
        }
    }
}