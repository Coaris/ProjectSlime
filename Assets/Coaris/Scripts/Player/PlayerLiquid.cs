using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Player {
        public class PlayerLiquid : MonoBehaviour {
                public TextMeshProUGUI curWaterTest;
                PlayerController _player;
                [SerializeField] PlayerData _data;

                const int maxLiquidValue = 100;
                [Range(0, maxLiquidValue)] int maxWaterValue = 100;
                float curWaterValue = 100;//Current Water Value

                bool isInWater => _player._isInWater;

                void Awake() {
                        _player = GetComponent<PlayerController>();
                        curWaterTest = GameObject.Find("CurrentWater").GetComponent<TextMeshProUGUI>();
                }

                void Update() {
                        UpdateWaterValue();
                }

                void UpdateWaterValue() {
                        //补水
                        if (isInWater) {
                                curWaterValue += _data.waterGetPerSecond * Time.deltaTime;
                        }
                        //耗水
                        else {
                                if (_player._frameMoveInput.x != 0 || !_player._grounded || _player._isDashing) {
                                        curWaterValue -= _data.waterLossPerSecond * Time.deltaTime;
                                }
                        }
                        curWaterValue = Mathf.Clamp(curWaterValue, 0, maxWaterValue);
                        curWaterTest.text = Mathf.Ceil(curWaterValue).ToString();
                }
                public void ChangeMaxWaterValue(int count) {
                        if (count * _data.liquidValueEachAbility >= maxLiquidValue) {
                                Debug.Log("自爆，游戏结束");
                        }
                        maxWaterValue = maxLiquidValue - count * _data.liquidValueEachAbility;
                }
        }
}
