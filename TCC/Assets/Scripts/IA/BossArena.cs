using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class BossArena : MonoBehaviour
    {
        [SerializeField]
        private GameObject leftWall;

        [SerializeField]
        private GameObject rightWall;

        [SerializeField]
        private Transform cameraPosition;

        private CinemachineVirtualCamera vcam;

        private bool firstTimeCheck;

        private BossIA bossIA;


        void Start()
        {
            vcam = FindObjectOfType(typeof(CinemachineVirtualCamera)) as CinemachineVirtualCamera;
            bossIA = FindObjectOfType(typeof(BossIA)) as BossIA;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            switch (col.gameObject.tag)
            {
                case "Player":

                    if(!firstTimeCheck)
                    {
                        firstTimeCheck = true;
                        leftWall.SetActive(true);
                        rightWall.SetActive(true);
                        vcam.Follow = cameraPosition;
                        bossIA.CurrentBossRoutine = BossIA.BossRoutine.A;
                    }
                    break;
            }
        }
    }
}

