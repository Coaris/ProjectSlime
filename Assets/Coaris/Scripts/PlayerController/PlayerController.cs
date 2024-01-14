using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoarisPlatformer2D {
        /// <summary>
        /// Coaris的2D平台跳跃，玩家角色控制器模板类
        /// 14/Jan/2024 开始编写
        ///             角色的重力、左右移动输入、跳跃输入
        /// </summary>

        //本脚本需要 Rigidbody2D 以及至少任一一种 Collider2D 组件
        [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

        public class PlayerController : MonoBehaviour {
                [SerializeField] PlayerData _data;//玩家的预设数据

                Rigidbody2D _rb;
                Vector2 _frameVelocity;//每帧，在引擎执行运动前的速度计算。在每帧执行运动前，将此值赋给 Rigidbody2D.velocity

                void Awake() {
                        _rb = GetComponent<Rigidbody2D>();
                }

                void FixedUpdate() {
                        HandleJump();
                        HandleGravity();

                        ApplyMovement();
                }

                #region 重力&跳跃 Gravity&Jump
                
                float _inAirGravity;//重力加速度
                
                bool _jumpPressed = false;//跳跃按键是否被按下触发
                bool _jumpHolding = false;//跳跃按键是否持续被按住

                //处理重力
                void HandleGravity() {
                        _inAirGravity = _data.fallAcceleration;
                        _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_data.maxFallSpeed, _inAirGravity * Time.fixedDeltaTime);
                }
                //跳跃预处理，包括土狼时间等等
                void HandleJump() {


                        if (!_jumpPressed) return;
                        ExecuteJump();
                        _jumpPressed = false;
                }
                //执行跳跃
                void ExecuteJump() {
                        _frameVelocity.y = _data.jumpPower;
                }
                //InputSystem 跳跃输入
                public void OnJump(InputAction.CallbackContext context) {
                        //按下跳跃键
                        if (context.phase == InputActionPhase.Started) {
                                _jumpPressed = true;
                                _jumpHolding = true;
                        }
                        //松开跳跃键
                        if (context.phase == InputActionPhase.Canceled) {
                                _jumpHolding = false;
                        }
                }
                #endregion

                //将每帧的运动速度的预计算执行
                void ApplyMovement() => _rb.velocity = _frameVelocity;
        }
}