using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat {

    public class Fighter : MonoBehaviour, IAction {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;

        float timeSinceLastAttack = 0;

        Transform combatTarget;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;

        private void Start() {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update() {

            timeSinceLastAttack += Time.deltaTime;

            if (combatTarget != null) {
                if (IsInRange()) {
                    mover.Cancel();
                    if (timeSinceLastAttack >= timeBetweenAttacks) {
                        AttackBehavior();
                        timeSinceLastAttack = 0f;
                    }
                }
                else {
                    mover.MoveTo(combatTarget.position);
                }

            }
        }

        private bool IsInRange() {
            return Vector3.Distance(this.transform.position, combatTarget.position) < weaponRange;
        }

        private void AttackBehavior() {
            animator.SetTrigger("attack");
        }

        public void Attack(CombatTarget target) {
            actionScheduler.StartAction(this);
            combatTarget = target.transform;
        }

        public void Cancel() {
            combatTarget = null;
        }

        //Animation Event
        void Hit() {

        }
    }
}
