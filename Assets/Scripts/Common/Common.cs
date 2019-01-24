using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Common
{
    public class Common
    {

    }

    public static class ExternalFunctionality
    {
        [DllImport("User32.dll")]
        public static extern int MessageBox(System.IntPtr handle, string message, string title, int type);

        public static void Toggle(ref bool obj)
        {
            obj =  obj ? false : true;
        }

        public static Vector3 Reverse(this Vector3 dir)
        {
            return new Vector3(-dir.x, dir.y, -dir.z);
        }

        public static string GenID(int n)
        {
            string ab = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890_";
            string s = "";
            for (int i = 0; i < n; i++)
            {
                s += ab[Random.Range(0, ab.Length - 1)];
            }
            return s;
        }

        public static T Last<T>(this Queue<T> q)
        {
            return q.ToArray()[q.Count - 1];
        }

        public static Vector3 MousePosition()
        {
            return Input.mousePosition + (Vector3)Random.insideUnitCircle * Shared.Shaking;
        }
    }

    public enum Presets
    {
        // Vector3 Rotation Presets
        x2y,
        y2z,
        z2x,
        y2x,
        z2y,
        x2z,
        x,
        y,
        z
    }

    public static class Shared
    {
        public static Vector3 SpectPoint;
        public static Vector3 SpectPointOffset = Vector3.zero;
        public static float Shaking = 1.0f;
    }
}
