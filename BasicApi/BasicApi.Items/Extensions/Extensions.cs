using Microsoft.EntityFrameworkCore;
using BasicApi.Items.Types;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BasicApi.Items.Extensions
{
    public static class Extensions
    {
        public static IQueryable<T> OrderByPropertyOrField<T>(this IQueryable<T> queryable, string propertyOrFieldName, bool ascending = true)
        {
            if (string.IsNullOrEmpty(propertyOrFieldName))
            {
                propertyOrFieldName = "Id";
            }

            var elementType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elementType);
            var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
                new[] { elementType, propertyOrFieldExpression.Type }, queryable.Expression, selector);

            return queryable.Provider.CreateQuery<T>(orderByExpression);
        }

        public static async Task<PagedResult<T>> GetPagedResultAsync<T>(this IQueryable<T> queryable, PagedQueryBase pagedQuery)
        {
            IEnumerable<T> results;

            var skip = pagedQuery.PageSize * (pagedQuery.Page - 1);

            if (queryable.Expression.Type == typeof(IOrderedQueryable<T>))
            {
                results = await queryable.Skip(skip).Take(pagedQuery.PageSize).ToListAsync();
            }
            else
            {
                results = await queryable.OrderByPropertyOrField(pagedQuery.OrderBy, pagedQuery.Asc)
                                        .Skip(skip)
                                        .Take(pagedQuery.PageSize)
                                        .ToListAsync();
            }

            var totalRecords = await queryable.CountAsync();

            var mod = totalRecords % pagedQuery.PageSize;

            var totalPageCount = (totalRecords / pagedQuery.PageSize) + (mod == 0 ? 0 : 1);

            return PagedResult<T>.Create(results, pagedQuery.Page, pagedQuery.PageSize, totalPageCount, totalRecords);

        }
    }
}
