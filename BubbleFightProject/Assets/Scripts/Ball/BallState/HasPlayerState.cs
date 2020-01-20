using UnityEngine;

public partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っているステート
    /// </summary>
    protected class HasPlayerState : BallStateBase
    {
        //ブーストの間隔の時間を測る
        float boostIntervalTimeCount = 0.0f;
        ParticleSystem particleSystem = null;
        protected override void Init()
        {
            particleSystem = ballBehaviour.GetComponentInChildren<ParticleSystem>();
        }

        public override BallStateBase Update()
        {
            Move();
            UpdateBoost();
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

            var stickInput = SwitchInput.GetStick(ballBehaviour.playerIndex);

            Vector3 addPower = new Vector3(stickInput.x, 0, stickInput.y);
            //力を加える
            AddForceAndTorque(addPower);
            //向きを更新
            ballBehaviour.UpdateLookatDirection(addPower);
            //アニメーションの更新
            if (stickInput.sqrMagnitude == 0)
            {
                ballBehaviour.playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
            }
            else
            {
                if (stickInput.magnitude > 0.9f)
                {
                    ballBehaviour.playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                }
                else
                {
                    ballBehaviour.playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Walk);
                }
            }
            ballBehaviour.PlayerRotation(ballBehaviour.lookatDir);
        }

        /// <summary>
        /// ブーストの更新
        /// </summary>
        void UpdateBoost()
        {
            //ブースト
            boostIntervalTimeCount -= Time.deltaTime;
            if (boostIntervalTimeCount <= 0.0f)
            {
                if (!particleSystem.isPlaying) particleSystem.Play();
            }
            else
            {
                if (!particleSystem.isStopped) particleSystem.Stop();
            }

            if ((SwitchAcceleration.GetAcceleration(ballBehaviour.playerIndex).magnitude > 3.0f ||
            SwitchInput.GetButtonDown(ballBehaviour.playerIndex, SwitchButton.Boost)) &&
                 boostIntervalTimeCount <= 0.0f)
            {
                //入力方向に力を加える
                ballBehaviour.thisRigidbody.AddForce(
                    ballBehaviour.lookatDir.normalized *
                    ballBehaviour.boostPower *
                    ballBehaviour.thisRigidbody.mass);

                boostIntervalTimeCount = ballBehaviour.boostInterval;
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
            //力
            float power = ballBehaviour.movePower *
                ballBehaviour.thisRigidbody.mass *
                Mathf.Pow(angle + 1, ballBehaviour.easyCurveWeight);
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
                        }
                    }
                    break;
                case "Goal":
                    {
                        //入れたゴールの番号を取得
                        int goalNumber = other.gameObject.GetComponent<GoalController>().GetGoalNumber();

                        if (!PlayerJoinManager.IsJoin(goalNumber) || PointManager.GetPoint(goalNumber) <= 0) return;
                        if (goalNumber == ballBehaviour.playerIndex)
                        {
                            PointManager.OwnGoalCalculate(goalNumber);
                        }

                        ballStateManager.TranslationState(new RespawnState());
                    }
                    break;
            }
        }
    }
}
