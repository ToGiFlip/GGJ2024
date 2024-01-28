using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField] private KeyCode _testButton;

    [SerializeField] private AnimationClip _spriteAnimation;
    private Animator _animator;
    private AnimatorOverrideController _animatorController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _animatorController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorController;

        _animatorController["Empty"] = _spriteAnimation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_testButton))
            PlayAnimation();
    }

    public void PlayAnimation()
    {
        _animator.Play("PlayVFX");
    }
}
