using UnityEngine;
using GorillaLocomotion;
using BananaDispenser;

namespace DevHoldableEngine
{
    public class HoldableEngine : HoldableObject
    {
        public bool InHand;
        public bool InLeftHand;
        public bool PickUp;
        public Rigidbody Rigidbody;
        public AudioSource audioSource;
        public AudioClip grabSound;
        public AudioClip throwSound;
        public Collider boxColl;

        public float Distance = 0.2f;
        public float ThrowForce = 3f;
        private const float velocityForSplat = 10f;
        public bool isCat;

        public virtual void OnGrab(bool isLeft)
        {
            if (Rigidbody != null)
            {
                Rigidbody.isKinematic = true;
                Rigidbody.useGravity = false;
            }
            if (boxColl == null)
            {
                boxColl = gameObject.GetComponent<BoxCollider>();
            }
            audioSource.pitch = 1;
            audioSource.clip = grabSound;
            audioSource.PlayOneShot(audioSource.clip);

        }

        public virtual void OnDrop(bool isLeft)
        {
            if (Rigidbody != null)
            {
                GorillaVelocityEstimator gorillaVelocityEstimator = (isLeft ? Player.Instance.leftControllerTransform.GetComponentInChildren<GorillaVelocityEstimator>() : Player.Instance.rightControllerTransform.GetComponentInChildren<GorillaVelocityEstimator>()) ?? null;

                if (gorillaVelocityEstimator != null)
                {
                    Rigidbody.isKinematic = false;
                    Rigidbody.useGravity = true;
                    Rigidbody.velocity = (gorillaVelocityEstimator.linearVelocity * ThrowForce) + Player.Instance.GetComponent<Rigidbody>().velocity;
                    Rigidbody.angularVelocity = gorillaVelocityEstimator.angularVelocity;
                }
            }
            audioSource.pitch = 1;
            audioSource.clip = throwSound;
            audioSource.PlayOneShot(audioSource.clip);
        }
        public void Update()
        {
            bool leftGrip = ControllerInputPoller.instance.leftGrab;
            bool rightGrip = ControllerInputPoller.instance.rightGrab;

            if (PickUp && leftGrip && Vector3.Distance(Player.Instance.leftControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.leftHandHeldEquipment == null)
            {
                // Hold logic
                InLeftHand = true;
                InHand = true;
                transform.SetParent(GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent);

                // Other logic
                GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
                EquipmentInteractor.instance.leftHandHeldEquipment = this;

                // Callback
                OnGrab(true);
            }
            else if (!leftGrip && InHand && InLeftHand)
            {
                // Drop logic
                InLeftHand = true;
                InHand = false;
                transform.SetParent(null);

                // Other logic
                GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
                EquipmentInteractor.instance.leftHandHeldEquipment = null;

                // Callback
                OnDrop(true);
            }

            if (PickUp && rightGrip && Vector3.Distance(Player.Instance.rightControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.rightHandHeldEquipment == null)
            {
                // Hold logic
                InLeftHand = false;
                InHand = true;
                transform.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent);

                // Other logic
                GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
                EquipmentInteractor.instance.rightHandHeldEquipment = this;

                // Callback
                OnGrab(false);
            }
            else if (!rightGrip && InHand && !InLeftHand)
            {
                // Drop logic
                InLeftHand = false;
                InHand = false;
                transform.SetParent(null);

                // Other logic
                GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
                EquipmentInteractor.instance.rightHandHeldEquipment = null;

                // Callback
                OnDrop(false);
            }
        }

        public void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            grabSound = Plugin.bundle.LoadAsset<AudioClip>("grab");
            throwSound = Plugin.bundle.LoadAsset<AudioClip>("throw");
        }
    }
}