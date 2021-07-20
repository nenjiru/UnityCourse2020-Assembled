using UnityEngine;
using UnityEditor;

/// <summary>
/// 一人称視点プレイヤー
/// </summary>
public class FPSPlayer : MonoBehaviour
{
    /// <summary> 移動速度 </summary>
    public float moveSpeed = 1.5f;

    /// <summary> ジャンプ速度 </summary>
    public float jumpSpeed = 4f;

    /// <summary> 重力値（constで定数化） </summary>
    private const float gravity = 9.81f;

    /// <summary> 速度ベクトル。CharacterController.Move 関数に渡してプレイヤーを移動させる </summary>
    private Vector3 velocity;

    /// <summary> キャラクター制御コンポーネント </summary>
    private CharacterController controller;

    /// <summary> 一人称視点のカメラ </summary>
    private Transform cameraTransform;

    /// <summary> 武器を掴んださいの親要素 </summary>
    private Transform handPoint;

    /// <summary> 武器を持っているか </summary>
    private bool hasWeapon = false;

    /// <summary> プレイモード直後に一度だけ実行される </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();
        // UseWeaponクラスを作成したらコメントアウトを解除
        UseWeapon useWeapon = GetComponent<UseWeapon>();
        handPoint = useWeapon?.handPoint;
    }

    /// <summary> 毎フレーム（およそ0.01667秒）ごとに実行される </summary>
    void Update()
    {
        // 武器を持っているか
        if (handPoint != null)
        {
            hasWeapon = (handPoint.childCount > 0);
        }

        // 武器を持っているか
        if (hasWeapon && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!hasWeapon && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (hasWeapon)
        {
            // 武器をもっていたら常に回転処理を行う
            rotateUpdate();
        }
        else if (!hasWeapon && Input.GetButton("Fire1"))
        {
            // 武器をもっていなかったら攻撃ボタン入力時に回転処理を行う
            rotateUpdate();
        }

        // 移動処理を行う
        moveUpdate();
    }

    /// <summary>
    /// 回転処理を行う関数
    /// 左右の回転はこのクラスがアタッチされたオブジェクトの向きを変える
    /// 上下の回転はこれはカメラに対して行う
    /// ＊左右の回転は体ごと、上下回転は首で行うイメージ
    ///</summary>
    void rotateUpdate()
    {
        //// マウスの移動量を取得
        // xが左右、yが上下
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        //// 左右の移動量をY軸に反映させる
        // Transform コンポーネントの Rotate 関数は現在の回転角度に追加できるのでこれを使う
        // 引数は X, Y, Z 軸の順。Y軸に移動量を渡す
        transform.Rotate(0f, x, 0f);

        //// 上下の移動量をX軸に反映させる
        // 上下の移動量をそのまま使うと逆回転してしまうため移動量を - に反転しておく
        // 飛行機シミュレーター的な操作方法であれば、この処理はいらない
        y = -y;

        //// 上下の回転はカメラなので cameraTransform に対して行う
        // しかし Rotate で行ってしまうと無限に回転できてしまう
        // cameraTransform.Rotate(y, 0f, 0f);

        // 人体の首と同じく限界値を設けるには回転量を計算しておき限界値を超えたら制限する、という実装を行う
        // まずはカメラの現在の角度にマウスの移動量を加える
        float angle = cameraTransform.localEulerAngles.x + y;
        // 回転量を制限内に収める
        angle = clampAngle(angle);
        // 回転量をクォータニオンにする
        Quaternion q = Quaternion.Euler(angle, 0f, 0f);
        // 左右の回転はすでに行ってあるのでローカルの回転角度として反映する
        cameraTransform.localRotation = q;
    }

    /// <summary>
    /// 移動処理を行う関数
    /// コントローラーの入力で前後左右に移動させる
    /// 移動の基準はプレイヤーの向きに従う。例えばプレイヤーが北を向いてる状態で左キーを入力すると西に移動する
    /// </summary>
    void moveUpdate()
    {
        //// コントローラーの入力値を取得
        // xが左右、zが前後
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // コントローラーの入力値をベクトルにする。シーンの原点を基準としたワールド座標となる
        Vector3 input = new Vector3(x, 0f, z);

        // 入力値ベクトルをプレイヤーを基点としたワールド座標に変換して方向ベクトルにする
        Vector3 direction = transform.TransformDirection(input);

        // 方向ベクトルに移動速度をかけて速度ベクトルに反映
        velocity.x = direction.x * moveSpeed;
        velocity.z = direction.z * moveSpeed;

        //// コントローラーのジャンプボタンの入力判定とプレイヤーが設置しているかの判定
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            // ジャンプボタン入力時に接地している場合にジャンプ速度を速度ベクトルのYに入れる
            velocity.y = jumpSpeed;
        }

        //// 速度ベクトルのY軸に重力で落下させる
        // Time.deltaTime は前回からのフレーム秒数。これに重力値を乗算することでこのフレームで落ちるべき速度が得られる
        // デクリメント（-=）しているのは常に重力をかけるため。これで最終的な速度量が得られる
        velocity.y -= gravity * Time.deltaTime;

        // 速度ベクトルに Time.deltaTime を乗せてこのフレームで動かしたい運動量にする
        Vector3 motion = velocity * Time.deltaTime;

        // CharacterController.Move に運動量を与える
        controller.Move(motion);
    }

    /// <summary>
    /// 制限内におさめた float を返す関数
    /// 制限は -80〜80
    /// </summary>
    float clampAngle(float angle)
    {
        angle += 180f;
        angle = Mathf.Repeat(angle, 360);
        angle -= 180;
        angle = Mathf.Clamp(angle, -80f, 80f);
        return angle;
    }

#if UNITY_EDITOR
    // Unityエディタ上でしか実行できないブロック（ビルド後は実行できない）

    // ギズモの切り替え
    [System.Serializable]
    public class Option
    {
        public bool isPlayerDraw = false;
        public bool isInputDraw = false;
        public bool isDirectionDraw = false;
        public bool isVelocityDraw = false;
    }
    [SerializeField]
    private Option option;

    void OnDrawGizmos()
    {
        // プレイヤーの座標
        var pos = transform.position;

        if (option.isPlayerDraw)
        {
            // プレイヤーの向きを描画
            Handles.color = Handles.zAxisColor;
            Handles.ArrowHandleCap(
                    0,
                    transform.position,
                    transform.rotation * Quaternion.LookRotation(Vector3.forward),
                    1f,
                    EventType.Repaint
                );
        }

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = Camera.main.transform.TransformDirection(input);

        if (option.isInputDraw)
        {
            // 入力ベクトルを描画
            Debug.DrawLine(Vector3.zero, input, Color.green);
            // キー入力を描画
            Handles.Label(Vector3.zero, string.Format("{0}, {1}", input.x.ToString("f2"), input.z.ToString("f2")));
        }

        if (option.isVelocityDraw)
        {
            // 速度ベクトルを描画
            Debug.DrawLine(pos, velocity + pos, Color.red);
        }

        if (option.isDirectionDraw)
        {
            // 方向ベクトルを描画
            Debug.DrawLine(pos, direction + pos, Color.cyan);
        }
    }
#endif
}