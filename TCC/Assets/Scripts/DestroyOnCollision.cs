using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class DestroyOnCollision : MonoBehaviour
    {
        [SerializeField]
        private LayerMask destroyLayer;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 0.1f, destroyLayer);

            if (hit == true)
            {
                Destroy(this.gameObject);
            }
        }
    }

}