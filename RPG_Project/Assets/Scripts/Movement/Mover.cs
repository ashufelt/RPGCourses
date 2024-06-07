using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement {

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Health))]

    public class Mover : MonoBehaviour, IAction {

        NavMeshAgent navMeshAgent;
        ActionScheduler actionScheduler;
        Animator animator;
        Health health;

        private void Start() {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = this.GetComponent<Animator>();
            health = this.GetComponent<Health>();
        }

        void Update() {

            navMeshAgent.enabled = health.isAlive();

            UpdateAnimator();
        }

        public void SetSpeed(float speed) {
            navMeshAgent.speed = speed;
        }

        public void StartMoveAction(Vector3 dest) {
            actionScheduler.StartAction(this);
            MoveTo(dest);
        }

        public void MoveTo(Vector3 dest) {
            if (navMeshAgent == null) {
                Debug.LogError("No NavMeshAgent found");
                return;
            }
            navMeshAgent.destination = dest;
            navMeshAgent.isStopped = false;
        }

        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator() {
            if (navMeshAgent == null) {
                Debug.LogError("No NavMeshAgent found");
                navMeshAgent = this.GetComponent<NavMeshAgent>();
                return;
            }
            if (animator == null) {
                Debug.LogError("No Animator found");
                animator = this.GetComponent<Animator>();
                return;
            }

            Vector3 globalVelocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(globalVelocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }
}
