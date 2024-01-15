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
                BoxCollider2D _col;

                Vector2 _frameVelocity;//每帧，在引擎执行运动前的速度计算。在每帧执行运动前，将此值赋给 Rigidbody2D.velocity
                bool _cachedQueriesStartInColliders;//存储项目设置，在碰撞体检测时需要临时改为false

                float _time;//游戏运行时间

                void Awake() {
                        _rb = GetComponent<Rigidbody2D>();
                        _col = GetComponent<BoxCollider2D>();

                        _cachedQueriesStartInColliders = Physics2D.queriesStartInColliders;
                }
                void Update() {
                        _time += Time.deltaTime;
                }

                void FixedUpdate() {
                        CheckCollisions();

                        HandleJump();
                        HandleHorizontal();
                        HandleGravity();

                        ApplyMovement();
                }

                #region 碰撞检测 Collision
                bool _grounded;//当玩家在地面上时，为true。当玩家在空中时，为false。

                bool _groundHit;
                bool _ceilingHit;
                void CheckCollisions() {
                        Physics2D.queriesStartInColliders = false;

                        //地面和天花板碰撞检测
                        _groundHit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, _data.groundCheckDistance, _data.groundLayerMask);
                        _ceilingHit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.up, _data.groundCheckDistance, _data.groundLayerMask);

                        //接触天花板
                        if (_ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

                        //接触地面
                        if (!_grounded && _groundHit) {
                                _grounded = true;
                                _endJumpEarly = false;
                        }
                        //离开地面
                        else if (_grounded && !_groundHit) {
                                _grounded = false;
                        }

                        Physics2D.queriesStartInColliders = _cachedQueriesStartInColliders;
                }
                #endregion

                #region 重力&跳跃 Gravity&Jump

                float _inAirGravity;//重力加速度
                bool _endJumpEarly;//控制小幅跳跃。当角色在跳跃上升阶段时，提前松开跳跃键，此值为true。

                bool _jumpPressed = false;//跳跃按键是否被按下触发。当玩家按下跳跃键的瞬间为true。当角色执行一次起跳后，立刻回归false。
                float _timeJumpPressed;//玩家按下跳跃键的游戏时间，用于计算 BufferJump 和 CoyoteJump 是否触发。
                bool _jumpHolding = false;//跳跃按键是否持续被按住。当玩家按下跳跃键瞬间为true。当玩家松开跳跃键瞬间回归false。

                bool _hasBufferJump => _grounded && _time < _timeJumpPressed + _data.bufferJumpTime;//是否触发了BufferJump。触发时为true。

                //跳跃预处理，包括土狼时间等等
                void HandleJump() {
                        if (!_endJumpEarly && !_grounded && !_jumpHolding && _rb.velocity.y > 0) _endJumpEarly = true;//在满足这些条件时，提前松开跳跃键，触发矮跳

                        if (!_jumpPressed || !_hasBufferJump) return;
                        if (_grounded) ExecuteJump();
                        _jumpPressed = false;
                }
                //执行跳跃
                void ExecuteJump() {
                        _endJumpEarly = false;
                        _frameVelocity.y = _data.jumpPower;
                }

                //处理重力
                void HandleGravity() {
                        if (_grounded && _frameVelocity.y <= 0) {
                                _frameVelocity.y = _data.groundForce;
                        }
                        else {
                                _inAirGravity = _data.fallAcceleration;//重力
                                if (_endJumpEarly && _frameVelocity.y > 0) _inAirGravity *= _data.jumpEndEarlyGravityMuldifier;//在跳跃上升阶段时提前松开跳跃键，重力将变为原来的n倍。
                                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_data.maxFallSpeed, _inAirGravity * Time.fixedDeltaTime);
                        }
                }

                //InputSystem 跳跃输入
                public void OnJump(InputAction.CallbackContext context) {
                        //按下跳跃键
                        if (context.phase == InputActionPhase.Started) {
                                _jumpPressed = true;
                                _jumpHolding = true;
                                _timeJumpPressed = _time;
                        }
                        //松开跳跃键
                        if (context.phase == InputActionPhase.Canceled) {
                                _jumpHolding = false;
                        }
                }
                #endregion

                #region 水平运动 Horizontal

                
                Vector2 _frameMoveInput;
                float _deceleration;

                void HandleHorizontal() {
                        //当玩家没有水平输入时
                        if (_frameMoveInput.x == 0) {
                                _deceleration = _grounded ? _data.groundDeceleration : _data.airDeceleration;
                                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, _deceleration * Time.fixedDeltaTime);
                        }
                        //当玩家有水平输入时
                        else {
                                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameMoveInput.x * _data.maxSpeed, _data.acceleration * Time.fixedDeltaTime);
                        }
                }
                //InputSystem 水平输入
                public void OnMove(InputAction.CallbackContext context) {
                        _frameMoveInput = context.ReadValue<Vector2>();
                }



                #endregion

                //将每帧的运动速度的预计算执行
                void ApplyMovement() => _rb.velocity = _frameVelocity;
        }
}