using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {

    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]

    public class AIController : MonoBehaviour {

        [SerializeField] float chaseDistance = 5f;

        private Mover mover;
        private Fighter fighter;
        private Health health;
        private GameObject player;

        private bool isAttacking;

        Vector3 guardPosition;

        private void Start() {
            mover = this.GetComponent<Mover>();
            fighter = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
            player = GameObject.FindWithTag("Player");

            isAttacking = false;

            guardPosition = this.transform.position;
        }

        private void Update() {
            if (!health.isAlive()) return;
            CombatBehavior();
        }

        private void CombatBehavior() {
            if (player == null) {
                GameObject player = GameObject.FindWithTag("Player");
                return;
            }

            if (fighter == null) {
                fighter = this.GetComponent<Fighter>();
                return;
            }
            
            if (Vector3.Distance(player.transform.position, this.transform.position) < chaseDistance) {
                if (!isAttacking) {
                    isAttacking = true;
                    fighter.Attack(player.GetComponent<Health>());
                }
            }
            else if (isAttacking) {
                isAttacking = false;
                mover.StartMoveAction(guardPosition);
            }
        }

        // Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}
