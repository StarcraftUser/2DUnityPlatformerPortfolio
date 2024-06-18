using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioFinish;
    public AudioClip audioReTry;

    public float maxSpeed;
    public float JumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;
    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    public void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("IsJumping"))
        {
            rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("IsJumping", true);
            PlaySound("JUMP");
        }
        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        //방향전환
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //에니메이션
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("IsWalking", false);
        else
            anim.SetBool("IsWalking", true);

    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        //Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        //Landing Platform
        //Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        if (rigid.velocity.y <= 0.1f)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.6f)
                    anim.SetBool("IsJumping", false);
                //Debug.Log(rayHit.collider.name);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.layer == 11)return;
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.layer != 7)
                //if (transform.position.y - 0.005f <= collision.transform.position.y)
                //{
                //    Debug.Log("플레이어가 맞았습니다.");
                //    OnDamaged(collision.transform.position);
                //    PlaySound("DAMAGED");
                //    return;
                //}
                if (transform.position.y - 0.005f > collision.transform.position.y)
                {
                    gameManager.statePoint += 100;
                    OnAttack(collision);
                    return;
                }
            Debug.Log("플레이어가 맞았습니다.");
            OnDamaged(collision.transform.position);
        }
    }

    public void OnDamaged(Vector2 targetPos)
    {
        if (gameObject.layer == 11) return;

        gameManager.HealthDown();

        gameObject.layer = 11;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;

        rigid.AddForce(new Vector2(dirc, 1) * 10, ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");

        PlaySound("DAMAGED");

        Invoke("OffDamaged", 3);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnAttack(Collision2D collision)
    {
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        EnemyMove enemyMove = collision.transform.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
        PlaySound("ATTACK");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Items")
        {
            bool isCopper = collision.gameObject.name.Contains("Copper");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isCopper)
                gameManager.statePoint += 50;
            else if (isSilver)
                gameManager.statePoint += 100;
            else if (isGold)
                gameManager.statePoint += 300;
            PlaySound("ITEM");
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            PlaySound("FINISH");
            gameManager.NextStage();
        }
    }

    public void OnDie()
    {
        PlaySound("DIE");
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);


        Invoke("DeActive", 1.5f);
    }
    void DeActive()
    {
        PlaySound("RETRY");
        Time.timeScale = 0f;
        return;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "RETRY":
                audioSource.clip = audioReTry;
                break;
        }
        audioSource.Play();
    }
}
