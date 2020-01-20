using UnityEngine;

/// <summary>
/// バンパー
/// </summary>
public class Bumper : MonoBehaviour
{
    public const float BouncePower = 3.0f;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            var ballBehaviour = other.gameObject.GetComponent<BallBehaviour>();
            var ballRigidbody = ballBehaviour.GetRigidbody();
            var ballVelocity = ballRigidbody.velocity;
            ballVelocity = Vector3.Scale(ballVelocity, new Vector3(BouncePower, 1, BouncePower));
            ballRigidbody.velocity = ballVelocity;
        }
    }
}
