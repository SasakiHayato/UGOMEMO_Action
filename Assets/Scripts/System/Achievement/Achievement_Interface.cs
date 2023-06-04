/// <summary>
/// 実績解放処理を担うインターフェース
/// これは継承しない
/// </summary>
public interface IAchievement
{
    Achievement_Enum Type { get; }

    /// <summary>
    /// 実績解放を行えるかを担う処理
    /// </summary>
    /// <returns>[TRUE:解放][FALSE:失敗]</returns>
    bool Condition();
}

/// <summary>
/// 実績解放処理を担うインターフェース
/// こいつを継承して実装する
/// </summary>
/// <typeparam name="Value"></typeparam>
public interface IAchievement<Value> : IAchievement
{
    Value SendValue { set; }
}