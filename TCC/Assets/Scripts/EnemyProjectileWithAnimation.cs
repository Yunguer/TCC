using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public class EnemyProjectileWithAnimation : MonoBehaviour
    {
        private PlayerScript playerScript;
        private _GameController _GameController;
        private SpriteRenderer spriteRenderer;
        private bool alreadyHit = false;
        private Animator animator;
        void Start()
        {
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            switch (col.gameObject.tag)
            {
                case "Player":

                    if (playerScript.TookHit == false && alreadyHit == false)
                    {
                        alreadyHit = true;
                        animator.enabled = false;
                        spriteRenderer.sprite = null;
                        playerScript.TookHit = true;
                        playerScript.PlayerRb.velocity = new Vector2(0, playerScript.PlayerRb.velocity.y);
                        _GameController.CurrentLife -= 1;
                        playerScript.PlayerAnimator.SetTrigger("hit");

                        if (gameObject.transform.position.x > playerScript.transform.position.x && !playerScript.IsLookingLeft)
                        {
                            GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, playerScript.transform.localRotation);
                            Destroy(fxTemp, 1);
                        }
                        else if (gameObject.transform.position.x > playerScript.transform.position.x && playerScript.IsLookingLeft)
                        {
                            GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, transform.localRotation);
                            fxTemp.transform.localScale = new Vector3(fxTemp.transform.localScale.x * -1, fxTemp.transform.localScale.y, fxTemp.transform.localScale.z);
                            Destroy(fxTemp, 1);
                        }
                        else if (gameObject.transform.position.x < playerScript.transform.position.x && !playerScript.IsLookingLeft)
                        {
                            GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, playerScript.transform.localRotation);
                            fxTemp.transform.localScale = new Vector3(fxTemp.transform.localScale.x * -1, fxTemp.transform.localScale.y, fxTemp.transform.localScale.z);
                            Destroy(fxTemp, 1);
                        }
                        else if (gameObject.transform.position.x < playerScript.transform.position.x && playerScript.IsLookingLeft)
                        {
                            GameObject fxTemp = Instantiate(_GameController.DamageFX[0], playerScript.VfxPosition.position, playerScript.transform.localRotation);
                            fxTemp.transform.localScale = new Vector3(fxTemp.transform.localScale.x * -1, fxTemp.transform.localScale.y, fxTemp.transform.localScale.z);
                            Destroy(fxTemp, 1);
                        }

                        if (gameObject.transform.position.x > playerScript.transform.position.x && playerScript.IsLookingLeft)
                        {
                            playerScript.KxTemp = playerScript.KnockX * -1;
                            print(playerScript.KxTemp);
                        }
                        else if (gameObject.transform.position.x > playerScript.transform.position.x && !playerScript.IsLookingLeft)
                        {
                            playerScript.KxTemp = playerScript.KnockX;
                            print(playerScript.KxTemp);
                        }
                        else if (gameObject.transform.position.x < playerScript.transform.position.x && playerScript.IsLookingLeft)
                        {
                            playerScript.KxTemp = playerScript.KnockX;
                            print(playerScript.KxTemp);
                        }
                        else if (gameObject.transform.position.x < playerScript.transform.position.x && !playerScript.IsLookingLeft)
                        {
                            playerScript.KxTemp = playerScript.KnockX * -1;
                            print(playerScript.KxTemp);
                        }

                        playerScript.KnockPosition.localPosition = new Vector3(playerScript.KxTemp, playerScript.KnockPosition.localPosition.y, 0);

                        GameObject knockTemp = Instantiate(playerScript.KnockForcePrefab, playerScript.KnockPosition.position, playerScript.KnockPosition.localRotation);
                        Destroy(knockTemp, 0.02f);

                        if (_GameController.CurrentLife <= 0)
                        {
                            playerScript.PlayerRb.velocity = new Vector2(0, playerScript.PlayerRb.velocity.y);
                        }

                        StartCoroutine(nameof(InvulnerablePlayer));
                    }
                    break;

                case "Ground":
                    if(alreadyHit == false)
                    {
                        Destroy(this.gameObject);
                    }
                    
                    break;

            }

        }

        IEnumerator InvulnerablePlayer()
        {
            playerScript.PRender.color = playerScript.CharacterColor[1];
            yield return new WaitForSeconds(0.4f);
            playerScript.PRender.color = playerScript.CharacterColor[0];
            playerScript.TookHit = false;
            Destroy(this.gameObject);
        }
    }
}
