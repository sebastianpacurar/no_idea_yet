using ScriptableObjects;
using UnityEngine;
using Utils;


namespace Prop.Interactables.Cart{
    public class CartScript : MonoBehaviour{
        [SerializeField] private CartPhysicsDataSo data;

        [Header("Current PhysicsMaterial2D")]
        [SerializeField] private PhysicsMaterial2D selectedPm2D;

        [Header("References")]
        [SerializeField] private GameObject[] wheels;

        [Header("For Debugging")]
        [SerializeField] private float speed;
        [SerializeField] private float direction;
        [SerializeField] private Vector2 localVelocity;
        public bool isPlayerCollision;

        private Rigidbody2D _rb;
        private CapsuleCollider2D _capsule;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _capsule = GetComponent<CapsuleCollider2D>();
        }


        private void Update()
        {
            SetSpeedAndDirection();

            // set the friction to low if player collision
            SetPhysicsMaterial(isPlayerCollision ? data.LowFriction : data.HighFriction);

            selectedPm2D = _capsule.sharedMaterial;

            // rotate wheels
            foreach (var wheel in wheels)
            {
                wheel.transform.Rotate(0, 0, speed * direction * Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            SetMaxVelocity();
            MassBasedOnCollision();
        }


        private void SetMaxVelocity()
        {
            var parsedX = Mathf.Clamp(_rb.velocity.x, -data.MaxSpeed, data.MaxSpeed);
            _rb.velocity = new Vector2(parsedX, _rb.velocity.y);
        }


        private void SetSpeedAndDirection()
        {
            // transform velocity from WorldSpace to LocalSpace
            localVelocity = transform.InverseTransformDirection(_rb.velocity);

            // get the length (speed) of the localVelocity (multiplied by rotFactor to get desired effect)
            speed = localVelocity.magnitude * data.WheelRotationFactor;

            // rotation is done on the opposite side of the direction
            direction = -Mathf.Sign(localVelocity.x);
        }


        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            // if player is on the left or right of the cart, then return true
            if (CollisionUtils.IsCollisionSideways(other))
            {
                isPlayerCollision = true;
            }
        }


        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isPlayerCollision = false;
            }
        }

        // set high mass for cart when not collided
        private void MassBasedOnCollision()
        {
            _rb.mass = isPlayerCollision ? data.MassWhenPushed : data.MassWhenStationary;
        }


        // Set the PhysicsMaterial2D with the provided version
        private void SetPhysicsMaterial(PhysicsMaterial2D material)
        {
            _capsule.sharedMaterial = material;
        }
    }
}