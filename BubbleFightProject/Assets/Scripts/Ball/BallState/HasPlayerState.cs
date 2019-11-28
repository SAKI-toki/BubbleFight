using UnityEngine;

public partial class BallController : MonoBehaviour
{
    //プレイヤーのアニメーションを制御するクラス
    PlayerAnimationController playerAnimation = null;
    //入力を受け付けない時間
    float cantInputTime = 0.0f;

    [SerializeField, Tooltip("入力を受け付けなくする衝突力")]
    float cantInputHitPower = 50.0f;
    [SerializeField, Tooltip("入力を受け付けない最大時間")]
    float maxCantInputTime = 1.0f;
    [SerializeField, Tooltip("力1に対してどのくらい入力を受け付けなくするか(50で0.01なら0.5秒")]
    float hitPowerPercenage = 0.003f;
    //ブーストの間隔の時間を測る
    float boostIntervalTimeCount = 0.0f;
    //プレイヤーの前方と右のベクトル
    Vector3 playerForward = Vector3.zero, playerRight = Vector3.zero;

    /// <summary>
    /// プレイヤーの前方と右のベクトルをセット
    /// </summary>
    public void SetPlayerForwardRight(Vector3 forward, Vector3 right)
    {
        playerForward = forward;
        playerRight = right;
    }


    /// <summary>
    /// プレイヤーが入っているステート
    /// </summary>
    class HasPlayerState : BallStateBase
    {
        PlayerTypeStatusScriptableObject playerStatus;

        protected override void Init()
        {
            playerStatus = PlayerTypeManager.GetInstance().GetPlayerStatus(ballController.playerIndex);
            ballController.transform.GetComponent<SphereCollider>().material = playerStatus.BallPhysicalMaterial;
            ballController.thisRigidbody.mass = playerStatus.BallMass;
            var mat = ballController.transform.GetComponent<MeshRenderer>().material;
            var color = PlayerColor.GetColor(ballController.playerIndex);
            color.a = 0.8f;
            mat.color = color;
            ballController.materialFlash.SetMaterial(mat);
            color.r = color.g = color.b = 0;
            ballController.materialFlash.SetFlashColor(color);
        }

        public override BallStateBase Update()
        {
            Move();
            UpdateBoost();
            //点滅する
            if (ballController.currentHitPoint * 2 < ballController.maxHitPoint)
            {
                ballController.materialFlash.FlashStart();
                float interval = ballController.currentHitPoint / ballController.maxHitPoint;
                ballController.materialFlash.SetInterval(interval < 0.1f ? 0.1f : interval);
            }
            return this;
        }

        /// <summary>
        /// 移動
        /// </summary>
        void Move()
        {
            //入力を受け付けない
            if (ballController.cantInputTime > 0.0f)
            {
                ballController.cantInputTime -= Time.deltaTime;
                return;
            }

            var stickInput = SwitchInput.GetLeftStick(ballController.playerIndex);
            Vector3 addPower = PlayerMath.ForwardAndRightMove(stickInput, ballController.playerForward, ballController.playerRight);

            AddForceAndTorque(addPower);
            ballController.UpdateLookatDirection(addPower);

            if (stickInput.sqrMagnitude == 0)
            {
                ballController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
            }
            else
            {
                if (stickInput.magnitude > 0.9f)
                {
                    ballController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                }
                else
                {
                    ballController.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Walk);
                }
            }
        }

        /// <summary>
        /// ブーストの更新
        /// </summary>
        void UpdateBoost()
        {
            //ブースト
            ballController.boostIntervalTimeCount -= Time.deltaTime;
            if (SwitchInput.GetButtonDown(ballController.playerIndex, SwitchButton.Boost) &&
                 ballController.boostIntervalTimeCount <= 0.0f)
            {
                //入力方向に力を加える
                ballController.thisRigidbody.AddForce(ballController.lookatDir.normalized * playerStatus.BallBoostPower * ballController.thisRigidbody.mass);
                ballController.boostIntervalTimeCount = playerStatus.BallBoostInterval;
            }
        }

        /// <summary>
        /// 力と回転を加える
        /// </summary>
        void AddForceAndTorque(Vector3 addPower)
        {
            //曲がりやすくする
            var velocity = ballController.thisRigidbody.velocity;
            velocity.y = 0;
            /*
            力の向きと入力の向きの内積
            同じ向きなら1,反対向きなら-1,垂直なら0のため
            (-dot + 1) / 2をすることで同じ向きなら0,反対向きなら1になるようにする
            */
            float angle = (-Vector3.Dot(velocity.normalized, addPower.normalized) + 1) / 2;
            //加える回転力
            Vector3 addTorque = Vector3.zero;
            addTorque.x = addPower.z;
            addTorque.z = -addPower.x;
            float power = playerStatus.BallMovePower * ballController.thisRigidbody.mass * Mathf.Pow(angle + 1, playerStatus.BallEasyCurveWeight);
            //入力方向に力を加える
            ballController.thisRigidbody.AddForce(addPower * power * 0.1f);
            //入力方向に回転の力を加える
            ballController.thisRigidbody.AddTorque(addTorque * power * 0.9f);
        }

        /// <summary>
        /// 入力不可時間をセット
        /// </summary>
        public void SetCantInputTime(float time)
        {
            ballController.cantInputTime = time;
            if (ballController.cantInputTime > ballController.maxCantInputTime) ballController.cantInputTime = ballController.maxCantInputTime;
        }

        public override void OnCollisionEnter(Collision other)
        {
            switch (other.gameObject.tag)
            {
                case "Ball":
                    {
                        var otherBallController = other.gameObject.GetComponent<BallController>();
                        //入っていて、力が一定以上なら入力不可時間を与える
                        if (otherBallController.IsInPlayer() && other.relativeVelocity.sqrMagnitude > ballController.cantInputHitPower)
                        {
                            SetCantInputTime(other.relativeVelocity.sqrMagnitude * ballController.hitPowerPercenage);
                            //ダメージに応じて揺らす幅を変える
                            float damage = ballController.DamageCalculate(other.relativeVelocity.sqrMagnitude,
                                                                            otherBallController.prevVelocity.sqrMagnitude) *
                                            (otherBallController.IsInPlayer() ? 1.0f : 0.1f);

                            ballController.tpsCamera.CameraShake(ballController.cantInputTime / 2, damage / 30);
                        }
                    }
                    break;
            }
        }
    }
}
