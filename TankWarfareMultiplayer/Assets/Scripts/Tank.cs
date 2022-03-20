using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Tank : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    GameManager gameManager;

    float MovemenetPerTurn = 5;
    float MovementLeft;
    private bool facingRight;

    public float Speed = 5;
    float TurretSpeed = 180;
    float TurretPowerSpeed = 10;

  

    public GameObject CurrentBulletPrefab;
    public Transform TurretPivot;
    public Transform BulletSpawnPoint;

    static public Tank LocalTank{ get; protected set; }


    [SyncVar]
    public bool tankTurn = false;

    [SyncVar (hook = "OnTurretAngleChange")]
    float turretAngle = 0f;


    float turretPower = 10f;

    [SyncVar]
    Vector3 serverPosition;






    Vector3 serverPositionSmoothVelocity;


   // [SyncVar]
    public bool TankIsLockedIn { get; protected set; }
    

    void NewTurn()
    {
        MovementLeft = MovemenetPerTurn;
    }



    // Update is called once per frame
    void Update()
    {
        if( isServer)
        {

        }

        if( hasAuthority && tankTurn == true)
        {

            LocalTank = this;

            AuthorityUpdate();
        }
       



        if ( hasAuthority == false || tankTurn == false)
        {
           // RpcFixPosition(transform.position);
           // transform.position = Vector3.SmoothDamp(transform.position, serverPosition,ref serverPositionSmoothVelocity , 0.25f);
            if(!hasAuthority)
            {
               // RpcFixPosition(Vector3.SmoothDamp(transform.position, serverPosition, ref serverPositionSmoothVelocity, 0.25f));
            }
            // RpcFixPosition(transform.position);
            //RpcFixPosition(Vector3.SmoothDamp(transform.position, serverPosition, ref serverPositionSmoothVelocity, 0.25f));
            // ChangePosition(Vector3.SmoothDamp(transform.position, serverPosition, ref serverPositionSmoothVelocity, 0.25f));
            //ChangePosition(transform.position);


            CmdUpdatePosition(Vector3.SmoothDamp(transform.position, serverPosition, ref serverPositionSmoothVelocity, 0.25f));
            //RpcFixPosition(Vector3.SmoothDamp(transform.position, serverPosition, ref serverPositionSmoothVelocity, 0.25f));
        }

        Vector3 euler = transform.eulerAngles;
        if (euler.z > 180) euler.z = euler.z - 360;
        euler.z = Mathf.Clamp(euler.z, -45, 45);
        transform.eulerAngles = euler;
        TurretPivot.localRotation = Quaternion.Euler(0, 0, turretAngle);

    }

    void AuthorityUpdate()
    {
        if(GameManager.Instance().IsProcessingEvent())
        {
            return;
        }


        AuthorityUpdateMovement();
        AuthorityUpdateShooting();

        GameObject pn_go = GameObject.Find("PowerNumber");
        pn_go.GetComponent<UnityEngine.UI.Text>().text = turretPower.ToString("#.00");

    }



    void AuthorityUpdateMovement()
    {

        if(TankIsLockedIn == true || gameManager.TankCanMove( this) == false)
        {
            return;
        }



        float movement = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float jumpVelocity = 1.2f;
        
        //float Jump = Input.GetAxis("Horizontal") * jumpVelocity * Time.deltaTime;


        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            movement *= 0.1f;
        }
        float jump = Input.GetAxis("Vertical") * Speed * Time.deltaTime; ;


        if(Input.GetKey(KeyCode.UpArrow))
        {

          jump *= jumpVelocity;
        }


        transform.Translate(movement, jump, 0);
        //Flip(movement);
        CmdUpdatePosition(transform.position);


        if (Input.GetKeyUp(KeyCode.Space))
        {
            TankIsLockedIn = true;
            CmdLockIn();
        }
    }



    void AuthorityUpdateShooting()
    {

        if (TankIsLockedIn == true || gameManager.TankCanShoot(this) == false)
        {
            return;
        }

        float turretMovement = Input.GetAxis("TurretHorizontal") * TurretSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
        {
            turretMovement *= 0.1f;
        }

        turretAngle = Mathf.Clamp(turretAngle + turretMovement, 0 , 180);
        CmdChangeTurretAngle(turretAngle);

        float powerChange = Input.GetAxis("Vertical") *TurretPowerSpeed* Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            powerChange *= 0.1f;
        }

        turretPower = Mathf.Clamp(turretPower + powerChange, 0, 20);
        CmdSetTurretPower(turretPower);



        if (Input.GetKeyUp(KeyCode.Space))
        {
            TankIsLockedIn = true;
            CmdLockIn();


            //Vector2 velocity = new Vector2(turretPower * Mathf.Cos(turretAngle * Mathf.Deg2Rad), turretPower * Mathf.Sin(turretAngle * Mathf.Deg2Rad));
            //CmdFireBullet(BulletSpawnPoint.position, velocity);
        }

    }


    public void Fire()
    {
        if(tankTurn == false)
        {
            return;
        }
        Vector2 velocity = new Vector2(turretPower * Mathf.Cos(turretAngle * Mathf.Deg2Rad), turretPower * Mathf.Sin(turretAngle * Mathf.Deg2Rad));
        CmdFireBullet(BulletSpawnPoint.position, velocity);
    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            float turretMovement = Input.GetAxis("TurretHorizontal") * TurretSpeed * Time.deltaTime;
            turretAngle = Mathf.Clamp(turretAngle + turretMovement, 0, 180);
            CmdChangeTurretAngle(turretAngle);

            ChangePosition(theScale);
            CmdUpdatePosition(transform.position);

        }
    }


    [Command(requiresAuthority = false)]
    public void ChangePosition(Vector3 newPosition)
    {
        CmdUpdatePosition(newPosition);
        RpcFixPosition(newPosition);
    }




    [Command(requiresAuthority = false)]
    public void CmdChangeTurn()
    {
        tankTurn = !tankTurn;
    }


    [Command(requiresAuthority = false)]
    public void DestroyTank()
    {
       // NetworkServer.Destroy(this);
    }




    [Command]
    void CmdLockIn()
    {
        TankIsLockedIn = true;
    }



    [Command]
    void CmdChangeTurretAngle(float angle)
    {
        turretAngle = angle;
    }

    [Command]
    void CmdSetTurretPower(float power)
    {
        turretPower = power;
    }


    [Command(requiresAuthority = false)]
    void CmdFireBullet(Vector2 bulletPosition, Vector2 velocity)
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        GameObject go = Instantiate(CurrentBulletPrefab,bulletPosition, Quaternion.Euler(0, 0, angle));
        go.GetComponent<Bullet>().SourceTank = this;

        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();        
        rb.velocity = velocity;
       

        NetworkServer.Spawn(go);
    }


    [Command(requiresAuthority = false)] // THIS IS BAD WE NEED TO BE CHANGE ALLOWES FOR CHEATING (TELEPORTING)
    public void CmdUpdatePosition(Vector3 newPosition)
    {
        if(gameManager.TankCanMove(this) == false)
        {
            //SHOULD NOT MOVE
        }
        serverPosition = newPosition;
    }


    [Command(requiresAuthority = false)] // THIS IS BAD WE NEED TO BE CHANGE ALLOWES FOR CHEATING (TELEPORTING)
    public void CmdUpdateFacing(Vector3 newFacing)
    {

       // serverFacing = newFacing;



    }



    [ClientRpc]
    void RpcFixPosition( Vector3 newPosition)
    {
        transform.position = newPosition;
    }



    [ClientRpc]
    void RpcNewTurn()
    {
       
    }


    [ClientRpc]
    public void RpcNewPhase()
    {
        TankIsLockedIn = false;
    }



    //SyncVar Hook

    void OnTurretAngleChange(float oldAngle, float newAngle)
    {
        if (hasAuthority)
        {
            return;
        }


        turretAngle = newAngle;
    }




}
