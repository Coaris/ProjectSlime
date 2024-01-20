using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
        [CreateAssetMenu(menuName = "PlayerData")]//可以在编辑器中创建PlayerData的预设体
        public class PlayerData : ScriptableObject {
                [Header("重力 / Gravity")]
                [Tooltip("玩家的空中的下降的基础加速度，建议值60 The player's capacity to gain fall speed")]
                public float fallAcceleration = 60f;

                [Tooltip("玩家提前松开跳跃键时，施加的重力倍增，建议值3 The gravity multiplier added when jump is released early")]
                public float jumpEndEarlyGravityMuldifier = 3f;

                [Tooltip("玩家在地面上时的重力，主要用于斜坡，建议值-1.5 A constant downward velocity applied while grounded. Helps on slopes"), Range(0, -10f)]
                public float groundForce = -1.5f;

                [Tooltip("玩家垂直速度的最大速度，建议值15 The maximum vertiacl movement speed")]
                public float maxFallSpeed = 15f;

                [Tooltip("滑翔。true时允许玩家在空中，以缓慢的速度降落")]
                public bool airFloat = false;

                [Tooltip("滑翔时的降落速度，建议值2.5")]
                public float maxFloatFallSpeed = 20f;

                [Header("跳跃 / Jump")]
                [Tooltip("玩家的跳跃力，会影响跳跃高度,建议值20 The immediate vertical velocity applied when jumping")]
                public float jumpPower = 20f;

                [Tooltip("跳跃缓冲，在到达平台前的这段时间内，如果玩家按下跳跃键，则允许角色在落地之后立刻起跳，建议值0.12f The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
                public float bufferJumpTime = 0.12f;

                [Tooltip("土狼时间，在离开平台后的这段时间内，如果玩家按下跳跃键，则仍视为有效跳跃，建议值0.07f The time before coyote jump becomes unusabel")]
                public float coyoteTime = 0.07f;

                [Tooltip("空跳，true时则允许在空中跳跃")]
                public bool airJump = false;

                [Tooltip("空跳的最大次数,当此值为1时，玩家可用二段跳，当此值为2时，玩家可用三段跳，以此类推")]
                public int maxAirJump = 1;

                [Header("冲刺 / Dash")]
                [Tooltip("是否拥有冲刺技能")]
                public bool dash;

                [Tooltip("冲刺持续时间")]
                public float dashTime = 0.5f;

                [Tooltip("冲刺冷却时间")]
                public float dashCD = 0.2f;

                [Tooltip("冲刺速度")]
                public float dashSpeed = 5f;

                [Header("碰撞检测 / CollisionCheck")]
                [Tooltip("地面与天花板的LayerMask")]
                public LayerMask groundLayerMask;

                [Tooltip("地面与天花板碰撞检测距离，建议值0.05 The detection distance for ground and ceiling check"), Range(0f, 0.5f)]
                public float groundCheckDistance = 0.05f;

                [Tooltip("碰撞体检测皮肤距离，建议值0.05"), Range(0f, 0.1f)]
                public float skinDistance = 0.05f;

                [Header("水平移动 / Horizontal")]
                [Tooltip("水平运动的最大速度，建议值6 The top horizontal speed")]
                public float maxSpeed = 6f;


                [Tooltip("水平运动的加速的加速度，建议值 马力欧120 蔚蓝200 The player's capacity to gain horizontal speed")]
                public float acceleration = 200f;

                [Tooltip("地面水平运动减速的加速度，建议值 马力欧60 蔚蓝200 The pace at which the player comes to a stop")]
                public float groundDeceleration = 200f;

                [Tooltip("空中水平运动减速的加速度，建议值 马力欧30 蔚蓝200 Deceleration in air only after stopping input mid-air")]
                public float airDeceleration = 200f;

        }
}