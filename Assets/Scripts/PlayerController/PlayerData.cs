using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoarisPlatformer2D {
        [CreateAssetMenu(menuName = "PlayerData")]//可以在编辑器中创建PlayerData的预设体
        public class PlayerData : ScriptableObject {
                [Header("垂直 Vertical")]
                [Tooltip("玩家的空中的下降的基础加速度 The player's capacity to gain fall speed")]
                public float fallAcceleration = 110f;

                //[Tooltip("玩家提前松开跳跃键时，施加的重力倍增/The gravity multiplier added when jump is released early")]
                //public float jumpEndEarlyGravityMultiplier = 3f;

                //[Tooltip("玩家垂直速度的最大速度/The maximum vertiacl movement speed")]
                //public float maxFallSpeed = 40f;
        }
}