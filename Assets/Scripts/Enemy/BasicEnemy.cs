using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    protected override void EnemyBehaviour()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
