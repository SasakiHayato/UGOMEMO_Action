/// <summary>
/// ���щ��������S���C���^�[�t�F�[�X
/// ����͌p�����Ȃ�
/// </summary>
public interface IAchievement
{
    Achievement_Enum Type { get; }

    /// <summary>
    /// ���щ�����s���邩��S������
    /// </summary>
    /// <returns>[TRUE:���][FALSE:���s]</returns>
    bool Condition();
}

/// <summary>
/// ���щ��������S���C���^�[�t�F�[�X
/// �������p�����Ď�������
/// </summary>
/// <typeparam name="Value"></typeparam>
public interface IAchievement<Value> : IAchievement
{
    Value SendValue { set; }
}