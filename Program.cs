using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace yap_lab_8i
{
    class Program
    {
        static void Main(string[] args)
        {
            Attestation a;
            string Labfilename, Datafilename;
            do
            {
                Console.Write("Введите имя файла с данными о лабораторных работах - "); 
                Labfilename = Console.ReadLine();
                Console.Clear();
            } while (!File.Exists(Labfilename));
            do
            {
                Console.Write("Введите имя файла с базой данных - ");
                Datafilename = Console.ReadLine();
                Console.Clear();
            } while (!File.Exists(Datafilename));
            a = new Attestation(Labfilename, Datafilename);
            a.LoadLabs();
            a.LoadStudents();
            bool exitFl = true;
            do
            {
                exitFl = MainMenu();
            } while (exitFl);

            MainMenu();

            bool View()
            {
                Console.Clear();
                Console.WriteLine("Меню->Информация");
                Console.WriteLine("1.Списоск сутеднтов");
                Console.WriteLine("2.Таблица успеваемости");
                Console.WriteLine("3.Сданные лабораторные");
                Console.WriteLine("4.Выйти");
                byte n;
                byte.TryParse(Console.ReadLine(), out n);
                Console.Clear();
                if (n == 1)
                {
                    a.ShowStudents();
                    Console.ReadKey();
                    return true;
                }
                if (n == 2)
                {
                    a.ShowStudents();
                    Console.Write("Введите номер студента - ");
                    int studentnumber = int.Parse(Console.ReadLine());
                    a.ShowDatabaseStudent(studentnumber - 1);
                    Console.ReadKey();
                    return true;
                }
                if (n == 3)
                {
                    a.ShowStudents();
                    a.ShowStudentCompleteLabs();
                    Console.ReadKey();
                    return true;
                }
                else if (n == 4) return false;
                else return true;
            }

            bool Edit()
            {
                Console.Clear();
                Console.WriteLine("Меню->Изменить оценку");
                a.ShowStudents();
                Console.WriteLine("Введите номер студента:");
                int num;
                if (!int.TryParse(Console.ReadLine(), out num)) { Console.WriteLine("Неправильный ввод"); Console.ReadKey(); return false; }
                if (num > a.StudentCount)
                {
                    Console.WriteLine("Неправильный ввод. Всего " + a.StudentCount + " студента(ов)"); Console.ReadKey(); return true;
                }
                else
                {
                    a.ShowDatabaseStudent(num - 1);
                    Console.WriteLine("Введите номер лабораторной:");
                    int lnum;
                    if (!int.TryParse(Console.ReadLine(), out lnum)) { Console.WriteLine("Неправильный ввод"); Console.ReadKey(); return false; }
                    if (lnum > a.LabCount || lnum < 1) { Console.WriteLine("Неправильный ввод. Всего " + a.LabCount + " лабораторные(ых)."); Console.ReadKey(); return true; }
                    else
                    {
                        Console.WriteLine("Введите номер задани:");
                        int tnum;
                        if (!int.TryParse(Console.ReadLine(), out tnum)) { Console.WriteLine("Неправильный ввод"); Console.ReadKey(); return false; }
                        if (tnum > a.TaskCount(lnum - 1) || tnum < 1) { Console.WriteLine("Неправильный ввод. Всего " + a.TaskCount(lnum - 1) + " Заданий."); Console.ReadKey(); return true; }
                        else
                        {
                            Console.WriteLine("Введите оценку:");
                            byte snum;
                            if (!byte.TryParse(Console.ReadLine(), out snum)) { Console.WriteLine("Неправильный ввод"); Console.ReadKey(); return false; }
                            if (snum > 4 || snum < 0) { Console.WriteLine("Неправильный ввод. Введите оценку от 0 до 4."); Console.ReadKey(); return true; }
                            else
                            {
                                a.StudentSetMark(num - 1, lnum - 1, tnum - 1, snum);
                                return false;
                            }
                        }

                    }
                }
            }

            bool Save()
            {
                Console.Clear();
                Console.WriteLine("Меню->Сохранить");
                Console.WriteLine("1.Сохранить");
                Console.WriteLine("2.Сохранить как...");
                Console.WriteLine("3.Выйти");
                byte n;
                byte.TryParse(Console.ReadLine(), out n);
                string filename = a.DataFilename;
                bool omd = true;
                if (n == 3) return false;
                else if (n == 2 || n == 1)
                {
                    if (n == 2)
                    {
                        Console.WriteLine("Сохранить как бинарный? (true/false) ");
                        string sOmd = Console.ReadLine();
                        bool omdTPR;
                        if (!bool.TryParse(sOmd, out omdTPR))
                        {
                            Console.WriteLine("Неправильный ввод.");
                            Console.ReadKey(); return true;
                        }
                        omd = omdTPR;
                        Console.WriteLine("Введите им файла:");
                        filename = Console.ReadLine();
                        if (File.Exists(filename))
                        {
                            Console.WriteLine("Неправильный ввод.");
                            Console.ReadKey(); return true;
                        }
                    }
                    a.SaveStudents(omd, filename);
                    return true;

                }
                else return true;
            }

            bool Load()
            {
                Console.Clear();
                Console.WriteLine("Меню->Загрузить");
                Console.WriteLine("Загрузить как бинарный? (true/false) ");
                string sOmd = Console.ReadLine();
                bool omdTPR;
                if (!bool.TryParse(sOmd, out omdTPR))
                {
                    Console.WriteLine("Неправильный ввод.");
                    Console.ReadKey(); return false;
                }
                bool omd = omdTPR;
                Console.WriteLine("Введите название файла:");
                string filename = Console.ReadLine();
                if (!File.Exists(filename))
                {
                    Console.WriteLine("Неправильный ввод.");
                    Console.ReadKey(); return false;
                }
                a.LoadStudents(omd,filename);
                return false;
            }

            bool MainMenu()
            {
                
                Console.Clear();
                Console.WriteLine("Меню");
                Console.WriteLine("1.Информация");
                Console.WriteLine("2.Изменить оценку");
                Console.WriteLine("3.Сохранить");
                Console.WriteLine("4.Загрузить");
                Console.WriteLine("5.Выйти");
                byte n;
                byte.TryParse(Console.ReadLine(), out n);
                bool exitFl = true;
                switch (n)
                {
                    case 1: do { exitFl = View(); } while (exitFl); return true;
                    case 2: do { exitFl = Edit(); } while (exitFl); return true;
                    case 3: do { exitFl = Save(); } while (exitFl); return true;
                    case 4: do { exitFl = Load(); } while (exitFl); return true;
                    case 5: return false;
                    default: return true;
                }
            }
        }
    }
}
