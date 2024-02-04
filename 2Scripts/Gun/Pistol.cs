using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [Header("Camera Shake")]
    public float fireCamShakeAmplitude;
    public float fireCamShakeTime;
    public CameraShake cameraShakeScript;

    [Header("Prefab Assignment")]
    public GameObject throwPistolPrefab;
    public GameObject bulletPrefab;
    public Transform firePosObject;

    private bool hasBullet = true;
    public bool hasPistol = true;

    private Vector3 bulletFireRelativeOffset;

    private Animator animator;
    private ParticleSystem particleSys;

    private void Start()
    {
        bulletFireRelativeOffset = firePosObject.localPosition;

        animator = GetComponent<Animator>();
        particleSys = GetComponentInChildren<ParticleSystem>();

        Player.onHoldChange += OnHoldChange;
        Player.onFireTriggered += OnFireTriggered;
    }

    public void FirePistol(Vector2 rotationVector)
    {
        hasBullet = false;
        Player.Instance.SetHasBullet(false);
        animator.SetTrigger("firePistol");

        Projectile projectileInstanceScript = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        projectileInstanceScript.FireProjectile(rotationVector.normalized, bulletFireRelativeOffset);

        particleSys.Play();
        cameraShakeScript.ShakeCamera(fireCamShakeTime, fireCamShakeAmplitude);
    }

    public void ThrowPistol(Vector2 rotationVector)
    {
        hasPistol = false;
        Player.Instance.SetHasGun(false);
        animator.SetBool("hasPistol", false);

        GameObject throwPistolInstance = Instantiate(throwPistolPrefab, transform.position, Quaternion.identity);
        throwPistolInstance.GetComponent<Projectile>().FireProjectile(rotationVector.normalized, Vector3.zero);
        throwPistolInstance.GetComponent<ThrowPistolFire>().SetCanFire(hasBullet);

        Player.Instance.SetPlayerForm(Form.Demon);
        Player.Instance.SetIsHolding(false);
    }

    private void OnHoldChange(bool _isHolding)
    {
        if (_isHolding)
        {
            AddPistolToHand();
        }
        else
        {
            RemovePistolFromHand();
        }
    }

    private void AddPistolToHand()
    {
        hasPistol = true;
        hasBullet = true;
        animator.SetBool("hasPistol", true);

        Player.Instance.SetHasGun(true);
        Player.Instance.SetHasBullet(true);
    }

    public void RemovePistolFromHand()
    {
        hasPistol = false;
        hasBullet = false;
        animator.SetBool("hasPistol", false);
    }

    private void OnFireTriggered(FireType _fireType, Vector2 _rotationVector)
    {
        if (_fireType == FireType.Gun)
        {
            if (hasPistol)
            {
                if (hasBullet)
                {
                    FirePistol(_rotationVector);
                }
                else
                {
                    ThrowPistol(_rotationVector);
                }
            }
        }
        else if (_fireType == FireType.ThrowGun)
        {
            ThrowPistol(_rotationVector);
        }
    }
}
