﻿
namespace Communication
{
    public class FileHandler
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Name;
            }

            throw new Exception("File does not exist");
        }

        public long GetFileSize(string path)
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Length;
            }

            throw new Exception("File does not exist");
        }

        public void DeleteFile(string path)
        {
            if (FileExists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new Exception("File does not exist");
            }
        }
    }
}