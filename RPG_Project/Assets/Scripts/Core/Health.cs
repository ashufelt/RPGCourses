using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core {

    [RequireComponent(typeof(Animator))]

    public class Health : MonoBehaviour {

        [SerializeField] float initialHealth = 100f;
        [SerializeField] float health = 10f;
        float maxHealth = 100f;
        bool isDead;

        Animator animator;

        private void Start() {
            health = initialHealth;
            maxHealth = initialHealth;
            isDead = false;
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage) {
            if (health <= damage) {
                health = 0;
                Die();
            }
            else {
                health -= damage;
            }
        }

        private void Die() {
            if (isDead) { return; }
            isDead = true;
            animator.SetTrigger("die");
            if (TryGetComponent<ActionScheduler>(out ActionScheduler actionScheduler)) {
                actionScheduler.CancelCurrentAction();
            }
        }

        public bool isAlive() {
            return health > 0;
        }

    }
}
