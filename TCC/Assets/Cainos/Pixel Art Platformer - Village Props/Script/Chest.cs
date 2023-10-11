using UnityEngine;
using Cainos.LucidEditor;
using System;

namespace Cainos.PixelArtPlatformer_VillageProps
{
    public class Chest : MonoBehaviour
    {
        public static Action OnAnimationEnded;

        [FoldoutGroup("Reference")]
        public Animator animator;

        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;
                animator.SetBool("IsOpened", isOpened);

            }
        }
        private bool isOpened;
        public void Open()
        {
            IsOpened = true;
        }
        public void Close()
        {
            IsOpened = false;
        }

        public void Interaction()
        {
            IsOpened = !IsOpened;
        }

        public void OnAnimationEnd()
        {
            OnAnimationEnded.Invoke();
        }
    }
}
