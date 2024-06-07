using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Combat {

    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Animator))]

    public class Fighter : MonoBehaviour, IAction {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;

        [SerializeField] float attackDamage = 5f;
        bool isAttacking;

        float timeSinceLastAttack = Mathf.Infinity;

        Health targetHealth;
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;

        private void Start() {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            isAttacking = false;
        }

        private void Update() {

            timeSinceLastAttack += Time.deltaTime;

            if (targetHealth != null && targetHealth.isAlive() && !isAttacking) {
                if (IsInRange()) {
                    isAttacking = true;
                    mover.Cancel();
                    AttackBehavior();
                }
                else {
                    mover.MoveTo(targetHealth.transform.position);
                }
            }
        }

        private bool IsInRange() {
            return Vector3.Distance(this.transform.position, targetHealth.transform.position) < weaponRange;
        }

        private void AttackBehavior() {
            transform.LookAt(targetHealth.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks) {
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
            else {
                isAttacking = false;
            }
        }

        public void Attack(Health target) {
            if (target == null) return;
            actionScheduler.StartAction(this);
            animator.ResetTrigger("cancelAttack");
            targetHealth = target;
        }

        public bool CanAttack(GameObject target) {
            if (target == null) return false;
            Health targetHealth = target.GetComponent<Health>();
            return targetHealth != null && targetHealth.isAlive();
        }

        public void Cancel() {
            targetHealth = null;
            animator.SetTrigger("cancelAttack");
            isAttacking = false;
        }

        //Animation Events
        void Hit() {
            if (targetHealth != null && IsInRange()) {
                targetHealth.TakeDamage(attackDamage);
            }
        }

        void AttackDone() {
            isAttacking = false;
            Debug.Log("Attack done");
        }
    }
}
