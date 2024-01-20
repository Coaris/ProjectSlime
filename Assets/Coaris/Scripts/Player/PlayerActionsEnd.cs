using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player {
        public class PlayerActionsEnd : MonoBehaviour {
                public event Action OnGetAbility;
                PlayerController _player;

                public event Action OnEnglobed;
                void Awake() {
                        _player = transform.parent.parent.GetComponent<PlayerController>();
                }
                public void OnEnglobeEnd() {
                        OnEnglobed?.Invoke();
                }
                public void OnEatNPC() {
                        OnGetAbility?.Invoke();
                        Destroy(_player.GetTouchingNPC());
                }
        }
}

