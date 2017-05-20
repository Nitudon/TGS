using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemParameter
{
    public static class GameEnum
    {
        public enum tresureColor { nothing, red, blue, yellow, green }

        public enum direction { left, up, down, right }

        public enum animTrigger { walk,stop,tresure}

        public enum BGM { title, battle, end}

        public enum SE { get, crash, start, end, cursor,decide,slide}
    }

    public static class GameValue
    {
        public static readonly int COLOR_LIST_COUNT = 4;

        public static readonly int SCORE_BASE = 100;

        public static readonly int SCORE_RATE_NUMBER = 150;

        public static readonly int SCORE_RATE_COLOR = 250;

        public static readonly int SCORE_RATE_PLAYER = 300;

        public static readonly float SCORE_RATE_CALCULATE = 1.5f;

        public static readonly float OWN_TRESURE_POSITION_OFFSET = 1.2f;

        public static readonly int OWN_TRESURE_MAX = 5;

        public static readonly int BATTLE_TIME = 120;

        public static readonly float SPEED_BASE = 0.7f;

        public static readonly float SPEED_BASE_SCALE = 1.0f;

        public static readonly float SPEED_RATE = 0.75f;

        public static readonly float SPIN_SPEED = 0.5f;

        public static readonly float SPEED_MAX = 0.3f;

        public static readonly float GENERATE_TIME_SPAN = 5f;

        public static readonly Vector3 LEFT_DIRECTION_ANGLE = new Vector3(0,-90,0);

        public static readonly Vector3 RIGHT_DIRECTION_ANGLE = new Vector3(0, 90, 0);

        public static readonly Vector3 UP_DIRECTION_ANGLE = new Vector3();

        public static readonly Vector3 DOWN_DIRECTION_ANGLE = new Vector3(0, 180, 0);
    }
}
