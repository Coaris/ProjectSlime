using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player {
        public class PlayerActionsEnd : MonoBehaviour {
                public event Action OnGetAbility;
                PlayerController _player;

                public event Action OnAbsorbed;
                void Awake() {
                        _player = transform.parent.parent.GetComponent<PlayerController>();
                }
                public void OnAbsorbEnd() {
                        OnAbsorbed?.Invoke();
                }
                public void OnEatNPC() {
                        OnGetAbility?.Invoke();
                        Destroy(_player.GetTouchingNPC());
                }
        }
}

