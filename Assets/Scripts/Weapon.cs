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

        Destroy(gameObj);

        // 화살이 복제는 됬지만 삭제가 되지 않는 상황 발생
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gObjParent = collision.transform.parent.gameObject;

        if (gObjParent != null)
        {
            if (gObjParent.name == "Unit")
            {
                _myUnit    = transform.parent.GetComponent<Unit>();
                _enemyUnit = collision.GetComponent<Unit>();
                if (gameObject.layer != collision.gameObject.layer && _enemyUnit != null)
                {
                    if (gameObject.name == "Arrow")
                    {
                        _myUnit.DoDamage(collision.gameObject, _myUnit._weaponConfig._damage);
                        InstantiateArrow(gameObject, transform.parent);
                    }
                }
            }
            else if ( gObjParent.name == "BaseGroup")
            {
                _myUnit    = transform.parent.GetComponent<Unit>();
                _enemyBase = collision.GetComponent<Base>();
                if (gameObject.layer != collision.gameObject.layer && _enemyBase != null)
                {
                    if (gameObject.name == "Arrow")
                    {
                        _myUnit.DoDamage(collision.gameObject, _myUnit._weaponConfig._damage);
                        InstantiateArrow(gameObject, transform.parent);
                    }
                }
            }
        }
    }
}
