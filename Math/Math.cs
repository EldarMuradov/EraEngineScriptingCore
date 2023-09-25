namespace EngineLibrary.Math
{
    public struct Math
    {
        public const float Epsilon = 0.000001f;
        public const float Rad2Deg = 57.29578f;
        public const float Deg2Rad = 0.01745329251f;
        public const float Pi = 3.1415926535f;

        public static float NormalizeAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
    }
}