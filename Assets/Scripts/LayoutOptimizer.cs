using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProjectDisplacement {
    class LayoutOptimizer : MonoBehaviour
    {
        public GameObject MasterDirectory;
        public List<Transform> AllTransformInLayout
        {
            get; private set;
        } = new List<Transform>();
        private bool active;
        private int currentFrame = 0;

        [SerializeField]
        private int maxIteration;

        public List<Constraint> Constraints = new List<Constraint>();


        // Start is called before the first frame update
        void Start()
        {
            // Parse all objects we want to deal with in the directory
            AllTransformInLayout = new List<Transform>(MasterDirectory.GetComponentsInChildren<Transform>());
            AllTransformInLayout.Remove(MasterDirectory.transform); // Prevent master directory itself being considered
            active = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                active = true;
                Debug.Log("Layout optimization start");
            }
            if (active)
            {
                if (currentFrame <= maxIteration)
                {
                    UpdateStiffness(Constraints, currentFrame);
                    ProjectConstraints(Constraints);
                    // Handle collisions
                    currentFrame++;
                }
                else
                {
                    Debug.Log("Layout synthesis complete");
                    active = false;
                }
            }
        }

        private void UpdateStiffness(List<Constraint> existingConstraints, int currentIteration)
        {
            foreach(Constraint constraint in existingConstraints)
            {
                constraint.UpdateStiffness(currentIteration);
            }
        }

        private void ProjectConstraints(List<Constraint> existingConstraints)
        {
            foreach(Constraint constraint in existingConstraints)
            {
                foreach(Transform transf in AllTransformInLayout)
                {
                    GameObject gameObj = transf.gameObject;
                    if (constraint.IsEquality)
                    {
                        MoveParticle(gameObj, constraint);
                    }
                    else
                    {
                        // inequality constraint C(p) >= 0
                        if(constraint.EvaluateAt(gameObj) < 0)
                        {
                            MoveParticle(gameObj, constraint);
                        }
                    }
                }
            }
        }

        private void MoveParticle(GameObject obj, Constraint constraint)
        {
            Vector3 gradient = constraint.GradientAt(obj);
            if (gradient != Vector3.zero) // Don't compute scale factor if we know no displacement will happen
            {
                obj.transform.position += -ScaleFactor(obj, constraint) * gradient;
            }
        }

        /// <summary>
        /// Scale factor s in paper section 3.1
        /// </summary>
        private float ScaleFactor(GameObject gameObj, Constraint constraint)
        {
            float k = constraint.CurrentStiffness;
            float numerator = k * (1 / gameObj.GetComponent<Rigidbody>().mass) * constraint.EvaluateAt(gameObj);
            float denominator = 0;
            foreach(Transform transf in AllTransformInLayout)
            {
                denominator += (1 / transf.gameObject.GetComponent<Rigidbody>().mass)
                    * constraint.GradientAt(transf.gameObject).sqrMagnitude;
            }
            return numerator / denominator;
        }
    }
}