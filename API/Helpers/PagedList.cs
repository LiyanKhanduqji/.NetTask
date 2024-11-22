using System;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public class PagedList<T> : List<T>
{
    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        TotalCount = count;
        // Adds the page's items to the PagedList
        AddRange(items); //function from list class allows you to add multiple items to a list at once, rather than adding them one by one using the Add method
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }


    //creating a PagedList<T> directly from a database query 
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        // Uses CountAsync() to get the total number of items in the database.
        var count = await source.CountAsync();
        // Skips the items of earlier pages
        // ToListAsync: Executes the query and retrieves the items as a list.
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        // Creates a new PagedList<T> instance, passing the items and pagination details.
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}

// PageSize: The number of items displayed on each page.
// TotalCount: The total number of items in the dataset.