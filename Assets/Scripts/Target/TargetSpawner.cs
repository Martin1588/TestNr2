using System.Collections;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.Target
{
    public class TargetSpawner : MonoBehaviour
    {
        public GameObject Enemy;

        private int numberOfBugs = 1;
        private float xPos;
        private float zPos;


        // Use this for initialization
        void Start()
        {
            StartCoroutine(spawnTargets());
        }

        IEnumerator spawnTargets()
        {
            while (true)
            {
                xPos = transform.position.x + Random.Range(-10f, 10f);
                zPos = transform.position.z + Random.Range(-10f, 10f);
                Instantiate(Enemy, new Vector3(xPos, 1, zPos), Quaternion.identity);
                yield return new WaitForSeconds(3);
            }
        }
    }
}