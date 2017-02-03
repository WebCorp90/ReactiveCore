using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReactiveHelpers.Core
{
    public static class FileExtensions
    {
        public static bool FileInUse(this string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    var b = fs.CanWrite;
                }
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }
    }
}
