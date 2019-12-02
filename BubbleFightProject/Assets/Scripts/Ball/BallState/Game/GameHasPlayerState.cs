using UnityEngine;

public abstract partial class BallBehaviour : MonoBehaviour
{


    /// <summary>
    /// プレイヤーが入っているステート
    /// </summary>
    protected class GameHasPlayerState : BallStateBase
    {
        PlayerTypeStatusScriptableObject playerStatus;

        protected override void Init()
        {
            playerStatus = PlayerTypeManager.GetInstance().GetPlayerStatus(ballBehaviour.playerIndex);
            ballBehaviour.transform.GetComponent<SphereCollider>().material = playerStatus.BallPhysicalMaterial;
            ballBehaviour.thisRigidbody.mass = playerStatus.BallMass;
            var mat = ballBehaviour.transform.GetComponent<MeshRenderer>().material;
            var color = PlayerColor.GetColor(ballBehaviour.playerIndex);
            color.a = 0.8f;
            mat.color = color;
            ballBehaviour.materialFlash.SetMaterial(mat);
            color.r = color.g = color.b = 0;
            ballBehaviour.materialFlash.SetFlashColor(color);
        }

        public override BallStateBase Update()
        {
            Move();
            UpdateBoost();
            //点滅する
            if (ballBehaviour.currentHitPoint * 2 < ballBehaviour.ballScriptableObject.MaxHitPoint)
            {
                ballBehaviour.materialFlash.FlashStart();
                float interval = ballBehaviour.currentHitPoint / ballBehaviour.ballScriptableObject.MaxHitPoint;
                ballBehaviour.materialFlash.SetInterval(interval < 0.1f ? 0.1f : interval);
            }
            return this;
        }

        /// <summary>
        /// 移動
        /// </summary>
        void Move()
        {
            //入力を受け付けない
            if (ballBehaviour.cantInputTime > 0.0f)
            {
                ballBehaviour.cantInputTime -= Time.deltaTime;
                return;
            }

            var stickInput = SwitchInput.GetLeftStick(ballBehaviour.playerIndex);
            Vector3 addPower = PlayerMath.ForwardAndRightMove(stickInput, ballBehaviour.playerForward, ballBehaviour.playerRight);

            AddForceAndTorque(addPower);
            ballBehaviour.UpdateLookatDirection(addPower);

            if (stickInput.sqrMagnitude == 0)
            {
                ballBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
            }
            else
            {
                if (stickInput.magnitude > 0.9f)
                {
                    ballBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                }
                else
                {
                    ballBehaviour.playerAnimation.AnimationSwitch(PlayerAnimationController.AnimationType.Walk);
                }
            }
        }

        /// <summary>
        /// ブーストの更新
        /// </summary>
        void UpdateBoost()
        {
            //ブースト
            ballBehaviour.boostIntervalTimeCount -= Time.deltaTime;
            if (SwitchInput.GetButtonDown(ballBehaviour.playerIndex, SwitchButton.Boost) &&
                 ballBehaviour.boostIntervalTimeCount <= 0.0f)
            {
                //入力方向に力を加える
                ballBehaviour.thisRigidbody.AddForce(ballBehaviour.lookatDir.normalized * playerStatus.BallBoostPower * ballBehaviour.thisRigidbody.mass);
                ballBehaviour.boostIntervalTimeCount = playerStatus.BallBoostInterval;
            }
        }

        /// <summary>
        /// 力と回転を加える
        /// </summary>
        void AddForceAndTorque(Vector3 addPower)
        {
            //曲がりやすくする
            var velocity = ballBehaviour.thisRigidbody.velocity;
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
            float power = playerStatus.BallMovePower * ballBehaviour.thisRigidbody.mass * Mathf.Pow(angle + 1, playerStatus.BallEasyCurveWeight);
            //入力方向に力を加える
            ballBehaviour.thisRigidbody.AddForce(addPower * power * 0.1f);
            //入力方向に回転の力を加える
            ballBehaviour.thisRigidbody.AddTorque(addTorque * power * 0.9f);
        }

        public override void OnCollisionEnter(Collision other)
        {
            switch (other.gameObject.tag)
            {
                case "Ball":
                    {
                        var otherballBehaviour = other.gameObject.GetComponent<BallBehaviour>();
                        //入っていて、力が一定以上なら入力不可時間を与える
                        if (otherballBehaviour.IsInPlayer() &&
                            other.relativeVelocity.sqrMagnitude > ballBehaviour.ballScriptableObject.CantInputHitPower)
                        {
                            ballBehaviour.SetCantInputTime(
                                other.relativeVelocity.sqrMagnitude * ballBehaviour.ballScriptableObject.HitPowerPercenage);
                            //ダメージに応じて揺らす幅を変える
                            float damage = ballBehaviour.DamageCalculate(other.relativeVelocity.sqrMagnitude,
                                                                            otherballBehaviour.prevVelocity.sqrMagnitude) *
                                            (otherballBehaviour.IsInPlayer() ? 1.0f : 0.1f);

                            ballBehaviour.tpsCamera.CameraShake(ballBehaviour.cantInputTime / 2, damage / 30);
                        }
                    }
                    break;
            }
        }
    }
}
