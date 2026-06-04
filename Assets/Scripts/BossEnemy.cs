using System;
using Unity.VisualScripting;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDanThuong = 20f;
    [SerializeField] private float speedDanVongTron = 10f;
    [SerializeField] private float hpValue = 100f;
    [SerializeField] private GameObject miniEnemy;
    [SerializeField] private float skillCooldown = 2f;
    [SerializeField] private GameObject usbPrefabs;
    private float nextSkillTime = 0f;

    protected override void Update()
    {
        base.Update();
        
        if (Time.time >= nextSkillTime)
        {
            SuDungSKill();
        }
    }
    protected override void Die()
    {
        Instantiate(usbPrefabs,transform.position, Quaternion.identity);
        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamage(stayDamage);
            }
        }
    }

    // Kỹ năng 1: Bắn thẳng vào Player
    private void BanDanThuong()
    {
        if (player != null && firePoint != null)
        {
            Vector3 directionToPlayer = player.transform.position - firePoint.position;
            directionToPlayer.Normalize();
            
            GameObject bullet = Instantiate(bulletPrefabs, firePoint.position, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            
            if (enemyBullet == null) 
            {
                enemyBullet = bullet.AddComponent<EnemyBullet>();
            }
            
            enemyBullet.SetMovementDirection(directionToPlayer * speedDanThuong);
        }
    }

    // Kỹ năng 2: Bắn đạn vòng tròn 360 độ
    private void BanDanVongTron()
    {
        const int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep;

            Vector3 bulletDirection = new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * angle),
                Mathf.Sin(Mathf.Deg2Rad * angle),
                0
            );

            GameObject bullet = Instantiate(bulletPrefabs, spawnPosition, Quaternion.identity);
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            
            if (enemyBullet == null)
            {
                enemyBullet = bullet.AddComponent<EnemyBullet>();
            }

            enemyBullet.SetMovementDirection(bulletDirection * speedDanVongTron);
        }
    }

    // Kỹ năng 3: Hồi máu
    private void HoiMau(float hpAmount)
    {
        currentHp = Mathf.Min(currentHp + hpAmount, maxHp);
        UpdateHpBar();
    }

    // Kỹ năng 4: Gọi đệ
    private void SinhMiniEnemy()
    {
        if (miniEnemy != null)
        {
            Instantiate(miniEnemy, transform.position, Quaternion.identity);
        }
    }

    // Kỹ năng 5: Dịch chuyển tới Player
    private void DichChuyen()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }

    private void ChonSkillNgauNhien()
    {
        // Sử dụng UnityEngine.Random để tránh lỗi xung đột Ambiguous với System.Random
        int randomSkill = UnityEngine.Random.Range(0, 5);
        switch (randomSkill)
        {
            case 0:
                BanDanThuong();
                break;
            case 1:
                BanDanVongTron();
                break;
            case 2:
                HoiMau(hpValue);
                break;
            case 3:
                SinhMiniEnemy();
                break;
            case 4:
                DichChuyen();
                break;
        }
    }

    private void SuDungSKill()
    {
        nextSkillTime = Time.time + skillCooldown;
        ChonSkillNgauNhien();
    }
}