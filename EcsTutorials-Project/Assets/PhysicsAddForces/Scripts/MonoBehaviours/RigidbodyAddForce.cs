using UnityEngine;

namespace TMG.PhysicsAddForces
{
    public class RigidbodyAddForce : MonoBehaviour
    {
        [SerializeField] private ForceMode _forceMode;
        [SerializeField] private float _forceAmount;
        [SerializeField] private KeyCode _forwardInputKey;
        
        private Rigidbody _rigidbody;
        private bool InputKeyHeld => Input.GetKey(_forwardInputKey);
        private Vector3 ForceVector => Vector3.forward * _forceAmount;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.maxAngularVelocity = Mathf.Infinity;
        }

        private void FixedUpdate()
        {
            if (InputKeyHeld)
            {
                _rigidbody.AddForce(ForceVector, _forceMode);
            }
        }
    }
}