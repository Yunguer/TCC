using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class MissionCheck : MonoBehaviour
    {
        private EnemyDamageController enemyDamageController;
        private _GameController _GameController;

        void Start()
        {
            enemyDamageController = gameObject.GetComponent<EnemyDamageController>();
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        }

        void Update()
        {
            if(enemyDamageController.Dead)
            {
                _GameController.Mission1Count = _GameController.Mission1Count + 1;
                if(_GameController.Mission1Count == 2)
                {
                    _GameController.Mission1 = true;
                }
            }
        }
    }
}
