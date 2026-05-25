using UnityEngine;
using UnityEngine.InputSystem;

public class RositaCharacterController : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody RositaRB;
    public Transform CameraTransform;
    public GameObject ZonaDiCura, Sword;
    public AudioSource HealSource, SwingSword, Kick;
    public AudioClip HealClip, SwordHit, KickHit;
    public float MoveSpeed = 5f;
    public float RotationSpeed = 10f;
    public float GroundThreshold = 1f;
    public float JumpForce = 1f;
    float HealingCooldown = 20f;
    float currentHealingCooldown = 0f;
    float AttackCooldown = 0.5f;
    float currentAttackCooldown = 0f;


    Vector3 forward;
    Vector3 right;
    Vector3 Direction;

    float speedMultiplier = 1f;
    bool isAttacking, isAttackingSecondary, isAttackingThird, hasJumped, isInAir, isHealing, healSoundPlayed, isRunningWithSword, isJumping, isBlocking, isCasting, isUsingMagic, isTwerking;
    //booleane per il primo e secondo attacco della spada
    bool AlternativeAttack, OtherAttack, TransitionAttack;



    void Update()
    {
        // TIMER SCALA NEL TEMPO
        if (currentHealingCooldown > 0)
        {
            currentHealingCooldown -= Time.deltaTime;
        }

        // FIX COOLDOWN ATTACCO
        if (currentAttackCooldown > 0f)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
        else
        {
            currentAttackCooldown = 0f;
        }

        TransitionAttack = currentAttackCooldown > 0f;

        //salva l'informazione del corrente stato di animazione nella variabile state
        AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(0);

        //assegna a isPunching true se siamo nell'animazione dei pugni
        isAttacking = state.IsName("SwordAttack(1)");
        isAttackingSecondary = state.IsName("SecondSwordAttack");
        isAttackingThird = state.IsName("HighKick");
        isHealing = state.IsName("Cast");
        isRunningWithSword = state.IsName("SpeedRun");
        isJumping = state.IsName("Jump");
        isBlocking = state.IsName("Blocking");
        isUsingMagic = state.IsName("SecondCast");
        isTwerking = state.IsName("Dancing Twerk");

        //animazione attacco spada
        //animazione attacco spada
        if (Input.GetMouseButtonDown(0)
            && isGrounded()
            && !AlternativeAttack
            && !isBlocking
            && !OtherAttack
            && !TransitionAttack
            && !isAttacking
            && !isAttackingSecondary
            && !isAttackingThird
            && !isTwerking)
        {
            Animator.SetTrigger("SwordAttack");
            SwingSword.PlayOneShot(SwordHit);

            AlternativeAttack = true;
            currentAttackCooldown = AttackCooldown;
        }

        else if (Input.GetMouseButtonDown(0)
            && isGrounded()
            && AlternativeAttack
            && !isBlocking
            && !OtherAttack
            && !TransitionAttack
            && !isAttacking
            && !isAttackingSecondary
            && !isAttackingThird
            && !isTwerking)
        {
            Animator.SetTrigger("SecondSwordAttack");
            SwingSword.PlayOneShot(SwordHit);

            OtherAttack = true;
            currentAttackCooldown = AttackCooldown;
        }

        else if (Input.GetMouseButtonDown(0)
            && isGrounded()
            && AlternativeAttack
            && !isBlocking
            && OtherAttack
            && !TransitionAttack
            && !isAttacking
            && !isAttackingSecondary
            && !isAttackingThird)
        {
            Animator.SetTrigger("HighKick");
            Kick.PlayOneShot(KickHit);

            AlternativeAttack = false;
            OtherAttack = false;

            currentAttackCooldown = AttackCooldown;
        }


        //attacco spada (air variant)


        if (Input.GetMouseButtonDown(0) && !isGrounded())
        {
            Animator.SetTrigger("HighKick");
            Kick.PlayOneShot(KickHit);

        }


        //se stà correndo la spada viene disattivata
        Sword.SetActive(!isRunningWithSword);

        if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.Q))
        {

            Animator.SetTrigger("RightDoge");
        }


        //parata

        Animator.SetBool("Blocking", isBlocking);
        if (Input.GetMouseButton(1))
        {
            isBlocking = true;
            Animator.SetBool("Blocking", true);
        }

        else
        {
            isBlocking = false;
            Animator.SetBool("Blocking", false);
        }




        //animazione zona di cura
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentHealingCooldown <= 0f && !isBlocking)
        {
            Animator.SetTrigger("Cast");
            currentHealingCooldown = HealingCooldown;
        }
        //twerk


        if (isHealing)
        {
            ZonaDiCura.SetActive(true);

            if (!healSoundPlayed)
            {
                HealSource.PlayOneShot(HealClip);
                healSoundPlayed = true;
            }
        }
        else
        {
            ZonaDiCura.SetActive(false);
            healSoundPlayed = false;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Animator.SetTrigger("Twerk");

        }

        if (isTwerking)
        {
            Sword.SetActive(false);
        }
        else
        {
            Sword.SetActive(true);
        }



        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Animator.SetTrigger("Cast2");
        }

        #region Movimento
        //variabili di movimento asse X e Z
        float Horizontal = 0f;
        float Vertical = 0f;

        //calcolo di forward e right
        forward = CameraTransform.forward;
        right = CameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        //assegna i vari valori a Vertical e Horizontal in base al tasto premuto
        if (Input.GetKey(KeyCode.W))
            Vertical = 1f;

        if (Input.GetKey(KeyCode.S))
            Vertical = -1f;


        if (Input.GetKey(KeyCode.A))
            Horizontal = -1f;

        if (Input.GetKey(KeyCode.D))
            Horizontal = 1f;

        //calcolo di isMoving e isRunning
        bool isMoving = Mathf.Abs(Horizontal) > 0.01f || Mathf.Abs(Vertical) > 0.01f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        //metti un moltiplicatore per la velocità in base a cosa sta facendo il giocatore
        speedMultiplier = isAttacking ? 0f : isRunning ? 1f : isAttackingSecondary ? 0f : isBlocking ? 0f : isHealing ? 0f : isAttackingThird ? 0f : isUsingMagic ? 0f : 0.6f;

        //metti le animazioni corrette in base alla modalità di movimento del giocatore
        Animator.SetBool("SoftRun", isMoving && !isRunning);
        Animator.SetBool("SpeedRun", isRunning);

        //calcolo della direzione di movimento
        Direction = (forward * Vertical) + (right * Horizontal);
        Direction.Normalize();

        //calcolo e applicazione della velocità di movimento
        Vector3 velocity = Direction * MoveSpeed * speedMultiplier;
        RositaRB.linearVelocity = new Vector3(velocity.x, RositaRB.linearVelocity.y, velocity.z);

        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Direction, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
        #endregion

        //controllo del salto
        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            hasJumped = true;
            Animator.SetTrigger("Jump");
        }

        isInAir = !isGrounded();
    }

    private void FixedUpdate()
    {
        //fisica del salto
        if (hasJumped)
        {
            RositaRB.AddForce(0, JumpForce, 0);
            hasJumped = false;
        }
    }

    bool isGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (hit.distance > GroundThreshold)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}