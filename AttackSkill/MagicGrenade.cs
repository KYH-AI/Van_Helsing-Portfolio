using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicGrenade : MonoBehaviour
{
    [SerializeField] GameObject grenadePlane;
    [SerializeField] Rigidbody2D magicGrenadeRigidBody;
    private Vector3 targetPosition;
    private int damage;

    /// <summary>
    /// ������, �� ��ǥ, �� ���� ���� ������ ����
    /// </summary>
    /// <param name="dmg">MagicGrenade ������ ��</param>
    /// <param name="target">�� ��ġ(�߶� ���� Ư�� ��ġ���� ���� ȿ���� ��Ÿ������)</param>
    /// <param name="enemyDir">�� ���� (MagicGrenade�� �߶��� ����)</param>
    public void SetMagicGrenadeData(int dmg, Vector2 enemyDir, Transform target)
    {
        damage = dmg;
        targetPosition = target.position;
        magicGrenadeRigidBody.velocity = enemyDir;
    }

     void Update()
    {
        transform.Rotate(0f, 0f, 150f * Time.deltaTime);
        CheckingTargetDistance();
    }

    private void CheckingTargetDistance()
    {
        if (targetPosition != null)
        {
            Vector2 offset = transform.position - targetPosition;    //  MagicGrenade ��ǥ - �� ��ǥ �Ÿ� ���� ����
            float sqrOffset = offset.sqrMagnitude;
            if (sqrOffset <= 0.25f)                                  // �Ÿ� ���� 0.25 ���� �۴ٸ�
            {
                
                CreateMaigcGrenadePlane();                           // MagicGrendae ���� ����
                Destroy(gameObject);                                 // MagicGrendae������Ʈ ����
            }
        }
    }

    private void CreateMaigcGrenadePlane()
    {
       GameObject maigcGrenadePlaneObj = Instantiate(grenadePlane, transform.position, Quaternion.identity);
       maigcGrenadePlaneObj.GetComponent<MagicGrenadePlane>().SetPlaneData(damage);
       SoundManager.GetInstacne().PlaySFXSound("MagicGrenadePlane", 0.2f);
    }
}
