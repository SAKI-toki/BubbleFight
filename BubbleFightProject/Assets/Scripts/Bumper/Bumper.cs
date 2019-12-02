using UnityEngine;

/// <summary>
/// バンパー
/// </summary>
public class Bumper : MonoBehaviour
{
    [SerializeField, Tooltip("跳ね返りの強さ")]
    float bouncePower = 2.0f;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            var ballBehaviour = other.gameObject.GetComponent<BallBehaviour>();
            var ballRigidbody = ballBehaviour.GetRigidbody();
            var ballVelocity = ballRigidbody.velocity;
            ballVelocity = Vector3.Scale(ballVelocity, new Vector3(bouncePower, 1, bouncePower));
            ballRigidbody.velocity = ballVelocity;
        }
    }
}
