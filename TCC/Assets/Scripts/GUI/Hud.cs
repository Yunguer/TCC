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
        private _GameController _GameController;
        #endregion

        [Header("Configuração Para a Barra de Vida")]
        #region Variaveis para a Barra de Vida
        [SerializeField]
        private Image[] hpBar;
        [SerializeField]
        private Image[] mpBar;
        [SerializeField]
        private GameObject mpHUD;
        #endregion

        void Start()
        {
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;

            mpHUD.SetActive(false);
            StartCoroutine(nameof(CheckClassManaBar));
        }


        void Update()
        {
            LifeBarController();
            if (mpHUD.activeSelf == true)
            {

                ManaBarController();
            }
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
        void ManaBarController()
        {
            float manaPercentage = (float)playerScript.CurrentMana / (float)playerScript.MaxMana;

            foreach (Image img in mpBar)
            {
                img.enabled = true;
            }

            if (manaPercentage >= 1)
            {

            }
            else if (manaPercentage >= 0.9f)
            {
                mpBar[9].enabled = false;
            }
            else if (manaPercentage >= 0.8f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
            }
            else if (manaPercentage >= 0.7f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
            }
            else if (manaPercentage >= 0.6f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
            }
            else if (manaPercentage >= 0.5f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
            }
            else if (manaPercentage >= 0.4f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
            }
            else if (manaPercentage >= 0.3f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
            }
            else if (manaPercentage >= 0.2f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
                mpBar[2].enabled = false;
            }
            else if (manaPercentage >= 0.1f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
                mpBar[2].enabled = false;
                mpBar[1].enabled = false;
            }
            else if (manaPercentage >= 0.0f)
            {
                mpBar[9].enabled = false;
                mpBar[8].enabled = false;
                mpBar[7].enabled = false;
                mpBar[6].enabled = false;
                mpBar[5].enabled = false;
                mpBar[4].enabled = false;
                mpBar[3].enabled = false;
                mpBar[2].enabled = false;
                mpBar[1].enabled = false;
                mpBar[0].enabled = false;
            }

        }

        IEnumerator CheckClassManaBar()
        {
            yield return new WaitForSeconds(0.1f);
            if (_GameController.CharacterClassID[_GameController.CharacterID] == 2)
            {
                mpHUD.SetActive(true);
                
            }
        }
    }
}
