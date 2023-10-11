using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetoTCC
{
    public class Hud : MonoBehaviour
    {
        #region Comunicação Entre Scripts
        private PlayerScript playerScript;
        #endregion

        [Header("Configuração Para a Barra de Vida")]
        #region Variaveis para a Barra de Vida
        [SerializeField]
        private Image[] hpBar;
        [SerializeField]
        private Sprite healthUnit;
        #endregion

        void Start()
        {
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
        }


        void Update()
        {
            LifeBarController();
        }

        void LifeBarController()
        {
            float lifePercentage = (float)playerScript.CurrentLife / (float)playerScript.MaxLife;

            foreach(Image img in hpBar)
            {
                img.enabled = true;
            }

            if(lifePercentage >= 1)
            {

            }
            else if(lifePercentage >= 0.9f)
            {
                hpBar[9].enabled = false;
            }
            else if (lifePercentage >= 0.8f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
            }
            else if (lifePercentage >= 0.7f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
            }
            else if (lifePercentage >= 0.6f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
            }
            else if (lifePercentage >= 0.5f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
            }
            else if (lifePercentage >= 0.4f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
            }
            else if (lifePercentage >= 0.3f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
            }
            else if (lifePercentage >= 0.2f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
                hpBar[2].enabled = false;
            }
            else if (lifePercentage >= 0.1f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
                hpBar[2].enabled = false;
                hpBar[1].enabled = false;
            }
            else if (lifePercentage >= 0.0f)
            {
                hpBar[9].enabled = false;
                hpBar[8].enabled = false;
                hpBar[7].enabled = false;
                hpBar[6].enabled = false;
                hpBar[5].enabled = false;
                hpBar[4].enabled = false;
                hpBar[3].enabled = false;
                hpBar[2].enabled = false;
                hpBar[1].enabled = false;
                hpBar[0].enabled = false;
            }

        }
    }
}
