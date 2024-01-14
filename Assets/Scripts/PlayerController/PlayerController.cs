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

                        ApplyMovement();
                }

                void ApplyMovement() => _rb.velocity = _frameVelocity;
        }
}