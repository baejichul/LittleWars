using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{   
    protected Unit _myUnit;
    protected Unit _enemyUnit;
    protected Base _enemyBase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateArrow(GameObject gameObj, Transform parentTf)
    {
        // 화살 복제
        GameObject gObj = Instantiate(gameObj, parentTf);
        gObj.transform.position = parentTf.transform.Find("ArrowDefaultPos").transform.position;
        gObj.name = "Arrow";
        gObj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _myUnit = transform.parent.GetComponent<Unit>();
        if (collision.gameObject.transform.parent.gameObject.name.Equals("Unit"))
        {
            _enemyUnit = collision.gameObject.GetComponent<Unit>();
            if (gameObject.layer != collision.gameObject.layer && _enemyUnit != null)
            {
                if ( gameObject.name == "Arrow")
                {   
                    _myUnit.DoDamage(collision.gameObject, _myUnit._weaponConfig._damage);

                    InstantiateArrow(gameObject, transform.parent);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            _enemyBase = collision.gameObject.GetComponent<Base>();
            if (gameObject.layer != collision.gameObject.layer && _enemyBase != null)
            {
                if ( gameObject.name == "Arrow")
                {
                    // Debug.LogFormat("collision.gameObject.layer : {0} , collision.gameObject : {1}, _myUnit._weaponConfig._damage : {2}", collision.gameObject.layer, collision.gameObject.name, _myUnit._weaponConfig._damage);
                    _myUnit.DoDamage(collision.gameObject, _myUnit._weaponConfig._damage);

                    InstantiateArrow(gameObject, transform.parent);
                    Destroy(gameObject);
                }
            }
        }
    }
}
