//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections.Generic;


namespace ColaFramework.Foundation
{
    /// <summary>
    /// 文件\目录操作助手类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// UTF8编码格式
        /// </summary>
        private static readonly UTF8Encoding UTF8Encode = new UTF8Encoding(false);

        private const int VERSION_LENGTH = 4;

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExist(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        public static string CurrentDirectory
        {
            get { return System.Environment.CurrentDirectory; }
        }

        public static long GetFileSize(string path)
        {
            System.IO.FileInfo info = new System.IO.FileInfo(path);
            return info.Length;
        }

        public static float GetFileSizeKB(string path)
        {
            return GetFileSize(path) / 1024f;
        }

        /// <summary>
        /// 获取目录下所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suffix"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string[] GetAllChildFiles(string path, string suffix = "", SearchOption option = SearchOption.AllDirectories)
        {
            string strPattner = "*";
            if (suffix.Length > 0 && suffix[0] != '.')
            {
                strPattner += "." + suffix;
            }
            else
            {
                strPattner += suffix;
            }

            string[] files = Directory.GetFiles(path, strPattner, option);

            return files;
        }

        /// <summary>
        /// 格式化路径为标准格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FormatPath(string path)
        {
            string result = path.Replace('\\', '/');
            return result.TrimEnd('/');
        }

        /// <summary>
        /// 遍历目录及其子目录，并将结果填充到files和paths中
        /// </summary>
        public static void Recursive(string path, List<string> files, List<string> paths)
        {
            if (string.IsNullOrEmpty(path) || null == files || null == paths)
            {
                Debug.LogError("Recursive 传入的参数错误!");
                return;
            }
            string[] names = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);
            foreach (string filename in names)
            {
                string ext = Path.GetExtension(filename);
                if (ext.Equals(".meta")) continue;
                files.Add(filename.Replace('\\', '/'));
            }
            foreach (string dir in dirs)
            {
                if (dir != ".idea")
                {
                    paths.Add(dir.Replace('\\', '/'));
                    Recursive(dir, files, paths);
                }
            }
        }

        public static void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        // 删除delPath目录中 不存在于srcPath中的文件
        public static void RidOfUnNecessaryFile(string srcPath, string checkPath)
        {
            Debug.LogFormat("RidOfUnNecessaryFile, srcPath:{0}, checkPath:{1}", srcPath, checkPath);
            if (!Directory.Exists(checkPath))
            {
                Debug.Log("目标路径不存在");
                return;
            }
            if (!Directory.Exists(srcPath))
            {
                Debug.Log("参考路径不存在，删除目标路径整个目录");
                RmDir(checkPath);
                Mkdir(checkPath);
                return;
            }


            string[] allSrcFiles = Directory.GetFiles(srcPath);
            string[] allCheckFiles = Directory.GetFiles(checkPath);
            Dictionary<string, bool> dicSrcFiles = new Dictionary<string, bool>();
            for (int i = 0; i < allSrcFiles.Length; i++)
            {
                string strFile = Path.GetFileName(allSrcFiles[i]);
                dicSrcFiles.Add(strFile, true);
            }
            for (int i = 0; i < allCheckFiles.Length; i++)
            {
                string strFile = allCheckFiles[i];
                string filename = Path.GetFileName(strFile);
                if (!dicSrcFiles.ContainsKey(filename))
                {
                    Debug.LogFormat("删除多余文件：{0}", strFile);
                    DeleteFile(strFile);
                }
            }
            Debug.Log("清除完成！");
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param> 路径
        /// <param name="isOverride"></param> 是否覆盖原有同名目录
        public static void Mkdir(string path, bool isOverride = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else if (Directory.Exists(path) && isOverride)
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="path"></param>
        public static void RmDir(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static void CopyDir(string srcPath, string destPath)
        {
            if (string.IsNullOrEmpty(srcPath) || string.IsNullOrEmpty(destPath))
            {
                return;
            }
            Mkdir(destPath);
            DirectoryInfo sDir = new DirectoryInfo(srcPath);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                if (file.Extension != ".meta")
                    file.CopyTo(destPath + "/" + file.Name, true);
            }
            //递归复制子文件夹
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                if (subDir.Name != ".idea")
                {
                    CopyDir(subDir.FullName, destPath + "/" + subDir.Name);
                }
            }
        }

        /// <summary>
        /// 清空目录下内容
        /// </summary>
        /// <param name="path"></param>
        public static void ClearDir(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            DirectoryInfo sDir = new DirectoryInfo(path);
            FileInfo[] fileArray = sDir.GetFiles();
            foreach (FileInfo file in fileArray)
            {
                file.Delete();
            }
            DirectoryInfo[] subDirArray = sDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirArray)
            {
                subDir.Delete(true);
            }
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="orginPath"></param>
        /// <param name="destPath"></param>
        /// <param name="isOverwrite"></param>
        public static void CopyFile(string orginPath, string destPath, bool isOverwrite)
        {
            EnsureParentDirExist(destPath);
            File.Copy(orginPath, destPath, isOverwrite);
        }

        /// <summary>
        /// 读取对应路径的文件到字节数组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// 读取对应路径的文件到string
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadString(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// 将字节数组写到对应路径的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteBytes(string filePath, byte[] content)
        {
            EnsureParentDirExist(filePath);
            File.WriteAllBytes(filePath, content);
        }

        /// <summary>
        /// 将string写到对应路径的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void WriteString(string filePath, string content)
        {
            EnsureParentDirExist(filePath);
            File.WriteAllText(filePath, content.Replace(Environment.NewLine, "\n"), UTF8Encode);
        }

        public static void EnsureParentDirExist(string path)
        {
            var dir = Path.GetDirectoryName(path);
            var parents = new Queue<string>();
            while (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir))
            {
                parents.Enqueue(dir);
                dir = Path.GetDirectoryName(dir);
            }
            while (parents.Count > 0)
            {
                Directory.CreateDirectory(parents.Dequeue());
            }
        }

        #region 文件读Md5相关
        /// <summary>
        /// 对指定路径的文件进行生成MD5码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            return GetMD5Hash(File.ReadAllBytes(filePath));
        }

        /// <summary>
        /// 二进制数据进行生成MD5码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string GetMD5Hash(byte[] buffer)
        {
            if (buffer == null)
                return null;

            MD5 md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "").ToLower();
        }
        #endregion
    }
}
