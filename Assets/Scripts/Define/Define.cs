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

        public static string CLIP_DIRECTORY_GUID = "ddffd68275303554ab00fb88aa3ea584";
        public static string ADDRESS_TEXT_GUID = "0bd26e639ee380f42a6e2aac7808099e";
        public static string SOUND_ADDRESS_GUID = "6d28f0e86ba601649b447b1fc2276ce7";
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
