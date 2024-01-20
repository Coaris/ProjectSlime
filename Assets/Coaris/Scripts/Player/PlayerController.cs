using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Player {
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
                PlayerActionsEnd _playerActionEnd;

                Vector2 _frameVelocity;//每帧，在引擎执行运动前的速度计算。在每帧执行运动前，将此值赋给 Rigidbody2D.velocity
                bool _cachedQueriesStartInColliders;//存储项目设置，在碰撞体检测时需要临时改为false

                float _time;//游戏运行时间

                void Awake() {
                        _rb = GetComponent<Rigidbody2D>();
                        _col = GetComponent<BoxCollider2D>();
                        _playerActionEnd = transform.GetChild(0).GetChild(0).GetComponent<PlayerActionsEnd>();

                        _cachedQueriesStartInColliders = Physics2D.queriesStartInColliders;

                        _data.airFloat = false;
                        _data.airJump = false;
                        _data.dash = false;
                }
                void OnEnable() {
                        _playerActionEnd.OnEnglobed += OnEnglobed;
                        _playerActionEnd.OnGetAbility += OnGetAbility;
                }
                void OnDisable() {
                        _playerActionEnd.OnEnglobed -= OnEnglobed;
                        _playerActionEnd.OnGetAbility -= OnGetAbility;
                }
                void Update() {
                        _time += Time.deltaTime;
                }

                void FixedUpdate() {
                        CheckCollisions();

                        HandleDash();
                        HandleJump();
                        HandleHorizontal();
                        HandleGravity();

                        ApplyMovement();
                }
                void OnTriggerEnter2D(Collider2D collision) {
                        if (collision.GetComponent<NPC_Base>() != null) {
                                npcTouching = collision.gameObject;
                        }
                }
                void OnTriggerExit2D(Collider2D collision) {
                        if (collision.GetComponent<NPC_Base>() != null) {
                                npcTouching = null;
                        }
                }

                #region 碰撞检测 Collision
                [HideInInspector] public bool _grounded;//当玩家在地面上时，为true。当玩家在空中时，为false。

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
                                _leftAirJump = _data.maxAirJump;
                                _endJumpEarly = false;
                                _coyoteUsable = true;
                                OnGrounded?.Invoke();
                        }
                        //离开地面
                        else if (_grounded && !_groundHit) {
                                _grounded = false;
                                _timeLeftGround = _time;
                        }

                        Physics2D.queriesStartInColliders = _cachedQueriesStartInColliders;
                }
                #endregion

                #region 跳跃 Jump

                public event Action OnJumped;//当跳跃成功触发时，触发该事件，让其他脚本收到跳跃的通知，例如动画脚本。
                public event Action OnFalling;//当角色在空中下降时，触发该事件，让其他脚本收到下落的通知，例如动画脚本。
                public event Action OnGrounded;//当角色着陆时，触发该事件，让其他脚本收到着陆的通知，例如动画脚本。

                //Basic Jump
                bool _jumpPressed = false;//跳跃按键是否被按下触发。当玩家按下跳跃键的瞬间为true。当角色执行一次起跳后，立刻回归false。
                //EndJumpEarly
                bool _endJumpEarly;//控制小幅跳跃。当角色在跳跃上升阶段时，提前松开跳跃键，此值为true。
                bool _jumpHolding = false;//跳跃按键是否持续被按住。当玩家按下跳跃键瞬间为true。当玩家松开跳跃键瞬间回归false。
                //BufferJump
                float _timeJumpPressed;//玩家按下跳跃键的游戏时间，用于计算 BufferJump是否触发。
                bool _hasBufferJump => _grounded && _time < _timeJumpPressed + _data.bufferJumpTime;//是否触发了BufferJump。触发时为true。
                //CoyoteJump
                float _timeLeftGround = float.MinValue;//角色离开平台的游戏时间，用于CoyoteJump触发的计算。
                bool _coyoteUsable = false;//土狼跳是否可用。当玩家接触到地面后为true。当玩家执行一次跳跃后为false。
                bool _hasCoyoteJump => _coyoteUsable && !_grounded && _time < _timeLeftGround + _data.coyoteTime;
                //AirJump
                int _leftAirJump = 0;

                //跳跃预处理，包括土狼时间等等
                void HandleJump() {
                        if (!_endJumpEarly && !_grounded && !_jumpHolding && _rb.velocity.y > 0) _endJumpEarly = true;//在满足这些条件时，提前松开跳跃键，触发矮跳

                        if (!_grounded && _coyoteUsable && _time > _timeLeftGround + _data.coyoteTime) _coyoteUsable = false;//土狼跳
                        if (!_jumpPressed && !_hasBufferJump) return;
                        //在地面上，或者触发土狼跳时
                        if (_grounded || _hasCoyoteJump) {
                                ExecuteJump();
                        }
                        //具备空跳条件时
                        else if (_data.airJump && _leftAirJump > 0) {
                                _leftAirJump -= 1;
                                ExecuteJump();
                        }
                        _jumpPressed = false;
                }
                //执行跳跃
                void ExecuteJump() {
                        _coyoteUsable = false;
                        _endJumpEarly = false;
                        _frameVelocity.y = _data.jumpPower;
                        OnJumped?.Invoke();
                }

                //InputSystem 跳跃输入
                public void OnJump(InputAction.CallbackContext context) {
                        if (_isDashing || _isEating) return;
                        //按下跳跃键
                        if (context.phase == InputActionPhase.Started) {
                                _endJumpEarly = false;
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

                #region 冲刺 Dash

                bool _isDashing = false;
                float _timeDashPressed = float.MinValue;
                bool _isDashEnd => _isDashing && _time > _timeDashPressed + _data.dashTime;
                bool _dashUsable = true;

                void HandleDash() {
                        if (_grounded) _dashUsable = true;
                        if (_isDashing && !_isDashEnd) ExecuteDash();
                        else _isDashing = false;
                }

                void ExecuteDash() {
                        _frameVelocity.x = _data.dashSpeed * (_isFacingLeft ? -1 : 1);
                }
                //InputSystem 冲刺输入
                public void OnDash(InputAction.CallbackContext context) {
                        if (_isEating) return;
                        if (_data.dash && _dashUsable && context.phase == InputActionPhase.Started) {
                                _isDashing = true;
                                _dashUsable = false;
                                _frameVelocity.x = 0;
                                _timeDashPressed = _time;
                        }
                }
                #endregion

                #region 重力 Gravity

                float _inAirGravity;//重力加速度

                //处理重力
                void HandleGravity() {
                        if (_isDashing) {
                                _frameVelocity.y = 0;
                                return;
                        }
                        if (_grounded && _frameVelocity.y <= 0) {
                                _frameVelocity.y = _data.groundForce;
                        }
                        else {
                                _inAirGravity = _data.fallAcceleration;//重力
                                if (_endJumpEarly && _frameVelocity.y > 0) _inAirGravity *= _data.jumpEndEarlyGravityMuldifier;//在跳跃上升阶段时提前松开跳跃键，重力将变为原来的n倍。
                                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -(_data.airFloat ? _data.maxFloatFallSpeed : _data.maxFallSpeed), _inAirGravity * Time.fixedDeltaTime);//拥有滑翔能力时会有不同的降落速度
                                if (!_grounded && !_coyoteUsable && _frameVelocity.y < 0) OnFalling?.Invoke();//当玩家在空中时，且土狼跳不可用时，且垂直速度小于零时，触发下落事件
                        }
                }
                #endregion

                #region 水平运动 Horizontal

                [HideInInspector] public Vector2 _frameMoveInput;
                [HideInInspector] public bool _isFacingLeft;
                float _deceleration;

                void HandleHorizontal() {
                        //if (_isDashing || _isEating) return;
                        //当玩家没有水平输入时
                        if (_frameMoveInput.x == 0) {
                                _deceleration = _grounded ? _data.groundDeceleration : _data.airDeceleration;
                                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, _deceleration * Time.fixedDeltaTime);
                        }
                        //当玩家有水平输入时
                        else {
                                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameMoveInput.x * _data.maxSpeed, _data.acceleration * Time.fixedDeltaTime);
                                _isFacingLeft = _frameVelocity.x < 0;
                        }
                }
                //InputSystem 水平输入
                public void OnMove(InputAction.CallbackContext context) {
                        if (_isDashing || _isEating) return;
                        _frameMoveInput = context.ReadValue<Vector2>();
                }
                #endregion

                #region 吞噬 Englobe
                public event Action OnEnglobing;
                GameObject npcTouching;
                bool _isEating;

                public GameObject GetTouchingNPC() {
                        return npcTouching;
                }

                public void OnEnglobe(InputAction.CallbackContext context) {
                        if (_isDashing || npcTouching == null) return;
                        if (_grounded && context.phase == InputActionPhase.Started) {
                                _isEating = true;
                                OnEnglobing?.Invoke();
                        }
                }
                void OnEnglobed() {
                        _isEating = false;
                }
                void OnGetAbility() {
                        if (npcTouching != null) {
                                foreach (Ability i in npcTouching.GetComponent<NPC_Base>().GetAbilityList()) {
                                        switch (i) {
                                                case Ability.AirJump:
                                                        _data.airJump = true;
                                                        break;
                                                case Ability.AirFloat:
                                                        _data.airFloat = true;
                                                        break;
                                                case Ability.Dash:
                                                        _data.dash = true;
                                                        break;
                                        }
                                }
                        }
                }
                #endregion

                //将每帧的运动速度的预计算执行
                void ApplyMovement() => _rb.velocity = _frameVelocity;
        }
}