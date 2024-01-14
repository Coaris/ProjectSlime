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
                Vector2 _frameVelocity;//ÿ֡��������ִ���˶�ǰ���ٶȼ��㡣��ÿִ֡���˶�ǰ������ֵ���� Rigidbody2D.velocity

                void Awake() {
                        _rb = GetComponent<Rigidbody2D>();
                }

                void FixedUpdate() {
                        HandleJump();
                        HandleGravity();

                        ApplyMovement();
                }

                #region ����&��Ծ Gravity&Jump
                
                float _inAirGravity;//�������ٶ�
                
                bool _jumpPressed = false;//��Ծ�����Ƿ񱻰��´���
                bool _jumpHolding = false;//��Ծ�����Ƿ��������ס

                //��������
                void HandleGravity() {
                        _inAirGravity = _data.fallAcceleration;
                        _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_data.maxFallSpeed, _inAirGravity * Time.fixedDeltaTime);
                }
                //��ԾԤ������������ʱ��ȵ�
                void HandleJump() {


                        if (!_jumpPressed) return;
                        ExecuteJump();
                        _jumpPressed = false;
                }
                //ִ����Ծ
                void ExecuteJump() {
                        _frameVelocity.y = _data.jumpPower;
                }
                //InputSystem ��Ծ����
                public void OnJump(InputAction.CallbackContext context) {
                        //������Ծ��
                        if (context.phase == InputActionPhase.Started) {
                                _jumpPressed = true;
                                _jumpHolding = true;
                        }
                        //�ɿ���Ծ��
                        if (context.phase == InputActionPhase.Canceled) {
                                _jumpHolding = false;
                        }
                }
                #endregion

                //��ÿ֡���˶��ٶȵ�Ԥ����ִ��
                void ApplyMovement() => _rb.velocity = _frameVelocity;
        }
}