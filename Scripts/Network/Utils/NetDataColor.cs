using LiteNetLib.Utils;

namespace TEDCore.Network.Utils
{
    public static class NetDataColor
    {
        public static void Put(this NetDataWriter dw, UnityEngine.Color color)
        {
            dw.Put(HalfTypeUtils.Convert(color.r));
            dw.Put(HalfTypeUtils.Convert(color.g));
            dw.Put(HalfTypeUtils.Convert(color.b));
            dw.Put(HalfTypeUtils.Convert(color.a));
        }

        public static UnityEngine.Color GetColor(this NetDataReader dr)
        {
            return new UnityEngine.Color(HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()));
        }
    }
}
