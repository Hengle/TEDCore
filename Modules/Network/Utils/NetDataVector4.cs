using LiteNetLib.Utils;

namespace TEDCore.Network.Utils
{
    public static class NetDataVector4
    {
        public static void Put(this NetDataWriter dw, UnityEngine.Vector4 v4)
        {
            dw.Put(HalfTypeUtils.Convert(v4.x));
            dw.Put(HalfTypeUtils.Convert(v4.y));
            dw.Put(HalfTypeUtils.Convert(v4.z));
            dw.Put(HalfTypeUtils.Convert(v4.w));
        }

        public static UnityEngine.Vector4 GetVector4(this NetDataReader dr)
        {
            return new UnityEngine.Vector4(HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()));
        }
    }
}
