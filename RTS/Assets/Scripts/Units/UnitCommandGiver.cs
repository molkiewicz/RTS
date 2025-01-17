using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace RTS.Units {
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
        [SerializeField] private LayerMask layerMask = new LayerMask();

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame) return;

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

            if(hit.collider.TryGetComponent(out Targetable target))
            {
                if (target.hasAuthority)
                {
                    TryMove(hit.point);
                    return;
                }
                TryTarget(target);
                return;

            }

            TryMove(hit.point);
        }

        private void TryTarget(Targetable target)
        {
            foreach (Unit unit in unitSelectionHandler.selectedUnits)
            {
                unit.GetTargeter().CmdSetTarget(target.gameObject);
            }
        }

        private void TryMove(Vector3 destination)
        {
            foreach (Unit unit in unitSelectionHandler.selectedUnits)
            {
                unit.GetUnitMovement().CmdMove(destination);
            }
        }
    }
}