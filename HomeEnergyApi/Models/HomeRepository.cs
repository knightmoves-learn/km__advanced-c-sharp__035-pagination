using HomeEnergyApi.Pagination;
using Microsoft.EntityFrameworkCore;

namespace HomeEnergyApi.Models
{
    public class HomeRepository : IWriteRepository<int, Home>,
        IPaginatedReadRepository<int, Home>
    {
        private HomeDbContext context;

        public HomeRepository(HomeDbContext context)
        {
            this.context = context;
        }

        public Home Save(Home home)
        {
            if (home.HomeUsageData != null)
            {
                var usageData = home.HomeUsageData;
                usageData.Home = home;
                context.HomeUsageDatas.Add(usageData);
            }

            context.Homes.Add(home);
            context.SaveChanges();
            return home;
        }

        public Home Update(int id, Home home)
        {
            home.Id = id;
            context.Homes.Update(home);
            context.SaveChanges();
            return home;
        }

        public List<Home> FindAll()
        {
            return context.Homes
            .Include(h => h.HomeUsageData)
            .Include(h => h.HomeUtilityProviders)
            .ToList();
        }

        public Home FindById(int id)
        {
            return context.Homes.Find(id);
        }

        public Home RemoveById(int id)
        {
            var home = context.Homes.Find(id);
            context.Homes.Remove(home);
            context.SaveChanges();
            return home;
        }

        public int Count()
        {
            return context.Homes.Count();
        }

        public List<Home> FindByOwnerLastName(string ownerLastName)
        {
            return context.Homes
                .Where(h => h.OwnerLastName == ownerLastName)
                .Include(h => h.HomeUsageData)
                .Include(h => h.HomeUtilityProviders)
                .ToList();
        }

        public PaginatedResult<Home> FindPaginated(int pageNum, int pageSize)
        {
            pageNum = pageNum < 1 ? 1 : pageNum;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int homeCount = context.Homes.Count();
            var items = context.Homes
                .Include(h => h.HomeUsageData)
                .Include(h => h.HomeUtilityProviders)
                .OrderBy(h => h.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Home>
            {
                Items = items,
                TotalCount = homeCount,
                PageNumber = pageNum,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)homeCount / pageSize),
                HasNextPage = (pageNum / pageSize) < homeCount
            };
        }

        public PaginatedResult<Home> FindPaginatedByOwnerLastName(string lastName, int pageNum, int pageSize)
        {
            pageNum = pageNum < 1 ? 1 : pageNum;
            pageSize = pageSize < 1 ? 10 : pageSize;

            int homeCount = context.Homes.Count();
            var items = context.Homes
                .Where(h => h.OwnerLastName == lastName)
                .Include(h => h.HomeUsageData)
                .Include(h => h.HomeUtilityProviders)
                .OrderBy(h => h.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Home>
            {
                Items = items,
                TotalCount = homeCount,
                PageNumber = pageNum,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)homeCount / pageSize),
                HasNextPage = (pageNum / pageSize) < homeCount
            };
        }
    }
}