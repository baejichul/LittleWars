using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitConfig : MonoBehaviour
{
    public float _speed{ get; set; } = 1.0f;
    public float _attackRange { get; set; } = 0.7f; // 0.4f 2.5f 3.5f
    public int _hp { get; set; } = 0;
    public int _maxHp { get; set; } = 1000;
    public int _power { get; set; } = 1;
    public GameObject[] _enemyObj = null;

    public TEAM _team = TEAM.BLUE;
    public UNIT_CLASS _unitClass = UNIT_CLASS.SWORD;
}
