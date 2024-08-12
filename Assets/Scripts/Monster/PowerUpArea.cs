using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            var monster = other.gameObject.GetComponent<Monster>();
            if (monster.PowerLevel != GameManager.Instance.TargetPowerLevel)
            {
                monster.PowerUp();
            }
        }
    }
}
