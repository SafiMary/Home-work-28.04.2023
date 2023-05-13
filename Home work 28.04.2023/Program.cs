using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Home_work_28._04._2023
{   
    delegate void MyProject();//создали делегат
    internal class Program
    {
         
        static SQLiteConnection connection;
        static SQLiteCommand command;
        
        static public bool Connect(string fileName)//подключение
        {
            try
            {
                connection = new SQLiteConnection("Data Source=" + fileName + ";Version=3; FailIfMissing=False");
                connection.Open();
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }
        static void Write()//ввод данных персонажа
        {
            Console.WriteLine("Введите имя персонажа: ");
            string uname = Console.ReadLine();
            Console.WriteLine("Введите возраст персонажа: ");
            int uage = 0;
            try
            {
                uage = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.FormatException) { }
            //запись данных в базу
            command.CommandText = "INSERT INTO Personage (name, year) VALUES (:name, :year)";

            Console.WriteLine($"Имя {uname}");
            command.Parameters.AddWithValue("name", uname);

            Console.WriteLine($"Возраст {uage}");
            command.Parameters.AddWithValue("year", uage);

            command.ExecuteNonQuery();
        }
        static void Read() //чтение из базы
        {
            command.CommandText = "SELECT * FROM Personage";
            DataTable data = new DataTable();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(data);
            Console.WriteLine($"Прочитано {data.Rows.Count} записей из таблицы БД");
            foreach (DataRow row in data.Rows)
            {
                Console.WriteLine($"id = {row.Field<long>("id")} name = {row.Field<string>("name")} year = {row.Field<byte>("year")} ");
            }
        }
        static void Main(string[] args)
        {
            if (Connect("MyBase.sqlite"))
            {
                Console.WriteLine("Идет подключение");
                command = new SQLiteCommand(connection)
                {
                    CommandText = "CREATE TABLE IF NOT EXISTS [Personage]([id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL UNIQUE, [name] TEXT, [year] byte);"
                };
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица создана");
            }
            MyProject myProject = null; ;//создали экземпляр делегата
            //сложили методы в делегат
            myProject += Write;
            myProject += Read;
            // вызываем методы у делегата
            myProject();

        }

    }
    }


