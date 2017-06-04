using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using project.recso.bascula.data;
using System.IO;
using System.Security.Cryptography;

namespace project.recso.bascula.logic
{
    public class claseIntercambio
    {
        public static String connectionString = "name=recso2011DBEntities";
        private static recso2011DBEntities gestorEntidades = null;

        /// <summary>         
        /// Obtiene el manejador de las entidades
        /// </summary>
        /// <returns></returns>
        public static recso2011DBEntities getGestor()
        {
            if (gestorEntidades == null)
            {
                gestorEntidades=new data.recso2011DBEntities(logic.claseIntercambio.connectionString);
            }
            return gestorEntidades;
        }

        public static String paraFiltros(String frase)
        {
            String res = frase.ToLower();
            res = res.Replace(" ", "");
            res = res.Replace("á", "a");
            res = res.Replace("é", "e");
            res = res.Replace("í", "i");
            res = res.Replace("ó", "o");
            res = res.Replace("ú", "u");
            return res;
        }

        public static string getIdByFecha()
        {
            return DateTime.Today.Year.ToString() + "_" +
                DateTime.Today.Month.ToString() + "_" +
                DateTime.Today.Day.ToString() + "_" +
                DateTime.Now.Hour.ToString() + "_" +
                DateTime.Now.Minute.ToString() + "_" +
                DateTime.Now.Second.ToString();
        }


        #region Encriptar

        /// <summary>

        /// Método para encriptar un texto plano usando el algoritmo (Rijndael).

        /// Este es el mas simple posible, muchos de los datos necesarios los

        /// definimos como constantes.

        /// </summary>

        /// <param name="textoQueEncriptaremos">texto a encriptar</param>

        /// <returns>Texto encriptado</returns>

        public static string Encriptar(string textoQueEncriptaremos)
        {

            return Encriptar(textoQueEncriptaremos,

              "pass75dc@avz10", "s@lAvz", "MD5", 1, "@1B2c3D4e5F6g7H8", 128);

        }

        /// <summary>

        /// Método para encriptar un texto plano usando el algoritmo md5

        /// </summary>

        /// <returns>Texto encriptado</returns>

        public static string Encriptar(string textoQueEncriptaremos,

          string passBase, string saltValue, string hashAlgorithm,

          int passwordIterations, string initVector, int keySize)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,

              saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged()
            {

                Mode = CipherMode.CBC
            };

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes,

              initVectorBytes);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor,

             CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();

            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;

        }

        #endregion

        #region Desencriptar

        /// <summary>

        /// Método para desencriptar un texto encriptado.

        /// </summary>

        /// <returns>Texto desencriptado</returns>

        public static string Desencriptar(string textoEncriptado)
        {

            return Desencriptar(textoEncriptado, "pass75dc@avz10", "s@lAvz", "MD5",

              1, "@1B2c3D4e5F6g7H8", 128);

        }

        /// <summary>

        /// Método para desencriptar un texto encriptado md5

        /// </summary>

        /// <returns>Texto desencriptado</returns>

        public static string Desencriptar(string textoEncriptado, string passBase,

          string saltValue, string hashAlgorithm, int passwordIterations,

          string initVector, int keySize)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passBase,

              saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged()
            {

                Mode = CipherMode.CBC
            };

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,

              initVectorBytes);

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor,

              CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0,

              plainTextBytes.Length);

            memoryStream.Close();

            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0,

              decryptedByteCount);

            return plainText;

        }

        #endregion 
    }
}
