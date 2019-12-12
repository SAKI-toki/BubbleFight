using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    Animator animator = null;

    Vector3 oldPos;

    private void Start()
    {
        oldPos = transform.position;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        float x = 0, z = 0;

        float speed = 1.0f;
        speed /= 100;

        if (Input.GetKey(KeyCode.W)) { z += speed; }
        if (Input.GetKey(KeyCode.S)) { z -= speed; }
        if (Input.GetKey(KeyCode.D)) { x += speed; }
        if (Input.GetKey(KeyCode.A)) { x -= speed; }

        Vector3 dir = transform.position - oldPos;

        oldPos = transform.position;

        // 動いたとき
        if (x != 0 || z != 0)
        {
            animator.SetInteger("State", 2);
            transform.position = new Vector3(
                transform.position.x + x,
                transform.position.y,
                transform.position.z + z);
            transform.rotation = Quaternion.LookRotation(dir);
        }
        // 止まっているとき
        else
        {
            animator.SetInteger("State", 0);
        }
    }
}
