using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGachaBallController : MonoBehaviour
{
    Vector3 dir = Vector3.zero;

    float destroyTime = 1.0f;
    float survivalTime = 0.0f;

    private void Update()
    {
        Move();
        DestroyCount();
    }

    public void Init(Vector3 v)
    {
        dir = v;
    }

    void Move()
    {
        transform.position = new Vector3(
            transform.position.x + dir.x,
            transform.position.y + dir.y,
            transform.position.z + dir.z);
    }

    void DestroyCount()
    {
        survivalTime += Time.deltaTime;

        if (survivalTime > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
