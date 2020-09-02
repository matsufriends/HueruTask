using UnityEngine;

namespace matsu.HueruTask {
    public class MasterParameter : Singleton<MasterParameter> {

        //----------変数&プロパティ----------

        [Header("ゲーム時間")]
        [SerializeField] private float Day_Time_Single = 10f;
        [SerializeField] private float Day_Time_Multi = 10f;
        public float DayTime { get { return MasterGame.Instance.IsMulti ? Day_Time_Multi : Day_Time_Single; } }

        [Header("タスク処理時間")]
        [SerializeField] private float Work_Time_Min_Single = 0.5f;
        [SerializeField] private float Work_Time_Max_Single = 1.5f;
        [SerializeField] private float Work_Time_Min_Multi = 0.5f;
        [SerializeField] private float Work_Time_Max_Multi = 1.5f;
        public float WorkTimeMin { get { return MasterGame.Instance.IsMulti ? Work_Time_Min_Multi : Work_Time_Min_Single; } }
        public float WorkTimeMax { get { return MasterGame.Instance.IsMulti ? Work_Time_Max_Multi : Work_Time_Max_Single; } }

        [Header("タスク発生間隔")]
        [SerializeField] private float Task_Spawn_Interval_Single = 0.4f;
        [SerializeField] private float Task_Spawn_Yuragi_Single = 1f;
        [SerializeField] private float Task_Spawn_Interval_Multi = 0.3f;
        [SerializeField] private float Task_Spawn_Yuragi_Multi = 0.8f;
        public float TaskSpawnInterval { get { return MasterGame.Instance.IsMulti ? Task_Spawn_Interval_Multi : Task_Spawn_Interval_Single; } }
        public float TaskSpawnYuragi { get { return MasterGame.Instance.IsMulti ? Task_Spawn_Yuragi_Multi : Task_Spawn_Yuragi_Single; } }

        [Header("得点")]
        [SerializeField] private float Task_Point_Single = 1;
        [SerializeField] private float Task_Point_Multi = 3;
        public float TaskPoint { get { return MasterGame.Instance.IsMulti ? Task_Point_Multi : Task_Point_Single; } }

        [Header("パラメータ変換")]
        [SerializeField, Range(0, 1)] private float Parameter_To_Point_Ratio_Min_Single = 0.5f;
        [SerializeField, Range(0, 1)] private float Parameter_To_Point_Ratio_Min_Multi = 0.5f;
        public float ParameterToPointRatioMin { get { return MasterGame.Instance.IsMulti ? Parameter_To_Point_Ratio_Min_Multi : Parameter_To_Point_Ratio_Min_Single; } }

        [Header("集中力変換")]
        [SerializeField, Range(0, 1)] private float Concentration_To_Point_Ratio_Min_Single = 0.5f;
        [SerializeField, Range(0, 1)] private float Concentration_To_Point_Ratio_Min_Multi = 0.5f;
        public float ConcentrationToPointRatioMin { get { return MasterGame.Instance.IsMulti ? Concentration_To_Point_Ratio_Min_Multi : Concentration_To_Point_Ratio_Min_Single; } }

        [Header("集中力増加")]
        [SerializeField] private int Concentration_Add_Single = 20;
        [SerializeField] private int Concentration_Add_Multi = 30;
        public int ConcentrationAdd { get { return MasterGame.Instance.IsMulti ? Concentration_Add_Multi : Concentration_Add_Single; } }

        [Header("集中力低下速度")]
        [SerializeField] private float Concentration_Down_Speed_Single = 0.7f;
        [SerializeField] private float Concentration_Down_Speed_Multi = 0.6f;
        public float ConcentrationDownSpeed { get { return MasterGame.Instance.IsMulti ? Concentration_Down_Speed_Multi : Concentration_Down_Speed_Single; } }

    }
}