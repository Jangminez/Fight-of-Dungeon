// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("wx2wVhKwljJXr1l/FuIeu3/j9kWSGPEsGbD3Ag/+MDRL+iOQIqVJcQ2OgI+/DY6FjQ2Ojo8n3OCAbiwrSEt1cJb6VwWV7x5iuYQlT0BlUDz98OTzll3iwy8Jbz54ft9BLqVK+bIgm1K1owYg7iQ2sgGl/R2U8xgV/cenz/5AhcIugNWc5aa6swCE84RqZCRAcFAGn6lGZXjAt3bC1Qglnb8Njq2/gomGpQnHCXiCjo6Oio+MawiqK58gx9gu7gHkWsJ81e03l7/RCRtRDeWr48EVuRixuaKifgdWbrd8QudIvEgzI2TGoVL406hdGKWGrI/q/PTF3sPQhVz76jVTN9Kvi/5pi9MtlPJOK21qszukN8EwllUG30Jjjsb+YwFY2I2Mjo+O");
        private static int[] order = new int[] { 8,6,6,12,12,13,13,13,8,11,12,13,13,13,14 };
        private static int key = 143;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
