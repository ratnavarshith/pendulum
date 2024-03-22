using UnityEngine;

public class Angle : MonoBehaviour
{
    public Transform targetTransform;
    private TextMesh textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
    }

    void Update() { 
    
        float angle = targetTransform.eulerAngles.x;
        if (angle > 180f)
        {
            angle -= 360f;
        }
        textMesh.text = angle.ToString("F4") ;
    }
}
