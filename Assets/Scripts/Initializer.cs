using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectDisplacement
{
    class Initializer : MonoBehaviour
    {
        public GameObject MasterDirectory;
        public List<Transform> AllObjects
        {
            get; private set;
        } = new List<Transform>();
        private List<Transform> originalPositions;
        private List<Vector3> spawnLocations;

        [SerializeField]
        private Vector3 SpawnZone;
        [SerializeField]
        private Vector3 SpawnOffset;
        [SerializeField]
        private int framesToComplete;
        private int currentFrame;

        private System.Random rnd = new System.Random();

        private void Start()
        {
            // Parse all objects we want to deal with in the directory
            AllObjects = new List<Transform>(MasterDirectory.GetComponentsInChildren<Transform>());
            AllObjects.Remove(MasterDirectory.transform); // Prevent master directory itself from moving
            currentFrame = 0;
            originalPositions = AllObjects;
            spawnLocations = Scramble();
        }

        private void FixedUpdate()
        {
            if(currentFrame <= framesToComplete)
            {
                for(int i = 0; i < AllObjects.Count; i++)
                {
                    AllObjects[i].position =
                        Vector3.MoveTowards(originalPositions[i].position, spawnLocations[i], Vector3.Distance(originalPositions[i].position, spawnLocations[i]) / framesToComplete);
                }
                currentFrame++;
            }
        }

        private List<Vector3> Scramble()
        {
            List<Vector3> result = new List<Vector3>();
            foreach(Transform t in AllObjects)
            {
                Vector3 spawn = new Vector3((float)rnd.NextDouble() * SpawnZone.x, (float)rnd.NextDouble() * SpawnZone.y, (float)rnd.NextDouble() * SpawnZone.z);
                spawn += SpawnOffset;
                result.Add(spawn);
            }
            return result;
        }
    }
}
