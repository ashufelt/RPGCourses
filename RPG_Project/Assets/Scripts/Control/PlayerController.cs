using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using System;
using RPG.Combat;
using static UnityEngine.GraphicsBuffer;
using RPG.Core;

namespace RPG.Control {

    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Health))]

    public class PlayerController : MonoBehaviour {

        const int LEFT_MOUSE_BUTTON = 0;
        const int RIGHT_MOUSE_BUTTON = 1;
        const int MIDDLE_MOUSE_BUTTON = 2;

        private Mover mover;
        private Fighter fighter;
        private Health health;

        private void Start() {
            mover = this.GetComponent<Mover>();
            fighter = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
        }

        private void Update() {
            if (!health.isAlive()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits) {
                Health target = hit.transform.GetComponent<Health>();
                if (target == null) continue;
                if (!target.isAlive()) continue;
                if (target.tag == "Player") continue;
                if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
                    fighter.Attack(target);
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"));

            if (hasHit && Input.GetMouseButton(RIGHT_MOUSE_BUTTON)) {
                mover.StartMoveAction(hit.point);
            }
            return hasHit;
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
