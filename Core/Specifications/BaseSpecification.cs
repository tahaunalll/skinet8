using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.XPath;
using Core.Interfaces;

namespace Core.Specifications
{
    public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
    {
        public BaseSpecification():this(null) {}


        public Expression<Func<T, bool>>? Criteria => criteria;

        public Expression<Func<T, object>>? OrderBy {get; private set;}

        public Expression<Func<T, object>>? OrderByDescending {get; private set;}

        public bool IsDistinct {get; private set;}

        public int Take {get; private set;}

        public int Skip {get; private set;}

        public bool IsPagingEnabled {get; private set;}

        public List<Expression<Func<T, object>>> Includes {get;} =[];

        public List<string> IncludeStrings {get;} =[];

        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            if (Criteria!=null)
            {
                query=query.Where(Criteria);
            }
            return query;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString); //For ThenInclude
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending ( Expression <Func<T, object>> orderByDescExpression)
        {
            OrderByDescending=orderByDescExpression;
        }

        protected void ApplyDistinct()
        {
            IsDistinct =true;

        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true; 
        }
    }
    public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria)
        : BaseSpecification<T>(criteria), ISpecification<T, TResult>
    {
        /*
        null! operatörü, C# dilinde nullable referans türleri ile
         çalışırken null kontrolü yapmamız gerektiğini ve
          fakat burada null değerinin sorun teşkil etmediğini belirtmemizi sağlar.
           Bu, daha güvenli ve okunabilir bir kod yazmanıza yardımcı olur,
            çünkü derleyiciye belirli bir değerin null olabileceği ancak bunun
             sizin kontrolünüzde olduğu bilgisini verirsiniz.
        */
        protected BaseSpecification():this(null!){}
        public Expression<Func<T, TResult>>? Select {get; private set;}
        protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
        {
            Select=selectExpression;
        }
    }
}