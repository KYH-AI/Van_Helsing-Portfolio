using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupExp : MonoBehaviour
{
    private const int expValue = 10;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Player"))
        {
            GameManager.GetInstacne().PlayerTakeExp(expValue);
            Destroy(this.gameObject);
        }
    }
}

