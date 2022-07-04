using System;
using UnityEngine;


namespace CustomMath
{
    public struct Quat //: IEquatable<Quat>, IFormattable
    {

        #region variables

        public float x;
        public float y;
        public float z;
        public float w;


        public Vec3 eulerAngles
        {
            get
            {
                return ToEulerAngles(this) * Mathf.Rad2Deg;    //Tranforma el Quaternion a Euler y transforma de Radianes a Grados
            }
            set
            {
                this = ToQuaternion(value * Mathf.Deg2Rad);    //Tranforma el Vector3 a Quaternion y transforma de Grados a Radianes
            }
        }

        public Quat normalized => Normalize(this);

        public static Quat Euler(Vec3 euler) => ToQuaternion(euler);

        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => x,
                    1 => y,
                    2 => z,
                    3 => w,
                    _ => throw new IndexOutOfRangeException("Invalid Quaternion index!"),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Quaternion index!");
                }
            }
        }

        #endregion

        #region Constants

        public const float kEpsilon = 1E-06f;

        #endregion

        #region Static_Methods 

        public static Quat LookRotation(Vec3 forward) => LookRotation(forward, Vec3.Up);

        public static Quat Slerp(Quat a, Quat b, float t)
        {
            return SlerpUnclamped(a, b, Mathf.Clamp01(t));
        }

        public static Quat SlerpUnclamped(Quat a, Quat b, float t)
        {
            Quat r;

            float time = 1 - t;

            float wa, wb, sn;

            float theta = Mathf.Acos(Dot(a, b)); //Se calcula el ArcoCoseno entre los 2 Quaterniones para calcular el sentido.

            if (theta < 0)
            {
                theta = -theta; // Si el resultado es menor a cero, se invierte el valor para que siempre sea un numero positivo.
            }

            sn = Mathf.Sin(theta); //En caso de ser mayor, se calcula el seno

            wa = Mathf.Sin(time * theta) / sn;
            wb = Mathf.Sin((1 - time) * theta) / sn;

            r.x = wa * a.x + wb * b.x;
            r.y = wa * a.y + wb * b.y;
            r.z = wa * a.z + wb * b.z;
            r.w = wa * a.w + wb * b.w;

            r.Normalize();

            return r;
        }

        public static Quat Lerp(Quat a, Quat b, float t)
        {
            return LerpUnclamped(a, b, Mathf.Clamp01(t));
        }

        public static Quat LerpUnclamped(Quat a, Quat b, float t)
        {
            Quat r;
            float time = 1 - t;

            r.x = time * a.x + t * b.x;
            r.y = time * a.y + t * b.y;
            r.z = time * a.z + t * b.z;
            r.w = time * a.w + t * b.w;

            r.Normalize();

            return r;
        }

        #endregion

        #region Constructors

        //
        // Resumen:
        //     Constructs new Quaternion with given x,y,z,w components.
        //
        // Parámetros:
        //   x:
        //
        //   y:
        //
        //   z:
        //
        //   w:
        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }


        #endregion

        #region Default_Values

        public static Quat identity => new Quat(0f, 0f, 0f, 1f);

        #endregion

        #region Operators

        public static Quat operator *(Quat lhs, Quat rhs)
        {
            return new Quat(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                            lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z,
                            lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x,
                            lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        public static Vec3 operator *(Quat rotation, Vec3 point)
        {
            float rotX = rotation.x * 2f; //rotX
            float rotY = rotation.y * 2f; //rotY
            float rotZ = rotation.z * 2f; //rotZ

            float rotX2 = rotation.x * rotX; //rotX2
            float rotY2 = rotation.y * rotY; //rotY2
            float rotZ2 = rotation.z * rotZ; //rotZ2

            float rotXY = rotation.x * rotY; //rotXY
            float rotXZ = rotation.x * rotZ; //rotXZ
            float rotYZ = rotation.y * rotZ; //rotYZ

            float rotWX = rotation.w * rotX;  //rotWX
            float rotWY = rotation.w * rotY;  //rotWY
            float rotWZ = rotation.w * rotZ;  //rotWZ

            Vec3 result = Vec3.Zero;

            result.x = (1f - (rotY2 + rotZ2)) * point.x + (rotXY - rotWZ) * point.y + (rotXZ + rotWY) * point.z;
            result.y = (rotXY + rotWZ) * point.x + (1f - (rotX2 + rotZ2)) * point.y + (rotYZ - rotWX) * point.z;
            result.z = (rotXZ - rotWY) * point.x + (rotYZ + rotWX) * point.y + (1f - (rotX2 + rotY2)) * point.z;

            return result; //Se calcula el vector Unitario
        }

        public static bool operator ==(Quat lhs, Quat rhs)
        {
            return IsEqualUsingDot(Dot(lhs, rhs));
        }

        public static bool operator !=(Quat lhs, Quat rhs)
        {
            return !(lhs == rhs);
        }

        private static bool IsEqualUsingDot(float dot)
        {
            return dot > 0.999999f; //Si da 1 es que son paralelos (iguales).
        }

        public static implicit operator Quaternion(Quat quat)
        {
            return new Quaternion(quat.x, quat.y, quat.z, quat.w);
        }

        public static implicit operator Quat(Quaternion quaternion)
        {
            return new Quat(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }
        #endregion

        #region Functions

        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        } //Ok

        public static float Dot(Quat a, Quat b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        } //Ok

        public static float Angle(Quat a, Quat b)
        {
            float num = Dot(a, b);
            return IsEqualUsingDot(num) ? 0f : (Mathf.Acos(Mathf.Min(Mathf.Abs(num), 1f)) * 2f * 57.29578f);
        } //Ok

        public static Quat Euler(float x, float y, float z) 
        {
            return ToQuaternion(new Vec3(x, y, z) * Mathf.Deg2Rad);
        } //Ok

        private static Vec3 ToEulerAngles(Quat quat)
        {
            Vec3 angles;

            float sinr_cosp = 2 * (quat.w * quat.x + quat.y * quat.z);
            float cosr_cosp = 1 - 2 * (quat.x * quat.x + quat.y * quat.y);
            angles.x = Mathf.Atan2(sinr_cosp, cosr_cosp);

            float sinp = 2 * (quat.w * quat.y - quat.z * quat.x);

            if (Mathf.Abs(sinp) >= 1)
            {
                angles.y = (Mathf.PI / 2) * Mathf.Sign(sinp); 
            }
            else
            {
                angles.y = Mathf.Asin(sinp);
            }

            float siny_cosp = 2 * (quat.w * quat.z + quat.x * quat.y);
            float cosy_cosp = 1 - 2 * (quat.y * quat.y + quat.z * quat.z);
            angles.z = (float)Mathf.Atan2(siny_cosp, cosy_cosp);

            return angles;

        } //Ok

        private static Quat ToQuaternion(Vec3 vec3)
        {
            float cosZ = Mathf.Cos(Mathf.Deg2Rad * vec3.z * .5f); //Real
            float senZ = Mathf.Sin(Mathf.Deg2Rad * vec3.z * .5f); //Imaginaria
            float cosY = Mathf.Cos(Mathf.Deg2Rad * vec3.y * .5f);
            float senY = Mathf.Sin(Mathf.Deg2Rad * vec3.y * .5f);
            float cosX = Mathf.Cos(Mathf.Deg2Rad * vec3.x * .5f);
            float senX = Mathf.Sin(Mathf.Deg2Rad * vec3.x * .5f);

            Quat quat = new Quat();

            quat.w = cosX * cosY * cosZ + senX * senY * senZ; 
            quat.x = senX * cosY * cosZ - cosX * senY * senZ;
            quat.y = cosX * senY * cosZ + senX * cosY * senZ;
            quat.z = cosX * cosY * senZ - senX * senY * cosZ;

            return quat;
        } //Ok

        public static Quat Normalize(Quat q)
        {
            float num = Mathf.Sqrt(Dot(q, q));
            if (num < Mathf.Epsilon)
            {
                return identity;
            }

            return new Quat(q.x / num, q.y / num, q.z / num, q.w / num);
        } 

        public void Normalize()
        {
            this = Normalize(this);
        } 

        public static Quat Inverse(Quat rotation)
        {
            return new Quat(-rotation.x, -rotation.y, -rotation.z, rotation.w);
        }

        public static Quat LookRotation(Vec3 forward, Vec3 upwards)
        {
            Vec3 dir = Vec3.Normalize(upwards - forward);
            Vec3 rotAxis = Vec3.Cross(Vec3.Forward, dir);
            float dot = Vec3.Dot(Vec3.Forward, dir);

            Quat result;
            result.x = rotAxis.x;
            result.y = rotAxis.y;
            result.z = rotAxis.z;
            result.w = dot + 1;

            return result.normalized;
        }

        public static Quat RotateTowards(Quat from, Quat to, float maxDegreesDelta)
        {
            float angle = Angle(from, to);

            if (angle == 0f)
            {
                return to;
            }

            return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / angle));
        }

        #endregion

        #region internal

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2) ^ (w.GetHashCode() >> 1);
        }

        public override bool Equals(object other)
        {
            if (!(other is Quat))
            {
                return false;
            }

            return Equals((Quat)other);
        }

        public bool Equals(Quat other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
        }

        public override string ToString()
        {
            return "X = " + x.ToString() + "   Y = " + y.ToString() + "   Z = " + z.ToString() + "   W = " + w.ToString();
        }

        #endregion
    }
}

