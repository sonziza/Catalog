using System;
using System.IO;
using System.Linq;
using Catalog.Context;
using Catalog.Migrations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Catalog
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");


            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;



            GetProductsAndCategories(options);
            GetProducts(connectionString);

            Console.Read();
        }
        /// <summary>
        /// Список товаров целиком (БЕЗ КАТЕГОРИЙ) - с помощью SQL
        /// </summary>
        /// <param name="connectionString"></param>
        public static void GetProducts(string connectionString)
        {
            string sqlExpression = "select * from Products";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0}\t{1}", reader.GetName(0), reader.GetName(1));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(1);

                        Console.WriteLine("{0} \t{1}", id, name);
                    }
                    reader.Close();
                }
            }
        }

        public static void GetProductsAndCategories(DbContextOptions<CatalogDbContext> opt)
        {
            //Список товаров для каждой категории - с помощью LINQ
            using (CatalogDbContext db = new CatalogDbContext(opt))
            {
                //Выводим список товаров для каждой категории
                var categories = db.Categories.Include(c => c.Products).ToList();

                foreach (Category category in categories)
                {
                    Console.WriteLine($"Категория: {category.Name}");

                    foreach (Product product in category.Products)
                    {
                        Console.WriteLine(product.Name);
                    }

                    Console.WriteLine("------------------");
                }
            }
        }
    }
}



