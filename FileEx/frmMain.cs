using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileEx
{
    public partial class frmMain : Form
    {
        private const string PASSWORD = "TollExpressJapan"; //16文字
        private string FilePath;

        public frmMain()
        {
            InitializeComponent();

            FilePath = String.Empty;
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.FileName = "";
                ofd.InitialDirectory = @"C:\";
                ofd.Filter = "すべてのファイル(*.*)|*.*";
                ofd.Title = "ファイル選択";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK) FilePath = ofd.FileName;
            }
        }

        private void btnExec_Click(object sender, EventArgs e)
        {
            if (FilePath == String.Empty) { MessageBox.Show("先にファイルを選択"); return; }
            if (File.Exists(FilePath) == false ) { MessageBox.Show("無効なファイル"); return; }
            string mes = string.Format("{0}\r\nをごにょごにょします。よろしいですか？", Path.GetFileName(FilePath));
            if (MessageBox.Show(mes, "確認", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            Program.FileExExec(FilePath, PASSWORD);
            MessageBox.Show("完了");
        }
    }
}
