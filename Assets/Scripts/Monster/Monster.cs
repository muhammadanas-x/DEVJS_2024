using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    #region Parameters
    public int PowerLevel = 0;

    [SerializeField] private GameObject PowerUpArea;
    #endregion

    private MonsterController controller;

    public void Awake()
    {
        controller = GetComponent<MonsterController>();
    }

    public void OnPuzzleComplete()
    {
        controller.SetDestination(PowerUpArea.transform.position);
    }

    public void PowerUp()
    {
        this.PowerLevel = GameManager.Instance.TargetPowerLevel;
    }
}
