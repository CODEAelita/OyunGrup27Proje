using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    public Transform character;     // Karakterin transform'u
    public float speed = 2f;        // Salýným hýzý
    public float distance = 0.5f;   // Salýným mesafesi
    public Vector3 offset;          // Karaktere göre ateþ topunun konumu (örneðin yukarýda, yanýnda vs)

    void Update()
    {
        if (character == null)
            return;

        // Karakterin pozisyonuna göre salýným merkezi
        Vector3 basePosition = character.position + offset;

        // Ýleri-geri salýným (örneðin X ekseninde)
        float offsetX = Mathf.Sin(Time.time * speed) * distance;

        // Yeni pozisyon
        transform.position = basePosition + new Vector3(offsetX, 0f, 0f);
    }
}

