/// <summary>
/// GSS からデータを受け取るデータクラス
/// </summary>
[System.Serializable]
public class AchievementJsonModel
{
    public Model[] Data;

    [System.Serializable]
    public class Model
    {
        public string Action;
        public int Rank;
        public string Text;
        public int Value;
    }
}
