using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 5f; // Bewegungsgeschwindigkeit des Spielers
    public float jumpForce = 10f; // Sprungkraft des Spielers
    public float lookSpeed = 2f; // Rotationsgeschwindigkeit der Kamera
    private bool isFrozen = false;
    private int health;
    private Rigidbody rb; 

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (isFrozen) return;
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
        if (camera != null) camera.transform.Rotate(Vector3.left, mouseY);
    
        // jump
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        
    }
    public void FreezePlayer() {
        isFrozen = true;
        if (rb == null) rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; // stop movement of player
        rb.angularVelocity = Vector3.zero; // stop rotation of player 
        Debug.Log("Player frozen");
    }

    public void UnfreezePlayer() {
        isFrozen = false;
        Debug.Log("Player unfrozen");
    }

    void Jump() {
        if (isFrozen) return;
        // check if player is grounded
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f)) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    
}
