using System;
using System.Collections.Generic;
using System.Text;

namespace yap_lab_8i
{
    class Student
    {
        public string name, surname;
        private byte[][] results;

        public Student(int LabCount, int[] TaskCount)
        {
            name = surname = null;
            results = null;
            results = new byte[LabCount][];
            for (int i = 0; i < LabCount; i++)
                results[i] = new byte[TaskCount[i]];
        }

        public void SetMark(int labn, int taskn, byte mark)
        {
            results[labn][taskn] = mark;
        }
        public byte GetMark(int labn, int taskn)
        {
            return results[labn][taskn];
        }
        public void ShowDatabase(int labcount,int[] taskcount)
        {
            for (int i = 0; i < labcount; i++)
            {
                Console.Write($"Лаб. № {i + 1}: ");
                for (int j = 0; j < taskcount[i]; j++)
                {
                    Console.Write(results[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        public void ShowCompleteLabs()
        {
            Console.Write("Complete labs: ");
            bool ofl = true;
            for (int i = 0; i < results.Length; i++)
            {
                bool fl = true;
                for (int j = 0; fl && j < results[i].Length; j++)
                    if (results[i][j] < 3) fl = false;
                if (fl) { ofl = false; Console.Write(i + 1 + " "); }
            }
            if (ofl) Console.Write("None");
            Console.Write('\n');
        }
    }
}
