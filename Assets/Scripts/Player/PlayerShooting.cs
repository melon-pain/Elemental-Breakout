using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PlayerShooting : MonoBehaviour, ISpreadHandler
{
    #region Upgrades
    [SerializeField] private Upgrades upgrades;
    #endregion Upgrades

    public enum WeaponType
    {
        Projectile,
        Beam
    }

    [Header("Weapon")]
    [SerializeField] private Element m_Element;
    [SerializeField] private WeaponType m_WeaponType;

    [Header("Projectile")]
    [SerializeField] private ParticleSystem m_Projectiles;
    [SerializeField] private List<Color> m_ProjectileColors = new List<Color>();
    [SerializeField] private float m_ProjectileDamage;
    [SerializeField] private float m_ProjectileMPCost;
    [SerializeField] private float m_ProjectileFireRate;
    private bool m_IsShooting = false;

    [Header("Beam")]
    [SerializeField] private GameObject m_Beam;
    [SerializeField] private ParticleSystem m_BeamHit;
    [SerializeField] private float m_BeamDamage;
    [SerializeField] private float m_BeamMPCost;
    [SerializeField] private float m_BeamTickRate;

    #region MP
    [Header("MP")]
    [Tooltip("The maximum amount of Mana Points the player can have")]
    [SerializeField] private float m_MaxMP;
    [Tooltip("The amount of Mana Points the player currently has")]
    [SerializeField] private float m_CurrentMP;
    public UnityEvent<float> OnMPChanged = new UnityEvent<float>();
    #endregion MP

    [Header("Aim")]
    [SerializeField] private GameObject m_AimTarget;
    private GameObject m_Target;

    [Space(4.0f)]
    public UnityEvent OnProjectileShoot;
    public UnityEvent OnBeamShoot;
    private Coroutine ShootCoroutine = null;

    // Start is called before the first frame update
    private void Start()
    {
        this.IntializeStats();
        this.ChangeElement(0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.acceleration.sqrMagnitude >= 2.0f)
        {
            RechargeMana();
        }

        this.transform.LookAt(m_AimTarget.transform);
    }

    private void IntializeStats()
    {
        m_MaxMP = upgrades.maxMP.GetValue();
        m_CurrentMP = m_MaxMP;

        m_ProjectileDamage = upgrades.projectileDMG.GetValue();
        m_ProjectileMPCost = upgrades.projectileCost.GetValue();
        m_ProjectileFireRate = upgrades.projectileRate.GetValue();

        m_BeamDamage = upgrades.beamDMG.GetValue();
        m_BeamMPCost = upgrades.beamCost.GetValue();
        m_BeamTickRate = upgrades.beamRate.GetValue();

        var emission = m_Projectiles.emission;
        emission.rateOverTime = m_ProjectileFireRate;
    }

    public void StartShooting()
    {
        m_IsShooting = true;
        ShootCoroutine = StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        switch (m_WeaponType)
        {
            case WeaponType.Projectile:
                m_Projectiles.Play();
                //m_VFX.Play();
                while (m_IsShooting && m_CurrentMP > 0.0f)
                {
                    ConsumeMana(m_ProjectileMPCost);
                    OnProjectileShoot.Invoke();
                    yield return new WaitForSeconds(1.0f / m_ProjectileFireRate);
                }
                this.m_Projectiles.Stop();
                //m_VFX.Stop();
                break;
            case WeaponType.Beam:
                m_Beam.SetActive(true);
                RemoveLockOn();
                while (m_IsShooting && m_CurrentMP > 0.0f)
                {
                    ConsumeMana(m_BeamMPCost * 1.0f / m_BeamTickRate);

                    Ray r = new Ray(this.transform.position, this.transform.forward);
                    RaycastHit hit;

                    if (Physics.SphereCast(r, 1.0f, out hit, 100.0f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))
                    {
                        IDamage damage = hit.collider.GetComponent<IDamage>();
                        if (damage != null)
                            damage.TakeDamage(m_Element, m_BeamDamage);
                        else
                            Debug.Log("No beam damage?");
                        m_BeamHit.transform.position = hit.point;
                        m_BeamHit.Play();
                    }
                    OnBeamShoot.Invoke();
                    yield return new WaitForSeconds(1.0f / m_BeamTickRate);
                }
                m_Beam.SetActive(false);
                break;
        }

        StopShooting();
        yield break;
    }

    public void StopShooting()
    {
        m_IsShooting = false;
    }

    public void ConsumeMana(float amount)
    {
        m_CurrentMP = Mathf.Max(0.0f, m_CurrentMP - amount);
        OnMPChanged.Invoke(m_CurrentMP / m_MaxMP);
    }

    private void RechargeMana()
    {
        if (m_CurrentMP < m_MaxMP)
        {
            m_CurrentMP = m_MaxMP;
            OnMPChanged.Invoke(m_CurrentMP / m_MaxMP);
        }
    }
    public void ChangeElement(int newElement)
    {
        m_Element = (Element)newElement;

        var main = m_Projectiles.main;
        main.startColor = m_ProjectileColors[newElement];

        var beamMain = m_BeamHit.main;
        beamMain.startColor = m_ProjectileColors[newElement];

        m_Beam.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", m_ProjectileColors[newElement] * 4);
        m_Beam.GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", m_ProjectileColors[newElement] * 16);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy")
        {
            IDamage damage = other.GetComponent<IDamage>();
            damage.TakeDamage(m_Element, m_ProjectileDamage);
        }
    }

    public void LockOn (TapEventData eventData)
    {
        if (eventData.gameObject && m_WeaponType == WeaponType.Projectile)
        {
            m_AimTarget.transform.parent = eventData.gameObject.transform;
            Debug.Log(this.transform.parent.name);
            if (eventData.gameObject.GetComponent<BoxCollider>())
                m_AimTarget.transform.localPosition = eventData.gameObject.GetComponent<BoxCollider>().center;
            else
                m_AimTarget.transform.localPosition = Vector3.zero; 
            m_AimTarget.transform.localRotation = Quaternion.identity;
            m_Target = eventData.gameObject;
        }
        else
        {
            RemoveLockOn();
        }
    }

    public void RemoveLockOn()
    {
        m_AimTarget.transform.parent = this.transform.parent;
        m_AimTarget.transform.localPosition = new Vector3(0.0f, 0.0f, 2.0f);
        m_AimTarget.transform.localRotation = Quaternion.identity;
        this.transform.localRotation = Quaternion.identity;
        m_Target = null;
    }

    public void OnSpread(SpreadEventData eventData)
    {
        switch (eventData.direction)
        {
            case SpreadDirection.Spread:
                m_WeaponType = WeaponType.Beam;
                break;
            case SpreadDirection.Pinch:
                m_WeaponType = WeaponType.Projectile;
                break;
        }
    }

}
