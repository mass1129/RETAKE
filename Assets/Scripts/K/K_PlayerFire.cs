using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class K_PlayerFire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject firePosition;

    public GameObject uFirePosition;
    public GameObject uAFirePosition;
    public GameObject bulletFactory;
    public GameObject uBulletFactory;
    public GameObject[] uBulletPool;
    public Vector3[] uBulletDir;
    public bool[] rigidPool;
    public Vector3[] startPosition;
    [SerializeField] private float animationPlayTransition = 0.01f;


    public TrailRenderer[] uTrailPool;

    private Animator animator;
    public int uBulletPoolSize = 5;
    [SerializeField] private float throwPower = 10f;
    [SerializeField] private Transform FireRoot;
    [SerializeField] private Transform FireRoot2;
    [SerializeField] private Transform FireRoot3;
    private K_JettController jettController;
    private K_PlayerController playerController;
    public Image ulitimateImg;


    public GameObject animBullet1;
    // Update is called once per frame

    public bool u1Start = true;
    public bool u2Start = true;
    public bool u3Start = true;
    float speed;

    public GameObject muzzleFlash;

    public GameObject U_muzzleFlash;

    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private TrailRenderer U_BulletTrail;

    [SerializeField]
    private LayerMask Mask;

    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private ParticleSystem U_ImpactParticleSystem; 
    [SerializeField]
    private ParticleSystem UA_ImpactParticleSystem;


    [SerializeField]
    private float BulletSpeed = 100;
    [SerializeField]
    private float damage = 4f;
    [SerializeField]
    private float headDamageMultiple = 6f;
    [SerializeField]
    private float ultimateDamage = 10f;


    public bool comboPossible;
    int comboStep;

    [System.NonSerialized] public float animSpeed = 1.5f;
    [System.NonSerialized] public float uAnimSpeed = 1.7f;

    private K_JettStates jettStates;
    public bool isUltimating { get; private set; } = false;

    private float ultimateAttempts;
    private float ultimateStartTime = 0.0f;
    public int i;
    public int j = 1;
    public int k = 0;


    private float _currentAVelocity;
    bool disableAttack = false;
    float speed1;
    bool uAnimEnd = false;




    [SerializeField] AttackSoundGenerator attackSoundGenerator;


    private void Awake()
    {

    }


    void Start()
    {
        jettController = GetComponent<K_JettController>();
        jettStates = GetComponent<K_JettStates>();
        ultimateAttempts = jettStates.maxUltimateAttempts;
        playerController = GetComponent<K_PlayerController>();
        animator = playerController._animator;
        comboPossible=true;
        attackSoundGenerator = GetComponent<AttackSoundGenerator>();

    }


    void PlayAttackSound(int num)
    {
        attackSoundGenerator.playerAudioSource.clip = attackSoundGenerator.attackSoundPool[num];
        attackSoundGenerator.playerAudioSource.Play();
    }

    void Update()
    {
        if (!disableAttack)
        { Attack(); }
        else if (disableAttack && !uAnimEnd && comboPossible)
        {
            UAttack();

        }
        HandleJettUltimateFire();

       
    }


    public void ComboPossible()
    {
        comboPossible = true;
        if (isUltimating) uAnimEnd = false;
    }


    public void DisComboPossible()
    {
        comboPossible = false;


    }


    
    public void ComboReset()
    {
        if (isUltimating) uAnimEnd = true;


        comboStep = 0;
    }



    public void Attack()
    {


        if (comboStep % 2 == 0)
        {
            firePosition.transform.position = FireRoot.position;
            firePosition.transform.localRotation = Quaternion.Euler(playerController.xRotation, 0f, 0f);
        }
        if (comboStep % 2 == 1)
        {
            firePosition.transform.position = FireRoot2.position;
            firePosition.transform.localRotation = Quaternion.Euler(playerController.xRotation, 0f, 0f);
        }


        animator.SetFloat("AttackSpeed", animSpeed);
        if (Input.GetMouseButtonDown(0))
        {

            if (comboStep == 0 && comboPossible)
            {
                animator.Play("LeftShoot");
                PlayAttackSound(0);

            }
            if (comboStep % 2 == 1 && comboPossible)
            {
                animator.Play("RightShoot");
                PlayAttackSound(0);

            }

            if (comboStep % 2 == 0 && comboStep != 0 && comboPossible)
            {
                animator.Play("LeftShoot");
                PlayAttackSound(0);
            }


        }
    }

    void GeneralFire()
    {


        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));


        //?????? ???? ???? ??????
        Vector3 targetPoint;

        ////?????????????? ???????? ?????? ???? ?????????? layer?? ??????. ???????? ???? ????????(????x)
        if (Physics.Raycast(ray, out RaycastHit hit,  float.MaxValue, Mask))
        {
            

            targetPoint = hit.point;

            if (Mask.Equals("Player"))
            {
                targetPoint = ray.GetPoint(75);
            }


            if (hit.collider.tag == "Enemy")
                {
                    N_Enemy enemy = hit.transform.GetComponent<N_Enemy>();
                    if (enemy != null)
                        {
                            enemy.AddDamage(damage);

                        }
                    K_Enemy kenemy = hit.transform.GetComponent<K_Enemy>();
                    if (kenemy != null)
                    {
                        kenemy.AddDamage(damage);

                    }

                    N_MiniBoss1 miniBoss1 = hit.transform.GetComponent<N_MiniBoss1>();
                    if (miniBoss1 != null)
                        {
                            miniBoss1.AddDamage(damage);
                        }

                    N_MiniBoss2 miniBoss2 = hit.transform.GetComponent<N_MiniBoss2>();
                    if (miniBoss2 != null)
                    {
                        miniBoss2.AddDamage(damage);
                    }
                    N_Boss boss = hit.transform.GetComponent<N_Boss>();
                    if (boss != null)
                        {
                            boss.AddDamage(damage);
                        }


                }

                else if (hit.collider.tag == "Head")
                {
                    N_Enemy enemy = hit.transform.GetComponentInParent <N_Enemy>();
                    if (enemy != null)
                    {
                    enemy.AddDamage(damage * headDamageMultiple);

                    }
                    K_Enemy kenemy = hit.transform.GetComponentInParent<K_Enemy>();
                    if (kenemy != null)
                    {
                        kenemy.AddDamage(damage * headDamageMultiple);
                        hit.transform.GetComponentInChildren<K_HeadShot>().HeadShot();

                    }
                    N_MiniBoss1 miniBoss1 = hit.transform.GetComponentInParent<N_MiniBoss1>();

                    if (miniBoss1 != null)
                    {
                        miniBoss1.AddDamage(damage * headDamageMultiple);
                    }
                
                    N_MiniBoss2 miniBoss2 = hit.transform.GetComponentInParent<N_MiniBoss2>();

                    if (miniBoss2 != null)
                    {
                        miniBoss2.AddDamage(damage * headDamageMultiple);
                    }

                    N_Boss boss = hit.transform.GetComponentInParent<N_Boss>();

                    if (boss != null)
                    {
                        boss.AddDamage(damage * headDamageMultiple);
                    }

                    if(K_1RScore.Instance!=null && !kenemy.isDie)
                    {
                    K_1RScore.Instance.CurScore++;
                    }
                }

            
            Debug.Log(hit.transform.tag);
            








            TrailRenderer trail = Instantiate(BulletTrail, firePosition.transform.position, Quaternion.identity);
            //???? ?????????? ?????? ????????.
            StartCoroutine(SpawnTrail(trail, targetPoint, hit.normal, hit, true));

        }
        //???? ???????? ?????? ??????
        else
        {
            targetPoint = ray.GetPoint(75);
            TrailRenderer trail = Instantiate(BulletTrail, firePosition.transform.position, Quaternion.identity);
            //???? ?????????? ?????? ????????.

            StartCoroutine(SpawnTrail(trail, targetPoint, hit.normal, hit, false));
        }


        if (muzzleFlash != null)
        {
            GameObject impact = Instantiate(muzzleFlash);
            impact.transform.position = firePosition.transform.position;
            impact.transform.up = -firePosition.transform.forward;
        }
        comboStep++;

    }


    public void UAttack()
    {





        animator.SetFloat("AttackSpeed", uAnimSpeed);
        if (Input.GetMouseButtonDown(0))
        {

            if (comboStep == 0 && comboPossible)
            {
                animator.Play("ULeftShoot");
                PlayAttackSound(2);

            }
            if (comboStep % 2 == 1 && comboPossible)
            {
                animator.Play("URightShoot");
                PlayAttackSound(3);

            }

            if (comboStep % 2 == 0 && comboStep != 0 && comboPossible)
            {
                animator.Play("ULeftShoot");
                PlayAttackSound(2);
            }


        }

        if (Input.GetMouseButtonDown(1))
        {
            if (comboPossible)
            {
                animator.Play("UltimateAllShoot");
                PlayAttackSound(4);
                for (k = 0; k < uBulletPoolSize; k++)
                {
                    if (rigidPool[k] == false)
                    {
                        rigidPool[k] = true;

                        Destroy(uBulletPool[k]);
                        //?????? ?????? ????????.

                    }
                }

            }

        }
    }




    void UFire()
    {
        comboStep++;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));


        //?????? ???? ???? ??????
        Vector3 targetPoint;

        ////?????????????? ???????? ?????? ???? ?????????? layer?? ??????. ???????? ???? ????????(????x)
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, Mask))
        {


            if (hit.collider.tag == "Enemy")
            {
                N_Enemy enemy = hit.transform.GetComponent<N_Enemy>();
                if (enemy != null)
                {
                    enemy.AddDamage(ultimateDamage);

                }
                K_Enemy kenemy = hit.transform.GetComponent<K_Enemy>();
                if (kenemy != null)
                {
                    kenemy.AddDamage(ultimateDamage);

                }
                N_MiniBoss1 miniBoss1 = hit.transform.GetComponent<N_MiniBoss1>();
                if (miniBoss1 != null)
                {
                    miniBoss1.AddDamage(ultimateDamage);
                }
                N_MiniBoss2 miniBoss2 = hit.transform.GetComponent<N_MiniBoss2>();
                if (miniBoss2 != null)
                {
                    miniBoss2.AddDamage(ultimateDamage);
                }
                N_Boss boss = hit.transform.GetComponent<N_Boss>();
                if (boss != null)
                {
                    boss.AddDamage(ultimateDamage);
                }


            }

            else if (hit.collider.tag == "Head")
            {
                N_Enemy enemy = hit.transform.GetComponentInParent<N_Enemy>();
                if (enemy != null)
                {
                    enemy.AddDamage(ultimateDamage * headDamageMultiple);

                }
                K_Enemy kenemy = hit.transform.GetComponentInParent<K_Enemy>();
                if (kenemy != null)
                {
                    kenemy.AddDamage(ultimateDamage * headDamageMultiple);

                }
                N_MiniBoss1 miniBoss1 = hit.transform.GetComponentInParent<N_MiniBoss1>();

                if (miniBoss1 != null)
                {
                    miniBoss1.AddDamage(ultimateDamage * headDamageMultiple);
                }

                N_MiniBoss2 miniBoss2 = hit.transform.GetComponentInParent<N_MiniBoss2>();

                if (miniBoss2 != null)
                {
                    miniBoss2.AddDamage(ultimateDamage * headDamageMultiple);
                }

                N_Boss boss = hit.transform.GetComponentInParent<N_Boss>();

                if (boss != null)
                {
                    boss.AddDamage(ultimateDamage * headDamageMultiple);
                }

                if (K_1RScore.Instance != null && !kenemy.isDie)
                {
                    K_1RScore.Instance.CurScore++;
                }

            }



            targetPoint = hit.point;
            if (Mask.Equals("Player"))
            {
                targetPoint = ray.GetPoint(75);
            }
            uTrailPool[comboStep - 1] = Instantiate(U_BulletTrail, uFirePosition.transform.GetChild(comboStep - 1).position, Quaternion.identity);
            //???? ?????????? ?????? ????????.

            StartCoroutine(USpawnTrail(uTrailPool, targetPoint, hit.normal, hit, true));


        }
        //???? ???????? ?????? ??????
        else
        {
            targetPoint = ray.GetPoint(75);

            uTrailPool[comboStep - 1] = Instantiate(U_BulletTrail, uFirePosition.transform.GetChild(comboStep - 1).position, Quaternion.identity);

            //???? ?????????? ?????? ????????.

            StartCoroutine(USpawnTrail(uTrailPool, targetPoint, hit.normal, hit, true));

        }



        if (muzzleFlash != null)
        {
            GameObject impact = Instantiate(U_muzzleFlash);
            impact.transform.position = uFirePosition.transform.GetChild(comboStep - 1).position;
            impact.transform.up = -uFirePosition.transform.GetChild(comboStep - 1).forward;
        }



    }

    void UAllFire()
    {

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));


        //?????? ???? ???? ??????
        Vector3 targetPoint;

        ////?????????????? ???????? ?????? ???? ?????????? layer?? ??????. ???????? ???? ????????(????x)
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, Mask))
        {


            targetPoint = hit.point;


            if (Mask.Equals("Player"))
            {
                targetPoint = ray.GetPoint(75);
            }

           
            if (hit.collider.tag == "Enemy")
            {
                N_Enemy enemy = hit.transform.GetComponent<N_Enemy>();
                if (enemy != null)
                {
                    enemy.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage);

                }
                K_Enemy kenemy = hit.transform.GetComponent<K_Enemy>();
                if (kenemy != null)
                {
                    kenemy.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage);

                }
                N_MiniBoss1 miniBoss1 = hit.transform.GetComponent<N_MiniBoss1>();
                if (miniBoss1 != null)
                {
                    miniBoss1.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage);
                }
                N_MiniBoss2 miniBoss2 = hit.transform.GetComponent<N_MiniBoss2>();
                if (miniBoss2 != null)
                {
                    miniBoss2.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage);
                }
                N_Boss boss = hit.transform.GetComponent<N_Boss>();
                if (boss != null)
                {
                    boss.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage);
                }


            }

            else if (hit.collider.tag == "Head")
            {
                N_Enemy enemy = hit.transform.GetComponentInParent<N_Enemy>();
                if (enemy != null)
                {
                    enemy.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage * headDamageMultiple);

                }
                K_Enemy kenemy = hit.transform.GetComponentInParent<K_Enemy>();
                if (kenemy != null)
                {
                    kenemy.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage * headDamageMultiple);

                }
                N_MiniBoss1 miniBoss1 = hit.transform.GetComponentInParent<N_MiniBoss1>();

                if (miniBoss1 != null)
                {
                    miniBoss1.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage * headDamageMultiple);
                }

                N_MiniBoss2 miniBoss2 = hit.transform.GetComponentInParent<N_MiniBoss2>();

                if (miniBoss2 != null)
                {
                    miniBoss2.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage * headDamageMultiple);
                }

                N_Boss boss = hit.transform.GetComponentInParent<N_Boss>();

                if (boss != null)
                {
                    boss.AddDamage((uBulletPoolSize - comboStep) * ultimateDamage * headDamageMultiple);
                }

                if (K_1RScore.Instance != null && !kenemy.isDie&&K_1RScore.Instance.CurScore<6 && K_1RScore.Instance.CurScore >=11)
                {
                    K_1RScore.Instance.CurScore++;
                }

            }


            for (int i = comboStep; i < uBulletPoolSize; i++)
            {

                uTrailPool[i] = Instantiate(U_BulletTrail, uFirePosition.transform.GetChild(i).position, Quaternion.identity);
                //???? ?????????? ?????? ????????.
                startPosition[i] = uTrailPool[i].transform.position;
                StartCoroutine(UAllSpawnTrail(uTrailPool, targetPoint, hit.normal, hit, true, i));
            }

        }
        //???? ???????? ?????? ??????
        else
        {
            targetPoint = ray.GetPoint(75);

            for (int i = comboStep; i < uBulletPoolSize; i++)
            {
                uTrailPool[i] = Instantiate(U_BulletTrail, uFirePosition.transform.GetChild(i).position, Quaternion.identity);
                //???? ?????????? ?????? ????????.
                startPosition[i] = uTrailPool[i].transform.position;
                StartCoroutine(UAllSpawnTrail(uTrailPool, targetPoint, hit.normal, hit, true, i));
            }

        }


        for (int i = comboStep; i < uBulletPoolSize; i++)
        {
            if (muzzleFlash != null)
            {
                GameObject impact = Instantiate(U_muzzleFlash);
                impact.transform.position = uFirePosition.transform.GetChild(i).position;
                impact.transform.up = -uFirePosition.transform.GetChild(i).forward;
            }

        }

        OnEndUltimate();
    }






    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, RaycastHit hit, bool MadeImpact)
    {

        //???? ?????? ????????
        Vector3 startPosition = Trail.transform.position;
        //?????????? ?????????? ???? ?? ?????? ?????? ??????.
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        //???? ?????? ???? ???? ????
        float remainingDistance = distance;
        //???? ?????? ????????????
        while (remainingDistance > 0)
        {
            //???? ?????? ???? ????
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));
            //???? ???? ????????
            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        //?????? ?????? ???????? ??????????
        Trail.transform.position = HitPoint;
        //?????? ?????? ????????
        if (MadeImpact)
        {
           
            //???? ???????? ????????.
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            PlayAttackSound(1);
        }

        Destroy(Trail.gameObject, Trail.time);
    }





    private IEnumerator USpawnTrail(TrailRenderer[] uTrailPool, Vector3 HitPoint, Vector3 HitNormal, RaycastHit hit, bool MadeImpact)
    {

        //???? ?????? ????????

        startPosition[comboStep - 1] = uTrailPool[comboStep - 1].transform.position;
        //?????????? ?????????? ???? ?? ?????? ?????? ??????.
        float distance = Vector3.Distance(uTrailPool[comboStep - 1].transform.position, HitPoint);
        //???? ?????? ???? ???? ????
        float remainingDistance = distance;
        //???? ?????? ????????????
        while (remainingDistance > 0)
        {
            //???? ?????? ???? ????
            uTrailPool[comboStep - 1].transform.position = Vector3.Lerp(startPosition[comboStep - 1], HitPoint, 1 - (remainingDistance / distance));
            //???? ???? ????????
            remainingDistance -= BulletSpeed * 2 * Time.deltaTime;

            yield return null;
        }
        //?????? ?????? ???????? ??????????
        uTrailPool[comboStep - 1].transform.position = HitPoint;
        //?????? ?????? ????????
        if (MadeImpact)
        {
            
            //???? ???????? ????????.
            Instantiate(U_ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            int rd = Random.Range(5, 7);
            PlayAttackSound(rd);
        }

        Destroy(uTrailPool[comboStep - 1].gameObject, uTrailPool[comboStep - 1].time);
    }







    private IEnumerator UAllSpawnTrail(TrailRenderer[] uTrailPool, Vector3 HitPoint, Vector3 HitNormal, RaycastHit hit, bool MadeImpact, int num)
    {
        
        //?????????? ?????????? ???? ?? ?????? ?????? ??????.
        float distance = Vector3.Distance(uTrailPool[num].transform.position, HitPoint);
        //???? ?????? ???? ???? ????
        float remainingDistance = distance;
        //???? ?????? ????????????
        while (remainingDistance > 0)
        {
            //???? ?????? ???? ????
            uTrailPool[num].transform.position = Vector3.Lerp(startPosition[num], HitPoint, 1 - (remainingDistance / distance));
            //???? ???? ????????
            remainingDistance -= BulletSpeed  * Time.deltaTime;

            yield return null;
        }
        //?????? ?????? ???????? ??????????
        uTrailPool[num].transform.position = HitPoint;
        //?????? ?????? ????????
        if (MadeImpact)
        {
           
            //???? ???????? ????????.
            Instantiate(UA_ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
            PlayAttackSound(7);
        }

        Destroy(uTrailPool[num].gameObject, uTrailPool[num].time);
    }

    void StartBulletAnim1()
    {
        u1Start = false;
        animBullet1.SetActive(true);
    }


    void StopBulletAnim1()
    {
        u1Start = true;
        animBullet1.SetActive(false);
        u2Start = false;
        //???? ????
        uBulletPool = new GameObject[uBulletPoolSize];
        uBulletDir = new Vector3[uBulletPoolSize];
        rigidPool = new bool[uBulletPoolSize];
        uTrailPool = new TrailRenderer[uBulletPoolSize];
        startPosition = new Vector3[uBulletPoolSize];
        //?????? ???? ????
        for (i = 0; i < uBulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(uBulletFactory);
            uBulletPool[i] = bullet;

            //bulletPool[i].transform.position = uFirePosition.transform.GetChild(i).position;
        }
    }

    void StopBulletAnim2()
    {
        
        u2Start = true;
        u3Start = false;

    }


    void StopBulletAnim3()
    {
        u3Start = true;
        isUltimating = true;
        

    }


    

    void HandleJettUltimateFire()
    {
        ultimateAttempts += Time.deltaTime;
        ulitimateImg.fillAmount = ultimateAttempts / jettStates.maxUltimateAttempts;
        ultimateAttempts = Mathf.Clamp(ultimateAttempts, 0f, jettStates.maxUltimateAttempts);

        //V???? ?????? ?????? ?????????? ???? ????.
        bool isTryingUltimate = Input.GetKeyDown(KeyCode.V);
        //?????? ???? ???? ?????? ???????? ????????
        if (isTryingUltimate && !isUltimating)
        {
            //?????? ???????? ??????????
            if (ultimateAttempts >= jettStates.maxUltimateAttempts)
            {

                comboPossible = false;

                disableAttack = true;
                //?????? ?????????? ???? ???????? ????.
                ultimateStartTime = Time.time;

                //?????? ?????? ?????? ????
                ultimateAttempts = 0;

                animator.Play("Ultimate");


            }
        }

        if (!u1Start && !isUltimating)
        {
            firePosition.transform.position = FireRoot2.transform.position;
            animBullet1.transform.position = firePosition.transform.position;
            speed += Time.deltaTime * 1000;
            animBullet1.transform.rotation = Quaternion.Euler(speed , speed, speed );
        }
        if (!u2Start && !isUltimating)
        {
            
            uAFirePosition.transform.position = FireRoot.position;
            for (i = 0; i < uBulletPoolSize; i++)
            {
                uBulletPool[i].transform.position = uAFirePosition.transform.GetChild(i).position;
                uBulletPool[i].transform.right = uAFirePosition.transform.GetChild(i).forward;



            }

        }
        
        if (!u3Start && !isUltimating)
        {
            speed += Time.deltaTime * 1000;
            
            uAFirePosition.transform.position = FireRoot.position;
            for (i = 0; i < uBulletPoolSize; i++)
            {
                uBulletPool[i].transform.position = uAFirePosition.transform.position;
                uBulletPool[i].transform.right = uAFirePosition.transform.forward;
                


            }

        }

        if (isUltimating && u3Start)
        {
            //?? ?????? ?????? ????
            //?????????? [0] ????, ???? ?? ?????? [1]????

            //???? ?????? [0] ???? ???? ->????,[1]-[4]?? ????
            //???? ?????? [1] ???? ???? ->????,[2]-[4]?? ????
            uFirePosition.transform.position = FireRoot3.position;
            //???? ???? : ?????? ???? ???? ????,   
            //1. ?????? ????????
            if (Time.time - ultimateStartTime <= jettStates.ultimateDurationSeconds)
            {

                

                if (comboStep == 0)
                {
                    //???? 5???? ?????????? ??????????.
                    for (i = comboStep; i < uBulletPoolSize; i++)
                    {
                        if (rigidPool[i] == false)
                        {
                            uFirePosition.transform.position = FireRoot3.position;
                            uBulletPool[i].transform.position = uFirePosition.transform.GetChild(i).position;
                            uBulletPool[i].transform.right = Camera.main.transform.forward;
                        }
                    }
                }
                //?? ???????? ?????? ???? ??????
                else if (comboStep > 0 && comboStep < 5)
                {
                    //?????? ???? ???? ???? ???? ?????? ???????? 
                    for (i = comboStep; i < uBulletPoolSize; i++)
                    {
                        //?????? ????.
                        uFirePosition.transform.position = FireRoot3.position;
                        uBulletPool[i].transform.position = uFirePosition.transform.GetChild(i).position;      
                        uBulletPool[i].transform.right = Camera.main.transform.forward;


                        //j-2???? ?????? ?????? ??????????
                        if (rigidPool[comboStep - 1] == false)
                        {
                            //j-2???? ???? ????
                            rigidPool[comboStep - 1] = true;
                            Destroy(uBulletPool[comboStep - 1]);


                        }



                    }
                }
                //?????? ???? ??
                else if (comboStep == 5)
                {
                    //?????? ?????? ????????????
                    if (rigidPool[4] == false)
                    {
                        uFirePosition.transform.position = FireRoot3.position;
                        //????????.
                        rigidPool[4] = true;
                        Destroy(uBulletPool[comboStep - 1]);
                        //?????? ?????? ????????.
                        OnEndUltimate();
                    }

                }
                //?????? ?????? ?????????? ?????? ?????????? ?????? ???????? ????????s
                
            }
            else
            {
                OnEndUltimate();
            }


        }
    }

   
    
    void OnEndUltimate()
    {
        //???? ?????? "?????? ??"???? ????, ???? ???????? 0???? ??????
        disableAttack = false;
        //???????? ???? ????
        isUltimating = false;
        //?????? ???????? ??????
        ultimateStartTime = 0f;
        j = 1;
        k = 0;
        //???? ????
        for (i = 0; i < uBulletPoolSize; i++)
        {
            if (rigidPool[i] == false)
            {
                    
                Destroy(uBulletPool[i]);
            }
        }
        speed1 = 0f;
        
    }

   
}
