using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Controller
    {
        public static Data? Data { get; set; }

        public static void LoadData(string path, char delimiter)
        {
            if (File.Exists(path))
            {
                Data = new Data(path, delimiter);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
