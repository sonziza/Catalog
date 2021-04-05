using System;
using System.IO;
using System.Linq;
using Catalog.Context;
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


            using (CatalogDbContext db = new CatalogDbContext(options))
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

            Console.Read();
        }
    }
}
