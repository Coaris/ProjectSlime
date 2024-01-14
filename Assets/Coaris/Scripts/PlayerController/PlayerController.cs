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

                        ApplyMovement();
                }

                void ApplyMovement() => _rb.velocity = _frameVelocity;
        }
}