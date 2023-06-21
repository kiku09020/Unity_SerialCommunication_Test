using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField, Tooltip("移動閾値"),Range(0,1)] 
    float moveThreshold = 0.75f;

    [SerializeField,Tooltip("移動距離")] float moveDistance = .1f;
    [SerializeField, Tooltip("可動範囲")] float movableRegion = 10;

    [Header("Gun")]
    [SerializeField, Tooltip("大砲の可動X角度の最小値"), Range(-180, 180)] 
    float gunRegion_Min = -30;
    [SerializeField, Tooltip("大砲の可動X角度の最大値"), Range(-180, 180)] 
    float gunRegion_Max = 0;

    [Header("Shot")]
    [SerializeField] Bullet bullet;
    [SerializeField] Transform muzzle;
    [SerializeField,Tooltip("発射までのクールタイム")] 
    float shootCoolTime = .25f;
    [SerializeField,Tooltip("長押しショット可能か")]
    bool enableLongPressShot;

    bool isCoolTime;         // 押されたか
    float shootCoolTimer;   // クールタイムのタイマー

    [Header("Components")]
    [SerializeField] TankDataReceiver dataReceiver;

	//--------------------------------------------------

	void Awake()
    {
        // 最小値、最大値補正
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

    // タンクの移動
    void Move()
    {
        // 横方向の傾きが、閾値以上だったら移動
        if (Mathf.Abs(dataReceiver.Gyro.x) >= moveThreshold && CheckMovable()) {
            var dir = Mathf.Sign(dataReceiver.Gyro.x);
            var moveX = dir * moveDistance;

            transform.Translate(new Vector3(moveX * -1, 0));
        }
    }

    // 動けるかどうかを判定する
    bool CheckMovable()
    {
        if (-movableRegion <= transform.position.x && transform.position.x <= movableRegion) {
            return true;
        }

        // 可動範囲はみ出し時の補正
        else {
            // 左側
            if (transform.position.x < 0) {
                transform.position = new Vector3(-movableRegion, transform.position.y, transform.position.z);
            }

            // 右側
            else {
                transform.position = new Vector3(movableRegion, transform.position.y, transform.position.z);
            }
            return false;
        }
    }

    // 大砲の照準移動
    void MoveGunAiming()
    {
        float gunAim = Mathf.Clamp(dataReceiver.Gyro.y * -30, gunRegion_Min, gunRegion_Max);

        transform.rotation = Quaternion.Euler(gunAim, 0, 0);
    }

    // 弾を撃つ
    void Shoot()
    {
        if (dataReceiver.IsPressed && !isCoolTime) {
            isCoolTime = true;
            var objBullet = Instantiate(bullet, muzzle.transform.position,Quaternion.identity);     // 弾生成
            objBullet.Shot(muzzle);
        }

        // クールタイム
        else if (isCoolTime) {
            shootCoolTimer += Time.deltaTime;

            // クールタイムを超えたら、再度押せるようにする
            if (shootCoolTimer >= shootCoolTime && (!dataReceiver.IsPressed ^ enableLongPressShot)) {
                isCoolTime = false;
                shootCoolTimer = 0;
            }
        }
    }
}
