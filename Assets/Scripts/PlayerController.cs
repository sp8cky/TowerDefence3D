using UnityEngine;

public class PlayerController : MonoBehaviour {
    private GameManager gameManager;
    public float moveSpeed = 5f; // Bewegungsgeschwindigkeit des Spielers
    public float jumpForce = 10f; // Sprungkraft des Spielers
    public float lookSpeed = 2f; // Rotationsgeschwindigkeit der Kamera
    private int health = 50;
    private Rigidbody rb; // Referenz auf den Rigidbody des Spielers

    void Start() {
        rb = GetComponent<Rigidbody>();
        // Deaktiviere das Einfrieren der Rotation des Rigidbody, damit sich der Spieler umsehen kann
        rb.freezeRotation = true;
        gameManager = FindObjectOfType<GameManager>(); 
        if (gameManager == null) Debug.LogError("Gamemanager nicht gefunden.");
    }

    void Update() {
        // player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // camera rotation
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // player rotation horizontal (around Y-axis)
        transform.Rotate(Vector3.up, mouseX);

        // camera rotation vertical (around X-axis)
        Camera camera = GetComponentInChildren<Camera>();
        camera.transform.Rotate(Vector3.left, mouseY);

        // jump
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        
    }

    void Jump() {
        // check if player is grounded
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f)) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        gameManager.UpdatePlayerHealth(health);
    }
}
