using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {

    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]

    public class AIController : MonoBehaviour {

        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float chaseSpeed = 5f;

        [SerializeField] float suspicionTime = 5f;
        float timeSinceLastSawPlayer;

        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float patrolSpeed = 2f;
        [SerializeField] float waypointWaitTime = 2f;
        int patrolWaypoint;
        bool isWaiting;
        float timeSpentWaiting;
        float waypointTolerance;

        private ActionScheduler actionScheduler;
        private Mover mover;
        private Fighter fighter;
        private Health health;
        private GameObject player;

        Vector3 guardPosition;

        private void Start() {
            actionScheduler = this.GetComponent<ActionScheduler>();
            mover = this.GetComponent<Mover>();
            fighter = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
            player = GameObject.FindWithTag("Player");

            timeSinceLastSawPlayer = Mathf.Infinity;

            patrolWaypoint = 0;
            isWaiting = false;
            timeSpentWaiting = Mathf.Infinity;
            waypointTolerance = 0.2f;

            guardPosition = this.transform.position;
        }

        private void Update() {
            if (!health.isAlive()) return;

            if (InChaseRangeOfPlayer() && fighter.CanAttack(player)) { // Chase/attack state
                timeSinceLastSawPlayer = 0;
                CombatBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime) { // wait around for a bit
                SuspicionBehavior();
            }
            else { // Patrol path
                PatrolBehavior();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSpentWaiting += Time.deltaTime;
        }
        
        private void CombatBehavior() {
            mover.SetSpeed(chaseSpeed);
            fighter.Attack(player.GetComponent<Health>());
        }

        private void SuspicionBehavior() {
            actionScheduler.CancelCurrentAction();
        }

        private void PatrolBehavior() {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null) {
                if (AtWaypoint(patrolWaypoint)) {
                    patrolWaypoint = patrolPath.GetNextIndex(patrolWaypoint);
                    timeSpentWaiting = 0f;
                }
                nextPosition = patrolPath.GetWaypoint(patrolWaypoint);
            }
            if (timeSpentWaiting > waypointWaitTime) {
                mover.SetSpeed(patrolSpeed);
                mover.StartMoveAction(nextPosition);
            }
        }
        
        private bool InChaseRangeOfPlayer() {
            if (player == null) {
                GameObject player = GameObject.FindWithTag("Player");
                return false;
            }
            if (fighter == null) {
                fighter = this.GetComponent<Fighter>();
                return false;
            }
            if (Vector3.Distance(player.transform.position, this.transform.position) < chaseDistance) {
                return true;
            }
            return false;
        }

        private bool AtWaypoint(int i) {
            if (Vector3.Distance(this.transform.position, patrolPath.GetWaypoint(i)) < waypointTolerance) {
                return true;
            }
            return false;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}
