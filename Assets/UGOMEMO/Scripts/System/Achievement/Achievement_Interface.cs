/// <summary>
/// ���щ��������S���C���^�[�t�F�[�X
/// </summary>
public interface IAchievement
{
    /// <summary>
    /// �A�`�[�u�����g�^�C�v
    /// </summary>
    Achievement_Enum Type { get; }

    /// <summary>
    /// GSS ����̎󂯎�����l
    /// </summary>
    int Value { set; }

    /// <summary>
    /// ���щ�����s���邩��S������
    /// </summary>
    /// <returns>[TRUE:���][FALSE:���s]</returns>
    bool Condition();
}