using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 1.0f;
    public float maxSwayAmount = 2.0f;
    public float smoothFactor = 4.0f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float swayX = Mathf.Clamp(mouseX * swayAmount, -maxSwayAmount, maxSwayAmount);
        float swayY = Mathf.Clamp(mouseY * swayAmount, -maxSwayAmount, maxSwayAmount);

        Vector3 targetPosition = new Vector3(swayX, swayY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition + initialPosition, Time.deltaTime * smoothFactor);
    }
}