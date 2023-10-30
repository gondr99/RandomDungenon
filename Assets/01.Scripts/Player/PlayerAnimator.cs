using UnityEngine;
using UnityEngine.U2D.Animation;
using DG.Tweening;

[RequireComponent(typeof(SpriteLibrary), typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _shockwaveRenderer;
    [SerializeField] private SpriteLibraryAsset[] _spriteAssets;
    
    private SpriteLibrary _spriteLibrary;
    private Animator _animator;

    private readonly int _hashIsMove = Animator.StringToHash("is_move");

    private readonly int _hashWaveDistance = Shader.PropertyToID("_WaveDistance");
    private Material _shockwaveMat;
    private void Awake()
    {
        _spriteLibrary = GetComponent<SpriteLibrary>();
        _animator = GetComponent<Animator>();

        _shockwaveMat = _shockwaveRenderer.material;
    }
    
    

    public void SetMovement(bool value)
    {
        _animator.SetBool(_hashIsMove, value);
    }

    public void SetSpriter(int idx)
    {
        idx = Mathf.Clamp(idx, 0, _spriteAssets.Length);
        _spriteLibrary.spriteLibraryAsset = _spriteAssets[idx];
        
        ShockwaveEffect();
    }

    private void ShockwaveEffect()
    {
        _shockwaveRenderer.gameObject.SetActive(true);
        _shockwaveMat.SetFloat(_hashWaveDistance, -0.1f);
        DOTween.To(() => _shockwaveMat.GetFloat(_hashWaveDistance),
            value => _shockwaveMat.SetFloat(_hashWaveDistance, value), 1f, 0.6f).OnComplete(() =>
        {
            _shockwaveRenderer.gameObject.SetActive(false);
        });
    }
}
