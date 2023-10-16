using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjetoTCC
{
    public enum EenemyState 
    { 
        STOPPED,
        ALERT,
        PATROL,
        ATTACKING    
    }

    public class Farmer : MonoBehaviour
    {
        private PlayerScript playerScript;

        #region Variavies de Componentes
#pragma warning disable CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
        private Rigidbody2D rigidbody2D;
#pragma warning restore CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
        
        private Animator animator;

        [SerializeField]
        private EenemyState currentEnemyState;
        #endregion

        [Header("Configuração para a Movimentação da IA")]
        #region Variaveis para a Movimentação da IA
        
        [SerializeField]
        private float changeRoteDistance;

        [SerializeField]
        private LayerMask obstaclesLayer;

        [SerializeField]
        private float seeCharacterDistance;

        [SerializeField]
        private LayerMask characterLayer;

        [SerializeField]
        private float atackDistance;

        [SerializeField]
        private float leaveAlertDistance;

        private Vector3 dir = Vector3.right;
        
        [SerializeField]
        private float baseSpeed;
        
        [SerializeField]
        private float Speed;
        
        [SerializeField]
        private bool isLookingLeft;

        [SerializeField]
        private float waitingTimeIdle;

        #endregion
        void Start()
        {
            playerScript = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            if(isLookingLeft)
            {
                Flip();
            }
        }


        void Update()
        {
            UnityEngine.Debug.DrawRay(transform.position, dir * seeCharacterDistance, Color.red);
            RaycastHit2D hitCharacter = Physics2D.Raycast(transform.position, dir, seeCharacterDistance, characterLayer);
            if(hitCharacter)
            {
                ChangeState(EenemyState.ALERT);
            }

            if (currentEnemyState == EenemyState.PATROL)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, changeRoteDistance, obstaclesLayer);

                if (hit == true)
                {
                    ChangeState(EenemyState.STOPPED);
                }
            }

            rigidbody2D.velocity = new Vector2(Speed, rigidbody2D.velocity.y);

            if(Speed == 0)
            {
                animator.SetInteger("idAnimation", 0);
            }
            else if(Speed != 0)
            {
                animator.SetInteger("idAnimation", 1);
            }

            if(currentEnemyState == EenemyState.ALERT)
            {
                float dist = Vector3.Distance(transform.position, playerScript.transform.position);

                if(dist <= atackDistance)
                {
                    ChangeState(EenemyState.ATTACKING);
                }
                else if(dist >= leaveAlertDistance)
                {
                    print("Saiu alerta");
                    ChangeState(EenemyState.STOPPED);
                }
            }



        }

        void Flip()
        {
            isLookingLeft = !isLookingLeft; // INVERTE O VALOR DA VARIAVEL BOLEANA
            float x = transform.localScale.x;
            x *= -1; // INVERTE O FINAL DO SCALE X
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
            baseSpeed *= -1;
            dir.x = x;
        }

        IEnumerator Idle()
        {
            yield return new WaitForSeconds(waitingTimeIdle);
            Flip();
            ChangeState(EenemyState.PATROL);
        }

        public void ChangeState(EenemyState newState)
        {
            currentEnemyState = newState;

            switch(newState)
            {
                case EenemyState.STOPPED:
                    Speed = 0;
                    StartCoroutine(nameof(Idle));
                    break;
                case EenemyState.PATROL:
                    Speed = baseSpeed;
                    break;
                case EenemyState.ALERT:
                    print("alerta");
                    Speed = 0;
                    break;
                case EenemyState.ATTACKING:
                    print("ataque");
                    break;
            }
        }
    }
}
