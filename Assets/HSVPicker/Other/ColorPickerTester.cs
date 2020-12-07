using UnityEngine;

public class ColorPickerTester : MonoBehaviour 
{

    public new Renderer renderer;
    public ColorPicker picker;

    public Color Color = Color.red;

    // Use this for initialization
    void Start () 
    {
        if (picker != null)
        {
            picker.onValueChanged.AddListener(color =>
            {
                renderer.material.color = color;
                Color = color;
            });

            renderer.material.color = picker.CurrentColor;

            picker.CurrentColor = Color;
        }
    }

    public void AddListener(ColorPicker picker)
    {
        this.picker = picker;

        this.picker.onValueChanged.AddListener(color =>
        {
            renderer.material.color = color;
            Color = color;
        });
    }
}
