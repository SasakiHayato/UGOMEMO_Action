/// <summary>
/// プロジェクトの定義クラス
/// </summary>
public static class Define
{
    public static class UI
    {
        public enum Type
        {

        }

        public static readonly UnityEngine.Vector2 Aspect = new UnityEngine.Vector2(1600, 900); 
    }

    public static class Input
    {
        public interface ICallback
        {
            void OnClick(IInputRayCastAddress address, UnityEngine.Vector2 click_position);
            void OnStay(IInputRayCastAddress address, UnityEngine.Vector2 current_position);
            void OnRelease(IInputRayCastAddress address, UnityEngine.Vector2 release_position);
            void OnFlicked(IInputRayCastAddress address, UnityEngine.Vector2 flick_derection, float distance);
        }

        public static float RADIUS = 0.05F;
        public static float FLICK_ATTRIBUTE_TIME = 0.15F;
        public static float FLICK_ATTRIBUTE_DISTANCE = 2.25F;
    }

    public static class Sound
    {
        public enum Type
        {
            BGM,
            SE,
            Environment,

            Length,
        }
        
        public static float MAX_VOLUME = 1;
    }

    public enum ObjectType
    {
        Player,
        Enemy,
        Obstacle,

        Item,

        None,
    }
}
