using System.Collections;
using System.Collections.Generic;
using Network;
using Network.Packet.Packets;
using Network.Player;
using UnityEngine;

namespace Player {

    public class PlayerMovement : MonoBehaviour {

        private Animator animator;
        
        private PlayerManager playerManager;
        private bool managerInit = false;
        private bool sendSync = false;
        
        [SerializeField] private float speed = 100;

        private Rigidbody2D rigid;

        private float horizontal;
        private float vertical;

        private Vector2 _velocity;
        private Transform _transform;

        private void Awake() {
            animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            _transform = transform;
            _velocity = rigid.velocity;
        }

        private void Start() {
            transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            StartCoroutine(Wait());
        }

        IEnumerator Wait() {
            yield return new WaitForSeconds(3f);
            playerManager = FindObjectOfType<PlayerManager>();
            managerInit = true;
        }

        private void Update() {
            
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0) {
                animator.SetBool("Moving", true);
            } else {
                animator.SetBool("Moving", false);
            }
            
        }

        private void FixedUpdate() {

            _velocity = new Vector2(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime);
            rigid.velocity = _velocity;
            
            //SEND MOVEMENT SYNC PACKET
            if (managerInit) {
                if (_velocity != Vector2.zero) {
                    playerManager.SendSync(_transform.position);
                    sendSync = false;
                } else if (!sendSync) {
                    sendSync = playerManager.SendSync(_transform.position);
                }
            }

        }

    }

}