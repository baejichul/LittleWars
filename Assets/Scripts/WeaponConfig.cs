using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponConfig
{
    public float _speed = 1.0f;
    public WEAPON _weapon = WEAPON.SWORD;
    public int _damage = 0;
    public Vector3 _defaultWeaponPos;
}
