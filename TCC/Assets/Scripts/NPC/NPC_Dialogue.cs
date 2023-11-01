using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml;
using System;

namespace ProjetoTCC
{
    public class NPC_Dialogue : MonoBehaviour
    {
        public static Action OnAnimationEnded;

        [SerializeField]
        private GameObject npcCanvas;

        [SerializeField]
        private TMP_Text textBox;

        [SerializeField]
        private int dialogueID;

        [SerializeField]
        private string[] dialogue_1;
        
        [SerializeField]
        private string[] dialogue_2;

        [SerializeField]
        private List<string> chatLines;

        private int chatID;

        private bool isTalking;


        void Start()
        {
            npcCanvas.SetActive(false);
            isTalking = false;

           
        }


        void Update()
        {

        }

        public void Interaction()
        {
            if(isTalking == false)
            {
                dialogueID = 0;

                if(chatLines != null)
                {
                    chatLines.Clear();
                }

                PreparDialogue();

                Dialogue();
                npcCanvas.SetActive(true);
                isTalking = true;
                OnAnimationEnd();
            }
            else
            {
                dialogueID += 1;
                Dialogue();
                OnAnimationEnd();
            }
        }

        public void Dialogue()
        {
            if(dialogueID < chatLines.Count)
            {
                textBox.text = chatLines[dialogueID];
            }
            else
            {
                switch(chatID)
                {
                    case 0:
                        chatID = 1;
                        break;

                    case 1:
                        break;
                }

                npcCanvas.SetActive(false);
                isTalking = false;
            }
            
        }

        public void PreparDialogue()
        {
            switch (chatID)
            {
                case 0:

                    foreach (string s in dialogue_1)
                    {
                        chatLines.Add(s);
                    }

                    break;

                case 1:

                    foreach (string s in dialogue_2)
                    {
                        chatLines.Add(s);
                    }

                    break;
            }
        }

        public void OnAnimationEnd()
        {
            OnAnimationEnded.Invoke();
        }
    }
}
