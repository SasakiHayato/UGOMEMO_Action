public class Achievement_Test : IAchievement
{
    int _value;

    int IAchievement.Value { set => _value = value; }

    Achievement_Enum IAchievement.Type => Achievement_Enum.Test;

    bool IAchievement.Condition()
    {
        return true;
    }
}
