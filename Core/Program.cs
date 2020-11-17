using System;

namespace Core {
    class Program {
        static void Main(string[] args) {
            DBTester dBTester = new DBTester();
            dBTester.PutStuffInDB();
        }
    }
}
