using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestNetwork : NetworkBehaviour
{

    //[SerializeField] private NetworkIdentity _networkIdentity;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _color;

    private void Awake()
    {
        Debug.Log($"IsSpawned: {isSpawned}");    
    }

    /*
    protected override void OnSpawned()
    {


        //base.OnSpawned();
        //Instantiate(_networkIdentity, Vector3.zero, Quaternion.identity);


    }
    */

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
            SetColor();
    }

    [ObserversRpc] //command or instrustion to run smth
    private void SetColor()
    {
        _renderer.material.color = _color;
    }






}
