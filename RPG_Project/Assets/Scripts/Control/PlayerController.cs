using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using System;
using RPG.Combat;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Control {

    public class PlayerController : MonoBehaviour {

        const int LEFT_MOUSE_BUTTON = 0;
        const int RIGHT_MOUSE_BUTTON = 1;
        const int MIDDLE_MOUSE_BUTTON = 2;

        private void Update() {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
                    this.GetComponent<Fighter>().Attack(target);
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit && Input.GetMouseButton(RIGHT_MOUSE_BUTTON)) {
                this.GetComponent<Mover>().StartMoveAction(hit.point);
            }
            return hasHit;
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
