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
    /// 데미지, 적 좌표, 적 방향 정보 데이터 받음
    /// </summary>
    /// <param name="dmg">MagicGrenade 데미지 값</param>
    /// <param name="target">적 위치(추락 도중 특정 위치에서 유폭 효과를 나타낼려고)</param>
    /// <param name="enemyDir">적 방향 (MagicGrenade가 추락할 방향)</param>
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
            Vector2 offset = transform.position - targetPosition;    //  MagicGrenade 좌표 - 적 좌표 거리 값을 구함
            float sqrOffset = offset.sqrMagnitude;
            if (sqrOffset <= 0.25f)                                  // 거리 값이 0.25 보다 작다면
            {
                
                CreateMaigcGrenadePlane();                           // MagicGrendae 장판 생성
                Destroy(gameObject);                                 // MagicGrendae오브젝트 삭제
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
