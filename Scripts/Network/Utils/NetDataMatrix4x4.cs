using LiteNetLib.Utils;

namespace TEDCore.Network.Utils
{
    public static class NetDataMatrix4x4
    {
        public static void Put(this NetDataWriter dw, UnityEngine.Matrix4x4 matrix)
        {
            dw.Put(matrix.GetColumn(0));
            dw.Put(matrix.GetColumn(1));
            dw.Put(matrix.GetColumn(2));
            dw.Put(matrix.GetColumn(3));
        }

        public static UnityEngine.Matrix4x4 GetMatrix4x4(this NetDataReader dr)
        {
            return new UnityEngine.Matrix4x4(dr.GetVector4(), dr.GetVector4(), dr.GetVector4(), dr.GetVector4());
        }
    }
}
