using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }

    [SerializeField] private Sword _sword;

    private void Awake()
    {
        Instance = this;
    }
    public Sword GetActiveWeapon()
    {
        return _sword;
    }
}
