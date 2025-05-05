using UnityEngine;

public class burningZone : MonoBehaviour
{
    [Header("Damage Stats")]
    [SerializeField] private float damagePerSecond = 0.1f;
    private float damageThisFrame;
    private health playerHealth;
    [SerializeField]private healthBar healthBar;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player")) return;

        playerHealth = collision.GetComponent<health>();
        //damageThisFrame = damagePerSecond * Time.deltaTime;
        playerHealth.healthAmount -= damagePerSecond;
        healthBar.healthBarImage.fillAmount -= damagePerSecond / 100;
        if(playerHealth.healthAmount <= 0)
        {
            playerHealth.takeDamage(damageThisFrame);
        }
        //Invoke("wait", 2f);
    }
    
    public void wait()
    {
        playerHealth.healthAmount = damagePerSecond;
        healthBar.healthBarImage.fillAmount -= damagePerSecond / 100;
    }
}
