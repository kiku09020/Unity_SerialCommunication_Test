using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField, Tooltip("‰Â“®”ÍˆÍ")] float rotatableRigion = 90;
    [SerializeField,Tooltip("”½“]‚·‚é‚©")] bool isReverced;

    [Header("Components")]
    [SerializeField] AccelarationDataReceiver receiver;

	//--------------------------------------------------

	private void Awake()
	{
        // ”½“]‚³‚¹‚éê‡A-1‚ğ‚©‚¯‚é
        if (isReverced) {
            rotatableRigion *= -1;
        }
	}

	// ˆÚ“®
	void LateUpdate()
    {
        var euler = receiver.Accelaration * rotatableRigion;

        transform.rotation = Quaternion.Euler(euler.y, euler.x, 0);
    }
}
