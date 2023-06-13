using UnityEngine;

/// <summary>
/// �f�o�C�X�̉����x���擾����
/// </summary>
public class AccelarationDataReceiver : DataReceiver_Base
{
	/// <summary>
	/// �f�o�C�X�̉����x
	/// </summary>
	public Vector2 Accelaration { get; private set; }       

    //--------------------------------------------------

    // ��M
    protected override void OnReceivedData()
    {
        var data = handler.GetSplitedData();     // �f�[�^�擾

        try {
            if (data.Length >= 2) {
                // �f�[�^�����񂩂�float�^�ɕϊ����āA�x�N�g���ɓK�p
                float x = float.Parse(data[0]);
                float y = float.Parse(data[1]);

                Accelaration = new Vector2(x, y);
            }
        }

        // ��O
        catch (System.Exception exc) {
            Debug.LogWarning(exc.Message);
        }
    }
}


