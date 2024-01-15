using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoarisPlatformer2D {
        [CreateAssetMenu(menuName = "PlayerData")]//可以在编辑器中创建PlayerData的预设体
        public class PlayerData : ScriptableObject {
                [Header("Gravity")]
                [Tooltip("玩家的空中的下降的基础加速度，建议值110 The player's capacity to gain fall speed")]
                public float fallAcceleration = 110f;

                [Tooltip("玩家提前松开跳跃键时，施加的重力倍增，建议值3 The gravity multiplier added when jump is released early")]
                public float jumpEndEarlyGravityMuldifier = 3f;

                [Tooltip("玩家在地面上时的重力，主要用于斜坡，建议值-1.5 A constant downward velocity applied while grounded. Helps on slopes"), Range(0, -10f)]
                public float groundForce = -1.5f;

                [Tooltip("玩家垂直速度的最大速度，建议值40 The maximum vertiacl movement speed")]
                public float maxFallSpeed = 40f;

                [Header("Jump")]
                [Tooltip("玩家的跳跃力，会影响跳跃高度,建议值36 The immediate vertical velocity applied when jumping")]
                public float jumpPower = 36f;

                [Tooltip("跳跃缓冲，在到达平台前的这段时间内，如果玩家按下跳跃键，则允许角色在落地之后立刻起跳，建议值0.12f The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
                public float bufferJumpTime = 0.12f;

                [Header("CollisionCheck")]
                [Tooltip("地面与天花板的LayerMask")]
                public LayerMask groundLayerMask;

                [Tooltip("地面与天花板碰撞检测距离，建议值0.05 The detection distance for ground and ceiling check"), Range(0f, 0.5f)]
                public float groundCheckDistance = 0.05f;

                [Tooltip("碰撞体检测皮肤距离，建议值0.05"), Range(0f, 0.1f)]
                public float skinDistance = 0.05f;

                [Header("Horizontal")]
                [Tooltip("水平运动的最大速度，建议值14 The top horizontal speed")]
                public float maxSpeed = 14f;


                [Tooltip("水平运动的加速的加速度，建议值 马力欧120 蔚蓝200 The player's capacity to gain horizontal speed")]
                public float acceleration = 200f;

                [Tooltip("地面水平运动减速的加速度，建议值 马力欧60 蔚蓝200 The pace at which the player comes to a stop")]
                public float groundDeceleration = 200f;

                [Tooltip("空中水平运动减速的加速度，建议值 马力欧30 蔚蓝200 Deceleration in air only after stopping input mid-air")]
                public float airDeceleration = 200f;

        }
}