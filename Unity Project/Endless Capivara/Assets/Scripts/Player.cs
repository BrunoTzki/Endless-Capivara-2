using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer playerSpriteRenderer;

    public float laneSpeed;
    public float speed;
    public float jumpLength;
    public float jumpHeight;
    public float slideLength;
    public float slideHeight;
    public int MaxLife = 3;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public GameObject model;
    public float invincibleTime;
    public float multiplySpeed;

    public GameObject particleUW;
    public GameObject particleRiver;


    public float score; 
    public int coins;

    private float speedMemory;
    private Rigidbody rb;
    //private BoxCollider boxCollider;
    private int currentLane = 1;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;
    private int currentlaneY = 2;
    private bool submerged = false;
    //private Vector3 boxColliderSize;
    private bool isSwipping = false;
    private Vector2 startingTouch;
    private int currentLife;
    private bool invincible = false;
    private UiManager uiManager;
    private CameraFollow cameraFollow;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //boxCollider = GetComponent<BoxCollider>();
        //boxColliderSize = boxCollider.size;
        currentLife = MaxLife;
        speed = minSpeed;
        uiManager = FindObjectOfType<UiManager>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        GameManager.gm.StartMissions();
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * speed;
        uiManager.UpdateScore((int)score);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(+1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Submerge(-1);
        }

        if(Input.touchCount == 1)
        {
            if (isSwipping)
            {
                Vector2 diff = Input.GetTouch(0).position - startingTouch;
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);
                if(diff.magnitude > 0.01f)
                {

                    if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        anim.SetFloat("Touch Y", diff.y);

                        //DownKey
                        if (diff.y < 0)
                        {

                            if (jumping)
                            {
                                CancelJump();
                            }
                            else
                            {
                                Submerge(-2);
                                submerged = true;

                            }
                        }
                        else
                        //UpKey
                        {
                            //if Submerged
                            if (currentlaneY == 0)
                            {
                                Submerge(2);
                                submerged = false;
                                cameraFollow.submerged = false;
                                
                            }
                            else
                            {
                            //Not Submerged
                                Jump();;
                            }
                        }
                    }
                    else
                    {
                        anim.SetFloat("Touch X", diff.x);

                        if (diff.x < 0)
                        {
                            ChangeLane(-1);
                        }
                        else
                        {
                            ChangeLane(1);
                        }
                    }

                    isSwipping = false;
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startingTouch = Input.GetTouch(0).position;
                isSwipping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isSwipping = false;
                anim.SetFloat("Touch Y", 0);
                anim.SetFloat("Touch X", 0);
            }
        }       


            if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if (ratio >= 1f)
            {
                jumping = false;
                anim.SetBool("Jumping", false);
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }
        }
        else
        {
            if (!submerged)
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }


        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);


    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;

        // Checagem de posição do Player no Grid

        if (rb.position.y > -0.5 && rb.position.y < 0.2)
        {
            particleRiver.SetActive(true);

        }
        else
        {
            particleRiver.SetActive(false);
        }

        if (rb.position.y <= 0.2)
        {
            particleUW.SetActive(true);
        }
        else
        {
            particleUW.SetActive(false);
            anim.SetBool("ToSurface", false);
        }

        if (rb.position.y <= -0.5)
        {
            anim.SetBool("ToSurface", true);
            playerSpriteRenderer.sortingOrder = -1;
        }
        else
        {
            anim.SetBool("ToSurface", false); 
            playerSpriteRenderer.sortingOrder = 1;

        }



        if (rb.position.x < -0.5f)
        {
            anim.SetBool("ToL", false);
        }
        else
        {
            anim.SetBool("ToL", true);
        }
        if (rb.position.x > 0.5f)
        {
            anim.SetBool("ToR", false);
        }
        else
        {
            anim.SetBool("ToR", true);
        }
    }

    void ChangeLane(int direction)
    {
        int targetlane = currentLane + direction;
        if (targetlane < 0 || targetlane > 2)
            return;
        currentLane = targetlane;
        verticalTargetPosition = new Vector3((currentLane - 1), transform.position.y, 0);
    }
    void Submerge(int directionY)
    {
        if (!jumping)
        {
            cameraFollow.submerged = true;
            int targetlaneY = currentlaneY + directionY;
            if (targetlaneY < 0 || targetlaneY > 2)
                return;
            currentlaneY = targetlaneY;
            verticalTargetPosition = new Vector3(transform.position.x, (currentlaneY - 2), 0);
        }
    }

    void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLength);
            anim.SetBool("Jumping", true);
            jumping = true;
        }
    }

    void CancelJump()
    {
        if (verticalTargetPosition.y > 0)
        {
            jumping = false;
            anim.SetBool("Jumping", false);
        }
    }

     
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins++;
            uiManager.UpdateCoins(coins);
            other.transform.parent.gameObject.SetActive(false);
        }
        if (invincible)
            return;

        if(other.CompareTag("Obstacle"))
        {
            currentLife--;
            uiManager.UpdateLive(currentLife);
            anim.SetTrigger("Hit");
            speedMemory = speed;
            speed = 0;
            if (currentLife <= 0)
            {
                //GAME OVER

                speed = 0;
                anim.SetBool("Dead", true);
                uiManager.gameOverPanel.SetActive(true);
                Invoke("CallMenu", 2f);
            }
            else
            {
                StartCoroutine(Blinking(invincibleTime));
            }
        }
    }

    IEnumerator Blinking(float time)
    {
        invincible = true;
        float timer = 0;
        float currentBlinking = 1f;
        float lastBlinking = 0f;
        float blinkingPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(1f);
        //speed = ini
        speed = speedMemory;
        while(timer < time && invincible)
        {
            model.SetActive(enabled);
            yield return null;
            timer += Time.deltaTime;
            lastBlinking += Time.deltaTime;
            if (blinkingPeriod < lastBlinking)
            {
                lastBlinking = 0;
                currentBlinking = 1f - currentBlinking;
                enabled = !enabled;
            }
        }
        model.SetActive(true);
        invincible = false;

    }
    
    void CallMenu()
    {
        GameManager.gm.EndRun();
    }

    public void IncreaseSpeed()
    {
        speed *= multiplySpeed;
        if (speed >= maxSpeed)
            speed = maxSpeed;
    }
}
