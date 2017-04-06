using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemParameter
{
    public static class GameEnum
    {
        public enum tresureColor { red, blue, yellow, green }

        public enum direction { left, up, down, right}
    }

    public static class GameValue
    {
        public static readonly float MOVE_BASE_SPEED = 0.5f;
    }
}
