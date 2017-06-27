using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemParameter
{
    public static class GameEnum
    {
        public enum gameType { team, battle}

        public enum tresureColor { nothing, red, blue, yellow, green }

        public enum animTrigger { walk,stop,tresure, crash}

        public enum resultAnimPose { win, lose, shock}

        public enum BGM { title, battle, end}

        public enum SE { get, crash, start, end, cursor, decide, slide, cancel, submit, role, slot}
    }

    public static class GameValue
    {
        #region[Tresure]

        public static readonly int COLOR_LIST_COUNT = 4;

        public static readonly int SCORE_BASE = 100;

        public static readonly int SCORE_RATE_NUMBER = 1000;

        public static readonly int SCORE_RATE_COLOR = 500;

        public static readonly int SCORE_RATE_PLAYER = 2500;

        public static readonly float SCORE_RATE_CALCULATE = 1.5f;

        public static readonly float OWN_TRESURE_POSITION_OFFSET = 1.2f;

        public static readonly int OWN_TRESURE_MAX = 3;

        #endregion

        #region[Battle]

        public static readonly float BASE_MOTION_SPEED = 2.0f;

        public static readonly int STAGE_NUM = 5;

        public static readonly int MIN_PLAYER_NUM = 2;

        public static readonly int MAX_PLAYER_NUM = 4;

        public static readonly int BATTLE_TIME = 120;

        public static readonly float SPEED_BASE = 0.7f;

        public static readonly float SPEED_BASE_SCALE = 1.0f;

        public static readonly float SPEED_RATE = 0.75f;

        public static readonly float SPIN_SPEED = 0.5f;

        public static readonly float SPEED_MAX = 0.3f;

        public static readonly float GENERATE_TIME_SPAN = 5f;

        public static readonly int S3_SCORE = 250000;

        public static readonly int S_SCORE = 150000;

        public static readonly int A_SCORE = 100000;

        public static readonly int B_SCORE = 75000;

        #endregion

    }
}
