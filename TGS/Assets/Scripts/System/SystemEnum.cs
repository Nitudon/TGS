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
        public static readonly float MOVE_BASE_SPEED = 0.01f;

        public static readonly int SCORE_BASE = 100;

        public static readonly int SCORE_RATE_NUMBER = 100;

        public static readonly int SCORE_RATE_COLOR = 100;

        public static readonly int SCORE_RATE_PLAYER = 300;

        public static readonly float SCORE_RATE_CALCULATE = 1.2f;
    }
}
