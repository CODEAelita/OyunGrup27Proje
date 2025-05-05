using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class cooldownsUI : MonoBehaviour
{
    [SerializeField]private characterController characterController;
    private characterStats characterStats;
    [SerializeField]private Image dashImage;
    private bool isCooling = false;
    void Awake()
    {
        dashImage = GetComponent<Image>();
    }
    private void Start()
    {
        dashImage.fillAmount = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCooling)
        {
            if(characterController.moveDirection != 0.0f)
            {
                StartCoroutine(wait());
            }
        }
    }

    private IEnumerator wait()
    {
        isCooling = true;
        float timer = 0f;
        dashImage.fillAmount = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime;
            dashImage.fillAmount = Mathf.Clamp01(timer / 1f);
            yield return null;
        }

        dashImage.fillAmount = 1f;
        isCooling = false;
    }
}
