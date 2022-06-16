using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{   
    protected Unit _myUnit;
    protected Unit _enemyUnit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _myUnit = transform.parent.GetComponent<Unit>();
        _enemyUnit = collision.gameObject.GetComponent<Unit>();

        // 적군인지 확인(무기제외)
        if ( gameObject.layer != collision.gameObject.layer && _enemyUnit != null )
        {
            // if ( gameObject.name == "Arrow")
            if (_myUnit._unitConfig._unitClass == UNIT_CLASS.ARCHER)
            {
                // Debug.LogFormat("collision.gameObject.layer : {0} , collision.gameObject : {1}, _myUnit._weaponConfig._damage : {2}", collision.gameObject.layer, collision.gameObject.name, _myUnit._weaponConfig._damage);
                _myUnit.DoDamage(collision.gameObject, _myUnit._weaponConfig._damage);

                // 화살 복제
                GameObject gObj = Instantiate(gameObject, transform.parent);
                gObj.transform.position = _myUnit._weaponConfig._defaultWeaponPos;
                gObj.name = "Arrow";
                gObj.SetActive(false);

                Destroy(gameObject);
            }
        }
    }
}
