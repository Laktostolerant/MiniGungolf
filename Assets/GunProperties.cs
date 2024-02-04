using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProperties : MonoBehaviour
{
    public WeaponAbility ability;
    [SerializeField] public GunStats properties;

    private void Start()
    {
        ability = GetComponent<WeaponAbility>();
    }
}
