using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField, Header("HPÉAÉCÉRÉì")]
    private GameObject _playerIcon;

    private PlayerMove _player;
    private int _deforeHP;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMove>();
        _deforeHP = _player.GetHP();
        _CreateHPIcon();

    }
    private void _CreateHPIcon()
    {
        for (int i = 0; i < _player.GetHP(); i++)
        {
            GameObject _playerHPObj = Instantiate(_playerIcon);
            _playerHPObj.transform.parent = transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        _ShowHPIcon();
    }
    private void _ShowHPIcon()
    {
        if (_deforeHP == _player.GetHP()) return;
        Image[] icone = transform.GetComponentsInChildren<Image>();
        for(int i = 0;i < icone.Length;i++) 
        {
            icone[i].gameObject.SetActive(i < _player.GetHP());
        }
        _deforeHP = _player.GetHP();
       
    }
}
