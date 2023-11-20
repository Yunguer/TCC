using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjetoTCC
{
    public class BossIA : MonoBehaviour
    {
        private enum BossRoutine
        {
            A, 
            B,
            C,
            D
        }

        private BossRoutine currentBossRoutine;

        private Rigidbody2D bossRb;
        
        private Animator bossAnimator;

        [SerializeField]
        private float speed;

        private int h;

        private bool needToMove;

        [SerializeField]
        private Transform[] wayPoints;

        private Transform target;

        private int movesetId;

        private float waitTime;

        private float currentTime;

        [SerializeField]
        private bool isLookingLeft;

        public float jumpForce;

        [SerializeField]
        private GameObject fireBall;

        private GameObject tempFireBall;

        [SerializeField]
        private Transform fireBallSpawnPosition;

        private Transform playerPosition;

        [SerializeField]
        private GameObject teste;

        private Vector2 targetDirection;

        private int shootCount = 0;

        [SerializeField]
        private Transform groundCheck;

        private bool isTouchingGround;
        private int idAnimation;


        // Start is called before the first frame update
        void Start()
        {
            bossRb = GetComponent<Rigidbody2D>();
            bossAnimator = GetComponent<Animator>();
            //playerPosition = FindObjectOfType<PlayerScript>().transform;

            currentBossRoutine = BossRoutine.A;
            movesetId = 0;
            currentTime = 0;
            waitTime = 3;
        }

        // Update is called once per frame
        void Update()
        {
            switch(currentBossRoutine)
            {
                case BossRoutine.A:

                    switch(movesetId)
                    {
                        case 0:
                            currentTime += Time.deltaTime;
                            if(currentTime >= waitTime)
                            {
                                movesetId = 1;
                                target = wayPoints[1];
                                h = -1;
                                needToMove = true;
                            }

                            break;

                        case 1:
                            if(transform.position.x <= target.position.x)
                            {
                                movesetId = 2;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                            }

                            break;

                        case 2:
                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 3;
                                target = wayPoints[0];
                                h = 1;
                                needToMove = true;
                            }

                            break;

                        case 3:
                            if (transform.position.x >= target.position.x)
                            {
                                h = 0;
                                currentBossRoutine = BossRoutine.B;
                                movesetId = 0;
                                currentTime = 0;
                                waitTime = 3;
                            }
                            break;
                    }

                    break;

                case BossRoutine.B:

                    switch (movesetId)
                    {
                        case 0:

                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 1;
                                target = wayPoints[1];
                                h = -1;
                                needToMove = true;
                            }

                            break;

                        case 1:

                            if (transform.position.x <= target.position.x)
                            {
                                movesetId = 2;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                            }

                            break;

                        case 2:

                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 3;
                                target = wayPoints[2];
                                h = 1;
                                needToMove = true;

                            }

                            break;

                        case 3:

                            if (transform.position.x >= target.position.x)
                            {
                                h = 0;
                                movesetId = 4;
                                bossRb.AddForce(new Vector2(0, 350));
                                idAnimation = 0;
                            }
                            break;

                        case 4:

                            Invoke(nameof(FireAtack), 2);
                            currentTime = 0;
                            waitTime = 3;
                            break;
                        
                        case 5:

                            shootCount = 0;
                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 6;
                                needToMove = false;
                                bossRb.AddForce(new Vector2(165, 250));
                                idAnimation = 0;
                                currentTime = 0;
                                waitTime = 1;
                            }
                            break;
                        
                        case 6:

                            currentTime += Time.deltaTime;
                            if(currentTime >= waitTime)
                            {
                                if (isTouchingGround)
                                {
                                    target = wayPoints[2];
                                    h = -1;
                                    needToMove = true;
                                    movesetId = 7;
                                    currentTime = 0;
                                    waitTime = 2;
                                }
                            }
                            
                            break;

                        case 7:
                            if (transform.position.x <= target.position.x)
                            {
                                h = 0;
                                currentTime += Time.deltaTime;
                                if(currentTime >= waitTime)
                                {
                                    int rand = Random.Range(0, 100);
                                    rand = 60;
                                    if (rand < 50)
                                    {
                                        target = wayPoints[0];
                                        h = 1;
                                        movesetId = 8;
                                    }
                                    else
                                    {
                                        target = wayPoints[1];
                                        h = -1;
                                        movesetId = 9;
                                    }
                                }
                            }

                            break;

                        case 8: 
                            if(transform.position.x >= target.position.x)
                            {
                                movesetId = 0;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                                currentBossRoutine = BossRoutine.A;
                            }

                            break;

                        case 9: 

                            if (transform.position.x <= target.position.x)
                            {
                                movesetId = 0;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                                currentBossRoutine = BossRoutine.C;
                            }
                            break;
                    }

                    break;

                case BossRoutine.C:

                    switch(movesetId)
                    {
                        case 0:

                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 1;
                                target = wayPoints[0];
                                h = 1;
                                needToMove = true;
                            }

                            break;

                        case 1:

                            if (transform.position.x >= target.position.x)
                            {
                                movesetId = 2;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                            }

                            break;

                        case 2:

                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 3;
                                target = wayPoints[1];
                                h = -1;
                                needToMove = true;

                            }

                            break;

                        case 3:
                            if (transform.position.x <= target.position.x)
                            {
                                h = 0;
                                currentBossRoutine = BossRoutine.D;
                                movesetId = 0;
                                currentTime = 0;
                                waitTime = 3;
                            }
                            break;
                    }

                    break;

               case BossRoutine.D:

                    switch (movesetId)
                    {
                        case 0:

                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 1;
                                target = wayPoints[0];
                                h = 1;
                                needToMove = true;
                            }

                            break;

                        case 1:

                            if (transform.position.x >= target.position.x)
                            {
                                movesetId = 2;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                            }

                            break;

                        case 2:

                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 3;
                                target = wayPoints[2];
                                h = -1;
                                needToMove = true;

                            }

                            break;

                        case 3:

                            if (transform.position.x <= target.position.x)
                            {
                                h = 0;
                                movesetId = 4;
                                bossRb.AddForce(new Vector2(0, 350));
                                idAnimation = 0;
                            }
                            break;

                        case 4:

                            Invoke(nameof(FireAtack), 2);
                            currentTime = 0;
                            waitTime = 3;
                            break;

                        case 5:

                            shootCount = 0;
                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                movesetId = 6;
                                needToMove = false;
                                bossRb.AddForce(new Vector2(-165, 250));
                                idAnimation = 0;
                                currentTime = 0;
                                waitTime = 1;
                            }
                            break;

                        case 6:

                            currentTime += Time.deltaTime;
                            if (currentTime >= waitTime)
                            {
                                if (isTouchingGround)
                                {
                                    target = wayPoints[2];
                                    h = 1;
                                    needToMove = true;
                                    movesetId = 7;
                                    currentTime = 0;
                                    waitTime = 2;
                                }
                            }

                            break;

                        case 7:
                            if (transform.position.x >= target.position.x)
                            {
                                h = 0;
                                currentTime += Time.deltaTime;
                                if (currentTime >= waitTime)
                                {
                                    int rand = Random.Range(0, 100);
                                    if (rand < 50)
                                    {
                                        target = wayPoints[0];
                                        h = 1;
                                        movesetId = 8;
                                    }
                                    else
                                    {
                                        target = wayPoints[1];
                                        h = -1;
                                        movesetId = 9;
                                    }
                                }
                            }

                            break;

                        case 8:
                            if (transform.position.x >= target.position.x)
                            {
                                movesetId = 0;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                                currentBossRoutine = BossRoutine.A;
                            }

                            break;

                        case 9:

                            if (transform.position.x <= target.position.x)
                            {
                                movesetId = 0;
                                currentTime = 0;
                                waitTime = 3;
                                h = 0;
                                currentBossRoutine = BossRoutine.C;
                            }
                            break;
                    }

                    break;
                
            }

            if(h > 0 && isLookingLeft)
            {
                Flip();
            }
            else if(h < 0 && !isLookingLeft)
            {
                Flip();
            }

            if(needToMove)
            {
                bossRb.velocity = new Vector2(h * speed, bossRb.velocity.y);
            }


            isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, 0.02f);

            bossAnimator.SetBool("jump", isTouchingGround);
            bossAnimator.SetInteger("horizontal", h);
            bossAnimator.SetInteger("idAnimation", idAnimation);
            bossAnimator.SetFloat("speedY", bossRb.velocity.y);
        }

        void Flip()
        {
            isLookingLeft = !isLookingLeft;
            float x = transform.localScale.x * -1;
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        private void MoveToPlayer()
        {
            if(tempFireBall != null)
            {
                tempFireBall.transform.Translate(5 * Time.fixedDeltaTime * targetDirection, Space.Self);
            }

        }

        private void FireAtack()
        {
            if (shootCount < 3 && tempFireBall == null)
            {
                shootCount++;
                CancelInvoke(nameof(MoveToPlayer));
                tempFireBall = Instantiate(fireBall, fireBallSpawnPosition.position, fireBallSpawnPosition.localRotation);  
                targetDirection = (teste.transform.position - tempFireBall.transform.position).normalized;
                InvokeRepeating(nameof(MoveToPlayer), 0, 1 / 30f);
                Destroy(tempFireBall, 3f);
            }
            
            if(shootCount == 3)
            {
                movesetId = 5;
            }
        }

    }
}
