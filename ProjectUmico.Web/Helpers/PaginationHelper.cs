namespace ProjectUmico.Web.Helpers;

public static class PaginationHelper
{
    public static IEnumerable<int> GivePagesArray(int currentPage,int pageSize,int totalPages)
    {
        int padding = pageSize / 2;

        int start,end;
        
        if((totalPages-currentPage) >= 0 && (currentPage - padding) > 0 && (currentPage - padding) > totalPages-pageSize)
        {
            start = totalPages-pageSize+1;
        }
        else if ((totalPages-currentPage) >= 0 && (currentPage - padding) > 0)
        {
            start = (currentPage - padding) + 1;
        }
        else
        {
            start = 1;
        }
        
        
        if ((currentPage + padding) > totalPages)
        {
            end = totalPages;
        }
        else if ((currentPage + padding) < pageSize)
        {
            end = pageSize;
        }
        else
        {
            end = currentPage + padding;	
        }

        
        return Enumerable.Range(start,end).ToList();
    }
}