using UnityEngine;

public static class MyMath {

    //------------メソッド----------

    public static float Remap(float value, float in_Min, float in_Max, float out_Min, float out_Max) {
        if (in_Max < in_Min) return Remap(value, in_Max, in_Min, out_Max, out_Min);
        value = Mathf.Clamp(value, in_Min, in_Max);
        float ratio = (value - in_Min) / (in_Max - in_Min);
        return out_Min + (out_Max - out_Min) * ratio;
    }

}
