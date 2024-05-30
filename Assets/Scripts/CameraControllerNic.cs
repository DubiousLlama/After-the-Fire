using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerNic : MonoBehaviour
{
    //Variables
    [SerializeField]
    private Transform z_Player;
    private float z_BoundX = 0.1f;
    private float z_BoundY = 0.2f;

    private void LateUpdate() {
        FollowPlayer();
    }

    private void FollowPlayer() {
        Vector2 moveDirection = Vector2.zero;

        float deltaX = z_Player.position.x - transform.position.x;
        float deltaY = z_Player.position.y - transform.position.y;

        if(deltaX > z_BoundX || deltaX < -z_BoundX) {
            if(z_Player.position.x > transform.position.x) {
                moveDirection.x = deltaX - z_BoundX;
            } else { 
                moveDirection.x = deltaX + z_BoundX;
            }
        }

        if(deltaY > z_BoundY || deltaY < -z_BoundY) {
            if(z_Player.position.y > transform.position.y) {
                moveDirection.y = deltaY - z_BoundY;
            } else { 
                moveDirection.y = deltaY + z_BoundY;
            }
        }

        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0);
    }
}
