using Models;
using System.Collections.Generic;

namespace Core {
    interface IApp {
        int Run(string interpreterPath, int count);
    }
}