using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemParameter
{
    public static class GameEnum
    {
        public enum tresureColor { nothing, red, blue, yellow, green }

        public enum direction { left, up, down, right }
    }

    public static class GameValue
    {
        public static readonly float MOVE_BASE_SPEED = 5.0f;

        public static readonly int SCORE_BASE = 100;

        public static readonly int SCORE_RATE_NUMBER = 100;

        public static readonly int SCORE_RATE_COLOR = 100;

        public static readonly int SCORE_RATE_PLAYER = 300;

        public static readonly float SCORE_RATE_CALCULATE = 1.2f;

        public static readonly float OWN_TRESURE_POSITION_OFFSET = 0.6f;

        public static readonly int OWN_TRESURE_MAX = 5;

        public static readonly Vector3 LEFT_DIRECTION_ANGLE = new Vector3(0,-90,0);

        public static readonly Vector3 RIGHT_DIRECTION_ANGLE = new Vector3(0, 90, 0);

        public static readonly Vector3 UP_DIRECTION_ANGLE = new Vector3();

        public static readonly Vector3 DOWN_DIRECTION_ANGLE = new Vector3(0, 180, 0);
    }
}
