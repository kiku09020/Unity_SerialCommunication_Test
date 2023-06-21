using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField, Tooltip("�ړ�臒l"),Range(0,1)] 
    float moveThreshold = 0.75f;

    [SerializeField,Tooltip("�ړ�����")] float moveDistance = .1f;
    [SerializeField, Tooltip("���͈�")] float movableRegion = 10;

    [Header("Gun")]
    [SerializeField, Tooltip("��C�̉�X�p�x�̍ŏ��l"), Range(-180, 180)] 
    float gunRegion_Min = -30;
    [SerializeField, Tooltip("��C�̉�X�p�x�̍ő�l"), Range(-180, 180)] 
    float gunRegion_Max = 0;

    [Header("Shot")]
    [SerializeField] Bullet bullet;
    [SerializeField] Transform muzzle;
    [SerializeField,Tooltip("���˂܂ł̃N�[���^�C��")] 
    float shootCoolTime = .25f;
    [SerializeField,Tooltip("�������V���b�g�\��")]
    bool enableLongPressShot;

    bool isCoolTime;         // �����ꂽ��
    float shootCoolTimer;   // �N�[���^�C���̃^�C�}�[

    [Header("Components")]
    [SerializeField] TankDataReceiver dataReceiver;

	//--------------------------------------------------

	void Awake()
    {
        // �ŏ��l�A�ő�l�␳
        if (gunRegion_Min > gunRegion_Max) {
            gunRegion_Min = gunRegion_Max;
        }
    }

    void LateUpdate()
    {
        Move();
        MoveGunAiming();

        Shoot();

        print(dataReceiver.IsPressed);
    }

    // �^���N�̈ړ�
    void Move()
    {
        // �������̌X�����A臒l�ȏゾ������ړ�
        if (Mathf.Abs(dataReceiver.Gyro.x) >= moveThreshold && CheckMovable()) {
            var dir = Mathf.Sign(dataReceiver.Gyro.x);
            var moveX = dir * moveDistance;

            transform.Translate(new Vector3(moveX * -1, 0));
        }
    }

    // �����邩�ǂ����𔻒肷��
    bool CheckMovable()
    {
        if (-movableRegion <= transform.position.x && transform.position.x <= movableRegion) {
            return true;
        }

        // ���͈͂͂ݏo�����̕␳
        else {
            // ����
            if (transform.position.x < 0) {
                transform.position = new Vector3(-movableRegion, transform.position.y, transform.position.z);
            }

            // �E��
            else {
                transform.position = new Vector3(movableRegion, transform.position.y, transform.position.z);
            }
            return false;
        }
    }

    // ��C�̏Ə��ړ�
    void MoveGunAiming()
    {
        float gunAim = Mathf.Clamp(dataReceiver.Gyro.y * -30, gunRegion_Min, gunRegion_Max);

        transform.rotation = Quaternion.Euler(gunAim, 0, 0);
    }

    // �e������
    void Shoot()
    {
        if (dataReceiver.IsPressed && !isCoolTime) {
            isCoolTime = true;
            var objBullet = Instantiate(bullet, muzzle.transform.position,Quaternion.identity);     // �e����
            objBullet.Shot(muzzle);
        }

        // �N�[���^�C��
        else if (isCoolTime) {
            shootCoolTimer += Time.deltaTime;

            // �N�[���^�C���𒴂�����A�ēx������悤�ɂ���
            if (shootCoolTimer >= shootCoolTime && (!dataReceiver.IsPressed ^ enableLongPressShot)) {
                isCoolTime = false;
                shootCoolTimer = 0;
            }
        }
    }
}
