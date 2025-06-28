using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class InputHandler : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float fireRate = 0.2f;

    private Controls controls;
    private Vector2 move;
    public float moveSpeed = 5f;

    private Vector2 aimDirection = Vector2.right; // default right
    private bool isAttacking = false;
    private float fireCooldown;
    private Vector2 direction;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Attack.performed += ctx => isAttacking = true;
        controls.Player.Attack.canceled += ctx => isAttacking = false;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }


    private void Update()
    {
        Vector3 movement = new Vector3(move.x, move.y, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;

        if (move != Vector2.zero)
        {
            aimDirection = move.normalized;
        }

        if (isAttacking)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                FireProjectile();
                fireCooldown = fireRate;
            }
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is null or destroyed!");
            return;
        }

        if (projectileSpawn == null)
        {
            Debug.LogError("Projectile spawn transform is null!");
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Init(aimDirection);
    }
}
