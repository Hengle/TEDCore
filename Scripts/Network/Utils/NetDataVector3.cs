using LiteNetLib.Utils;

namespace TEDCore.Network.Utils
{
    public static class NetDataVector3
    {
        public static void Put(this NetDataWriter dw, UnityEngine.Vector3 v3)
        {
            dw.Put(HalfTypeUtils.Convert(v3.x));
            dw.Put(HalfTypeUtils.Convert(v3.y));
            dw.Put(HalfTypeUtils.Convert(v3.z));
        }

        public static UnityEngine.Vector3 GetVector3(this NetDataReader dr)
        {
            return new UnityEngine.Vector3(HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()), HalfTypeUtils.Convert(dr.GetUShort()));
        }
    }
}
