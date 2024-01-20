using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CoarisPlatformer2D {
        public class PlayerActionsEnd : MonoBehaviour {
                public event Action OnEnglobed;
                public void OnEnglobeEnd() {
                        OnEnglobed?.Invoke();
                }
        }
}

