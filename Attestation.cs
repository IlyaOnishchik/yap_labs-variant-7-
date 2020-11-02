using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace yap_lab_8i
{
    class Attestation
    {
        string labfilename, datafilename;
        int labcount;
        int[] taskcount;
        int studentcount;
        Student[] students;

        public Attestation(string Labfilename, string Datafilename)
        {
            labfilename = Labfilename;
            datafilename = Datafilename;
        }

        public void LoadLabs()
        {
            FileStream labfile = new FileStream(labfilename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(labfile);

            labcount = int.Parse(reader.ReadLine());
            taskcount = new int[labcount];
            string[] taskcountstring = reader.ReadLine().Split(',');
            for (int i = 0; i < labcount; i++)
                taskcount[i] = int.Parse(taskcountstring[i]);

            reader.Close();
            labfile.Close();
        }
        public void LoadStudents()
        {
            FileStream datafile = new FileStream(datafilename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(datafile);

            studentcount = int.Parse(reader.ReadLine());
            students = new Student[studentcount];
            for (int st = 0; st < studentcount; st++)
            {
                students[st] = new Student(labcount, taskcount);
                students[st].name = reader.ReadLine();
                students[st].surname = reader.ReadLine();
                for (int i = 0; i < labcount; i++)
                {
                    string labres = reader.ReadLine();
                    for (int j = 0, k = 0; j < taskcount[i]; j++, k += 2)
                    {
                        students[st].SetMark(i, j, byte.Parse(labres[k].ToString()));
                    }
                }
            }
            reader.Close();
            datafile.Close();
        }
        public void LoadStudents(string filename)
        {
            FileStream datafile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(datafile);

            studentcount = int.Parse(reader.ReadLine());
            students = new Student[studentcount];
            for (int st = 0; st < studentcount; st++)
            {
                students[st] = new Student(labcount, taskcount);
                students[st].name = reader.ReadLine();
                students[st].surname = reader.ReadLine();
                for (int i = 0; i < labcount; i++)
                {
                    string labres = reader.ReadLine();
                    for (int j = 0, k = 0; j < taskcount[i]; j++, k += 2)
                    {
                        students[st].SetMark(i, j, byte.Parse(labres[k].ToString()));
                    }
                }
            }
            reader.Close();
            datafile.Close();
        }
        public void LoadStudents(bool binaryMode, string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            if (!file.CanRead)
            {
                Console.WriteLine("students file read error");
                return;
            }
            datafilename = filename;
            if (binaryMode)
            {
                BinaryReader reader = new BinaryReader(file);

                studentcount = reader.ReadInt32();
                students = new Student[studentcount];

                for (int st = 0; st < studentcount; st++)
                {
                    students[st] = new Student(labcount, taskcount);
                    students[st].name = reader.ReadString();
                    students[st].surname = reader.ReadString();
                    for (int i = 0; i < labcount; i++)
                    {
                        for (int j = 0; j < taskcount[i]; j++)
                        {
                            students[st].SetMark(i, j, reader.ReadByte());
                        }
                    }
                }
                reader.Close();
            }
            else
            {
                StreamReader reader = new StreamReader(file);

                studentcount = int.Parse(reader.ReadLine());
                students = new Student[studentcount];

                for (int st = 0; st < studentcount; st++)
                {
                    students[st] = new Student(labcount, taskcount);
                    students[st].name = reader.ReadLine();
                    students[st].surname = reader.ReadLine();

                    for (int i = 0; i < labcount; i++)
                    {
                        string labRes = reader.ReadLine();
                        for (int j = 0, k = 0; j < taskcount[i]; j++, k += 2)
                        {
                            students[st].SetMark(i, j, byte.Parse(labRes[k].ToString()));
                        }
                    }
                }
                reader.Close();
            }
            file.Close();
        }
        public void ShowStudents()
        {
            Console.WriteLine("Студенты:");
            for (int i = 0; i < studentcount; i++)
            {
                Console.WriteLine($"{i + 1}:\t{students[i].name}\t{students[i].surname}");
            }
        }
        public void ShowDatabaseStudent(int studentnumber)
        {
            students[studentnumber].ShowDatabase(labcount,taskcount);
        }
        public void ShowStudentCompleteLabs()
        {
            Console.Write("Введите номер студента - ");
            int n = int.Parse(Console.ReadLine()) - 1;
            students[n].ShowCompleteLabs();
        }
        public void StudentSetMark(int studNum, int labNum, int taskNum, byte mark)
        {
            students[studNum].SetMark(labNum, taskNum, mark);
        }
        public int LabCount
        {
            get { return labcount; }
        }
        public string DataFilename
        {
            get { return datafilename; }
        }
        public int TaskCount(int labNum)
        {
            return taskcount[labNum];
        }
        public int StudentCount
        {
            get { return studentcount; }
        }
        public void SaveStudents(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            if (!file.CanWrite)
            {
                Console.WriteLine("Ошибка записи файла");
                return;
            }
            datafilename = filename;

            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(studentcount);
            foreach (Student student in students)
            {

                writer.WriteLine(student.name);
                writer.WriteLine(student.surname);

                for (int i = 0; i < labcount; i++)
                {
                    for (int j = 0; j < taskcount[i]; j++)
                    {
                        writer.Write(student.GetMark(i, j));
                        if (j < taskcount[i] - 1) writer.Write(',');
                    }
                    writer.WriteLine();
                }
            }
            writer.Close();
            file.Close();
        }
        public void SaveStudents(bool binaryMode, string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write);
            if (!file.CanWrite)
            {
                Console.WriteLine("file write error");
                return;
            }
            datafilename = filename;
            if (binaryMode)
            {
                BinaryWriter writer = new BinaryWriter(file);
                writer.Write(studentcount);
                foreach (Student student in students)
                {
                    writer.Write(student.name);
                    writer.Write(student.surname);
                    for (int i = 0; i < labcount; i++)
                    {
                        for (int j = 0; j < taskcount[i]; j++)
                        {
                            writer.Write(student.GetMark(i, j));
                        }
                    }
                }

                writer.Close();
            }
            else
            {
                StreamWriter writer = new StreamWriter(file);

                writer.WriteLine(studentcount);
                foreach (Student student in students)
                {

                    writer.WriteLine(student.name);
                    writer.WriteLine(student.surname);

                    for (int i = 0; i < labcount; i++)
                    {
                        for (int j = 0; j < taskcount[i]; j++)
                        {
                            writer.Write(student.GetMark(i, j));
                            if (j < taskcount[i] - 1) writer.Write(',');
                        }
                        writer.WriteLine();
                    }
                }
                writer.Close();
            }
            file.Close();
        }
    }
}