using UnityEngine;

public class AddPointUI : MonoBehaviour
{
    [SerializeField, Tooltip("時間")]
    float lifetime = 0.0f;
    float lifetimeCount = 0.0f;

    void Update()
    {
        lifetimeCount += Time.deltaTime;
        if (lifetimeCount > lifetime) Destroy(gameObject);
    }
}