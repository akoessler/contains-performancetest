# contains-performancetest
Performance test for contains method in collections like List&lt;>, HashSet&lt;>

This example shows a special use case where a list of new records needs to be synchronized with a dictionary of current records.
The new records need to be added to the dictionary, old entries need to be removed.

This test is simplified here and shows especially the performance difference for calculating the entries that need to be removed from the dictionary.

For this calculation, the new IDs are added to a collection, then the dictionary is iterated and all items that are not contained in that collection can be removed.

This example here shows an extreme edge case where many new records are added and no records are to be removed. This is ok, as this test should only compare the difference for this "not contained in the collection" calculation.

As a start, only the Contains of List<> and HashSet<> are compared.

The test shows a time comparison of many different sizes of the "new entries" list. Each run is executed 10 times.

## Result

For full comparison result see [results.txt](results.txt).

Timings there were taken without debugger, in release configuration, on an Intel Core i7-9700k.

The first time in each line is the total time of "execute 10 times", the second time is the time per run.

That result shows that for smaller lists, the Contains method of List<> is ok to use, but when having more entries, the benefits of using HashSet<> are huge!

Example with largest tested set:
```
run with entity count: 500,000
List      00:45:33.889548     00:04:33.389000
HashSet   00:00:01.476674     00:00:00.148000
```

## Conclusion

Use HashSet<>!
