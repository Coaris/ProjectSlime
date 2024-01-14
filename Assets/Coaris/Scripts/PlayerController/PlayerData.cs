using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoarisPlatformer2D {
        [CreateAssetMenu(menuName = "PlayerData")]//�����ڱ༭���д���PlayerData��Ԥ����
        public class PlayerData : ScriptableObject {
                [Header("��ֱ Vertical")]
                [Tooltip("��ҵĿ��е��½��Ļ������ٶȣ�����ֵ110 The player's capacity to gain fall speed")]
                public float fallAcceleration = 110f;

                //[Tooltip("�����ǰ�ɿ���Ծ��ʱ��ʩ�ӵ���������������ֵ3 The gravity multiplier added when jump is released early")]
                //public float jumpEndEarlyGravityMultiplier = 3f;

                [Tooltip("��Ҵ�ֱ�ٶȵ�����ٶȣ�����ֵ40 The maximum vertiacl movement speed")]
                public float maxFallSpeed = 40f;

                [Tooltip("��ҵ���Ծ������Ӱ����Ծ�߶�,����ֵ36 The immediate vertical velocity applied when jumping")]
                public float jumpPower = 36f;
        }
}