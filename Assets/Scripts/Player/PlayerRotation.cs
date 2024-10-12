using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : GameBehaviour
{

    private void Update()
    {
        Rotation();
    }
    void Rotation()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, angle, 0), 360);
    }
}
