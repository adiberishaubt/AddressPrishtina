﻿namespace AddressPrishtina.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; set;  }
    public int PageIndex { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int pageIndex, int totalPages)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalPages = totalPages;
    }
}