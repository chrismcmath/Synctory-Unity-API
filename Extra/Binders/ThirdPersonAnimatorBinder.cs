using System;
using System.Collections;
using UnityEngine;

namespace Synctory.Extra.Binders {
    [RequireComponent (typeof (Animator))]
        public class ThirdPersonAnimatorBinder : MonoBehaviour {
            public const string PARAM_FORWARD = "Forward";
            public const string PARAM_TURN = "Turn";
            public const float MOVE_THRESHOLD = 0.01f;

            public float ForwardAnimationCoefficient = 1f;
            public float TurnAnimationCoefficient = 1f;

            private Animator _Animator;
            private Vector3 _PrevPosition;
            private float _PrevYRotation;

            public void Start() {
                _Animator = GetComponent<Animator>();
            }

            public void Update() {
                if (_Animator == null) {
                    return;
                }

                Vector3 positionDelta = transform.position - _PrevPosition;
                float rotationDelta = transform.rotation.eulerAngles.y - _PrevYRotation;

                _PrevPosition = transform.position;
                _PrevYRotation = transform.parent.rotation.eulerAngles.y;

                float forward = positionDelta.magnitude * ForwardAnimationCoefficient;
                float turn = rotationDelta * TurnAnimationCoefficient;

                _Animator.SetFloat(PARAM_FORWARD, forward);
                //_Animator.SetFloat(PARAM_TURN, turn);
                Vector3 rotationViewVector = new Vector3(positionDelta.x, 0f, positionDelta.z);
                if (rotationViewVector != Vector3.zero) {
                    transform.rotation = Quaternion.LookRotation(rotationViewVector);
                }

                //Debug.Log("[3RD PERSON ANIMATOR] forward: " + forward + ", turn: " + Quaternion.LookRotation(positionDelta));
            }
        }
}
