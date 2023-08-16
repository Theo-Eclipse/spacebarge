using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flier.Controls.AI;
using UnityEngine.EventSystems;

namespace Flier.Controls
{
    public class PointerTargetSelect : MonoBehaviour
    {
        [SerializeField] private DefaultFlierControl playerFlier;
        [SerializeField] private LayerMask castLayer;
        [SerializeField] private float castRadius = 1.5f;
        private Collider[] collidersClicked = new Collider[6];
        private Vector3 lastWorldClickPosition = Vector3.zero;
        private Ray worldPoint;
        private float rayDistance = 10;
        private Plane floor;
        private PointerEventData pointerData;
        private List<RaycastResult> results = new List<RaycastResult>(10);

        private void Awake()
        {
            pointerData = new PointerEventData(EventSystem.current);
        }
        private void Start()
        {

            playerFlier.onFlierDestroyed.AddListener(ForgetTarget);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                CheckSphereForTargets();
        }

        private Vector3 InputToWorldPosition() 
        {
            worldPoint = CameraFollow.camInstance.ScreenPointToRay(Input.mousePosition);
            floor.normal = Vector3.up;
            if (floor.Raycast(worldPoint, out rayDistance))
                return worldPoint.origin + worldPoint.direction * rayDistance;
            return Vector3.zero;
        }

        private void CheckSphereForTargets() 
        {
            if (getMouseOverUi())
                return;
            collidersClicked = Physics.OverlapSphere(InputToWorldPosition(), castRadius, castLayer);
            foreach (var collider in collidersClicked)
                if (collider.GetComponentInParent<BasicAiFlier>())
                {
                    LockTarget(collider.GetComponentInParent<BasicAiFlier>());
                    return;
                }
            if (playerFlier.lockAtTarget)
                ForgetTarget();
        }

        private void LockTarget(BasicAiFlier aiFlier) 
        {
            if (playerFlier.lockAtTarget)
                ForgetTarget();
            playerFlier.lockAtTarget = aiFlier.transform;
            aiFlier.onFlierDestroyed.AddListener(ForgetTarget);
        }

        private void ForgetTarget()
        {
            if (!playerFlier.lockAtTarget)
                return;
            playerFlier.lockAtTarget.GetComponent<BasicAiFlier>().onFlierDestroyed.RemoveListener(ForgetTarget);
            playerFlier.lockAtTarget = null;
        }

        private void OnDrawGizmos()
        {
            if (lastWorldClickPosition == Vector3.zero)
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(lastWorldClickPosition, castRadius);
        }
        public bool getMouseOverUi()
        {
            pointerData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(pointerData, results);
            return results.Count > 0;
        }
    }
}
