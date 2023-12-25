using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Header("�ړ����x")]
    private float _moveSpeed;
    [SerializeField, Header("�W�����v���x")]
    private float _jumpSpeed;
    [SerializeField, Header("�̗�")]
    private int _hp;
    [SerializeField, Header("���G����")]
    private float _damageTime;
    [SerializeField, Header("�_�Ŏ���")]
    private float _flashTime;

    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private bool _bJump;

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _LookMoveDirec();
        
    }

    private void _Move()
    {
        _rigid.velocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.velocity.y);
        _anim.SetBool("Walk", _inputDirection.x != 0.0f);
    }

    private void _LookMoveDirec()
    {
        if(_inputDirection.x > 0.0f) 
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(_inputDirection.x < 0.0f) 
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }

    public void _OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Floor")
        {
            _bJump = false;
            _anim.SetBool("Jump", _bJump);
        }
        if(collision.gameObject.tag=="Enemy")
        {
            _HitEnemy(collision.gameObject);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            
        }
    }
    private void _HitEnemy(GameObject enemy)
    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
        if(transform.position.y-(halfScaleY-0.1f)>= enemy.transform.position.y+(enemyHalfScaleY-0.1f))
        {
            Destroy(enemy);
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
        else
        {
            enemy.GetComponent<EnemyMove>().PlayerDamage(this);
            StartCoroutine(_Damage());
        }
    }

    IEnumerator _Damage()
    {
        Color color = _spriteRenderer.color;
        for (int i = 0; i < _damageTime; i++)
        {
            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        }
        _spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    private void _Dead()
    {
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void _OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || _bJump) return;

            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            _bJump = true;
            _anim.SetBool("Jump", _bJump);



    }
    public void Damage(int damage)
    {
        _hp = Mathf.Max(_hp - damage, 0);
        _Dead();
    }

    public int GetHP()
    {
        return _hp;
    }


}
