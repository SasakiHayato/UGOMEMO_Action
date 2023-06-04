public class Achievement_Test : IAchievement<int>
{
    private int _a;

    int IAchievement<int>.SendValue { set => _a = value; }

    Achievement_Enum IAchievement.Type => Achievement_Enum.Test;

    bool IAchievement.Condition()
    {
        return true;
    }
}
