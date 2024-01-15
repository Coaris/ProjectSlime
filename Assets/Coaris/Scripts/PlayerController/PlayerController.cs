using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoarisPlatformer2D {
        /// <summary>
        /// Coaris��2Dƽ̨��Ծ����ҽ�ɫ������ģ����
        /// 14/Jan/2024 ��ʼ��д
        ///             ��ɫ�������������ƶ����롢��Ծ����
        /// </summary>

        //���ű���Ҫ Rigidbody2D �Լ�������һһ�� Collider2D ���
        [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

        public class PlayerController : MonoBehaviour {
                [SerializeField] PlayerData _data;//��ҵ�Ԥ������

                Rigidbody2D _rb;
                BoxCollider2D _col;

                Vector2 _frameVelocity;//ÿ֡��������ִ���˶�ǰ���ٶȼ��㡣��ÿִ֡���˶�ǰ������ֵ���� Rigidbody2D.velocity
                bool _cachedQueriesStartInColliders;//�洢��Ŀ���ã�����ײ����ʱ��Ҫ��ʱ��Ϊfalse

                float _time;//��Ϸ����ʱ��

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

                #region ��ײ��� Collision
                bool _grounded;//������ڵ�����ʱ��Ϊtrue��������ڿ���ʱ��Ϊfalse��

                bool _groundHit;
                bool _ceilingHit;
                void CheckCollisions() {
                        Physics2D.queriesStartInColliders = false;

                        //������컨����ײ���
                        _groundHit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.down, _data.groundCheckDistance, _data.groundLayerMask);
                        _ceilingHit = Physics2D.BoxCast(_col.bounds.center, _col.bounds.size, 0f, Vector2.up, _data.groundCheckDistance, _data.groundLayerMask);

                        //�Ӵ��컨��
                        if (_ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

                        //�Ӵ�����
                        if (!_grounded && _groundHit) {
                                _grounded = true;
                                _endJumpEarly = false;
                        }
                        //�뿪����
                        else if (_grounded && !_groundHit) {
                                _grounded = false;
                        }

                        Physics2D.queriesStartInColliders = _cachedQueriesStartInColliders;
                }
                #endregion

                #region ����&��Ծ Gravity&Jump

                float _inAirGravity;//�������ٶ�
                bool _endJumpEarly;//����С����Ծ������ɫ����Ծ�����׶�ʱ����ǰ�ɿ���Ծ������ֵΪtrue��

                bool _jumpPressed = false;//��Ծ�����Ƿ񱻰��´���������Ұ�����Ծ����˲��Ϊtrue������ɫִ��һ�����������̻ع�false��
                float _timeJumpPressed;//��Ұ�����Ծ������Ϸʱ�䣬���ڼ��� BufferJump �� CoyoteJump �Ƿ񴥷���
                bool _jumpHolding = false;//��Ծ�����Ƿ��������ס������Ұ�����Ծ��˲��Ϊtrue��������ɿ���Ծ��˲��ع�false��

                bool _hasBufferJump => _grounded && _time < _timeJumpPressed + _data.bufferJumpTime;//�Ƿ񴥷���BufferJump������ʱΪtrue��

                //��ԾԤ������������ʱ��ȵ�
                void HandleJump() {
                        if (!_endJumpEarly && !_grounded && !_jumpHolding && _rb.velocity.y > 0) _endJumpEarly = true;//��������Щ����ʱ����ǰ�ɿ���Ծ������������

                        if (!_jumpPressed || !_hasBufferJump) return;
                        if (_grounded) ExecuteJump();
                        _jumpPressed = false;
                }
                //ִ����Ծ
                void ExecuteJump() {
                        _endJumpEarly = false;
                        _frameVelocity.y = _data.jumpPower;
                }

                //��������
                void HandleGravity() {
                        if (_grounded && _frameVelocity.y <= 0) {
                                _frameVelocity.y = _data.groundForce;
                        }
                        else {
                                _inAirGravity = _data.fallAcceleration;//����
                                if (_endJumpEarly && _frameVelocity.y > 0) _inAirGravity *= _data.jumpEndEarlyGravityMuldifier;//����Ծ�����׶�ʱ��ǰ�ɿ���Ծ������������Ϊԭ����n����
                                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_data.maxFallSpeed, _inAirGravity * Time.fixedDeltaTime);
                        }
                }

                //InputSystem ��Ծ����
                public void OnJump(InputAction.CallbackContext context) {
                        //������Ծ��
                        if (context.phase == InputActionPhase.Started) {
                                _jumpPressed = true;
                                _jumpHolding = true;
                                _timeJumpPressed = _time;
                        }
                        //�ɿ���Ծ��
                        if (context.phase == InputActionPhase.Canceled) {
                                _jumpHolding = false;
                        }
                }
                #endregion

                #region ˮƽ�˶� Horizontal
                
                Vector2 _frameMoveInput;
                float _deceleration;

                void HandleHorizontal() {
                        //�����û��ˮƽ����ʱ
                        if (_frameMoveInput.x == 0) {
                                _deceleration = _grounded ? _data.groundDeceleration : _data.airDeceleration;
                                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, _deceleration * Time.fixedDeltaTime);
                        }
                        //�������ˮƽ����ʱ
                        else {
                                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameMoveInput.x * _data.maxSpeed, _data.acceleration * Time.fixedDeltaTime);
                        }
                }
                //InputSystem ˮƽ����
                public void OnMove(InputAction.CallbackContext context) {
                        _frameMoveInput = context.ReadValue<Vector2>();
                }


                #endregion

                //��ÿ֡���˶��ٶȵ�Ԥ����ִ��
                void ApplyMovement() => _rb.velocity = _frameVelocity;
        }
}