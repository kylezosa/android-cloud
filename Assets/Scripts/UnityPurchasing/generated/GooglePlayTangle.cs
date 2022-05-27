#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("j3BtdL71/tkYEoXIxPy0bKapKimQ6S6WF/XpuZcXlgCH/CBkoOvhncoDa1pDsrpkEK199QdHrw54433lvjd0M6zjFCo5qklbIFdGHP/C+4lTrg05O4nvlw7hcz2T/5NCuEbKCJESHBMjkRIZEZESEhORpA0OY0XCXVNyOvmjHR4SYRmgPiHiTm2ksfOmglwW8s3A6+O/YDaXuGTW4d9DJJ8AIfX8Eb72+DpiObSp6c1xOQa/yhPCwnthYC2oy6s/gZiwdM6VykzovxC9bpZSofghJY7l4jWzN2zKMSApNrKF3Lvq+w9rAaqalNWA/7u2I5ESMSMeFRo5lVuV5B4SEhIWExB48MgHdw/ygGIjQXPtfyjQdMfYLZ9+KtBOi12C4hEQEhMS");
        private static int[] order = new int[] { 5,12,7,11,4,12,9,8,10,13,10,13,12,13,14 };
        private static int key = 19;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
