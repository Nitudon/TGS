using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemParameter
{
    public static class GameEnum
    {
        public enum gameType { team, battle}

        public enum tresureColor { nothing, red, blue, yellow, green }

        public enum animTrigger { walk,stop,tresure}

        public enum resultAnimPose { win, lose}

        public enum BGM { title, battle, end}

        public enum SE { get, crash, start, end, cursor, decide, slide, cancel, submit}
    }

    public static class GameValue
    {
        #region[Tresure]

        public static readonly int COLOR_LIST_COUNT = 4;

        public static readonly int SCORE_BASE = 100;

        public static readonly int SCORE_RATE_NUMBER = 150;

        public static readonly int SCORE_RATE_COLOR = 400;

        public static readonly int SCORE_RATE_PLAYER = 700;

        public static readonly float SCORE_RATE_CALCULATE = 1.5f;

        public static readonly float OWN_TRESURE_POSITION_OFFSET = 1.2f;

        public static readonly int OWN_TRESURE_MAX = 3;

        #endregion

        #region[Battle]

        public static readonly int MIN_PLAYER_NUM = 2;

        public static readonly int MAX_PLAYER_NUM = 4;

        public static readonly int BATTLE_TIME = 120;

        public static readonly float SPEED_BASE = 0.7f;

        public static readonly float SPEED_BASE_SCALE = 1.0f;

        public static readonly float SPEED_RATE = 0.75f;

        public static readonly float SPIN_SPEED = 0.5f;

        public static readonly float SPEED_MAX = 0.3f;

        public static readonly float GENERATE_TIME_SPAN = 5f;

        #endregion

    }
}
