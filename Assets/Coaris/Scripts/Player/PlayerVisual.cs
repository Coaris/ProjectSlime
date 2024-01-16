using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoarisPlatformer2D {
        public class PlayerVisual : MonoBehaviour {
                PlayerController _player;
                Animator _anim;
                SpriteRenderer _sprite;

                void Awake() {
                        _player = GetComponentInParent<PlayerController>();
                        _anim = GetComponentInChildren<Animator>();
                        _sprite = GetComponentInChildren<SpriteRenderer>();
                }
                void OnEnable() {
                        _player.OnJumped += OnJumped;
                        _player.OnFalling += OnFalling;
                        _player.OnGrounded += OnGrounded;
                }
                void OnDisable() {
                        _player.OnJumped -= OnJumped;
                        _player.OnFalling -= OnFalling;
                        _player.OnGrounded -= OnGrounded;
                }
                void Update() {
                        if (_player == null) return;

                        HandleSpriteDirection();
                        HandleWalkingAnim();
                }

                //控制角色左右朝向
                void HandleSpriteDirection() {
                        _sprite.flipX = _player._isFacingLeft;
                }
                //控制角色行走的动画切换
                void HandleWalkingAnim() {
                        if (!_player._grounded) return;
                        _anim.SetFloat(SpeedKey, Mathf.Abs(_player._frameMoveInput.x));
                }

                void OnJumped() {
                        _anim.SetBool(JumpingKey, true);
                        _anim.SetBool(FallingKey, false);
                        _anim.SetBool(GroundedKey, false);
                }
                void OnFalling() {
                        _anim.SetBool(JumpingKey, false);
                        _anim.SetBool(FallingKey, true);
                        _anim.SetBool(GroundedKey, false);
                }
                void OnGrounded() {
                        _anim.SetBool(JumpingKey, false);
                        _anim.SetBool(FallingKey, false);
                        _anim.SetBool(GroundedKey, true);
                }

                static readonly int JumpingKey = Animator.StringToHash("Jumping");
                static readonly int FallingKey = Animator.StringToHash("Falling");
                static readonly int GroundedKey = Animator.StringToHash("Grounded");
                static readonly int SpeedKey = Animator.StringToHash("Speed");
        }
}