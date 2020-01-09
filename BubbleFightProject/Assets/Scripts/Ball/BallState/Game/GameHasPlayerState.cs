using UnityEngine;

public abstract partial class BallBehaviour : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが入っているステート
    /// </summary>
    protected class GameHasPlayerState : BallStateBase
    {
        PlayerTypeStatusScriptableObject playerStatus = null;
        PlayerAnimationController playerAnimationController = null;
        Transform playerTransform = null;
        Quaternion playerRotation = Quaternion.identity;
        GameObject playerRotationObject = null;
        Vector3 initPosition = Vector3.zero;
        Quaternion initRotation = Quaternion.identity;
        ParticleSystem particleSystem = null;
        protected override void Init()
        {
            //プレイヤーの情報を格納
            playerStatus = PlayerTypeManager.GetInstance().GetPlayerStatus(ballBehaviour.playerIndex);
            ballBehaviour.transform.GetComponent<SphereCollider>().material = playerStatus.BallPhysicalMaterial;
            ballBehaviour.thisRigidbody.mass = playerStatus.BallMass;
            playerAnimationController = ballBehaviour.GetComponentInChildren<PlayerAnimationController>();
            playerTransform = playerAnimationController.transform;
            //色をプレイヤーの色に変える

            var mat = ballBehaviour.transform.GetComponent<MeshRenderer>().material;
            var color = PlayerColor.GetColor(ballBehaviour.playerIndex);
            mat.SetColor("_ColorDown", color);

            //回転しやすいように空のオブジェクトを作成
            playerRotationObject = new GameObject("PlayerRotationObject");
            playerRotationObject.transform.parent = ballBehaviour.transform;

            playerTransform.localPosition = Vector3.zero;
            initPosition = ballBehaviour.transform.position;
            initRotation = ballBehaviour.transform.rotation;

            particleSystem = ballBehaviour.GetComponentInChildren<ParticleSystem>();
        }

        public override BallStateBase Update()
        {
            Move();
            UpdateBoost();
            return this;
        }

        public override void LateUpdate()
        {
            playerTransform.rotation = playerRotation;
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
                playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Idle);
            }
            else
            {
                if (stickInput.magnitude > 0.9f)
                {
                    playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Run);
                }
                else
                {
                    playerAnimationController.AnimationSwitch(PlayerAnimationController.AnimationType.Walk);
                }
            }
            PlayerRotation(ballBehaviour.lookatDir);
        }

        /// <summary>
        /// ブーストの更新
        /// </summary>
        void UpdateBoost()
        {
            //ブースト
            ballBehaviour.boostIntervalTimeCount -= Time.deltaTime;
            if (ballBehaviour.boostIntervalTimeCount <= 0.0f)
            {
                if (!particleSystem.isPlaying) particleSystem.Play();
            }
            else
            {
                if (!particleSystem.isStopped) particleSystem.Stop();
            }

            if ((SwitchAcceleration.GetAcceleration(ballBehaviour.playerIndex).magnitude > 3.0f ||
            SwitchInput.GetButtonDown(ballBehaviour.playerIndex, SwitchButton.Boost)) &&
                 ballBehaviour.boostIntervalTimeCount <= 0.0f)
            {
                //入力方向に力を加える
                ballBehaviour.thisRigidbody.AddForce(
                    ballBehaviour.lookatDir.normalized *
                    playerStatus.BallBoostPower *
                    ballBehaviour.thisRigidbody.mass);

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
            //力
            float power = playerStatus.BallMovePower *
                ballBehaviour.thisRigidbody.mass *
                Mathf.Pow(angle + 1, playerStatus.BallEasyCurveWeight);
            //入力方向に力を加える
            ballBehaviour.thisRigidbody.AddForce(addPower * power * 0.1f);
            //入力方向に回転の力を加える
            ballBehaviour.thisRigidbody.AddTorque(addTorque * power * 0.9f);
        }

        /// <summary>
        /// プレイヤーの回転
        /// </summary>
        void PlayerRotation(Vector3 lookatDir)
        {
            if (lookatDir == Vector3.zero) return;
            //プレイヤーの回転
            playerRotationObject.transform.position = Vector3.zero;
            playerRotationObject.transform.LookAt(Vector3.Cross(playerTransform.right, Vector3.up));
            var startQ = playerRotationObject.transform.rotation;
            playerRotationObject.transform.LookAt(lookatDir);
            var endQ = playerRotationObject.transform.rotation;
            playerRotation = Quaternion.Lerp(startQ, endQ, 10 * Time.deltaTime);
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

                        PointManager.GoalCalculate(goalNumber);
                        if (PointManager.GetPoint(ballBehaviour.playerIndex) > 0) PointManager.GoalCalculate(ballBehaviour.playerIndex);

                        ballBehaviour.transform.position = initPosition;
                        ballBehaviour.transform.rotation = initRotation;
                        ballBehaviour.thisRigidbody.velocity = Vector3.zero;

                        ballBehaviour.boostIntervalTimeCount = 0.0f;
                    }
                    break;
            }
        }
    }
}
