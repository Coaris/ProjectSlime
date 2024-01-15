using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoarisPlatformer2D {
        [CreateAssetMenu(menuName = "PlayerData")]//�����ڱ༭���д���PlayerData��Ԥ����
        public class PlayerData : ScriptableObject {
                [Header("Gravity")]
                [Tooltip("��ҵĿ��е��½��Ļ������ٶȣ�����ֵ110 The player's capacity to gain fall speed")]
                public float fallAcceleration = 110f;

                [Tooltip("�����ǰ�ɿ���Ծ��ʱ��ʩ�ӵ���������������ֵ3 The gravity multiplier added when jump is released early")]
                public float jumpEndEarlyGravityMuldifier = 3f;

                [Tooltip("����ڵ�����ʱ����������Ҫ����б�£�����ֵ-1.5 A constant downward velocity applied while grounded. Helps on slopes"), Range(0, -10f)]
                public float groundForce = -1.5f;

                [Tooltip("��Ҵ�ֱ�ٶȵ�����ٶȣ�����ֵ40 The maximum vertiacl movement speed")]
                public float maxFallSpeed = 40f;

                [Header("Jump")]
                [Tooltip("��ҵ���Ծ������Ӱ����Ծ�߶�,����ֵ36 The immediate vertical velocity applied when jumping")]
                public float jumpPower = 36f;

                [Tooltip("��Ծ���壬�ڵ���ƽ̨ǰ�����ʱ���ڣ������Ұ�����Ծ�����������ɫ�����֮����������������ֵ0.12f The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
                public float bufferJumpTime = 0.12f;

                [Header("CollisionCheck")]
                [Tooltip("�������컨���LayerMask")]
                public LayerMask groundLayerMask;

                [Tooltip("�������컨����ײ�����룬����ֵ0.05 The detection distance for ground and ceiling check"), Range(0f, 0.5f)]
                public float groundCheckDistance = 0.05f;

                [Tooltip("��ײ����Ƥ�����룬����ֵ0.05"), Range(0f, 0.1f)]
                public float skinDistance = 0.05f;

                [Header("Horizontal")]
                [Tooltip("ˮƽ�˶�������ٶȣ�����ֵ14 The top horizontal speed")]
                public float maxSpeed = 14f;

                [Tooltip("ˮƽ�˶��ļ��ٵļ��ٶȣ�����ֵ ����ŷ120 ε��200 The player's capacity to gain horizontal speed")]
                public float acceleration = 200f;

                [Tooltip("����ˮƽ�˶����ٵļ��ٶȣ�����ֵ ����ŷ60 ε��200 The pace at which the player comes to a stop")]
                public float groundDeceleration = 200f;

                [Tooltip("����ˮƽ�˶����ٵļ��ٶȣ�����ֵ ����ŷ30 ε��200 Deceleration in air only after stopping input mid-air")]
                public float airDeceleration = 200f;
        }
}