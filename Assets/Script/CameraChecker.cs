using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChecker : MonoBehaviour
{
    private enum Mode
    {
        None,
        Render,
        RenderOut,
    }

    private Mode _mode;

    
    void Start()
    {
        _mode = Mode.None;
    }

    // Update is called once per frame
    void Update()
    {
        _Dead();
    }

    private void OnWillRenderObject()
    {
        if(Camera.current.name == "Main Camera")
        {
            _mode = Mode.Render;
        }
    }
    private void _Dead()
    {
        if(_mode == Mode.RenderOut)
        {
            Destroy(gameObject);
        }
        if(_mode == Mode.Render) 
        {
            _mode = Mode.RenderOut;
        }
    }

}
