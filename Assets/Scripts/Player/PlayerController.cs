using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField, Tooltip("���͈�")] float rotatableRigion = 90;
    [SerializeField,Tooltip("���]���邩")] bool isReverced;

    [Header("Components")]
    [SerializeField] AccelarationDataReceiver receiver;

	//--------------------------------------------------

	private void Awake()
	{
        // ���]������ꍇ�A-1��������
        if (isReverced) {
            rotatableRigion *= -1;
        }
	}

	// �ړ�
	void LateUpdate()
    {
        var euler = receiver.Accelaration * rotatableRigion;

        transform.rotation = Quaternion.Euler(euler.y, euler.x, 0);
    }
}
