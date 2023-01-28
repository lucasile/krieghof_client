using System.Collections;
using System.Collections.Generic;
using Event.Events.Network.Disconnect;
using Event.Events.Network.Movement;
using Network.Player;
using TMPro;
using UnityEngine;
using Utils;

namespace Player {

    public class OtherPlayer : MonoBehaviour {

        private float lerpSpeed = 0.2f;
        
        private PlayerManager playerManager;
        private Animator animator;

        private Dictionary<string, ConnectedPlayer> playersConnected;
        
        private bool canMove;
        
        private string uuid = "";

        private Transform _transform;
        private Vector3 _lastPosition;

        private float _lastTime;
        private float _time;
        private Vector3 _target;

        private readonly float SERVER_COMPENSATION_TIMEOUT = 5f;
        
        private void Awake() {
            playerManager = FindObjectOfType<PlayerManager>();
            animator = GetComponent<Animator>();
            playersConnected = playerManager.playersConnected;
            _transform = transform;
            _lastTime = 0;
            MovementSyncEvent.RegisterListener(MoveSync);
        }

        private void OnDestroy() {
            MovementSyncEvent.UnregisterListener(MoveSync);
        }

        private void Update() {

            if (!canMove)
                return;

            if (_time - _lastTime > SERVER_COMPENSATION_TIMEOUT ) {
                _transform.position = _target;
                _lastTime = _time;
                return;
            }

            _transform.position = Vector3.MoveTowards(_transform.position, _target, Time.deltaTime / lerpSpeed);

            _lastTime = _time;

            if (_transform.position != _lastPosition) {
                animator.SetBool("Moving", true);
            } else {
                animator.SetBool("Moving", false);
            }
            
            _lastPosition = _transform.position;

        }
        
        private void MoveSync(MovementSyncEvent e) {
            
            if (uuid == "") {
                ConnectedPlayer player = playersConnected[e.uuid];
                if (player.otherPlayer == this) {
                    uuid = e.uuid;
                    canMove = true;
                }
            } else if (e.uuid == uuid) {
                _time = e.time;
                MainThreadDispatcher.RunOnMainThread(() => {
                    _target = new Vector3(e.posX, e.posY, _transform.position.z);
                });
            }
            
        }

    }

}
