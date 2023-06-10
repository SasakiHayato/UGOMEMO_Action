/// <summary>
/// 実績解放処理を担うインターフェース
/// </summary>
public interface IAchievement
{
    /// <summary>
    /// アチーブメントタイプ
    /// </summary>
    Achievement_Enum Type { get; }

    /// <summary>
    /// GSS からの受け取った値
    /// </summary>
    int Value { set; }

    /// <summary>
    /// 実績解放を行えるかを担う処理
    /// </summary>
    /// <returns>[TRUE:解放][FALSE:失敗]</returns>
    bool Condition();
}