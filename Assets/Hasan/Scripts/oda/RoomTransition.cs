using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public Transform cameraTransform;  // Kameran?n transform'u
    public float transitionSpeed = 5f; // Ge�i? h?z?
    public Vector3 targetPosition;    // Hedef odan?n pozisyonu

    private bool isTransitioning = false;

    void Update()
    {
        // E?er ge�i? yap?l?yorsa, kameray? yeni pozisyona kayd?r
        if (isTransitioning)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, transitionSpeed * Time.deltaTime);

            // Ge�i? tamamland???nda
            if (Vector3.Distance(cameraTransform.position, targetPosition) < 0.1f)
            {
                isTransitioning = false;
                cameraTransform.position = targetPosition;
            }
        }
    }

    // Ge�i? ba?latmak i�in bu fonksiyon �a?r?lacak
    public void StartTransition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        isTransitioning = true;
    }
}
