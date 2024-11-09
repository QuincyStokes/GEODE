using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    IEnumerator Attack(Vector3 position);
    bool IsAttacking{
        get;
        set;
    }

    float Damage {
        get;
        set;
    }
}
