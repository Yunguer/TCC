using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjetoTCC
{
    public class Title : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SelectCharacter(int titleCharacterID)
        {
            PlayerPrefs.SetInt("titleCharacterID", titleCharacterID);
            SceneManager.LoadScene("Cena_1");
        }
    }

}