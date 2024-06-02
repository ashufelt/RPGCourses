using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Combat {

    public class Fighter : MonoBehaviour, IAction {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] float attackDamage = 5f;

        float timeSinceLastAttack = 0;

        Health combatTarget;
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

            if (combatTarget != null && combatTarget.hasHP()) {
                if (IsInRange()) {
                    mover.Cancel();
                    AttackBehavior();
                }
                else {
                    mover.MoveTo(combatTarget.transform.position);
                }

            }
        }

        private bool IsInRange() {
            return Vector3.Distance(this.transform.position, combatTarget.transform.position) < weaponRange;
        }

        private void AttackBehavior() {
            transform.LookAt(combatTarget.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks) {
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
            
        }

        public void Attack(Health target) {
            actionScheduler.StartAction(this);
            animator.ResetTrigger("cancelAttack");
            combatTarget = target;
        }

        public void Cancel() {
            combatTarget = null;
            animator.SetTrigger("cancelAttack");
        }

        //Animation Event
        void Hit() {
            if (combatTarget != null) {
                combatTarget.TakeDamage(attackDamage);
            }
        }
    }
}
