using DapperGenericRepository.Contracts;
using DapperGenericRepository.Models;
using DapperGenericRepository.Repositories;
using System;
using System.Collections.Generic;

namespace DapperGenericRepository
{
    internal class Program
    {
        public static void Main(string[] args)
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

            Console.WriteLine();
            Console.WriteLine("end...");
            Console.ReadLine();
        }

        private static IEnumerable<categories> Get()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetAll();
            }
        }

        private static string GetIdColumn()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetIdColumn();
            }
        }

        private static IEnumerable<categories> ExecuteGetCategories()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetCategories("[production].[GetCategories]");
            }
        }

        private static IEnumerable<categories> GetCategorByID()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                return repository.GetCategoryById("[production].[GetCategoryById]", 5);
            }
        }

        private static void InsertMulti()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                List<categories> entities = new List<categories>
                {
                    new categories { category_id = 9, category_name = "category 3" },
                    new categories { category_id = 10, category_name = "category 1" }
                };

                repository.Insert(entities);
            };
        }

        private static void InsertSingle()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                repository.Insert(new categories { category_id = 11, category_name = "test category 11" });
            }
        }

        private static void Delete(int Id)
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                repository.DeleteById(Id);
            }
        }

        private static void UpdateMulti()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                var entities = new List<categories>
                {
                    new categories { category_id = 9, category_name = "test category 9" },
                    new categories { category_id = 10, category_name = "test category 10" }
                };

                repository.Update(entities);
            }
        }

        private static void UpdateSingle()
        {
            using (IBikeStoresCategoriesRepository repository = new CategoriesRepository())
            {
                repository.Update(new categories { category_id = 8, category_name = "test category 8" });
            }
        }
    }
}