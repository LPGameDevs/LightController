using UnityEngine;
using UnityEngine.UI;
using Light = LightController.Light;

public class LedLight : MonoBehaviour
{
    private Light _light;

    private Image _image;
    private bool _isSpecialPosition;

    private void Start()
    {
        _image = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (_light == null) return;

        _image.color = _light.IsOn ? (_isSpecialPosition ? Color.red : Color.yellow) : (_isSpecialPosition ? Color.green : Color.black);
    }

    public void TurnOn()
    {
        _image.color = Color.yellow;
    }

    public void TurnOff()
    {
        _image.color = Color.black;
    }

    public void SetLight(Light light)
    {
        _light = light;
    }

    private void SetSpecialPosition(int position)
    {
        if (position != _light.GetPosition())
        {
            return;
        }
        _isSpecialPosition = true;
    }

    private void Awake()
    {
        GameController.OnSetSpecialPosition += SetSpecialPosition;
    }

    private void OnDestroy()
    {
        GameController.OnSetSpecialPosition -= SetSpecialPosition;
    }
}
