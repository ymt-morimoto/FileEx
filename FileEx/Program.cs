using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.IO.Compression;

namespace FileEx
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        public static bool FileExExec(string p_filePath, string p_pass)
        {
            int nByteSize = 4096;
            string OutFilePath = Path.Combine(Path.GetDirectoryName(p_filePath), Path.GetFileNameWithoutExtension(p_filePath)) + ".toll";
            using (FileStream ofs = new FileStream(OutFilePath, FileMode.Create, FileAccess.Write))
            {
                using (RijndaelManaged rm = new RijndaelManaged())
                {
                    rm.BlockSize = 256;              
                    rm.KeySize = 256;                
                    rm.Mode = CipherMode.CBC;        
                    rm.Padding = PaddingMode.PKCS7;  
                    
                    Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(p_pass, 16);
                    byte[] salt = new byte[16];
                    salt = deriveBytes.Salt;
                    byte[] bufferKey = deriveBytes.GetBytes(16);
                    
                    rm.Key = bufferKey;
                    rm.GenerateIV();
                    
                    ICryptoTransform encryptor = rm.CreateEncryptor(rm.Key, rm.IV);

                    using (CryptoStream cse = new CryptoStream(ofs, encryptor, CryptoStreamMode.Write))
                    {
                        ofs.Write(salt, 0, 16);
                        ofs.Write(rm.IV, 0, 16); 
                        using (DeflateStream ds = new DeflateStream(cse, CompressionMode.Compress))
                        {
                            using (FileStream fs = new FileStream(p_filePath, FileMode.Open, FileAccess.Read))
                            {
                                int len;
                                byte[] buf = new byte[nByteSize];
                                while ((len = fs.Read(buf, 0, nByteSize)) > 0)
                                {
                                    ds.Write(buf, 0, len);
                                }
                            }
                        }
                    }
                }
            }

            return (true);
        }
    }
}
