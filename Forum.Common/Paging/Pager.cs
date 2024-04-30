﻿// Copyright (C) TBC Bank. All Rights Reserved.

namespace Forum.Common.Paging;

public class Pager
{
    public int TotalItems { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int StartPage { get; private set; }
    public int EndPage { get; private set; }

    public Pager()
    {
        
    }

    public Pager(int totalItems, int page, int pageSize = 10)
    {
        var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
        var currentPage = page;

        var startPage = currentPage - 5;
        var endPage = currentPage + 4;

        if (startPage <= 0)
        {
            endPage = endPage - (startPage - 1);
            startPage = 1;
        }

        if (endPage > totalPages)
        {
            endPage = totalPages;
            if (endPage > 10)
                startPage = endPage - 9;
        }

        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        StartPage = startPage;
        EndPage = endPage;
    }

}
