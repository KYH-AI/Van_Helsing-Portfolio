using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    #region �̵����� ����
    private Rigidbody2D playerRigidbody;
    private const float moveSpeed = 3.5f;
    #endregion

    #region PlayerHitBox ����
    private LayerMask enemyLayerMask = 1 << 10;
    private const float hitDelay = 0.25f;
    private float delayTime = 0;
    private Collider2D[] playerHitBox;
    #endregion

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        currentHp =100;
    }


    private void Update()
    {
        CheckPlayerHitBox();
    }
    private void CheckPlayerHitBox()
    {
        playerHitBox = Physics2D.OverlapCircleAll(this.transform.position, 0.7f, enemyLayerMask);
        if (playerHitBox.Length > 0) //�÷��̾� ���ؿ��� ������ 0.7�ݰ濡 ���� �ִٴ� ��
        {
            delayTime += Time.deltaTime;
            if (delayTime >= hitDelay)
            {
                foreach (Collider2D enemyArray in playerHitBox)
                {
                    TakeDamage(enemyArray.gameObject.GetComponent<Enemy>().getDamage);
                    UiManager.GetInstacne().UpdateHpBar(currentHp);
                }
                delayTime = 0.00f;
            }
        }
    }

    public Transform GetPlayerObjectPosition()
    {
        return this.transform;
    }

    public bool Move(Vector2 moveVector)
    {
        playerRigidbody.velocity = moveVector * moveSpeed;
        return playerRigidbody.velocity != Vector2.zero;  // ĳ���Ͱ� �����̸� true, �ƴϸ� false ��ȯ
    }

    private void OnDestroy()
    {
        UiManager.GetInstacne().GameOverPanel();
    }
}
