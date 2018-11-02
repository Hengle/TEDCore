using LiteNetLib.Utils;

namespace TEDCore.Network.Utils
{
    public static class NetDataVector2
    {
        public static void Put(this NetDataWriter dw, UnityEngine.Vector2 vector)
        {
            dw.Put(HalfTypeUtils.Convert(vector.x));
            dw.Put(HalfTypeUtils.Convert(vector.y));
        }

        public static UnityEngine.Vector2 GetVector2(this NetDataReader dr)
        {
            return new UnityEngine.Vector2(HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()));
        }
    }
}
