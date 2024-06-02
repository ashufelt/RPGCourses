using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat {

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
            animator.SetTrigger("die");
        }

        public bool hasHP() {
            return health > 0;
        }

    }
}
