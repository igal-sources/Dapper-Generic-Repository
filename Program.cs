using System;
using DapperGenericRepository.Contracts;
using DapperGenericRepository.Models;
using DapperGenericRepository.Repositories;
using System.Collections.Generic;

namespace DapperGenericRepository
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ManageCategories();

            Console.WriteLine();
            Console.WriteLine("end...");
            Console.ReadLine();
        }

        private static void ManageCategories()
        {
            var categories = ExecuteGetCategories();
            var categorySingle = GetCategorByID();

            var categoryList = Get();

            Console.WriteLine("PrimaryKey Name: " + GetIdColumn());
            Console.WriteLine();

            foreach (var category in categoryList)
            {
                Console.WriteLine($"{category.category_id} - {category.category_name}");
            }

            Delete(9);
            Delete(10);
            Delete(11);

            categoryList = Get();

            Console.WriteLine();
            Console.WriteLine("After delete...");
            Console.WriteLine();
            foreach (var category in categoryList)
            {
                Console.WriteLine($"{category.category_id} - {category.category_name}");
            }

            Console.WriteLine();

            InsertMulti();
            InsertSingle();
            UpdateMulti();
            UpdateSingle();

            Console.WriteLine();
            categoryList = Get();
            Console.WriteLine("After insert/update...");
            Console.WriteLine();

            foreach (var category in categoryList)
            {
                Console.WriteLine($"{category.category_id} - {category.category_name}");
            }
        }

        private static IEnumerable<categories> Get()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetAllCategories();
            }
        }

        private static string GetIdColumn()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetIdColumn();
            }
        }

        private static IEnumerable<categories> ExecuteGetCategories()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetCategories("[production].[GetCategories]");
            }
        }

        private static IEnumerable<categories> GetCategorByID()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetCategoryById("[production].[GetCategoryById]", 5);
            }
        }

        private static void InsertMulti()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                List<categories> entities = new List<categories>
                {
                    new categories { category_id = 9, category_name = "category 3" },
                    new categories { category_id = 10, category_name = "category 1" }
                };

                repository.InsertCategories(entities);
            };
        }

        private static void InsertSingle()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                repository.InsertCategory(new categories { category_id = 11, category_name = "test category 11" });
            }
        }

        private static void Delete(int Id)
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                repository.DeleteCategoryById(Id);
            }
        }

        private static void UpdateMulti()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                var entities = new List<categories>
                {
                    new categories { category_id = 9, category_name = "test category 9" },
                    new categories { category_id = 10, category_name = "test category 10" }
                };

                repository.UpdateCategories(entities);
            }
        }

        private static void UpdateSingle()
        {
            using (ICategoriesRepository repository = new CategoriesRepository())
            {
                repository.UpdateCategory(new categories { category_id = 8, category_name = "test category 8" });
            }
        }
    }
}