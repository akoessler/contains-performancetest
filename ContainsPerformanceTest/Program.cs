using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace ContainsPerformanceTest
{
	class Program
	{
		private const int RunCount = 10;
		private const int RunBeforeCount = 10;
		private const int EntityMin = (int)10e5;
		private const int EntityMax = (int)10e8;

		private static int[] entityCounts = { 10, 50, 100, 500, 1000, 5000, 10000, 50000, 100000, 500000 };

        static void Main()
		{
			// create simple entities with random numbers, non-consecutive to not have the ids sorted
			var entitiesArray = new List<Entity>[entityCounts.Length];
			for (int i = 0; i < entityCounts.Length; i++)
			{
				var entityCount = entityCounts[i];
				var list = entitiesArray[i] = new List<Entity>();

                foreach (var randomNumber in Utils.GenerateRandom(entityCount, EntityMin, EntityMax))
				{
					list.Add(new Entity { Id = randomNumber });
				}
			}

            // run before to rule out jit stuff
			for (int i = 0; i < RunBeforeCount; i++)
			{
				RunOnce_List(entitiesArray.First());
				RunOnce_HashSet(entitiesArray.First());
			}

            // run the tests
			foreach(var entities in entitiesArray)
			{
				Console.WriteLine("run with entity count: " + entities.Count.ToString("#,###,###", CultureInfo.InvariantCulture));
				RunTest(RunCount, entities, RunOnce_List, "List");
				RunTest(RunCount, entities, RunOnce_HashSet, "HashSet");
				Console.WriteLine();
            }
        }

        private static void RunTest(int runCount, List<Entity> entities, Action<List<Entity>> action, string name)
        {
	        var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < runCount; i++)
            {
	            action(entities);
            }

            stopwatch.Stop();
            var duration = stopwatch.Elapsed;
            var perRun = TimeSpan.FromMilliseconds(duration.TotalMilliseconds / runCount);

            Console.WriteLine(
	            name.PadRight(10)
	            + duration.ToString("hh\\:mm\\:ss\\.ffffff").PadRight(20)
	            + perRun.ToString("hh\\:mm\\:ss\\.ffffff").PadRight(20)
	        );
		}

		private static void RunOnce_List(List<Entity> entities)
		{
			var dict = new Dictionary<long, Entity>();
            var ids = new List<long>();

			foreach (var entity in entities)
			{
				dict.Add(entity.Id, entity);
                ids.Add(entity.Id);
			}

			var toDelete = dict.Where(x => !ids.Contains(x.Key)).ToList();
		}

		private static void RunOnce_HashSet(List<Entity> entities)
		{
			var dict = new Dictionary<long, Entity>();
			var ids = new HashSet<long>();

			foreach (var entity in entities)
			{
				dict.Add(entity.Id, entity);
				ids.Add(entity.Id);
			}

			var toDelete = dict.Where(x => !ids.Contains(x.Key)).ToList();
        }
	}
}
