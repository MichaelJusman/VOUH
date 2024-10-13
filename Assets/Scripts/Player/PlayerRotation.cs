using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : GameBehaviour
{
    [SerializeField] private Vector3Variable _playerPosition;
    private Vector3 pTransform;
    private void Update()
    {
        transform.position = _playerPosition;

        HandleTorsoRotation();
    }
    void HandleTorsoRotation()
    {
        // Get mouse position in the world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            // Calculate direction from torso to mouse point
            Vector3 direction = point - transform.position;
            direction.y = 0; // Lock rotation to the horizontal plane

            // Rotate the torso to face the mouse
            if (direction.magnitude > 0.1f)  // Prevent jittering when close to the mouse
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
