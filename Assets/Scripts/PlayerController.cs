using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;

    [Header("Vida")]
    public int maxHealth = 3;

    [Header("Reflejo")]
    public KeyCode reflectKey = KeyCode.Space;
    public float reflectActiveTime = 0.3f;   // Tiempo total que el aura está activa
    public float parryWindow = 0.2f;         // Ventana perfecta desde que se presiona el botón

    [Header("Energía de Reflejo")]
    public int maxEnergy = 5;                // Cargas máximas de reflejo
    public float energyRegenInterval = 3f;   // Cada cuántos segundos regenera 1 carga

    [HideInInspector] public int currentHealth;
    [HideInInspector] public int currentEnergy;

    [SerializeField] private InputAction playerActions;
    [SerializeField] private InputAction playerReflect;

    [SerializeField] Animation reflectorAnimation;
   
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    // Estado del reflejo
    private bool reflectAuraActive = false;
    private float lastReflectPressTime = -999f;

    // Regeneración
    private float energyRegenTimer = 0f;

    private float xMin, xMax, yMin, yMax;

    private void OnEnable()
    {
        playerActions.Enable();
        playerReflect.Enable();
    }
    private void OnDisable()
    {
        playerActions.Disable();
        playerReflect.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        float distanceToCamera = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distanceToCamera));

        xMin = bottomLeft.x;
        xMax = topRight.x;
        yMin = bottomLeft.y;
        yMax = topRight.y;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = playerActions.ReadValue<Vector2>();
        
        if (playerReflect.triggered)
        {
            TryActivateReflect();
        }
        if (reflectAuraActive && Time.time - lastReflectPressTime > reflectActiveTime)
        {
            reflectAuraActive = false;
            //VFX Off
        }

        HandleEnergyRegen();

    }

    private void FixedUpdate()
    {

        rb.linearVelocity = moveDirection * moveSpeed;
        if (moveDirection!= Vector2.zero) 
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }

        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, xMin, xMax);
        currentPosition.y = Mathf.Clamp(currentPosition.y, yMin, yMax);
        transform.position = currentPosition;


    }

    void TryActivateReflect()
    {
       
        if (currentEnergy <= 0) return;

        reflectAuraActive = true;
        lastReflectPressTime = Time.time;
        SpendEnergyOnFailedReflect();
        Debug.Log(currentEnergy);
        reflectorAnimation.Play();
        //VFX On

    }

    void HandleEnergyRegen()
    {
        if (currentEnergy >= maxEnergy) return;

        energyRegenTimer += Time.deltaTime;
        if (energyRegenTimer >= energyRegenInterval)
        {
            currentEnergy++;
            energyRegenTimer = 0f;
            Debug.Log(currentEnergy);
            
        }
    }

    public bool IsReflectActive()
    {
        return reflectAuraActive;
    }

    public bool IsInParryWindow()
    {
        return reflectAuraActive && (Time.time - lastReflectPressTime <= parryWindow);
    }
    public void SpendEnergyOnFailedReflect()
    {
        currentEnergy = Mathf.Max(0, currentEnergy - 1);
        energyRegenTimer = 0f; 
    }
    public void RegenEnergyOnPerfectReflect()
    {
        currentEnergy++;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        energyRegenTimer = 0f;
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Debug.Log("Player muerto");
            var hud = FindAnyObjectByType<HUD>();
            if (hud) 
            {
                hud.OnGameOver();
                Time.timeScale = 0.3f;
                playerActions.Disable();
                playerReflect.Disable();
            }
            
        }
    }

    public void AddLife()
    {
        currentHealth = Mathf.Clamp(currentHealth++, 0, maxHealth);
    }
}
